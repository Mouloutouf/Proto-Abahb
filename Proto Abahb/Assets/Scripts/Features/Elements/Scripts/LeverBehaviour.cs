using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverBehaviour : MonoBehaviour
{
    public Rigidbody leverRb;

    private void Start()
    {
        leverRb.isKinematic = true;
    }

    public void _ActivateLeverRigidbody()
    {
        leverRb.isKinematic = false;
    }
}
