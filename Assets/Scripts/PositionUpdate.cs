using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUpdate : MonoBehaviour
{
    public Transform mirroringObject;

    /// <summary>
    /// Updates its local position based on its mirroring object
    /// </summary>
    void Update()
    {
        transform.position = new Vector3(mirroringObject.position.x, transform.position.y, mirroringObject.position.z);
        transform.localPosition = new Vector3(transform.localPosition.x, mirroringObject.localPosition.y, transform.localPosition.z);
    }
}
