using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Closes specified door when the player triggers its collider
/// </summary>
public class CloseDoorPlayer : MonoBehaviour
{
    public Transform Door;
    public Transform DoorMirroring;
    public Transform Barrier;
    public bool openAtStart = true;
    void Start()
    {
        if(openAtStart)
            StartCoroutine(OpenDoor());
    }

    /// <summary>
    /// Opens specified door and also the mirroring door
    /// </summary>
    /// <returns></returns>
    IEnumerator OpenDoor()
    {
        Door.GetComponent<OpeningClosingObj>().GoUp(transform);
        DoorMirroring.GetComponent<OpeningClosingObj>().GoUp(transform);
        yield return new WaitForSeconds(1);
        Door.GetComponent<OpeningClosingObj>().GoUp(null);
        DoorMirroring.GetComponent<OpeningClosingObj>().GoUp(null);
    }

    /// <summary>
    /// When triggered, it closes specified door as well as the mirroring door
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if(openAtStart)
            Door.GetComponent<OpeningClosingObj>().GoDown(transform);
        Door.GetComponent<OpeningClosingObj>().GoDown(transform);
        if (DoorMirroring != null)
        {
            DoorMirroring.GetComponent<OpeningClosingObj>().GoDown(transform);
            DoorMirroring.GetComponent<OpeningClosingObj>().GoDown(transform);
        }
        if (Barrier != null)
        {
            Barrier.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
