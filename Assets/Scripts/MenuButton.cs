using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Rendering.HybridV2;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages animation of the menu buttons
/// </summary>
public class MenuButton : Button
{

    private void Start()
    {
        
    }

    void Awake()
    {  
        normalPosition = transform.localPosition;
    }

    /// <summary>
    /// Starts the moving animation
    /// </summary>
    override public void StartAnim()
    {
        Debug.Log("Button anim started");
        closingAnimStarted = false;
        Vector3 newPos = new Vector3(transform.localPosition.x, normalPosition.y, transform.localPosition.z) - Vector3.Normalize(Vector3.Cross(transform.up, transform.forward));

        openingAnimStarted = true;
        StartCoroutine(SmoothlyUpdatePosition(newPos));
    }


    /// <summary>
    /// Smoothly updates the button object's position
    /// </summary>
    /// <param name="newPos"></param>
    /// <returns></returns>
    override public IEnumerator SmoothlyUpdatePosition(Vector3 newPos)
    {
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
    override public IEnumerator SmoothlyClosePosition(Vector3 newPos)
    {
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
