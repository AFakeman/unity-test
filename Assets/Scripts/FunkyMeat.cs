using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunkyMeat : ItemSpendingItem
{
    public Sprite cookedMeat;
    private bool cooked = false;
    
    [SerializeField]
    private List<InventoryItem> priceSequence;

    private int _sequencePosition = 0;
    
    private SpriteRenderer _sr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override List<InventoryItem> Price
    {
        get => new List<InventoryItem> {priceSequence[_sequencePosition]};
        set { }
    }

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void ItemsSpent(PlayerInteractionController Caller)
    {
        _sequencePosition++;
        if (_sequencePosition == priceSequence.Count)
        {
            cooked = true;
            _sr.sprite = cookedMeat;
            var pickableItem = gameObject.AddComponent<PickableItem>();
            pickableItem.itemName = "CookedMeat";
            Destroy(this);
        }

    }

    public override bool Enabled()
    {
        return !cooked;
    }
}
