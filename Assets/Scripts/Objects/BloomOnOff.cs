using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages turning off and turning on the bloom effect
/// </summary>
public class BloomOnOff : MonoBehaviour
{
    public Material materialOff;
    public Material materialOn;
    
    public void TurnOn()
    {
        GetComponent<MeshRenderer>().material = materialOn;
    }

    public void TurnOff()
    {
        GetComponent<MeshRenderer>().material = materialOff;
    }

}
