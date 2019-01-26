using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : InteractableItem
{
    public InventoryItem item;

    public override void Use(PlayerInteractionController Caller)
    {
        if (Caller.AddItem(item))
        {
            Destroy(this.gameObject);
        }

    }
}
