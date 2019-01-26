using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class InteractableItem : MonoBehaviour
{
    public uint interactionTime;

    public abstract void Use(PlayerInteractionController Caller);
    public abstract uint GetUseTime(PlayerInteractionController Caller);
}
