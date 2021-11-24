using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calls the Main door in the Middle Room to opens/closes itself 
/// </summary>
public class OpenMainDoor : PressurePlateFunc
{
    private Transform lastTriggerObject;

   /* private new void Start()
    {
        if (controlledObj2)
            controlledObj2.GetComponent<MainDoorOpening>().GoUp(transform);
    }*/

    

    /// <summary>
    /// When triggered, it calls Game Manager to update its active level according to the name of the pressure plate
    /// It also calls the main door to open itself
    /// </summary>
    /// <param name="other"></param>
    public override void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        Debug.Log("Opening door");
        controlledObj.GetComponent<MainDoorOpening>().GoUp(transform);
        if (crystal != null)
            crystal.GetComponent<BloomOnOff>().TurnOn();
        lastTriggerObject = other.transform;
        /*if (controlledObj2 && !door2Closed)
        {
            door2Closed = true;
            controlledObj2.GetComponent<MainDoorOpening>().GoDown(transform);
        }*/
        if (transform.parent.name.Contains("1"))
            GameManager.instance.ChangeLevel(false, true, false, false);
        else if(transform.parent.name.Contains("2"))
            GameManager.instance.ChangeLevel(false, false, true, false);
        else if(transform.parent.name.Contains("3"))
            GameManager.instance.ChangeLevel(false, false, false, true);
        
    }

    /// <summary>
    /// Calls the main door to close itself
    /// </summary>
    /// <param name="other"></param>
    public override void OnTriggerExit(Collider other)
    {
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        
        if (other.transform.Equals(lastTriggerObject))
        {
            Debug.Log("Closing door");
            controlledObj.GetComponent<MainDoorOpening>().GoDown(transform);
            if (crystal != null)
                crystal.GetComponent<BloomOnOff>().TurnOff();
        }


    }
}
