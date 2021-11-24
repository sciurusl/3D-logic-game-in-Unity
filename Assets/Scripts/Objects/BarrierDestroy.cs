using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates or deactivates itself when called based on the called method
/// </summary>
public class BarrierDestroy : ObjectOperation
{
    public bool reversed = false;

    public override void StartFunctionality(Transform triggerObject)
    {
        gameObject.SetActive(reversed);
    }

    public override void EndFunctionality(Transform triggerObject)
    {
        gameObject.SetActive(!reversed);
    }
}
