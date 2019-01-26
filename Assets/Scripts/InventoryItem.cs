using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public GameObject Prefab;
    public string Name;

    public InventoryItem(GameObject pref, string name)
    {
        Prefab = pref;
        this.Name = name;
    }
}
