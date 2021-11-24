using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    /// <summary>
    /// When triggered by the player, it triggers dialogue by calling TriggerDialogue() method of DialogueTrigger class
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            GameManager.instance.room++;
            DialogueTrigger.instance.TriggerDialogue();
            this.gameObject.SetActive(false);
        }
    }
}
