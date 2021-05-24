using UnityEngine;

public class AimInput : MonoBehaviour
{
    public KeyCode aimKey;

    public bool isAimKeyPressed() => Input.GetKeyDown(aimKey);
    public bool isAimKeyReleased() => Input.GetKeyUp(aimKey);
    public bool isAimKeyHeld() => Input.GetKey(aimKey);
}
