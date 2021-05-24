using Controller.Camera;
using Controller.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aiming : MonoBehaviour
{
    public float aimSpeed;

    public Transform cameraTarget;
    public Transform baseCamera, aimCamera;

    public CameraController cameraController;
    public float baseCamSpeed, aimCamSpeed;

    public AdvancedWalkerController movementController;
    public float baseMoveSpeed, aimMoveSpeed;

    public GrappleBehaviour grappleBehaviour;

    public Image reticleImage;

    public AimInput aimInput;

    private bool aiming;

    private void Update()
    {
        if (aiming) {
            if (aimInput.isAimKeyReleased()) {
                AimState(false);
            }
        }
        else {
            if (aimInput.isAimKeyPressed()) {
                AimState(true);
            }
        }
    }

    private void AimState(bool isAiming)
    {
        StopCoroutine("AimLerp");
        StartCoroutine(AimLerp(isAiming));

        aiming = isAiming;
        grappleBehaviour.IsGrappleAimed = isAiming;
    }

    IEnumerator AimLerp(bool isAiming)
    {
        Vector3 currentCamPos = cameraTarget.localPosition;

        Vector3 destinationPos = isAiming ? aimCamera.localPosition : baseCamera.localPosition;

        float camSpeed = isAiming ? aimCamSpeed : baseCamSpeed;
        float moveSpeed = isAiming ? aimMoveSpeed : baseMoveSpeed;

        for (float ft = 0f; ft <= 1f; ft += Time.deltaTime * aimSpeed)
        {
            cameraTarget.localPosition = Vector3.Lerp(currentCamPos, destinationPos, ft);
            yield return new WaitForEndOfFrame();
        }

        cameraController.cameraSpeed = camSpeed;
        movementController.movementSpeed = moveSpeed;

        reticleImage.gameObject.SetActive(isAiming);
    }
}
