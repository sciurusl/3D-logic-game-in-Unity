using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the functionality of every object which can be somehow operated
/// </summary>
public abstract class ObjectOperation : MonoBehaviour
{
    public bool openingAnimStarted;
    public bool closingAnimStarted;
    public bool moved;
    public bool closed;
    public float numOfElements;
    public float curNumOfEl;

    /// <summary>
    /// Specific function gets called by a controlling object
    /// </summary>

    public abstract void StartFunctionality(Transform triggerObject);


    public abstract void EndFunctionality(Transform triggerObject);

    public virtual void EndFuncUsingRigidbody()
    {

    }

}
