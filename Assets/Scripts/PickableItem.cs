using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : InteractableItem
{
    public GameObject itemPrefab;
    public string itemName;
    private InventoryItem item;
    
    void Start()
    {
        item = new InventoryItem(itemPrefab, itemName);
    }

    public override void Use(PlayerInteractionController Caller)
    {
        if (Caller.AddItem(item))
        {
            Destroy(this.gameObject);
        }

    }
}
