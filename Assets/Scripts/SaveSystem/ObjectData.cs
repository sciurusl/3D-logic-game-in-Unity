using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public bool loaded = false;
    public string currentRoom;
    public bool currentlyInUpWorld;
    public bool[] currentLevelsActive = new bool[4];
    public int numOfCrystals;
    public int room;
    public int videoCount;
    public int arrowCount;
   
    /// <summary>
    /// Sets variables that will be saved
    /// </summary>
    public void SetVariables()
    {
        currentLevelsActive = GameManager.instance.currentLevelsActive;
        currentlyInUpWorld = !GameManager.instance.inParallelWorld;
        currentRoom = GameManager.instance.currentRoom;
        numOfCrystals = GameManager.instance.curNumOfCrystals;
        videoCount = Object.FindObjectOfType<DialogueManager>().videoCount;
        arrowCount = Object.FindObjectOfType<DialogueManager>().count;
        room = GameManager.instance.room;
    }
    
}
