using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages closing and opening of the object when the player uses the portal to change worlds
/// </summary>
public class OpeningClosingObjWhenTeleported : OpeningClosingObj
{
   
    override public void GoUp(Transform triggerObject)
    {
        
        if (moved)
            return;
            closingAnimStarted = false;
            openingAnimStarted = true;
        StartCoroutine(SmoothlyOpenDoor());      
    }

    IEnumerator SmoothlyOpenDoor()
    {

        float elapsedTime = 0f;
        float waitTime = 1f;
        Vector3 curPos = transform.localPosition;


        while (elapsedTime < waitTime && openingAnimStarted)
        {
            transform.localPosition = Vector3.Lerp(curPos, new Vector3(curPos.x, normalPosition.y + height/*/(curNumOfEl/numOfElements)*/, curPos.z), (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        openingAnimStarted = false;
        if (transform.localPosition.y >= normalPosition.y + height)
            moved = true;
    }

    override public void GoDown(Transform triggerObject)
    {
      
        openingAnimStarted = false;
        closingAnimStarted = true;
       
        StartCoroutine(SmoothlyCloseDoor());

    }
    IEnumerator SmoothlyCloseDoor()
    {

        float elapsedTime = 0f;
        float waitTime = 1f;
        Vector3 curPos = transform.localPosition;


        while (elapsedTime < waitTime && closingAnimStarted)
        {
            transform.localPosition = Vector3.Lerp(curPos, new Vector3(curPos.x, normalPosition.y, curPos.z), (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        moved = false;
        closingAnimStarted = false;
    }
}
