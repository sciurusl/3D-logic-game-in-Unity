using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    public bool upWorld;
    private void Start()
    {
        upWorld = transform.parent.parent.parent.name.Equals("UpWorld");
    }

    /// <summary>
    /// Calls SetRoom method of GameManager class when triggered
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.SetRoom(transform.parent.parent.name, upWorld);
    }

    /// <summary>
    /// Deactivates itself when the object exits the collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            this.gameObject.SetActive(false);
           
        }
    }
}
