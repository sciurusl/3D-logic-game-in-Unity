using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages mirroring pressure plate functionality 
/// </summary>
public class PressurePlateFuncMirror : PressurePlateFunc
{

    /// <summary>
    /// Same functionality as OnTriggerEnter of the PressurePlateFunc class
    /// This plate can be triggered by the player when the other does not get triggered
    /// But when both get triggered by mirroring stone, it would get to loop if both plates had the same functions
    /// </summary>
    /// <param name="other"></param>
    public override void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (transform.parent.parent.parent != other.transform.parent.parent && !other.CompareTag("player"))
            return;
        if (crystal != null)
            crystal.GetComponent<BloomOnOff>().TurnOn();

        foreach (PressurePlateFunc oD in mirroringPlate.GetComponents<PressurePlateFunc>())
        {
            oD.StartTrigger(other);
        }
        if (GetComponent<PressurePlate>() != null)
        {
            Debug.Log("start pressure plate on mirror pressure plate");
            GetComponent<PressurePlate>().StartTrigger();
        }
        triggered = true;
    }

    public override void OnTriggerExit(Collider other)
    {

        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (transform.parent.parent.parent != other.transform.parent.parent && !other.CompareTag("player"))
            return;
        if (crystal != null)
            crystal.GetComponent<BloomOnOff>().TurnOff();
        foreach (PressurePlateFunc oD in mirroringPlate.GetComponents<PressurePlateFunc>())
        {
            oD.EndTrigger(other);
        }
        if (GetComponent<PressurePlate>() != null)
            GetComponent<PressurePlate>().EndTrigger();
        //mirroringPlate.GetComponent<GoUp>().EndTrigger(other);
    }

    
}
