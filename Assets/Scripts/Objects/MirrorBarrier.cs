using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mirrors its position to the mirroring barrier
/// </summary>
public class MirrorBarrier : MonoBehaviour
{
    public Transform mirrorObject;
    
    void LateUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, mirrorObject.localPosition.y, transform.localPosition.z);
    }
}
