using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private const uint maxInventorySize = 5;
    private List<InventoryItem> _inventory = new List<InventoryItem>();
    private uint WaitTime;
    private Animator animator;

    public List<InventoryItem> Inventory
    {
        get { return _inventory; }
        protected set { _inventory = value; }
    }
    private InteractableItem _interactableItem;
    private InteractableItem CurrentInteractableItem;

    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var use = Input.GetKey(KeyCode.E);

        if (WaitTime != 0)
        {
            WaitTime--;
        }
        else
        {
            if (_interactableItem && CurrentInteractableItem)
            { 
                Debug.unityLogger.Log("Use");
                _interactableItem.Use(this);
                CurrentInteractableItem = null;
                animator.SetBool("action", false);
                Debug.unityLogger.Log(animator.GetBool("action"));
            }
            if (use && _interactableItem)
            {
                WaitTime = _interactableItem.GetUseTime(this);
                CurrentInteractableItem = _interactableItem;
                animator.SetBool("action", true);
                Debug.unityLogger.Log(animator.GetBool("action"));

            }
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
            CurrentInteractableItem = null;
            WaitTime = 0;
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
