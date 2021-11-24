using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeInside : MonoBehaviour
{
    public Transform barrier;
    public Transform otherBarrier;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("player"))
            return;
        Debug.Log("changed inside from " + transform.name);
        barrier.GetComponents<OpenDoorFromDirection>()[0].inside = true;
        barrier.GetComponents<OpenDoorFromDirection>()[1].inside = true;       
        otherBarrier.GetComponents<OpenDoorFromDirection>()[0].inside = false;
        otherBarrier.GetComponents<OpenDoorFromDirection>()[1].inside = false;
        otherBarrier.GetComponents<OpenDoorFromDirection>()[0].OpenDoor();
        otherBarrier.GetComponents<OpenDoorFromDirection>()[1].OpenDoor();
    }
}
