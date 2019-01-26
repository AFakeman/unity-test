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
        public GameObject Prefab;
    }

    [SerializeField]
    public List<ItemPair> ItemList;
    private Dictionary<string, GameObject> _iconPrefabs;

    private List<InventoryItem> renderedItems = new List<InventoryItem>();

    // Start is called before the first frame update
    void Start()
    {
        _iconPrefabs = new Dictionary<string, GameObject>();
        foreach (var pair in ItemList)
        {
            _iconPrefabs[pair.Name] = pair.Prefab;
        }
        
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
            GameObject newObj = Object.Instantiate(_iconPrefabs[item.Name], transform);
            newObj.transform.localPosition += new Vector3(zOffset, 0, 0);
            newObj.layer = 5;
            renderedItems.Add(item);
            zOffset += -itemSpacing;
        }
    }
}        

