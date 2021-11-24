using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    #region Singleton

    public static DialogueTrigger instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    /// <summary>
    /// Starts the dialogue by calling StartDialogue() function of DialogueManager class
    /// </summary>
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue();
    }
}
