using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : InteractableItem
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Use(PlayerInteractionController Caller)
    {
        Open(Caller);
    }
    public void Open(PlayerInteractionController Caller)
    {
        animator.SetBool("Open", true);
    }
    public override uint GetUseTime(PlayerInteractionController Caller)
    {
        throw new System.NotImplementedException();
    }
}
