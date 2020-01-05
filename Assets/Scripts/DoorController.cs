using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : InteractableItem
{
    private Animator animator;
    private bool _open = false;
    private bool _broken = true;
    public BoxCollider2D physicalCollider;
    public BoxCollider2D standCollider;
    public Sprite UseSprite;
    public Sprite ForbiddenSprite;
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
        if (!IsPlayerInBounds(Caller.gameObject))
        {
            Open(Caller);
        }
        
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
    private bool IsPlayerInBounds(GameObject Player)
    {
        BoxCollider2D playerCollider = Player.GetComponent<BoxCollider2D>();
        if (standCollider.bounds.Intersects(playerCollider.bounds))
        {
            Debug.Log("You're standing in the fucking door you wanker");
            return true;
        }
        else
        {
            Debug.Log("Good Job you're out of the bounds of the door youre still a cunt");
            return false;
        }

    }
    public override List<Sprite> GetSprite(PlayerInteractionController Caller)
    {
        List <Sprite> sprites = new List<Sprite>();
        if (IsPlayerInBounds(Caller.gameObject))
        {
            sprites.Add(ForbiddenSprite);
        }
        else
        {
            sprites.Add(UseSprite);
        }
        return sprites;
    }
}
