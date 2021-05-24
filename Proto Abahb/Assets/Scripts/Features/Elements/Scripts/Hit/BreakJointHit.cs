using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakJointHit : HitBehaviour
{
    public Joint joint;

    public override void OnHit()
    {
        Destroy(joint);

        base.OnHit();
    }
}
