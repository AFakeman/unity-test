using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private const uint maxInventorySize = 5;
    private List<InventoryItem> _inventory = new List<InventoryItem>();

    public List<InventoryItem> Inventory
    {
        get { return _inventory; }
        protected set { _inventory = value; }
    }
    private InteractableItem _interactableItem;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var use = Input.GetKey(KeyCode.E);

        if (use && _interactableItem)
        {
            Debug.unityLogger.Log("Use");
            _interactableItem.Use(this);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.unityLogger.Log("TriggerEnter");
        var a = other.gameObject.GetComponent<InteractableItem>();
        if (a)
        {
            Debug.unityLogger.Log("InteractbleItem entered");
            _interactableItem = a;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.unityLogger.Log("TriggerExit");
        var a = other.gameObject.GetComponent<InteractableItem>();
        if (a && a == _interactableItem)
        {
            Debug.unityLogger.Log("InteractbleItem exited");
            _interactableItem = null;
        }
    }

    public bool AddItem(InventoryItem item)
    {
        if (_inventory.Count >= maxInventorySize)
        {
            return false;
        }
        _inventory.Add(item);
        return true;
    }
}
