using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GrappleInput : MonoBehaviour
{
    public bool useJoystick;

    [ShowIf("useJoystick")]
    public KeyCode joystickGrappleKey = KeyCode.Joystick1Button0;
    [HideIf("useJoystick")]
    public KeyCode keyboardGrappleKey = KeyCode.LeftShift;

    public KeyCode GrappleKey { get => useJoystick ? joystickGrappleKey : keyboardGrappleKey; }

    public virtual bool IsGrappleKeyPressed()
    {
        return Input.GetKeyDown(GrappleKey);
    }
    public virtual bool IsGrappleKeyReleased()
    {
        return Input.GetKeyUp(GrappleKey);
    }
}
