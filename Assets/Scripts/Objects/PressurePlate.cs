using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Changes its position's y coordinate up or down of the plate if something steps on the pressure plate or otherwise.
/// </summary>
public class PressurePlate : MonoBehaviour
{
    private float slideHeight = 0.1f;
    private Vector3 normalPosition;
    private bool openingAnimStarted;
    private bool closingAnimStarted;
    BloomOnOff[] bloomOnOffs;
    public int weight;
    AudioSource audioSource;
    public List<Transform> lastTriggerObject = new List<Transform>();
    void Start()
    {
        normalPosition = transform.localPosition;
        bloomOnOffs = GetComponentsInChildren<BloomOnOff>();
        audioSource = transform.parent.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Checks if 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (lastTriggerObject.Count > 0)
        {
            if (lastTriggerObject.Contains(other.transform))
                return;
            lastTriggerObject.Add(other.transform);
            return;
        }
        if (!lastTriggerObject.Contains(other.transform))
            lastTriggerObject.Add(other.transform);
        closingAnimStarted = false;
        Vector3 newPos = new Vector3(transform.localPosition.x, normalPosition.y - slideHeight, transform.localPosition.z);
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }
        openingAnimStarted = true;
        StartCoroutine(SmoothlyUpdatePosition(newPos));     
    }

    private void OnTriggerExit(Collider other)
    {

        if (lastTriggerObject.Contains(other.transform))
        {
            lastTriggerObject.Remove(other.transform);
        }
        if (lastTriggerObject.Count > 0)
            return;
        openingAnimStarted = false;
        closingAnimStarted = true;
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }
        StartCoroutine(SmoothlyClosePosition(normalPosition));
       
    }
    IEnumerator SmoothlyUpdatePosition(Vector3 newPos)
    {
        if (bloomOnOffs != null)
        {
            foreach (BloomOnOff bloomOnOff in bloomOnOffs)
            {
                bloomOnOff.TurnOn();
            }
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

    IEnumerator SmoothlyClosePosition(Vector3 newPos)
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
    public void StartTrigger()
    {
        closingAnimStarted = false;
        Vector3 newPos = new Vector3(transform.localPosition.x, normalPosition.y - slideHeight, transform.localPosition.z);
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }
        openingAnimStarted = true;
        StartCoroutine(SmoothlyUpdatePosition(newPos));

    }

    public void EndTrigger()
    {
        openingAnimStarted = false;
        closingAnimStarted = true;
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }
        StartCoroutine(SmoothlyClosePosition(normalPosition));
    }

}
