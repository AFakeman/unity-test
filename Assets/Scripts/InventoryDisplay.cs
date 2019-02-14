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

    private Dictionary<InventoryItem, int> renderedItems = new Dictionary<InventoryItem, int>();
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
        if (player.inventory.Count != renderedItems.Count || renderedItems.Except(player.inventory).Any())
        {
            RenderItems(player.inventory);
        }
    }

    // Renders items. The item sprites should be the same size, or,
    // at least, the same pixels per unit.
    private void RenderItems(Dictionary<InventoryItem, int> toRender)
    {
        Debug.unityLogger.Log("RedrawInventory");
        renderedItems = new Dictionary<InventoryItem, int>(toRender);
        var spritesToRender = new List<Sprite>();
        foreach (var itemPair in toRender)
        {
            for (int i = 0; i < itemPair.Value; ++i)
            {
                spritesToRender.Add(iconSprites[itemPair.Key.Name]);
            }
        }
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

