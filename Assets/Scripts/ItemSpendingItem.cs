using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ItemSpendingItem : InteractableItem
{
    public List<InventoryItem> price;

    public void SpendItems(PlayerInteractionController Caller)
    {
        foreach (var item in price)
        {
            Caller.RemoveItem(item);
        }
    }

    public override void Use(PlayerInteractionController Caller)
    {
        if (Caller.CanSpendItems(price))
        {
            SpendItems(Caller);
            Debug.unityLogger.Log("Here you go!");
        }
        else
        {
            Debug.unityLogger.Log("No money, honey!");
        }
    }

    public override uint GetUseTime(PlayerInteractionController Caller)
    {
        return interactionTime;
    }
}
