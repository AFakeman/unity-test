using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public PlayerInteractionController player;
    public float itemSpacing = 1.5f;

    [System.Serializable]
    public struct ItemPair
    {
        public string Name;
        public Sprite Sprite;
    }

    public List<ItemPair> ItemList;
    public Dictionary<string, Sprite> iconSprites;

    private List<InventoryItem> renderedItems = new List<InventoryItem>();

    // Start is called before the first frame update
    private void Start()
    {
        iconSprites = new Dictionary<string, Sprite>();
        foreach (var pair in ItemList)
        {
            iconSprites[pair.Name] = pair.Sprite;
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!renderedItems.SequenceEqual(player.inventory))
        {
            RenderItems(player.inventory);
        }
        
    }

    private void RenderItems(List<InventoryItem> toRender)
    {
        Debug.unityLogger.Log("RedrawInventory");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        renderedItems = new List<InventoryItem>();

        float xOffset = 0;
        int itemNumber = 0;
        foreach (InventoryItem item in toRender)
        {
            GameObject newObj = new GameObject("Item" + itemNumber);
            var spriteRenderer = newObj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = iconSprites[item.Name];
            newObj.transform.parent = transform;
            newObj.transform.localPosition = new Vector3(xOffset, 0, 0);
            newObj.layer = 5;
            renderedItems.Add(item);
            xOffset += -itemSpacing;
            itemNumber++;
        }
    }
}        

