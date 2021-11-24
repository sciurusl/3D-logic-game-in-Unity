using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Saves and loads ObjectData object into\from a file
/// </summary>
public class SaveSystem : MonoBehaviour
{
    #region Singleton

    public static SaveSystem instance;
    public GameObject savingInfo;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public static ObjectData objectDataVar;

    /// <summary>
    /// Creates ObjectData object, calls its function to set the variables
    /// Saves the ObjectData object into a file
    /// </summary>
    public void SaveData()
    {
        Debug.Log("saving data");
        savingInfo.SetActive(true);
        StartCoroutine(TurnOffSavingInfo());

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.txt";
        Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);
        ObjectData objectData = new ObjectData();
        objectData.SetVariables();

        formatter.Serialize(stream, objectData);
        stream.Close();
    }

    /// <summary>
    /// Loads ObjectData object from a file and returns it
    /// </summary>
    /// <returns></returns>
    public ObjectData LoadData()
    {
        string path = Application.persistentDataPath + "/save.txt";

        if (File.Exists(path))
        {
            Debug.Log("loading data");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            objectDataVar = formatter.Deserialize(stream) as ObjectData;
            objectDataVar.loaded = true;
            stream.Close();
            return objectDataVar;
        }
        else
        {
            Debug.Log("File not found in " + path);
            objectDataVar = null;
            return null;
        }
    }

    IEnumerator TurnOffSavingInfo()
    {
        yield return new WaitForSeconds(2);
        savingInfo.SetActive(false);
    }
}
