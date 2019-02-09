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

    public List<InventoryItem> inventory;

    private List<InteractableItem> _interactableItems;
    private InteractableItem _itemInUse;
    private InteractableItem _itemInMind;
    private List<InventoryItem> _inventory = new List<InventoryItem>();

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
                        SetThoughtForInteractableItem(null);
                    }
                    WaitTime = closestItem.GetUseTime(this);
                    animator.SetBool("action", true);
                    Debug.unityLogger.Log(animator.GetBool("action"));
                }
                else if (_itemInMind != closestItem)
                {
                    _itemInMind = closestItem;
                    SetThoughtForInteractableItem(closestItem);
                }
            }
        }
    }

    private InteractableItem GetClosestInteractableItem()
    {
        float closestDist = float.MaxValue;
        InteractableItem closestItem = null;
        var currentPosition = transform.position;
        foreach (InteractableItem item in _interactableItems)
        {
            var dist = Vector2.Distance(currentPosition, item.transform.position);
            if (dist < closestDist)
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
            thoughtBubble.RenderSprites(null);
            return;
        }
        var thought = new List<Sprite>();
        if (item is ItemSpendingItem it)
        {
            foreach (InventoryItem inventoryItem in it.price)
            {
                var sprite = _inventoryDisplay.iconSprites[inventoryItem.Name];
                thought.Add(sprite);
            }
        }
        else
        {
            thought.Add(itemUseThought);
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
        inventory.Add(item);
        return true;
    }

    public bool CanSpendItems(List<InventoryItem> items)
    {
        Dictionary<string, uint> counts = new Dictionary<string, uint>();
        foreach (var item in inventory)
        {
            if (!counts.ContainsKey(item.Name))
            {
                counts[item.Name] = 0;
            }
            counts[item.Name] += 1;
        }

        foreach (var item in items)
        {
            if (!counts.ContainsKey(item.Name) || counts[item.Name] == 0)
            {
                return false;
            }

            counts[item.Name]--;
        }

        return true;
    }

    public bool RemoveItem(InventoryItem item)
    {
        return inventory.Remove(inventory.First(it => it.Name == item.Name));
    }
}
