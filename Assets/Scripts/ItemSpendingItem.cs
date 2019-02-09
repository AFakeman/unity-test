using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ItemSpendingItem : InteractableItem
{
    [SerializeField]
    private List<InventoryItem> price; 
    
    public virtual List<InventoryItem> Price
    {
        get => price;
        set => price = value;
    }

    public void SpendItems(PlayerInteractionController Caller)
    {
        foreach (var item in Price)
        {
            Caller.RemoveItem(item);
        }
    }

    public override void Use(PlayerInteractionController Caller)
    {
        if (Caller.CanSpendItems(Price))
        {
            SpendItems(Caller);
            ItemsSpent(Caller);
        }
        else
        {
            ItemsNotSpent(Caller);
        }
    }

    public override uint GetUseTime(PlayerInteractionController Caller)
    {
        return interactionTime;
    }

    protected virtual void ItemsSpent(PlayerInteractionController Caller)
    {
        Debug.unityLogger.Log("Here you go!");
    }

    protected virtual void ItemsNotSpent(PlayerInteractionController Caller)
    {
        Debug.unityLogger.Log("No money, honey!");
    }
}
