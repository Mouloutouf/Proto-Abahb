using UnityEngine;

public class GrabbableSpring : Grabbable
{
    public float grabOffset = 0.1f;
    public SpringJoint spring { get => (SpringJoint)joint; }

    public float springValue = 100;
    public float damperValue = 9;

    public float breakForce = 100;
    public float breakTorque = 100;

    public float massScale = 10;

    protected override void Start()
    {
        ResetJoint();
    }

    public void ResetJoint()
    {
        spring.anchor = Vector3.zero;
        spring.autoConfigureConnectedAnchor = false;
        spring.connectedAnchor = Vector3.zero;

        spring.spring = springValue;
        spring.damper = damperValue;

        spring.breakForce = breakForce;
        spring.breakTorque = breakTorque;

        spring.massScale = massScale;

        Detach();
    }

    public override void Attach(GrappleBehaviour grapple)
    {
        base.Attach(grapple);

        spring.minDistance = grapple.throwDistance + grabOffset;
        spring.maxDistance = grapple.throwDistance - grabOffset;
        spring.tolerance = 0.0f;
    }

    public override void Detach()
    {
        spring.minDistance = 0;
        spring.maxDistance = Mathf.Infinity;
        spring.tolerance = 0.0f;

        base.Detach();
    }

    protected override void OnJointBreak(float _breakingForce)
    {
        Debug.LogWarning("Break");

        if (connectedGrapple) connectedGrapple.RetrieveGrapple();

        Detach();

        joint = (SpringJoint)parent.AddComponent(typeof(SpringJoint));
        ResetJoint();
    }
}