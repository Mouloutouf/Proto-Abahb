using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveToggle : ToggleBehaviour
{
    public override void Activate() => gameObject.SetActive(true);
    public override void Deactivate() => gameObject.SetActive(false);
}
