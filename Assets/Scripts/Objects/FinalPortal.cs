using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPortal : MonoBehaviour
{
    /// <summary>
    /// When triggered, it calls the Game Manager to finish the game
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("player"))
            GameManager.instance.GameFinish();
    }
}
