using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PressurePlateFunc : MonoBehaviour
{
    [FormerlySerializedAs("door")]
    public Transform controlledObj;
    [FormerlySerializedAs("door2")]
    public Transform controlledObj2;
    public Transform crystal;
    public Transform crystal2;
    public bool isSecondControlledObj;
    public List<Transform> lastTriggerObjects = new List<Transform>();
    public bool triggered = false;
    public Transform mirroringPlate;
    public int currentWeight = 0;
    public int maxWeight = 0;

    public void Start()
    {
        if (GetComponent<PressurePlate>() != null)
            maxWeight = GetComponent<PressurePlate>().weight;
        if (controlledObj2 != null)
        {
            isSecondControlledObj = true;
            controlledObj2.GetComponent<ObjectOperation>().StartFunctionality(transform);
        }
        else
        {
            isSecondControlledObj = false;
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (transform.parent.parent.parent != other.transform.parent.parent && !other.CompareTag("player"))
            return;
        if (lastTriggerObjects.Count > 0)
        {
            if (lastTriggerObjects.Contains(other.transform))
                return;
            lastTriggerObjects.Add(other.transform);
        }
        if (!lastTriggerObjects.Contains(other.transform))
            lastTriggerObjects.Add(other.transform);
        if (triggered)
            return;

        if (controlledObj != null)
            controlledObj.GetComponent<ObjectOperation>().StartFunctionality(transform);
        if (crystal != null)
            crystal.GetComponent<BloomOnOff>().TurnOn();
        if (isSecondControlledObj)
        {
            controlledObj2.GetComponent<ObjectOperation>().EndFunctionality(transform);
            crystal2.GetComponent<BloomOnOff>().TurnOff();
        }
        if (mirroringPlate != null)
        {
            mirroringPlate.GetComponent<PressurePlateFunc>().StartTrigger(other);
        }
        triggered = true;
    }

    public virtual void OnTriggerExit(Collider other)
    {

        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (!triggered)
            return;

        if (lastTriggerObjects.Contains(other.transform))
        {
            lastTriggerObjects.Remove(other.transform);



            if (lastTriggerObjects.Count == 0 || currentWeight < maxWeight)
            {
                if (controlledObj != null)
                    controlledObj.GetComponent<ObjectOperation>().EndFunctionality(transform);
                if (crystal != null)
                    crystal.GetComponent<BloomOnOff>().TurnOff();
                if (isSecondControlledObj)
                {
                    controlledObj2.GetComponent<OpeningClosingObj>().StartFunctionality(transform);
                    crystal2.GetComponent<BloomOnOff>().TurnOn();
                }
                triggered = false;
            }
        }
        if (mirroringPlate != null)
        {
            mirroringPlate.GetComponent<PressurePlateFunc>().EndTrigger(other);
        }
    }

    public void StartTrigger(Collider other)
    {
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;

        if (lastTriggerObjects.Count > 0)
        {
            if (lastTriggerObjects.Contains(other.transform))
                return;
            lastTriggerObjects.Add(other.transform);
        }
        if (!lastTriggerObjects.Contains(other.transform))
            lastTriggerObjects.Add(other.transform);
        if (triggered)
            return;


        if (controlledObj != null)
        {
            controlledObj.GetComponent<ObjectOperation>().StartFunctionality(transform);
        }
        if (crystal != null)
            crystal.GetComponent<BloomOnOff>().TurnOn();
        if (isSecondControlledObj)
        {
            controlledObj2.GetComponent<ObjectOperation>().EndFunctionality(transform);
            crystal2.GetComponent<BloomOnOff>().TurnOff();
        }

        triggered = true;
    }
    public void EndTrigger(Collider other)
    {
        if (GameManager.instance.player.GetComponentInChildren<PlayerMovement>().teleported)
            return;
        if (!triggered)
            return;

        if (lastTriggerObjects.Contains(other.transform))
        {

            Debug.Log("Closing door by mirroring");
            lastTriggerObjects.Remove(other.transform);


            if (lastTriggerObjects.Count == 0 || currentWeight < maxWeight)
            {
                if (controlledObj != null)
                {
                    controlledObj.GetComponent<ObjectOperation>().EndFunctionality(transform);
                }
                if (crystal != null)
                    crystal.GetComponent<BloomOnOff>().TurnOff();
                if (isSecondControlledObj)
                {
                    controlledObj2.GetComponent<ObjectOperation>().StartFunctionality(transform);
                    crystal2.GetComponent<BloomOnOff>().TurnOn();
                }
                triggered = false;
            }
        }
    }
}
