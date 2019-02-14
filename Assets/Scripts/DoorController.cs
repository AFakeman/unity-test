using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : InteractableItem
{
    private Animator animator;
    private bool _open = false;
    private bool _broken = true;
    public BoxCollider2D physicalCollider;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Broken", true);
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
        if (_broken)
        {
            FixDoor();
        }
        else
        {
            _open = !_open;
            animator.SetBool("Open", _open);
            physicalCollider.enabled = !physicalCollider.enabled;
        }
    }
    public void FixDoor()
    {
        _broken = false;
        animator.SetBool("Broken", false);
    }

    public override uint GetUseTime(PlayerInteractionController Caller)
    {
        return interactionTime;
    }
}
