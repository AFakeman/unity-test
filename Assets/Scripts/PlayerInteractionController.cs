using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlayerInteractionController : MonoBehaviour
{
    public Sprite itemUseThought;
    public ItemBubble thoughtBubble;

    private const uint maxInventorySize = 5;
    private uint WaitTime;
    private uint Cooldown;
    private Animator animator;
    private InventoryDisplay _inventoryDisplay;

    public Dictionary<InventoryItem, int> inventory = new Dictionary<InventoryItem, int>();

    private List<InteractableItem> _interactableItems;
    private InteractableItem _itemInUse;
    private InteractableItem _itemInMind;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _interactableItems = new List<InteractableItem>();
        _inventoryDisplay = GameObject.Find("InventoryUI").GetComponent<InventoryDisplay>();
    }

    // Update is called once per frame
    private void Update()
    {
        var use = Input.GetKey(KeyCode.E);

        if (WaitTime != 0 || Cooldown != 0)
        {
            if (WaitTime != 0)
            {
                WaitTime--;
            }
            else
            {
                Cooldown--;
            }
        }
        else
        {
            if (_itemInUse)
            {
                Debug.unityLogger.Log("Use");
                _itemInUse.Use(this);
                _itemInUse = null;
                animator.SetBool("action", false);
                Debug.unityLogger.Log(animator.GetBool("action"));
                Cooldown = 45;
            }
            else
            {
                var closestItem = GetClosestInteractableItem();
                if (use && closestItem)
                {
                    _itemInUse = closestItem;
                    if (_itemInMind)
                    {
                        _itemInMind = null;
                        SetThoughtForInteractableItem(null);
                    }
                    WaitTime = closestItem.GetUseTime(this);
                    animator.SetBool("action", true);
                    Debug.unityLogger.Log(animator.GetBool("action"));
                }
                else
                {
                    if (_itemInMind != closestItem)
                    {
                        _itemInMind = closestItem;
                    }
                    SetThoughtForInteractableItem(_itemInMind);
                }
            }
        }
    }

    private InteractableItem GetClosestInteractableItem()
    {
        float closestDist = float.MaxValue;
        InteractableItem closestItem = null;
        var currentPosition = transform.position;
        _interactableItems.RemoveAll(item => !item);
        foreach (InteractableItem item in _interactableItems)
        {
            var dist = Vector2.Distance(currentPosition, item.transform.position);
            if (dist < closestDist && item.Enabled())
            {
                closestDist = dist;
                closestItem = item;
            }
        }
        return closestItem;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.unityLogger.Log("TriggerEnter");
        var a = other.gameObject.GetComponent<InteractableItem>();
        if (a)
        {
            Debug.unityLogger.Log("InteractbleItem entered");
            _interactableItems.Add(a);
        }
    }

    private void SetThoughtForInteractableItem(InteractableItem item)
    {
        if (!item)
        {
            thoughtBubble.RenderSprites((List<ItemBubble.StyledIcon>) null);
            return;
        }
        var thought = new List<ItemBubble.StyledIcon>();
        if (item is ItemSpendingItem it)
        {
            var priceCounts = new Dictionary<InventoryItem, int>();
            foreach (InventoryItem inventoryItem in it.Price)
            {
                if (!priceCounts.ContainsKey(inventoryItem))
                {
                    priceCounts.Add(inventoryItem, 1);
                }
                else
                {
                    priceCounts[inventoryItem]++;
                }
            }

            foreach (var priceCount in priceCounts)
            {
                var sprite = _inventoryDisplay.iconSprites[priceCount.Key.Name];
                var inventoryCount = inventory.ContainsKey(priceCount.Key) ? inventory[priceCount.Key] : 0;
                int i;
                for (i = 0; i < inventoryCount && i < priceCount.Value; ++i)
                {
                    thought.Add(new ItemBubble.StyledIcon(){Sprite = sprite, Style = ItemBubble.Style.Solid});
                }

                for (; i < priceCount.Value; ++i)
                {
                    thought.Add(new ItemBubble.StyledIcon(){Sprite = sprite, Style = ItemBubble.Style.Semitransparent});
                }
            }
        }
        else
        {
            thought.Add(new ItemBubble.StyledIcon(){Sprite = item.GetSprite(this)[0], Style = ItemBubble.Style.Solid});
        }
        thoughtBubble.RenderSprites(thought);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.unityLogger.Log("TriggerExit");
        var a = other.gameObject.GetComponent<InteractableItem>();

        if (a && a == _itemInUse)
        {
            Debug.unityLogger.Log("Use interrupted");
            _itemInUse = null;
            animator.SetBool("action", false);
            WaitTime = 0;
        }

        _interactableItems.Remove(a);
    }

    public bool AddItem(InventoryItem item)
    {
        if (inventory.Count >= maxInventorySize)
        {
            return false;
        }

        if (!inventory.ContainsKey(item))
        {
            inventory.Add(item, 1);
        }
        else
        {
            inventory[item]++;
        }
        return true;
    }

    public bool CanSpendItems(List<InventoryItem> items)
    {
        Dictionary<InventoryItem, int> counts = (
            from s in items group s by s into g select
            new { Stuff = g.Key, Count = g.Count() }
        ).ToDictionary(g => g.Stuff, g => g.Count);

        foreach (var item in items)
        {
            if (!inventory.ContainsKey(item) || inventory[item] < counts[item])
            {
                return false;
            }
        }

        return true;
    }

    public bool RemoveItem(InventoryItem item)
    {
        if (!inventory.ContainsKey(item))
        {
            return false;
        }

        inventory[item]--;
        if (inventory[item] == 0)
        {
            inventory.Remove(item);
        }
        return true;
    }
}
