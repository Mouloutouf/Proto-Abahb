using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTriggerArea : MonoBehaviour
{
    public GameObject triggerGrabbable;
    public ToggleBehaviour observedObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(triggerGrabbable))
        {
            observedObject.Activate();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(triggerGrabbable))
        {
            observedObject.Deactivate();
        }
    }
}
