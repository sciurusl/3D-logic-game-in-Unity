using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// It manages sliding platform's functionality
/// </summary>
public class SlidingPlatform : ObjectOperation
{

    public float moveLength = 6f;
    public Vector3 normalPosition;
    public ArrayList triggerObjects = new ArrayList();
    public List<Transform> onObjects = new List<Transform>();

    public float waitTimeVar = 5f;
    public int axis;
    private Vector3 pushDir;
    public float pushPower = 2.0f;

   
    public void Start()
    {
        waitTimeVar = moveLength / 3;
        if (!moved)
            normalPosition = transform.localPosition;
        else
            normalPosition = new Vector3(transform.localPosition.x , transform.localPosition.y, transform.localPosition.z - moveLength);

        FreezeRigidbody();     
    }

    public override void StartFunctionality(Transform triggerObject)
    {
        SlidePlatform(triggerObject);
    }

    public override void EndFunctionality(Transform triggerObject)
    {
        ReturnPlatform(triggerObject);
    }

    /// <summary>
    /// If not already moved and the current number of controlling objects is equal to number of all controlling objects,
    /// it moves using its rigidbody component
    /// </summary>
    /// <param name="triggerObject"></param>
    public virtual void SlidePlatform(Transform triggerObject)
    {

        if (moved)
            return;
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (triggerObjects.Contains(triggerObject))
        {
            return;
        }

        curNumOfEl++;
        triggerObjects.Add(triggerObject);

        if (curNumOfEl == numOfElements)
        {
            StopAllCoroutines();
            Debug.Log("moving paltfoooorm " + name);
            closingAnimStarted = false;
            openingAnimStarted = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().velocity = pushDir * pushPower;
            GetComponent<Rigidbody>().AddForce(pushDir * pushPower);
            StartCoroutine(StopMoving());
        }
        else
        {

            StartCoroutine(PartlySlidePlatformAndReturn());
        }

    }

    /// <summary>
    /// Moves only slightly and then returns
    /// </summary>
    /// <returns></returns>
    IEnumerator PartlySlidePlatformAndReturn()
    {
        closingAnimStarted = false;
        openingAnimStarted = true;
        StartCoroutine(StopMoving());
        yield return new WaitForSeconds(0.2f);
        if (curNumOfEl == numOfElements)
            yield break;
        openingAnimStarted = false;
        closingAnimStarted = true;

        StopCoroutine(StopMoving());
        StartCoroutine(SmoothlyReturnPlatform());

    }

    /// <summary>
    /// After specified time (waitTimeVar) passed, it stops the game object by setting its velocity to zero
    /// </summary>
    /// <returns></returns>
    IEnumerator StopMoving()
    {

        float elapsedTime = 0f;
        float waitTime = waitTimeVar;


        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    /// <summary>
    /// Returns to default position using the rigidbody component and setting the velocity in the opposite direction
    /// </summary>
    /// <param name="triggerObject"></param>
    public virtual void ReturnPlatform(Transform triggerObject)
    {

        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (triggerObject != null)
            triggerObjects.Remove(triggerObject);

        curNumOfEl--;

        StopAllCoroutines();
       
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = -pushDir * pushPower;
        GetComponent<Rigidbody>().AddForce(-pushDir * pushPower);
        StartCoroutine(StopMoving());

    }
    /// <summary>
    /// Smoothly returns to deafult position
    /// </summary>
    /// <returns></returns>
    IEnumerator SmoothlyReturnPlatform()
    {

        float elapsedTime = 0f;
        float waitTime = waitTimeVar;
        Vector3 curPos = transform.localPosition;


        while (elapsedTime < waitTime && closingAnimStarted)
        {
            transform.localPosition = Vector3.Lerp(curPos, new Vector3(curPos.x, curPos.y, normalPosition.z), (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        moved = false;
        closingAnimStarted = false;
    }

  
    /// <summary>
    /// Sets constraints of the rigidbody according to the specified axis
    /// </summary>
    public void FreezeRigidbody()
    {
        
        if (axis == 0)
        {
            pushDir = new Vector3(-1, 0, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;

        }
        else if (axis == 1)
        {
            pushDir = new Vector3(0, 1, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
        else if (axis == 2)
        {
            pushDir = new Vector3(0, 0, -1);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        }
        GetComponent<Rigidbody>().freezeRotation = true;
    }
}
