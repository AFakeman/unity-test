using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : InteractableItem
{
    public string itemName;
    private InventoryItem _item;
    
    void Start()
    {
        _item = new InventoryItem();
        _item.Name = itemName;
    }

    public override void Use(PlayerInteractionController Caller)
    {
        if (Caller.AddItem(_item))
        {
            Destroy(this.gameObject);
        }

    }
    public override uint GetUseTime(PlayerInteractionController Caller)
    {
        return interactionTime;
    }
}
