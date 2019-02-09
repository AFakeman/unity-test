using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class InteractableItem : MonoBehaviour
{
    public uint interactionTime = 0;

    public abstract void Use(PlayerInteractionController Caller);
    public abstract uint GetUseTime(PlayerInteractionController Caller);

    public virtual bool Enabled()
    {
        return true;
    }
}
