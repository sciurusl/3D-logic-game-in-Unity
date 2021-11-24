using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates rigidbodies, lights, pressure plates, mirroring objects... before and after changing of worlds
/// </summary>
public class GravityObjects : MonoBehaviour
{
    List<Rigidbody> rigidbodyChildren;
    Component[] mirroringObjects;
    Component[] pressurePlates;
    public GameObject[] parallelLights;
    public List<Transform> nonMirroringObejcts = new List<Transform>();
    List<Rigidbody> notUpdatedList = new List<Rigidbody>();
    List<Rigidbody> toRemove = new List<Rigidbody>();
    List<OpeningClosingObj> doorList = new List<OpeningClosingObj>();

    public bool upWorld;

    /// <summary>
    /// Fills arrays and lists of objects
    /// </summary>
    public void StartAgain()
    {
        rigidbodyChildren = new List<Rigidbody>(transform.GetComponentsInChildren<Rigidbody>());

        foreach (Rigidbody rigidbody in rigidbodyChildren)
        {
            if (rigidbody.CompareTag("NotUpdatedRigidbody"))
            {
                notUpdatedList.Add(rigidbody);
            }

            if (rigidbody.CompareTag("Platform") || rigidbody.CompareTag("Lift"))
            {
                toRemove.Add(rigidbody);
            }
        }
        foreach (Rigidbody rigidbody in toRemove)
        {
            rigidbodyChildren.Remove(rigidbody);
        }
        mirroringObjects = transform.GetComponentsInChildren<MirroringObjectInfo>();
        pressurePlates = transform.GetComponentsInChildren<PressurePlate>();
        FindObjectwithTag("NonMirroringObject");
        parallelLights = GameObject.FindGameObjectsWithTag("ParallelLight");

        if (!GameManager.instance.inParallelWorld)
        {
            Debug.Log("Setting lights");
            foreach (GameObject light in parallelLights)
            {
                light.SetActive(false);
            }
        }

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
        {
            if (door.GetComponentInChildren<OpeningClosingObj>() != null)
                doorList.Add(door.GetComponentInChildren<OpeningClosingObj>());
        }

        if (transform.CompareTag("ParallelWorld"))
            upWorld = false;
        else
            upWorld = true;

        if ((!GameManager.instance.inParallelWorld && !upWorld) || (GameManager.instance.inParallelWorld && upWorld))
        {
            for (int i = 0; i < rigidbodyChildren.Count; i++)
            {
                rigidbodyChildren[i].GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    /// <summary>
    /// sets isKinematic property of rigidbodies based on the val parameter
    /// </summary>
    /// <param name="val"></param>
    public void SetRigidbody(bool val)
    {
        for (int i = 0; i < rigidbodyChildren.Count; i++)
        {
            rigidbodyChildren[i].GetComponent<Rigidbody>().isKinematic = val;
        }
    }

    /// <summary>
    /// Updates isKinematic property and gravity property of rigidbodies based on the rotationStarted parameter 
    /// and whether the objects are in the up world or in the parallel world
    /// </summary>
    /// <param name="rotationStarted"></param>
    public void UpdateRigidbody(bool rotationStarted)
    {
        if (rotationStarted)
        {
            for (int i = 0; i < rigidbodyChildren.Count; i++)
            {
                rigidbodyChildren[i].GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else
        {
            if(!upWorld && !GameManager.instance.inParallelWorld)
            {
                Debug.Log("doing this");
                for (int i = 0; i < rigidbodyChildren.Count; i++)
                {
                    rigidbodyChildren[i].GetComponent<Rigidbody>().isKinematic = false;
                }
                foreach(Transform crate in nonMirroringObejcts)
                {
                    crate.GetComponent<Rigidbody>().useGravity = true;
                }
                foreach (Rigidbody notUpdated in notUpdatedList)
                {
                    if (notUpdated.transform.GetComponent<OpeningClosingObj>() != null)
                    {
                        if(notUpdated.transform.GetComponent<OpeningClosingObj>().curNumOfEl == notUpdated.transform.GetComponent<OpeningClosingObj>().numOfElements)
                        {
                            notUpdated.isKinematic = true;
                        }
                        else
                        {
                            notUpdated.isKinematic = false;
                        }
                    }
                    else
                    {
                        notUpdated.isKinematic = true;
                    }
                    notUpdated.useGravity = true;

                }
            }
            else if(!upWorld && GameManager.instance.inParallelWorld)
            {
                foreach (Transform crate in nonMirroringObejcts)
                {
                    crate.GetComponent<Rigidbody>().useGravity = false;
                    crate.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
            else if(upWorld && GameManager.instance.inParallelWorld)
            {
                Debug.Log("updating rigidbody from upWorld");
                for (int i = 0; i < rigidbodyChildren.Count; i++)
                {
                    rigidbodyChildren[i].GetComponent<Rigidbody>().useGravity = true;
                    rigidbodyChildren[i].GetComponent<Rigidbody>().isKinematic = true;
                }
                foreach (Rigidbody notUpdated in notUpdatedList)
                {
                    if (notUpdated.transform.GetComponent<OpeningClosingObj>() != null)
                    {
                        if (notUpdated.transform.GetComponent<OpeningClosingObj>().curNumOfEl == notUpdated.transform.GetComponent<OpeningClosingObj>().numOfElements)
                        {
                            notUpdated.isKinematic = true;
                        }
                        else
                        {
                            notUpdated.isKinematic = false;
                        }
                        notUpdated.useGravity = true;
                    }
                    else
                    {
                        notUpdated.isKinematic = true;
                    }

                }
                foreach (var child in nonMirroringObejcts)
                {
                    child.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
            else if(upWorld && !GameManager.instance.inParallelWorld)
            {
                for (int i = 0; i < rigidbodyChildren.Count; i++)
                {                   
                    rigidbodyChildren[i].GetComponent<Rigidbody>().useGravity = false;
                    rigidbodyChildren[i].GetComponent<Rigidbody>().isKinematic = false;
                }
                foreach (Rigidbody notUpdated in notUpdatedList)
                {
                    if (notUpdated.transform.GetComponent<OpeningClosingObj>() != null)
                    {
                        if (notUpdated.transform.GetComponent<OpeningClosingObj>().curNumOfEl == notUpdated.transform.GetComponent<OpeningClosingObj>().numOfElements)
                        {
                            notUpdated.isKinematic = true;
                        }
                        else
                        {
                            notUpdated.isKinematic = false;
                        }
                    }
                    else
                    {
                        notUpdated.isKinematic = true;
                    }

                }
            }
        }
        
    }

    /// <summary>
    /// disables/enables DoorOpening class of every door based on the rotationStarted parameter
    /// </summary>
    /// <param name="rotationStarted"></param>
    public void UpdateDoor(bool rotationStarted)
    {
        if (rotationStarted) {
            foreach (OpeningClosingObj door in doorList)
            {
                door.enabled = false;
            }
        }
        else
        {
            foreach (OpeningClosingObj door in doorList)
            {
                door.enabled = true;
            }
        }
    }

    /// <summary>
    /// Activates\deactivates parallel world lights based on whether the player is in the parallel world or in the up world
    /// </summary>
    public void UpdateLights()
    {
        if (GameManager.instance.inParallelWorld)
        {
            foreach(GameObject light in parallelLights)
            {
                light.SetActive(false);
            }
        }
        else
        {
            Debug.Log("actinvating lights");
            foreach (GameObject light in parallelLights)
            {
                light.SetActive(true);
            }
        }
    }

    public void UpdateLights(bool var)
    {
            foreach (GameObject light in parallelLights)
            {
                light.SetActive(var);
            }      
    }

    /// <summary>
    /// Enables\disables pressure plates
    /// </summary>
    public void UpdatePressurePlates()
    {
        for (int i = 0; i < pressurePlates.Length; i++)
        {
            pressurePlates[i].GetComponent<BoxCollider>().enabled = !pressurePlates[i].GetComponent<BoxCollider>().enabled;
            pressurePlates[i].GetComponent<PressurePlate>().enabled = !pressurePlates[i].GetComponent<PressurePlate>().enabled;
        }
    }

    /// <summary>
    /// Enables\disables mirroring objects
    /// </summary>
    public void UpdateMirroringObejcts()
    {      
        for (int i = 0; i <mirroringObjects.Length; i++)
        {
            mirroringObjects[i].GetComponent<MirroringObjectInfo>().enabled = !mirroringObjects[i].GetComponent<MirroringObjectInfo>().enabled;
        }
    }

    /// <summary>
    ///  Finds children with specified tag
    /// </summary>
    /// <param name="_tag"></param>
    public void FindObjectwithTag(string _tag)
    {
        Transform parent = transform;
        GetChildObject(parent, _tag);
    }

    /// <summary>
    /// Finds all children with specified tag and adds them into nonMirroringObjects list
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="_tag"></param>
    public void GetChildObject(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                nonMirroringObejcts.Add(child.gameObject.transform);
            }
            if (child.childCount > 0)
            {
                GetChildObject(child, _tag);
            }
        }
    }

    /// <summary>
    /// Updates rigidbody list
    /// </summary>
    public void UpdateMyRigidbodyList()
    {
        rigidbodyChildren = new List<Rigidbody>(transform.GetComponentsInChildren<Rigidbody>());

        foreach (Rigidbody rigidbody in rigidbodyChildren)
        {
            if (rigidbody.CompareTag("NotUpdatedRigidbody"))
            {
                notUpdatedList.Add(rigidbody);
            }
        }
        nonMirroringObejcts = new List<Transform>();
        FindObjectwithTag("NonMirroringObject");
    }

}
