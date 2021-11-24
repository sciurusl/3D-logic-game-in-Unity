using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OpeningClosingObj : ObjectOperation
{   
    public float height = 6f;
    public Vector3 normalPosition;
    public ArrayList triggerObjects = new ArrayList();
    public bool isRigidbody;
    public AudioSource[] audios;

    public void Start()
    {
        if (!moved)
            normalPosition = transform.localPosition;
        else
            normalPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - height, transform.localPosition.z);
        if (GetComponent<Rigidbody>() != null)
            isRigidbody = true;
        else
            isRigidbody = false;
        if (transform.name.Contains("arrier"))
        {
            audios = transform.GetComponents<AudioSource>();
            return;
        }

        audios = transform.parent.GetComponents<AudioSource>();
        if (audios.Length == 0)
            audios = transform.GetComponents<AudioSource>();
        
    }

    public override void StartFunctionality(Transform triggerObject)
    {
        GoUp(triggerObject);
    }

    public override void EndFunctionality(Transform triggerObject)
    {
        GoDown(triggerObject);
    }

    public override void EndFuncUsingRigidbody()
    {
        StartCoroutine(GoDownUsingRigidbody());
    }

    /// <summary>
    /// If not already opened and the triggerObject has not already affected the object, 
    /// it checks if number of currently controlling objects is equal to number of every object controlling this object
    /// If it is, it calls a method to start the animation
    /// </summary>
    /// <param name="triggerObject"></param>
    public virtual void GoUp(Transform triggerObject)
    {
        
        if (moved)
            return;
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (triggerObjects.Contains(triggerObject))
        {
            return;
        }
        //Debug.Log("opening " + name);
        curNumOfEl++;
        triggerObjects.Add(triggerObject);

        if (audios.Length == 2)
        {
            audios[1].Stop();
            audios[0].Play();
        }
        else if(audios.Length == 1)
        {
            audios[0].Stop();
            audios[0].Play();
        }

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
    /// Starts the opening animation but after 0.2 seconds, it stops the animation and calls the closing animation
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator PartlyGoUpAndGoDown()
    {
        closingAnimStarted = false;
        openingAnimStarted = true;
        StartCoroutine(SmoothlyGoUp());
        yield return new WaitForSeconds(0.2f);
        if (curNumOfEl == numOfElements)
            yield break;
        openingAnimStarted = false;
        closingAnimStarted = true;
        
        StopCoroutine(SmoothlyGoUp());
        StartCoroutine(SmoothlyGoDown());
        
    }

    /// <summary>
    /// Smoothly changes the y coordinate in the transform.position vector to open 
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator SmoothlyGoUp()
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

    /// <summary>
    /// Removes the triggerObject from list of triggered objects. 
    /// Calls a method to start the closing animation
    /// If the object has Rigidbody component assigned to it, it closes using the rigidbody component using the OpeningClosingRigidbody method
    /// </summary>
    /// <param name="triggerObject"></param>
    public virtual void GoDown(Transform triggerObject)
    {
        
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (triggerObject != null)
            triggerObjects.Remove(triggerObject);

        curNumOfEl--;
        openingAnimStarted = false;
        closingAnimStarted = true;

        if (audios.Length == 2)
        {
            audios[0].Stop();
            audios[1].Play();
        }
        else if (audios.Length == 1)
        {
            audios[0].Stop();
            audios[0].Play();
        }

        if (isRigidbody)
        {
            GetComponent<OpeningClosingRigidbody>().StartCoroutine(GetComponent<OpeningClosingRigidbody>().GoDown());

        }
        else
            StartCoroutine(SmoothlyGoDown());

    }

    /// <summary>
    /// Smoothly changes the y coordinate in the transform.position vector to get to the default position
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator SmoothlyGoDown()
    {

        float elapsedTime = 0f;
        float waitTime = 1f;
        Vector3 curPos = transform.localPosition;

        if (curNumOfEl == numOfElements)
        {
            yield break;
        }
        while (elapsedTime < waitTime && closingAnimStarted)
        {
            transform.localPosition = Vector3.Lerp(curPos, new Vector3(curPos.x, normalPosition.y, curPos.z), (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        moved = false;
        closingAnimStarted = false;
    }

    /// <summary>
    /// Closes using the rigidbody component
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator GoDownUsingRigidbody()
    {

        GetComponent<Rigidbody>().isKinematic = false;
        float elapsedTime = 0f;
        float waitTime = 2f;
        while (elapsedTime < waitTime && closingAnimStarted)
        {
            if (transform.localPosition.y < normalPosition.y)
            {
                
                break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        moved = false;
        closingAnimStarted = false;
    }
}
