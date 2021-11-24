using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the menu button's functionality
/// </summary>
public class MenuButtonListener : MonoBehaviour
{
    public bool clicked = false;
    public int type;
    public int buttonNum;

    /// <summary>
    /// Listens if the player clicked on its collider
    /// </summary>
    void OnMouseDown()
    {
        Debug.Log("clicked on button");
        if (GetComponent<MenuButton>().closingAnimStarted || GetComponent<MenuButton>().openingAnimStarted)
            return;       
        clicked = !clicked;
            if (clicked)
            {
            Debug.Log("got here");
                transform.GetComponent<MenuButton>().StartAnim();
                StartCoroutine(CallFunction());
            }
            else
            {
                transform.GetComponent<MenuButton>().EndAnim();             
            }
        
    }
    public void Awake()
    {
        clicked = false;
    }

    /// <summary>
    /// Based on the button's number, it calls specific method
    /// </summary>
    /// <returns></returns>
    public IEnumerator CallFunction()
    {
        yield return new WaitForSeconds(1f);
        if (buttonNum == 0)
            StartMainGame();
        else if (buttonNum == 1)
        {
            LoadMainGame();
        }
        else if (buttonNum == 2)
        {
            //settings
        }
        else if (buttonNum == 3)
        {
            QuitGame();
        }

    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Loads the Main Scene
    /// </summary>
    public void LoadMainGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// Starts new game by loading the Main Scene and deleting save
    /// </summary>
    public void StartMainGame()
    {
        Debug.Log("starting game");
        string path = Application.persistentDataPath + "/save.txt";
        File.Delete(path);
        SceneManager.LoadScene("MainScene");
    }
}
