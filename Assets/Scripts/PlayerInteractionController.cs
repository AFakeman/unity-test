using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlayerInteractionController : MonoBehaviour
{
    private const uint maxInventorySize = 5;
    private List<InventoryItem> _inventory = new List<InventoryItem>();
    private uint WaitTime;
    private uint Cooldown;
    private Animator animator;

    public List<InventoryItem> Inventory
    {
        get { return _inventory; }
        protected set { _inventory = value; }
    }
    private InteractableItem _interactableItem;
    private InteractableItem CurrentInteractableItem;

    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
            if (_interactableItem && CurrentInteractableItem)
            { 
                Debug.unityLogger.Log("Use");
                _interactableItem.Use(this);
                CurrentInteractableItem = null;
                animator.SetBool("action", false);
                Debug.unityLogger.Log(animator.GetBool("action"));
                Cooldown = 45;
            }
            if (use && _interactableItem)
            {
                WaitTime = _interactableItem.GetUseTime(this);
                CurrentInteractableItem = _interactableItem;
                animator.SetBool("action", true);
                Debug.unityLogger.Log(animator.GetBool("action"));

            }
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.unityLogger.Log("TriggerEnter");
        var a = other.gameObject.GetComponent<InteractableItem>();
        if (a)
        {
            Debug.unityLogger.Log("InteractbleItem entered");
            _interactableItem = a;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.unityLogger.Log("TriggerExit");
        var a = other.gameObject.GetComponent<InteractableItem>();
        if (a && a == _interactableItem)
        {
            Debug.unityLogger.Log("InteractbleItem exited");
            _interactableItem = null;
            CurrentInteractableItem = null;
            animator.SetBool("action", false);
        }
    }

    public bool AddItem(InventoryItem item)
    {
        if (_inventory.Count >= maxInventorySize)
        {
            return false;
        }
        _inventory.Add(item);
        return true;
    }

    public bool CanSpendItems(List<InventoryItem> items)
    {
        Dictionary<string, uint> counts = new Dictionary<string, uint>();
        foreach (var item in _inventory)
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
        return _inventory.Remove(_inventory.First(it => it.Name == item.Name));
    }
}
