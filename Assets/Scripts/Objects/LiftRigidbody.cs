using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftRigidbody : MonoBehaviour
{
    private bool forceAdded = false;

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(GoDown());
    }

    /// <summary>
    /// If the object is in the same world as the player, it can use the Rigidbody component to shrink itself
    /// Otherwise, it closes using the SmoothlyGoDown method of the Lift class
    /// </summary>
    /// <returns></returns>
    public IEnumerator GoDown()
    {
        if (transform.parent.parent.CompareTag("ParallelWorld") && !GameManager.instance.inParallelWorld || !transform.parent.parent.CompareTag("ParallelWorld") && GameManager.instance.inParallelWorld)
        {
            Debug.Log("closing here");
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = false;
            forceAdded = true;
            yield return new WaitForSeconds(1);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = true;
            forceAdded = false;
        }
        else
        {
            Debug.Log("closing using doorOpening");
            GetComponent<Lift>().openingAnimStarted = false;
            GetComponent<Lift>().closingAnimStarted = true;
            GetComponent<Lift>().StartCoroutine(GetComponent<Lift>().SmoothlyGoDown());
        }
    }

    private void FixedUpdate()
    {
        if (forceAdded)
            GetComponent<Rigidbody>().AddForce(0, 9.81f, 0);
    }
}
