using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using static Obi.ObiSolver;

public class ObiGrappleBehaviour : GrappleBehaviour
{
    public ObiSolver solver;

    public ObiColliderBase characterCollider;

    public ObiRopeBlueprint blueprint;

    public ObiRope rope;
    public ObiRopeCursor cursor;

    private void Start()
    {
        blueprint.path.Clear();
        blueprint.path.FlushEvents();

        CreatePinBatch();

        cursor.ChangeLength(1f);
    }

    protected override void Update()
    {
        /// Input
        CheckForInput();

        /// Process
        if (IsGrabbing == false)
        {
            if (grappleKeyPressed && isHookThrown == false)
            {
                ThrowGrapple(currentCameraForward);
            }
        }
        else if (IsGrabbing == true)
        {
            if (grappleKeyPressed)
            {
                RetrieveGrapple();
            }
        }
    }

    protected override void ThrowGrapple(Vector3 _throwDir)
    {
        isHookThrown = true;
        if (hookModel) hookModel.gameObject.SetActive(true);

        _throwDir += characterTransform.position;

        // Procedurally generate the rope path (a simple straight line):
        blueprint.path.Clear();
        blueprint.path.AddControlPoint(characterTransform.position, -_throwDir.normalized, _throwDir.normalized, Vector3.up, 0.1f, 0.1f, 1, 1, Color.white, "Hook start");
        blueprint.path.AddControlPoint(_throwDir, -_throwDir.normalized, _throwDir.normalized, Vector3.up, 0.1f, 0.1f, 1, 1, Color.white, "Hook end");
        blueprint.path.FlushEvents();

        solver.OnCollision += OnGrappleCollision;

        StartCoroutine(UpdateThrow());
    }
    protected override IEnumerator UpdateThrow()
    {
        // Generate the particle representation of the rope (wait until it has finished):
        yield return blueprint.Generate();

        // Set the blueprint (this adds particles/constraints to the solver and starts simulating them).
        rope.ropeBlueprint = blueprint;
        rope.GetComponent<MeshRenderer>().enabled = true;

        for (float ft = 0f; ft <= 1; ft += Time.deltaTime * lineDrawingSpeed)
        {
            float length = Mathf.Lerp(0, throwDistance, ft);
            cursor.ChangeLength(length);
            Debug.Log(rope.restLength);

            yield return new WaitForEndOfFrame();
        }

        HookCheck();
    }

    public override void RetrieveGrapple()
    {
        if (currentGrabbed != null)
        {
            currentGrabbed.Detach();
            currentGrabbed = null;
        }

        StartCoroutine(UpdateRetrieve());
    }
    protected override IEnumerator UpdateRetrieve()
    {
        for (float ft = 1f; ft >= 0; ft -= Time.deltaTime * lineDrawingSpeed)
        {
            float length = Mathf.Lerp(0, throwDistance, ft);
            cursor.ChangeLength(length);
            Debug.Log(rope.restLength);

            yield return new WaitForEndOfFrame();
        }

        rope.GetComponent<MeshRenderer>().enabled = false;

        solver.OnCollision -= OnGrappleCollision;

        isHookThrown = false;
        if (hookModel) hookModel.gameObject.SetActive(false);
    }

    public void OnGrappleCollision(ObiSolver _solver, ObiCollisionEventArgs _contacts)
    {
        foreach (var contact in _contacts.contacts)
        {
            ObiColliderBase collider = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;

            if (collider.gameObject.layer == grabMask)
            {
                AddPinConstraint(blueprint.activeParticleCount - 1, collider);
            }
            else if (collider.gameObject.layer == hitMask)
            {
                collider.gameObject.TryGetComponent(out HitBehaviour hitBehaviour);
                hitBehaviour.OnHit();
            }
        }
    }

    ObiConstraints<ObiPinConstraintsBatch> pinConstraints;
    ObiPinConstraintsBatch batch;

    private void CreatePinBatch()
    {
        InitPinBatch();

        AddPinConstraint(0, characterCollider);
    }

    private void InitPinBatch()
    {
        pinConstraints = rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
        pinConstraints.Clear();
        batch = new ObiPinConstraintsBatch();
        pinConstraints.AddBatch(batch);
    }

    private void AddPinConstraint(int particleIndex, ObiColliderBase colliderBase)
    {
        batch.AddConstraint(rope.solverIndices[particleIndex], colliderBase, colliderBase.transform.localPosition, Quaternion.identity, 0, 0, float.PositiveInfinity);

        batch.activeConstraintCount++;
        rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
    }
    private void RemovePinConstraint(int index)
    {
        batch.RemoveConstraint(index);

        batch.activeConstraintCount--;
        rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
    }

    protected override void HookCheck()
    {
        base.HookCheck();
    }
}
