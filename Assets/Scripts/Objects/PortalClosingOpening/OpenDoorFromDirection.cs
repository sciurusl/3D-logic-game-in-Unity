using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorFromDirection : PressurePlateFunc
{
    public bool inside;
    public override void OnTriggerEnter(Collider other)
    {           
        if (!other.transform.CompareTag("player"))
            return;
        if (!inside)
        {
            return;
        }
        controlledObj.GetComponent<OpeningClosingObj>().GoDown(null);
        
    }
    public override void OnTriggerExit(Collider other)
    {
        if (!other.transform.CompareTag("player"))
            return;
        Debug.Log("been triggered " + name);
    }

    public void OpenDoor()
    {
            controlledObj.GetComponent<OpeningClosingObj>().GoUp(null);
    }
}
