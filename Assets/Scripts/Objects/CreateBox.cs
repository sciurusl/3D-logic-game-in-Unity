using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates stone prefab in specific position
/// </summary>
public class CreateBox : ObjectOperation
{
    public Transform box;
    public Vector3 pos;

    public int count = 1;
    public bool myWorld = true;
    public string otherWorld;
    public bool inBothWorlds = true;
    public bool upWorld;

    public void Start()
    {
        if (transform.parent.parent.name == "UpWorld")
            otherWorld = "ParallelWorld";
        else
            otherWorld = "UpWorld";
    }

    public override void StartFunctionality(Transform triggerObject)
    {
        CreateBoxMethod();
    }

    public override void EndFunctionality(Transform triggerObject)
    {
        CreateBoxMethod();
    }

    /// <summary>
    /// Instantiates specific prefab in specified position
    /// </summary>
    public void CreateBoxMethod()
    {
        Debug.Log("got here");
        if (count > 0)
        {
            count--;
            return;
        }
        Debug.Log("creating box");
        Transform createdBox = Instantiate(box, Vector3.one, Quaternion.identity);
        if (myWorld)
        {
            createdBox.SetParent(transform.parent);
            if ((upWorld && GameManager.instance.inParallelWorld) || (!upWorld && !GameManager.instance.inParallelWorld))
                createdBox.GetComponent<Rigidbody>().useGravity = false;
        }
        else
        {
            if ((upWorld && !GameManager.instance.inParallelWorld) || (!upWorld && GameManager.instance.inParallelWorld))
                createdBox.GetComponent<Rigidbody>().useGravity = false;
            createdBox.SetParent(transform.parent.parent.parent.Find(otherWorld).Find(transform.parent.name));
        }
        createdBox.localPosition = pos;
        createdBox.gameObject.AddComponent(typeof(GravityChange));
        StartCoroutine(TurnOfRigidbody(createdBox));
        if (inBothWorlds)
        {
            Transform createdBoxPar = Instantiate(box, Vector3.one, Quaternion.identity);
            if ((upWorld && !GameManager.instance.inParallelWorld) || (!upWorld && GameManager.instance.inParallelWorld))
                createdBoxPar.GetComponent<Rigidbody>().useGravity = false;
            createdBoxPar.gameObject.AddComponent(typeof(GravityChange));
            createdBoxPar.SetParent(transform.parent.parent.parent.Find(otherWorld).Find(transform.parent.name));
            createdBoxPar.localPosition = pos;
            StartCoroutine(TurnOfRigidbody(createdBoxPar));
        }

    }
    /// <summary>
    /// Destroys the Rigidbody component of the obj object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private IEnumerator TurnOfRigidbody(Transform obj)
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(obj.GetComponent<Rigidbody>());
        Destroy(obj.GetComponent<GravityChange>());
    }
}
