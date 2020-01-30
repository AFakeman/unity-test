using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunkyPile : ItemSpendingItem
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void ItemsSpent(PlayerInteractionController Caller)
    {
        var item = new InventoryItem();
        item.Name = "WoodenLog";
        Caller.AddItem(item);
        Destroy(this.gameObject);
    }
}
