using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLeftTheTeleport : MonoBehaviour
{
    public Transform thisTeleport;
    public Transform otherTeleport;

    public void OnTriggerEnter(Collider other)
    {
        thisTeleport.GetComponent<RotatingWorld>().leftTheTeleport = true;
        otherTeleport.GetComponent<RotatingWorld>().leftTheTeleport = false;
    }
}
