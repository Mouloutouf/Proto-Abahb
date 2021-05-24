using System.Collections;
using System.Collections.Generic;
using Controller.Input.Character;
using Controller.Input.Camera;
using UnityEngine;

public class GrappleBehaviour : MonoBehaviour
{
    public bool IsGrappleAimed { get; set; }

    /// Line Rendering
    public LineRenderer lineRenderer;
    public float lineDrawingSpeed;

    /// Hook Model
    public Transform hookModel;

    /// Hook Settings
    public float throwDistance;

    public float grabRadius;
    public LayerMask grabMask;

    public float hitRadius;
    public LayerMask hitMask;

    public float retrieveHookWaitTime;

    /// Input
    public GrappleInput grappleInput;
    protected bool grappleKeyPressed;

    public Transform cameraTransform;
    public Transform reticleTransform;
    protected Vector3 currentCameraForward;

    /// Character
    public Transform characterTransform;
    public Rigidbody characterRigidbody;

    /// Positions
    protected Vector3 throwPosition;
    protected Vector3 localHookPos;
    protected Vector3 WorldHookPos => characterTransform.position + localHookPos;

    /// Grab
    protected bool IsGrabbing => currentGrabbed != null; // e.g isGrabbing
    protected bool isHookThrown;
    protected Grabbable currentGrabbed; public Grabbable CurrentGrabbed {
        get => currentGrabbed;
        set => currentGrabbed = value;
    }

    protected void CheckForInput() => grappleKeyPressed = grappleInput.IsGrappleKeyPressed();
    //protected void CheckForDirection() => currentCameraForward = reticleTransform.position - characterTransform.position;

    protected virtual void Update()
    {
        /// Input
        CheckForInput();

        /// Process
        if (IsGrabbing == false) {
            if (grappleKeyPressed && isHookThrown == false && IsGrappleAimed) {
                ThrowGrapple(currentCameraForward);
            }
        }
        else if (IsGrabbing == true) {
            localHookPos = currentGrabbed.transform.position - characterTransform.position;

            if (grappleKeyPressed) {
                if (IsGrappleAimed) {
                    DetachGrapple(); ThrowGrapple(currentCameraForward);
                }
                else {
                    RetrieveGrapple();
                }
            }
        }

        if (hookModel)
        {
            hookModel.position = WorldHookPos;
            hookModel.LookAt(WorldHookPos + throwPosition);
        }

        /// Display
        UpdateLine();
    }
    void UpdateLine()
    {
        lineRenderer.SetPosition(0, characterTransform.position);
        lineRenderer.SetPosition(1, localHookPos + characterTransform.position);
    }

    protected virtual void ThrowGrapple(Vector3 _forwardDir)
    {
        isHookThrown = true;
        if (hookModel) hookModel.gameObject.SetActive(true);
        lineRenderer.gameObject.SetActive(true);

        Vector3 throwDir = (reticleTransform.position + (cameraTransform.forward * throwDistance)) - characterTransform.position;
        throwPosition = throwDir.normalized * throwDistance;

        localHookPos = Vector3.zero;
        StartCoroutine(UpdateThrow());
    }
    protected virtual IEnumerator UpdateThrow()
    {
        Vector3 endHookPos = throwPosition;
        for (float ft = 0f; ft <= 1; ft += Time.deltaTime * lineDrawingSpeed)
        {
            localHookPos = Vector3.Lerp(Vector3.zero, endHookPos, ft);
            
            yield return new WaitForEndOfFrame();
        }
        HookCheck();
    }

    protected virtual void HookCheck()
    {
        Vector3 hookCheckPosOffset = localHookPos * 0.2f + characterTransform.position;

        #region Gizmos
        gizmoHookStartPos = hookCheckPosOffset;
        gizmoHookWorldPos = WorldHookPos;
        hookGizmosDisplay = true;
        StartCoroutine(StopGizmos());
        #endregion

        bool singleGrab = false;
        RaycastHit[] grabRaycastHits = Physics.CapsuleCastAll(hookCheckPosOffset, WorldHookPos, grabRadius, throwPosition, throwDistance, grabMask);

        if (grabRaycastHits.Length > 0 && singleGrab == false) {
            foreach (var hit in grabRaycastHits)
            {
                Collider col = hit.collider;

                col.gameObject.TryGetComponent(out Grabbable grabbable);
                if (currentGrabbed != null) currentGrabbed.Detach();
                grabbable.Attach(this);

                singleGrab = true;
            }
        }

        bool singleHit = false;
        RaycastHit[] hitRaycastHits = Physics.CapsuleCastAll(hookCheckPosOffset, WorldHookPos, hitRadius, throwPosition, throwDistance, hitMask);

        if (hitRaycastHits.Length > 0 && singleHit == false) {
            foreach (var hit in hitRaycastHits)
            {
                Collider col = hit.collider;

                col.gameObject.TryGetComponent(out HitBehaviour hitBehaviour);
                Debug.LogWarning("yes hit");
                hitBehaviour.OnHit();

                singleHit = true;
            }
        }

        if (IsGrabbing == false)
        {
            StartCoroutine(WaitRetrieve(retrieveHookWaitTime));
        }
    }

    IEnumerator WaitRetrieve(float time)
    {
        yield return new WaitForSeconds(time);
        RetrieveGrapple();
    }
    private void DetachGrapple()
    {
        if (currentGrabbed != null)
        {
            currentGrabbed.Detach();
            currentGrabbed = null;
        }
    }
    public virtual void RetrieveGrapple()
    {
        DetachGrapple();

        StartCoroutine(UpdateRetrieve());
    }
    protected virtual IEnumerator UpdateRetrieve()
    {
        Vector3 initialHookPos = localHookPos;
        for (float ft = 1f; ft >= 0; ft -= Time.deltaTime * lineDrawingSpeed)
        {
            localHookPos = Vector3.Lerp(Vector3.zero, initialHookPos, ft);
            yield return new WaitForEndOfFrame();
        }

        isHookThrown = false;
        if (hookModel) hookModel.gameObject.SetActive(false);
        lineRenderer.gameObject.SetActive(false);
    }

    // This is some real shit gizmos code, don't mind it
    #region Gizmos
    Vector3 characterGizmoPos;
    Vector3 hookGizmoPos;

    Vector3 gizmoHookStartPos;
    Vector3 gizmoHookWorldPos;
    bool hookGizmosDisplay;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        characterGizmoPos = characterTransform.position;
        hookGizmoPos = characterGizmoPos + characterTransform.forward * throwDistance;
        Gizmos.DrawLine(characterGizmoPos, hookGizmoPos);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hookGizmoPos, grabRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(hookGizmoPos, hitRadius);

        if (hookGizmosDisplay)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(gizmoHookStartPos, grabRadius);
            Gizmos.DrawLine(gizmoHookStartPos, gizmoHookWorldPos);
            Gizmos.DrawWireSphere(gizmoHookWorldPos, grabRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gizmoHookStartPos, hitRadius);
            Gizmos.DrawLine(gizmoHookStartPos, gizmoHookWorldPos);
            Gizmos.DrawWireSphere(gizmoHookWorldPos, hitRadius);
        }
    }
    IEnumerator StopGizmos()
    {
        yield return new WaitForSeconds(retrieveHookWaitTime);
        hookGizmosDisplay = false;
    }
    #endregion
}
