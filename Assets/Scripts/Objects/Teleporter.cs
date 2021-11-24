using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the functionality of the Teleporter
/// </summary>
public class Teleporter : MonoBehaviour
{
    public Transform otherWorldRoom;
    private string otherWorldName;
    public void Start()
    {
        if (transform.parent.parent.name.Equals("UpWorld"))
            otherWorldName = "ParallelWorld";
        else
            otherWorldName = "UpWorld";
        otherWorldRoom = transform.parent.parent.parent.Find(otherWorldName).Find(transform.parent.name);
    }

    /// <summary>
    /// When an object exits its collider, it starts the WaitToChangeGravity coroutine 
    /// and passes the transform of the object as parameter
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
            return;
        if (!transform.GetComponent<TeleporterOpening>().opening)
            return;
        StartCoroutine(WaitToChangeGravity(other.transform));
    }

    /// <summary>
    /// Changes the gravity of the object based on if it was passed in the up world or the parallel world 
    /// and whether the player is curently in the up world or in the parallel world
    /// Also calls the parallelWorld rooms' parent and the upWorld rooms' parent to update its list of rigidbody objects
    /// </summary>
    /// <param name="obj"></param>
    private void ChangeGravity(Transform obj)
    {  
       
        Debug.Log("teleporting crate");
        if (obj.GetComponent<GravityChange>().upWorld) {
            obj.SetParent(otherWorldRoom);
            if(GameManager.instance.inParallelWorld)
                obj.GetComponent<Rigidbody>().useGravity = true;
            else
                obj.GetComponent<Rigidbody>().useGravity = false;
        }
        else {
            obj.SetParent(transform.parent);
            if (GameManager.instance.inParallelWorld)
                obj.GetComponent<Rigidbody>().useGravity = false;
            else
                obj.GetComponent<Rigidbody>().useGravity = true;
        }      
        obj.GetComponent<GravityChange>().upWorld = !obj.GetComponent<GravityChange>().upWorld;
        foreach(GravityObjects gravObjsChild in transform.parent.parent.parent.GetComponentsInChildren<GravityObjects>())
        {
            gravObjsChild.UpdateMyRigidbodyList();
        }
    }

    private IEnumerator WaitToChangeGravity(Transform obj)
    {
        yield return new WaitForSeconds(0.2f);
        ChangeGravity(obj);
    }
}
