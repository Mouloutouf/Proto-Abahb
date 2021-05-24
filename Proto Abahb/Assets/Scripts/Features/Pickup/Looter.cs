using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Looter : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private SphereCollider col;

    [SerializeField] 
    private UnityEvent onReceiveLoot;
    private void Reset()
    {
        if (col == null) TryGetComponent(out col);
        col.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Lootable") return;
        other.GetComponentInParent<Lootable>().Loot(this, col.radius);
    }

    public void ReceiveLoot(Lootable loot)
    {
        onReceiveLoot.Invoke();
    }
}
