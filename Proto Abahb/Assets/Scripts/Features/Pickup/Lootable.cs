using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lootable : MonoBehaviour
{
    private const float minSpeed = 0.75f;
    private const float maxSpeed = 4f;
    private const float maxRot = 1000f;
    private const float maxRotRandomRange = 500f;
    private const float maxPickupTime = 1.5f;
    private const float scaleDownSpeed = 5f;
    private const float pickupRange = 0.35f;

    private float range;
    private Looter looter;
    private Transform targetTransform;
    private Coroutine lootRoutine = null;
    public void Loot(Looter looter, float range)
    {
        if(lootRoutine != null) return;
        this.looter = looter;
        this.targetTransform = looter.transform;
        this.range = range;
        lootRoutine = StartCoroutine(LootRoutine());
    }

    private void OnDestroy()
    {
        if(looter == null) return;
        looter.ReceiveLoot(this);
    }
    IEnumerator LootRoutine()
    {
        Vector3 relativePos = transform.position - targetTransform.position;
        float distance = relativePos.magnitude;
        float progress = 0;
        float timer = 0;
        float maxRot = Lootable.maxRot + (Random.Range(-1f, 1f) * maxRotRandomRange);

        while (distance > pickupRange && timer != 1)
        {
            var target = targetTransform.position;
            //if(distance < (transform.position - target).magnitude) 
            transform.position = (target + relativePos);
            progress = (Time.deltaTime / maxPickupTime);
            
            // Lerp Position towards target. The distance can't increase even if the target moved
            transform.position = Vector3.MoveTowards(transform.position, target, (range-pickupRange) * progress);
            // Rotate around target. Rotation speed increases the closer to the target
            transform.RotateAround(target, transform.up, (maxRot * timer) * Time.deltaTime);

            // Lerp the Scale towards 0
            if(timer > 0.75f) transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, scaleDownSpeed* Time.deltaTime);
            
            relativePos = (transform.position - target);
            distance = relativePos.magnitude;
            timer = Mathf.Clamp(timer + progress, 0, 1);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
