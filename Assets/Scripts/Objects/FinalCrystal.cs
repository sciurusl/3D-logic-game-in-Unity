using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deactivates itself when its collider gets triggers and activates specified Final crystal in the Middle Room
/// It also activates specified pressure plate in the Middle Room
/// </summary>
public class FinalCrystal : MonoBehaviour
{
    public Transform crystal;
    public Transform pressurePlateCrystal;
    public Transform pressurePlateCollider;
    public int num;
    public Transform finalPortal;

    /// <summary>
    /// When triggered, number of currently collected final crystals increases
    /// Deactivates itself and activates specified crystal in the Middle Room
    /// When number of collected crystals equals to number of all final crystals, the final portal barrier gets activated
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        crystal.gameObject.SetActive(true);
        gameObject.SetActive(false);

        foreach (OpenMainDoor plate in FindObjectsOfType<OpenMainDoor>())
        {
            if (!plate.GetComponent<Collider>().enabled)
                continue;
            plate.transform.GetComponentInChildren<BloomOnOff>().TurnOn();
            
            plate.transform.GetComponent<BoxCollider>().enabled = false;
            plate.controlledObj.GetComponent<MainDoorOpening>().Close();
        }

        pressurePlateCollider.GetComponent<Collider>().enabled = true;
        pressurePlateCrystal.GetComponent<BloomOnOff>().TurnOff();

        GameManager.instance.curNumOfCrystals++;
        if(GameManager.instance.curNumOfCrystals == GameManager.instance.numOfCrystals)
        {
            finalPortal.gameObject.SetActive(true);
        }
    }

    public void SetTrigger()
    {
        crystal.gameObject.SetActive(true);
        gameObject.SetActive(false);
        pressurePlateCollider.GetComponent<Collider>().enabled = true;
        pressurePlateCrystal.GetComponent<BloomOnOff>().TurnOff();
    }
}
