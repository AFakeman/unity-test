using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public PlayerInteractionController player;
    public float itemSpacing = 1.5f;

    private List<InventoryItem> renderedItems = new List<InventoryItem>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!renderedItems.SequenceEqual(player.Inventory))
        {
            RenderItems(player.Inventory);
        }
        
    }

    void RenderItems(List<InventoryItem> toRender)
    {
        Debug.unityLogger.Log("RedrawInventory");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        renderedItems = new List<InventoryItem>();

        float zOffset = 0;
        foreach (InventoryItem item in toRender)
        {
            GameObject newObj = Object.Instantiate(item.Prefab, transform);
            newObj.transform.localPosition += new Vector3(zOffset, 0, 0);
            newObj.layer = 5;
            renderedItems.Add(item);
            zOffset += -itemSpacing;
        }
    }
}        

