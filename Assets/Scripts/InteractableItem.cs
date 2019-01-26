using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class InteractableItem : MonoBehaviour
{
    public abstract void Use(PlayerInteractionController Caller);
}
