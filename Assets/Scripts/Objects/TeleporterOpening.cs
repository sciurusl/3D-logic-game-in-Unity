using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the animation of the teleporter opening and closing
/// </summary>
public class TeleporterOpening : ObjectOperation
{
    private float openingOffset = 2f;

    private int childCount = 4;

    private List<Transform> children = new List<Transform>();
    private List<Vector3> childrenNormalPosition = new List<Vector3>();
    private List<Vector3> axis = new List<Vector3>();

    public bool opening = false;

    private Collider[] colliders; 


    private float numOfClosing = 0.3f;
    private float numOfOpening = 0.5f;


    private void Start()
    {
        colliders = transform.Find("teleporter").transform.Find("Cube.009").GetComponents<Collider>();
        moved = false;
        for (int i = 1; i < childCount + 1; i++)
        {
            children.Add(transform.GetChild(i));
            childrenNormalPosition.Add(transform.GetChild(i).localPosition);
        }

        axis.Add(new Vector3(-1, 0, 1));
        axis.Add(new Vector3(1, 0, 1));
        axis.Add(new Vector3(-1, 0, -1));
        axis.Add(new Vector3(1, 0, -1));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            Open();
        if (Input.GetKeyDown(KeyCode.C))
            Close();
    }

    public override void StartFunctionality(Transform triggerObject)
    {
        Open();
    }

    public override void EndFunctionality(Transform triggerObject)
    {
        Close();
    }


    public void Open()
    {
        if (moved)
            return;
        closingAnimStarted = false;
        openingAnimStarted = true;

        StartCoroutine(SmoothlyOpen());

    }

    /// <summary>
    /// Smoothly updates the position of its four children to move them along different axis
    /// </summary>
    /// <returns></returns>
    IEnumerator SmoothlyOpen()
    {
        StartCoroutine(ChangeOpening(true, numOfOpening));
        float elapsedTime = 0f;
        float waitTime = 1f;
        List<Vector3> curPoses = new List<Vector3>();
        foreach (Transform child in children)
            curPoses.Add(child.localPosition);


        while (elapsedTime < waitTime && openingAnimStarted)
        {
            int i = 0;
            foreach (Transform child in children)
            {
                child.localPosition = Vector3.Lerp(curPoses[i], childrenNormalPosition[i] + axis[i] * openingOffset, (elapsedTime / waitTime));
                i++;
            }


            elapsedTime += Time.deltaTime;
            yield return null;
        }
        openingAnimStarted = false;
        if (children[0].localPosition == childrenNormalPosition[0] + axis[0] * openingOffset)
            moved = true;
    }

    public void Close()
    {
        openingAnimStarted = false;
        closingAnimStarted = true;
        StartCoroutine(SmoothlyClose());

    }

    /// <summary>
    /// Smoothly updates the position of its four children to move them along different axis to close the teleporter
    /// </summary>
    /// <returns></returns>
    IEnumerator SmoothlyClose()
    {
        StartCoroutine(ChangeOpening(false, numOfClosing));
        float elapsedTime = 0f;
        float waitTime = 1f;
        List<Vector3> curPoses = new List<Vector3>();
        foreach (Transform child in children)
            curPoses.Add(child.localPosition);


        while (elapsedTime < waitTime && closingAnimStarted)
        {
            int i = 0;
            foreach (Transform child in children)
            {
                child.localPosition = Vector3.Lerp(curPoses[i], childrenNormalPosition[i], (elapsedTime / waitTime));
                i++;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        moved = false;
        closed = true;
        closingAnimStarted = false;
    }

    IEnumerator ChangeOpening(bool val, float num)
    {
        yield return new WaitForSeconds(num);
        GetComponent<BoxCollider>().isTrigger = val;
        opening = val;

        colliders[1].enabled = !val;
        colliders[0].enabled = val;

    }
}
