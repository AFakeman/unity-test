using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportItem : InteractableItem
{
    public GameObject Target;

    public override void Use(PlayerInteractionController Caller)
    {
        Caller.transform.position = Target.transform.position;
    }
}
