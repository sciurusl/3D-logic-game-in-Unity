using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Manages Button's functionality (opening doors, operating lifts, destroying barriers)
/// </summary>
public class ButtonFunc : MonoBehaviour
{
    public bool clicked;
    public Transform crystal;
    public Transform crystal2;
    [FormerlySerializedAs("door")]
    public Transform controlledObj;
    [FormerlySerializedAs("door2")]
    public Transform controlledObj2;

    public bool secondControlledObj = false;
    private Transform cameraMain;
    [FormerlySerializedAs("doorIsClosed")]
    public bool controlledObjIsClosed;
    private float distance = 7f;
    public Transform mirroringButton;
    AudioSource[] audios;
    void Start()
    {
        if (controlledObj2 != null)
        {
            secondControlledObj = true;
            controlledObj2.GetComponent<ObjectOperation>().StartFunctionality(transform);
        }
        audios = transform.parent.GetComponentsInChildren<AudioSource>();
        cameraMain = Camera.main.transform;
    }

    /// <summary>
    /// Checks if the main camera is close enough to reach the button (when clicked)
    /// If it is, based on if already clicked, it starts Button animation and operates the controlled object
    /// </summary>
    void OnMouseDown()
    {
        Debug.Log("clicked on button");
        Debug.Log(Vector3.Distance(transform.position, cameraMain.position));
        if (GetComponent<Button>().closingAnimStarted || GetComponent<Button>().openingAnimStarted)
            return;
        if (Vector3.Distance(transform.position, cameraMain.position) < distance)
        {
            Debug.Log("got here");
            clicked = !clicked;
            if (mirroringButton != null)
                mirroringButton.GetComponent<ButtonFunc>().Click();
            if (clicked)
            {
                if (controlledObj != null)
                {
                    if (controlledObj.GetComponent<ObjectOperation>() != null)
                        controlledObj.GetComponent<ObjectOperation>().StartFunctionality(transform);
                    if (controlledObj.GetComponent<TeleporterOpening>() != null)
                    {
                        StartCoroutine(CloseAfterTeleporter());
                    }
                }
                if (crystal != null)
                    crystal.GetComponent<BloomOnOff>().TurnOn();
                if (audios.Length >= 2)
                    audios[1].Play();
                GetComponent<Button>().StartAnim();
                if (secondControlledObj)
                {
                    controlledObj2.GetComponent<ObjectOperation>().EndFunctionality(transform);
                    crystal2.GetComponent<BloomOnOff>().TurnOff();
                }
            }
            else
            {

                Debug.Log("Closing door");
                if (controlledObj != null)
                {
                    if (controlledObj.GetComponent<ObjectOperation>() != null)
                        controlledObj.GetComponent<ObjectOperation>().EndFunctionality(transform);
                }
                if (crystal != null)
                    crystal.GetComponent<BloomOnOff>().TurnOff();
                if (audios.Length >= 1)
                    audios[0].Play();
                GetComponent<Button>().EndAnim();
                if (secondControlledObj)
                {
                    controlledObj2.GetComponent<ObjectOperation>().StartFunctionality(transform);
                    crystal2.GetComponent<BloomOnOff>().TurnOn();
                }

            }
            controlledObjIsClosed = !controlledObjIsClosed;
        }
    }

    /// <summary>
    /// Almost the same functionality as OnMouseDown, but when called, the player is already close enough
    /// It is called by its mirroring button
    /// </summary>
    public void Click()
    {
        if (GetComponent<Button>().closingAnimStarted || GetComponent<Button>().openingAnimStarted)
            return;
        clicked = !clicked;
        if (clicked)
        {
            if (controlledObj != null)
            {
                if (controlledObj.GetComponent<ObjectOperation>() != null)
                    controlledObj.GetComponent<ObjectOperation>().StartFunctionality(transform);
                if (controlledObj.GetComponent<TeleporterOpening>() != null)
                {
                    StartCoroutine(CloseAfterTeleporter());
                }
            }
            if (crystal != null)
                crystal.GetComponent<BloomOnOff>().TurnOn();
            if (audios.Length >= 2)
                audios[1].Play();
            GetComponent<Button>().StartAnim();
            if (secondControlledObj)
            {
                controlledObj2.GetComponent<ObjectOperation>().EndFunctionality(transform);
                crystal2.GetComponent<BloomOnOff>().TurnOff();
            }
        }
        else
        {

            Debug.Log("Closing door");
            if (controlledObj != null)
            {
                if (controlledObj.GetComponent<ObjectOperation>() != null)
                    controlledObj.GetComponent<ObjectOperation>().EndFunctionality(transform);
            }
            if (crystal != null)
                crystal.GetComponent<BloomOnOff>().TurnOff();
            GetComponent<Button>().EndAnim();
            if (audios.Length >= 1)
                audios[0].Play();
            if (secondControlledObj)
            {
                controlledObj2.GetComponent<OpeningClosingObj>().StartFunctionality(transform);
                crystal2.GetComponent<BloomOnOff>().TurnOn();
            }

        }
        controlledObjIsClosed = !controlledObjIsClosed;
    }

    /// <summary>
    /// The button gets clicked again after 1.5 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator CloseAfterTeleporter()
    {
        yield return new WaitForSeconds(1.5f);
        if (clicked)
            Click();
    }
}
