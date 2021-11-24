using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///Simulates gravity in the opposite direction to every GameObejct 
///at the start in the UpWorld with a Rigidbody component affected by gravity
/// </summary>
public class GravityChange : MonoBehaviour
{
    public bool upWorld;
    private void Start()
    {
        if (transform.parent.parent.CompareTag("ParallelWorld"))
            upWorld = false;
        else
        {
            upWorld = true;
            if(tag == "Crate")
            {
                foreach (Transform child in transform)
                {
                    if (child.name == "Crystals")
                    {
                        child.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Calls the AddForce method of the Rigidbody component and passes it a Vector3 with zeros in x
    /// and z coordinates and 9.18 in y coordinate
    /// </summary>
    public void FixedUpdate()
    {
        if (upWorld)
        {
            if (GameManager.instance.inParallelWorld)
            {
            
                GetComponent<Rigidbody>().AddForce(0, 9.81f, 0);
            }
        }
        else
        {
            if(!GameManager.instance.inParallelWorld)
                GetComponent<Rigidbody>().AddForce(0, 9.81f, 0);
        }
    }
   
}
