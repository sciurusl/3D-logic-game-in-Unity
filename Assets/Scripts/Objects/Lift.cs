using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages lift functionality
/// </summary>
public class Lift : OpeningClosingObj
{
    public float scaleHeight = 6.808176f;
    public float yPosition;

    private Vector3 normalScale;
    public float waitTimeVar = 1f;

    public new void Start()
    {

        normalScale = transform.localScale;
        normalPosition = transform.localPosition;//new Vector3(transform.localPosition.x, yPosition, transform.localPosition.z);
        if (GetComponent<Rigidbody>() != null)
        {
            isRigidbody = true;
            GetComponent<Rigidbody>().useGravity = false;
        }
        else
            isRigidbody = false;
    }

    /// <summary>
    /// When number of currently controlling elements equals to the number of all elements controlling this object,
    /// it calls the SmoothlyGoUp method
    /// It also checks if the controlling object has not already affected this object
    /// </summary>
    /// <param name="triggerObject"></param>
    public override void GoUp(Transform triggerObject)
    {
        if (moved)
            return;
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (triggerObjects.Contains(triggerObject))
            return;
        Debug.Log("getting up");
        curNumOfEl++;
        triggerObjects.Add(triggerObject);
        if (isRigidbody)
            GetComponent<Rigidbody>().isKinematic = true;
        if (curNumOfEl == numOfElements)
        {
            closingAnimStarted = false;
            openingAnimStarted = true;
            StartCoroutine(SmoothlyGoUp());
        }
        else
        {

            StartCoroutine(PartlyGoUpAndGoDown());
        }

    }

    /// <summary>
    /// Smoothly increases scale in the Y coordinate and changes its position to create the effect of growing
    /// </summary>
    /// <returns></returns>
    public override IEnumerator SmoothlyGoUp()
    {

        float elapsedTime = 0f;
        float waitTime = waitTimeVar;
        Vector3 curPos = transform.localPosition;
        Vector3 curScale = transform.localScale;

        while (elapsedTime < waitTime && openingAnimStarted)
        {
            transform.localScale = Vector3.Lerp(curScale, new Vector3(curScale.x, normalScale.y+scaleHeight, curScale.z), (elapsedTime / waitTime));
            transform.localPosition = Vector3.Lerp(curPos, new Vector3(curPos.x, normalPosition.y + height, curPos.z), (elapsedTime / waitTime));
           
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        openingAnimStarted = false;
        if (transform.localPosition == new Vector3(curPos.x, normalPosition.y + height, curPos.z))
            moved = true;
    }

    /// <summary>
    /// When called, it removes the controlling object of the list of currently controlling objects
    /// It calls the SmoothlyGoDown() method to smoothly shrink itself
    /// </summary>
    /// <param name="triggerObject"></param>
    public override void GoDown(Transform triggerObject)
    {
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (triggerObject != null)
            triggerObjects.Remove(triggerObject);
        curNumOfEl--;
        openingAnimStarted = false;
        closingAnimStarted = true;
        
        StartCoroutine(SmoothlyGoDown());

    }
    
    /// <summary>
    /// Smoothly decreases scale in the Y coordinate and changes its position to create the shrinking effect
    /// </summary>
    public override IEnumerator SmoothlyGoDown()
    {

        float elapsedTime = 0f;
        float waitTime = waitTimeVar;
        Vector3 curPos = transform.localPosition;
        Vector3 curScale = transform.localScale;


        while (elapsedTime < waitTime && closingAnimStarted)
        {
            transform.localScale = Vector3.Lerp(curScale, new Vector3(curScale.x, normalScale.y, curScale.z), (elapsedTime / waitTime));
            transform.localPosition = Vector3.Lerp(curPos, new Vector3(curPos.x, normalPosition.y, curPos.z), (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
        transform.localPosition = normalPosition;
        moved = false;
        closingAnimStarted = false;
    }

    /// <summary>
    /// Shrinking with the help of Rigidbody
    /// </summary>
    /// <returns></returns>
    public override IEnumerator GoDownUsingRigidbody()
    {
        GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(2f);

        moved = false;
        closingAnimStarted = false;
    }
}
