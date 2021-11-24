using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningClosingRigidbody : MonoBehaviour
{
    private bool forceAdded = false;
    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Floor"))
            StartCoroutine(GoDown());
    }

    /// <summary>
    /// If the object is in the same world as the player, it can use the Rigidbody component to close itself
    /// Otherwise, it closes itself using the SmoothlyGoDown method of the OpeningClosingObj class
    /// </summary>
    /// <returns></returns>
    public IEnumerator GoDown()
    {
        if (transform.parent.parent.CompareTag("ParallelWorld") && !GameManager.instance.inParallelWorld || !transform.parent.parent.CompareTag("ParallelWorld") && GameManager.instance.inParallelWorld)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(1);

        }
        else
        {
            GetComponent<OpeningClosingObj>().openingAnimStarted = false;
            GetComponent<OpeningClosingObj>().closingAnimStarted = true;
            GetComponent<OpeningClosingObj>().StartCoroutine(GetComponent<OpeningClosingObj>().GoDownUsingRigidbody());
        }
    }

    private void FixedUpdate()
    {
        if(forceAdded)
            GetComponent<Rigidbody>().AddForce(0, 9.81f, 0);
    }
}
