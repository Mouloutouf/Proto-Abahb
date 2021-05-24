using System;
using System.Collections;
using System.Collections.Generic;
using Controller.Controllers;
using Controller.Core_scripts;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[ExecuteAlways]
public class ClimbingController : MonoBehaviour
{
    #region References
    private Collider col;
    private Rigidbody rb;
    private Mover mover;
    private AdvancedWalkerController controller;

    #endregion

    #region Parameters
    [FoldoutGroup("Controller Settings"), SerializeField]
    private LayerMask wallMasks;
    [FoldoutGroup("Controller Settings"), SerializeField]
    private float range = 0.35f;
    [FoldoutGroup("Controller Settings"), SerializeField]
    private float smoothing = 5f;
    [FoldoutGroup("Controller Settings"), SerializeField]
    private float identitySmoothing = 10f;
    
    [FoldoutGroup("Endurance Settings"), SerializeField]
    private float enduranceLossDuration = 3f;
    [FoldoutGroup("Endurance Settings"), SerializeField]
    private int enduranceLossTicks = 6;
    [FoldoutGroup("Endurance Settings"), SerializeField]
    private float enduranceHealDuration = 3f;
    [FoldoutGroup("Endurance Settings"), SerializeField]
    private int enduranceHealTicks = 6;
    [FoldoutGroup("Endurance Settings"), SerializeField]
    private float exhaustingAngle = 60;
    [FoldoutGroup("Endurance Settings"), SerializeField]
    private float jumpTimeBeforeGravity = 0.2f;

    
    #endregion
    #region Events
    [FoldoutGroup("Events"), SerializeField]
    private UnityEvent<float> onEnduranceChanged;
    #endregion
    
    #region Const
    private const float offset = 0.15f;
    #endregion
    
    #region Runtime Values
    private float rayLength => size.x + range + offset;
    [ShowInInspector, HideInEditorMode, BoxGroup("Debug")]
    private float endurance;
    [ShowInInspector, HideInEditorMode, BoxGroup("Debug")]
    private float enduranceTimer;
    [ShowInInspector, HideInEditorMode, BoxGroup("Debug")]
    private bool enduranceHealing;
    [ShowInInspector, HideInEditorMode, BoxGroup("Debug")]
    private float groundAngle;
    private Vector3 extents;
    private Vector3 size;
    
    private CollisionInfo infos;
    private bool climbKeyDown;
    private bool unstick;
    private float jumpTimer;
    #endregion
    
    // Start is called before the first frame update
    private void Awake()
    {
        TryGetComponent(out col);
        TryGetComponent(out rb);
        TryGetComponent(out mover);
        TryGetComponent(out controller);

        extents = col.bounds.extents;
        size = col.bounds.size;
        
    }

    private void Start()
    {

        UpdateEndurance(1);
    }

    private void Update()
    {
        climbKeyDown |= Input.GetKeyDown(KeyCode.LeftShift);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DetectWalls();
    }

    private void DetectWalls()
    {
        infos = new CollisionInfo();
        infos.grounded = mover.IsGrounded();
        infos.groundNormal = mover.GetGroundNormal();

        //Unstick from surface if endurance is at 0
        if (unstick)
        {
            if (infos.grounded && Vector3.Angle(Vector3.up, infos.groundNormal) < exhaustingAngle)
                unstick = false;
            else
            {
                var upRot = Quaternion.FromToRotation(transform.up, Vector3.up) * rb.rotation;
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, upRot, identitySmoothing * Time.deltaTime));
                return;
            }
        }
        
        infos.up = transform.up;
        infos.direction = controller.GetMovementVelocity().normalized;
        infos.origin = transform.position - infos.direction * offset + transform.up;

        if (infos.grounded)
        {
            jumpTimer = 0;
            groundAngle = Vector3.Angle(infos.groundNormal, Vector3.up);

            if (groundAngle < exhaustingAngle)
            {
                if (!enduranceHealing)
                {
                    enduranceTimer = enduranceHealDuration / enduranceHealTicks;
                    enduranceHealing = true;
                }
            }
            else
            {
                if (endurance == 0) unstick = true;
                if (enduranceHealing)
                {
                    enduranceTimer = enduranceLossDuration / enduranceLossTicks; 
                    enduranceHealing = false;
                }
            }

            if (enduranceHealing && endurance < 1 || !enduranceHealing && endurance > 0)
            {
                enduranceTimer -= Time.deltaTime;
                if (enduranceTimer <= 0)
                {
                    enduranceTimer = enduranceHealing ? enduranceHealDuration / enduranceHealTicks : enduranceLossDuration / enduranceLossTicks;
                    UpdateEndurance(enduranceHealing ? 1f / enduranceHealTicks : -1f / enduranceLossTicks);
                }
            }
        }
        
        //Detect a Wall in range
        if (Physics.Raycast(infos.origin, infos.direction, out infos.hitInfo, rayLength, wallMasks) && infos.angle > 30 && (endurance != 0 || infos.angle < exhaustingAngle))
        {
            infos.hit = true;
            RotateTowards(infos.normal, identitySmoothing);
        }
        else if (infos.grounded) RotateTowards(infos.groundNormal, smoothing);
        else
        {
            if (jumpTimer != 1) IncreasePercentage(ref jumpTimer, jumpTimeBeforeGravity);
            else RotateTowards(Vector3.up, identitySmoothing);
        }

        climbKeyDown = false;
    }

    public void UpdateEndurance(float amount)
    {
        var prevEndurance = endurance;
        endurance = Mathf.Clamp(endurance + amount, 0, 1);
        if (prevEndurance != endurance)
        {
            onEnduranceChanged.Invoke(endurance);
        }
    }
    
    private void RotateTowards(Vector3 up, float smoothing)
    {
        var upRot = Quaternion.FromToRotation(transform.up, up) * rb.rotation;
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, upRot, smoothing * Time.deltaTime));
    }
    private void IncreasePercentage(ref float value, float timeToMax)
    {
        value = Mathf.Clamp(value + Time.deltaTime / timeToMax, 0, 1);
    }

    private struct CollisionInfo
    {
        public Vector3 up;
        public Vector3 origin;
        public Vector3 direction;
        public float distance => hitInfo.distance;
        public Vector3 point => hitInfo.point;
        public Vector3 normal => hitInfo.normal;
        public bool grounded;
        public Vector3 groundNormal;
        public float angle
        {
            get
            {
                if(_angle == null) _angle = Vector3.Angle(up, normal);
                return _angle.Value;
            }
        }
        private float? _angle;
        
        public Collider contact => hitInfo.collider;
        public bool hit;
        public RaycastHit hitInfo;
    }
}
