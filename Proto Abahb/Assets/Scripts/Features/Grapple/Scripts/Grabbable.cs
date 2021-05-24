using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum JointType { FixedJoint, SpringJoint, HingeJoint, CharacterJoint, ConfigurableJoint }

public abstract class Grabbable : MonoBehaviour
{
    public GameObject parent;

    public Joint joint;

    protected GrappleBehaviour connectedGrapple;

    public bool useEvent;
    [ShowIf("useEvent")]
    public UnityEvent grabEvent;

    protected virtual void Start()
    {
        Detach();
    }

    public virtual void Attach(GrappleBehaviour grapple)
    {
        connectedGrapple = grapple;
        connectedGrapple.CurrentGrabbed = this;

        joint.connectedBody = grapple.characterRigidbody;

        grabEvent.Invoke();
    }
    public virtual void Detach()
    {
        joint.connectedBody = null;
        
        connectedGrapple = null;
    }

    [Button]
    protected virtual void OnJointBreak(float breakForce)
    {
    }
}