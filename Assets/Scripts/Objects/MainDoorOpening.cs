using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

/// <summary>
/// Rotates the main door object and all its components around specified object for 90 degrees
/// </summary>
public class MainDoorOpening : OpeningClosingObj
{
    public float doorHeight = 6f;
    public Vector3 rotatingVector = new Vector3(0, 1, 0);
    public int rotAnglePerSecond = 60;
    public float rotAngle = 90;
    public Transform rotatingObject;
    public bool tutorialDoor = false;

    public List<Transform> children = new List<Transform>();
    public new void Start()
    {
        normalPosition = transform.localPosition;
        if (GetComponent<Rigidbody>() != null)
            isRigidbody = true;
        else
            isRigidbody = false;
        children.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }
    }

/// <summary>
/// If not already opened and the triggerObject has not already affected the object, 
/// it checks if number of currently controlling objects is equal to number of every object controlling this object
/// If it is, it calls a method to open the door
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
        curNumOfEl++;
        triggerObjects.Add(triggerObject);

        if (curNumOfEl == numOfElements)
        {
            closingAnimStarted = false;
            openingAnimStarted = true;
            StartCoroutine(SmoothlyGoUp());
        }

    }


    public override IEnumerator SmoothlyGoUp()
    {
        float time = Time.time;
        float elapsedTime = 0f;

        while (Time.time - time <= rotAngle / rotAnglePerSecond && openingAnimStarted)
        {
            foreach (Transform child in children)
            {
                child.transform.RotateAround(rotatingObject.position, rotatingVector, rotAnglePerSecond * Time.deltaTime);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        openingAnimStarted = false;
        if (children[1].rotation.z == 90f)
            moved = true;
    }

    public override void GoDown(Transform triggerObject)
    {
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (triggerObject != null)
            triggerObjects.Remove(triggerObject);
        Debug.Log("main door closing");
        curNumOfEl--;
        openingAnimStarted = false;
        closingAnimStarted = true;

        StartCoroutine(SmoothlyGoDown());

    }

    public override IEnumerator SmoothlyGoDown()
    { 

        float elapsedTime = 0f;

        float tempRotAngle = children[0].transform.localEulerAngles.z;
        while (elapsedTime < tempRotAngle / rotAnglePerSecond && closingAnimStarted)
        {
            foreach (Transform child in children)
            {

                child.transform.RotateAround(rotatingObject.position, -rotatingVector, rotAnglePerSecond * Time.deltaTime);

            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        foreach (Transform child in children)
        {
            child.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        moved = false;
        closingAnimStarted = false;
    }

    public void Open()
    {
        if (children == null || !GameManager.instance.inParallelWorld)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }
        }
        Debug.Log("main door opening");
        Vector3 rotatingVectorTmp = new Vector3(0, 1, 0);
        if (GameManager.instance.inParallelWorld)
            rotatingVectorTmp = new Vector3(0, -1, 0);
 
        foreach (Transform child in children)
        {
            child.transform.RotateAround(rotatingObject.position, rotatingVectorTmp, rotAngle);
        }
        children.Clear();
    }

    public void Close()
    {
        foreach (Transform child in children)
        {

            child.transform.RotateAround(rotatingObject.position, -rotatingVector, rotAngle);

        }
        foreach (Transform child in children)
        {
            child.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        moved = false;
    }
}
