using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public PlayerInteractionController player;
    public ItemBubble itemBubble;

    [System.Serializable]
    public struct ItemPair
    {
        public string Name;
        public Sprite Sprite;
    }

    public List<ItemPair> ItemList;
    public Dictionary<string, Sprite> iconSprites;

    private List<InventoryItem> renderedItems = new List<InventoryItem>();
    private SpriteRenderer _itemBubbleSR;

    // Start is called before the first frame update
    private void Start()
    {
        iconSprites = new Dictionary<string, Sprite>();
        foreach (var pair in ItemList)
        {
            iconSprites[pair.Name] = pair.Sprite;
        }
    }

    private void Awake()
    {
        _itemBubbleSR = itemBubble.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!renderedItems.SequenceEqual(player.inventory))
        {
            RenderItems(player.inventory);
        }
        
    }

    // Renders items. The item sprites should be the same size, or,
    // at least, the same pixels per unit.
    private void RenderItems(List<InventoryItem> toRender)
    {
        Debug.unityLogger.Log("RedrawInventory");
        renderedItems = new List<InventoryItem>(toRender);
        var spritesToRender = toRender.Select(item => iconSprites[item.Name]).ToList();
        itemBubble.RenderSprites(spritesToRender);
        AlignBubble();
    }

    private void AlignBubble()
    {
        var xPos = -_itemBubbleSR.bounds.extents.x;
        var yPos = -_itemBubbleSR.bounds.extents.y;
        var zPos = itemBubble.transform.localPosition.z;
        itemBubble.transform.localPosition = new Vector3(xPos, yPos, zPos);
    }
}        

