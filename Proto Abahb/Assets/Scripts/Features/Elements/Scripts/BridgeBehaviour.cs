using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBehaviour : MonoBehaviour
{
    public Rigidbody bridgeRb;

    public bool left;
    public float force = 100f;

    public void _PushBridge()
    {
        if (left) {
            bridgeRb.AddForce(Vector3.left * force);
        }
        else {
            bridgeRb.AddForce(Vector3.right * force);
        }
    }
}
