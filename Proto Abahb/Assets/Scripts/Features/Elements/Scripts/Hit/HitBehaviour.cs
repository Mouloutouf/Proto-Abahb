using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class HitBehaviour : MonoBehaviour
{
    public bool useEvent;
    [ShowIf("useEvent")]
    public UnityEvent hitEvent;

    public virtual void OnHit()
    {
        hitEvent.Invoke();
    }
}
