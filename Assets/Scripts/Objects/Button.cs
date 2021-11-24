using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Rendering.HybridV2;
using UnityEngine;

/// <summary>
/// Manages animation of the button
/// </summary>
public class Button : MonoBehaviour
{
    public Vector3 normalPosition;
    public bool openingAnimStarted;
    public bool closingAnimStarted;
    List<BloomOnOff> bloomOnOffs;
    void Start()
    {
        normalPosition = transform.localPosition;
        bloomOnOffs = new List<BloomOnOff>(GetComponentsInChildren<BloomOnOff>());
        BloomOnOff[] bOOinSiblings = transform.parent.GetComponentsInChildren<BloomOnOff>();
        foreach(BloomOnOff bloom in bOOinSiblings)
        {
            bloomOnOffs.Add(bloom);
        }
    }

    /// <summary>
    /// Starts the moving animation
    /// </summary>
    public virtual void StartAnim()
    {
        Debug.Log("Button anim started");
        closingAnimStarted = false;
        Vector3 newPos = new Vector3(transform.localPosition.x, normalPosition.y, transform.localPosition.z) + Vector3.Normalize(Vector3.Cross(transform.up, transform.forward))/6f;

        openingAnimStarted = true;
        StartCoroutine(SmoothlyUpdatePosition(newPos));
    }

    /// <summary>
    /// Stops the animation
    /// </summary>
    public void EndAnim()
    {
        openingAnimStarted = false;
        closingAnimStarted = true;
        StartCoroutine(SmoothlyClosePosition(normalPosition));

    }

    /// <summary>
    /// Smoothly updates the button object's position
    /// </summary>
    /// <param name="newPos"></param>
    /// <returns></returns>
    virtual public IEnumerator SmoothlyUpdatePosition(Vector3 newPos)
    {
        foreach (BloomOnOff bloomOnOff in bloomOnOffs)
        {
            bloomOnOff.TurnOn();
        }

        float elapsedTime = 0f;
        float waitTime = 1f;
        Vector3 curPos = transform.localPosition;


        while (elapsedTime < waitTime && openingAnimStarted)
        {
            transform.localPosition = Vector3.Lerp(curPos, newPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        openingAnimStarted = false;
    }


    /// <summary>
    /// Smoothly returns the button object to the specified position (newPos)
    /// </summary>
    /// <param name="newPos"></param>
    /// <returns></returns>
    virtual public IEnumerator SmoothlyClosePosition(Vector3 newPos)
    {
        foreach (BloomOnOff bloomOnOff in bloomOnOffs)
        {
            bloomOnOff.TurnOff();
        }
        float elapsedTime = 0f;
        float waitTime = 1f;
        Vector3 curPos = transform.localPosition;


        while (elapsedTime < waitTime && closingAnimStarted)
        {
            transform.localPosition = Vector3.Lerp(curPos, newPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        closingAnimStarted = false;
    }
}
