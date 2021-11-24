using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements movement of mirroring object
/// It connects mirroing objects. When one moves, its mirroring part moves, and otherwise
/// </summary>
public class MirroringObjectInfo : MonoBehaviour
{
    public Transform mirroringObejct;

    Rigidbody body;
    public void Start()
    {
        body = mirroringObejct.GetComponent<Rigidbody>();
        body.freezeRotation = true;
    }

    /// <summary>
    /// Checks if the x or z coordinates of their position differ
    /// If does, it checks if one of the objects is not slower than the other one (suppose there is a barrier or other movable object),
    /// if it is, it adjusts the position of one of the objects
    /// </summary>
    public void LateUpdate()
    {
        if (transform.position.x != mirroringObejct.position.x || transform.position.z != mirroringObejct.position.z)
        {
            if(body.velocity.x<transform.GetComponent<Rigidbody>().velocity.x || body.velocity.z < transform.GetComponent<Rigidbody>().velocity.z)
            {
                transform.position = new Vector3(mirroringObejct.position.x, transform.position.y, mirroringObejct.position.z);
            }
            else
            {
                mirroringObejct.position = new Vector3(transform.position.x, mirroringObejct.position.y, transform.position.z);
            }
        }
    }

    /// <summary>
    /// By calling its rigidbody, it moves according to the pushForce parameter
    /// </summary>
    /// <param name="pushForce"></param>
    /// <returns></returns>
    public bool UpdateMirObjectPosition(Vector3 pushForce)
    {

        var pos = mirroringObejct.position;
        body.velocity = pushForce;
        body.AddForce(pushForce);
        var posUpdated = body.position;
        if (pos.x == posUpdated.x && pos.z == posUpdated.z)
        {
            return false;
        }
        return true;
    }
}
