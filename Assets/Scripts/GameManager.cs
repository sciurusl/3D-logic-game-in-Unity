using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// It pauses the game when the menu is active, quits the game, loads, and saves the game.
/// GameManager contains many important variables controlling the game, such as curNumOfCrystals, 
/// currentLevelsActive, currentRoom, isParallelWorld.
/// It also changes levels and activates/deactivates rooms according to the currently active level
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public Transform playerRoomLocation;
    public Transform playerParallelRoomLocation;
    public bool inParallelWorld;
    public bool rotated;
    public Transform player;
    public bool teleportationEnded = true;
    public int room = 0;
    public Animator anim;
    public Transform unblock;

    public Transform pillarsLookAt;
    public Transform doorLookAt;
    public Transform crateLookAt;
    public Transform nonMirroringCrateLookAt;
    public Transform barrierLookAt;

    public Transform openingDoorButton;
    public Transform openingDoorButtonParalel;

    private bool rotate = false;
    private int speed = 3;
    private Transform currentLookAtObj;
    private Transform playerCamera;
    private GameObject[] rooms0;
    public GameObject[] rooms1;
    private GameObject[] rooms2;
    private GameObject[] rooms3;

    public GameObject menu;
    public bool isDialogue = false;
    private bool isMenu = false;
    public bool[] currentLevelsActive = new bool[4];
    public string currentRoom;
    public int curNumOfCrystals = 0;
    public int numOfCrystals = 3;
    public Transform finishSign;
    public List<Transform> finalCrystals;
    public bool loaded = false;
    private DialogueManager dialogueManager;

    /// <summary>
    /// Sets current level and loads the game
    /// </summary>
    void Start()
    {
        PauseGame();
        rooms0 = GameObject.FindGameObjectsWithTag("Room0");
        rooms1 = GameObject.FindGameObjectsWithTag("Room1");
        rooms2 = GameObject.FindGameObjectsWithTag("Room2");
        rooms3 = GameObject.FindGameObjectsWithTag("Room3");
        ChangeLevel(true, false, false, false);

        inParallelWorld = false;
        rotated = false;
        dialogueManager = FindObjectOfType<DialogueManager>();
        
        LoadData();
        anim.GetComponent<Animator>().enabled = false;
        playerCamera = player.GetComponentInChildren<MouseView>().transform;
        menu = GameObject.FindGameObjectWithTag("Menu");
        menu.SetActive(false);
    }

    /// <summary>
    /// Loads the game.
    /// If there is nothing to be loaded, it starts the game from beginning
    /// Otherwise it sets variables based on the variables stored in SaveSystem.objectDataVar
    /// It also changes worlds if necessary
    /// </summary>
    void LoadData()
    {
        SaveSystem.instance.LoadData();
        if (SaveSystem.objectDataVar == null || !SaveSystem.objectDataVar.loaded)
        {
            Debug.Log("objectDataVar is null");
            foreach (MainDoorOpening door in FindObjectsOfType<MainDoorOpening>())
            {
                if (!door.tutorialDoor)
                    continue;
                door.Open();
                break;
            }
            StartCoroutine(OpenDoor());
            dialogueManager.StartDialogueForTheFirstTime();

           

            return;
        }
        Transform doorTmp = null;
        loaded = false;
        ChangeLevel(SaveSystem.objectDataVar.currentLevelsActive[0], SaveSystem.objectDataVar.currentLevelsActive[1], SaveSystem.objectDataVar.currentLevelsActive[2], SaveSystem.objectDataVar.currentLevelsActive[3]);
        Vector3 tmpPlayerPos = Vector3.zero;
        foreach (var door in FindObjectsOfType<ChangeRoom>())
        {
            
            if (door.transform.parent.parent.name.Equals(SaveSystem.objectDataVar.currentRoom))
            {
                doorTmp = door.transform;
                tmpPlayerPos = door.transform.position;
                loaded = true;
                door.gameObject.SetActive(false);
                break;
            }
        }
        if (!loaded)
        {
            Debug.Log("Saved room has not been found");
            ChangeLevel(true, false, false, false);

            foreach (MainDoorOpening door in FindObjectsOfType<MainDoorOpening>())
            {
                if (!door.tutorialDoor)
                    continue;
                door.Open();
                break;
            }

            StartCoroutine(OpenDoor());
            dialogueManager.StartDialogueForTheFirstTime();

            return;
        }
        dialogueManager.videoCount = SaveSystem.objectDataVar.videoCount;
        dialogueManager.count = SaveSystem.objectDataVar.arrowCount;
        room = SaveSystem.objectDataVar.room;
        curNumOfCrystals = SaveSystem.objectDataVar.numOfCrystals;
        if (GameObject.FindGameObjectWithTag("LoadStop") != null)
            GameObject.FindGameObjectWithTag("LoadStop").GetComponent<Collider>().enabled = true;
        for (int i = 0; i< curNumOfCrystals; i++)
        {
            finalCrystals[i].GetComponent<FinalCrystal>().SetTrigger();
        }
        //inParallelWorld = !SaveSystem.objectDataVar.currentlyInUpWorld;
        player.position = tmpPlayerPos;
        if (!SaveSystem.objectDataVar.currentlyInUpWorld)
            StartCoroutine(ChangeWorld(doorTmp));
        else if(SaveSystem.objectDataVar.currentLevelsActive[0])
        {
            foreach (MainDoorOpening door in FindObjectsOfType<MainDoorOpening>())
            {
                if (!door.tutorialDoor)
                    continue;
                door.Open();
                break;
            }
        }
        ContinueGame();
    }

    /// <summary>
    /// Rotates worlds and by calling ChangeBackToNormal method of CameraRotating class, it updates objects after the rotation
    /// </summary>
    /// <param name="door"></param>
    private void RotateWorld(Transform door)
    {
        GameObject.FindGameObjectWithTag("AllRooms").transform.RotateAround(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 4, Camera.main.transform.position.z), Vector3.left, 180);
        Camera.main.GetComponent<CameraRotating>().ChangeBackToNormal();
        if (door)
        {
            Debug.Log("updating players position to " + door.transform.position);
            player.transform.position = door.transform.position;
        }
       
    }

    /// <summary>
    /// Calls RotateWorld() function after two frames
    /// </summary>
    /// <param name="door"></param>
    /// <returns></returns>
    IEnumerator ChangeWorld(Transform door)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        RotateWorld(door);
        
    }

    /// <summary>
    /// Listens if Escape key was pressed and activates/deactivates in-game menu
    /// </summary>
    void Update()
    {
        if (rotate)
        {
            var targetRotation = Quaternion.LookRotation(currentLookAtObj.position - playerCamera.position);
            playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRotation, speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenu)
            {
                ContinueGameFromMenu();
            }
            else
            {
                isMenu = true;
                menu.SetActive(true);
                PauseGame();
                Time.timeScale = 0;
            }
        }
    }

    /// <summary>
    /// Opens the cell in the starting room after 5 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(5);
        openingDoorButton.GetComponent<ButtonFunc>().Click();
        openingDoorButtonParalel.GetComponent<ButtonFunc>().Click();
    }

    /// <summary>
    /// Pauses the game by disabling the player's movement and enabling the cursor
    /// </summary>
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        player.GetComponentInChildren<PlayerMovement>().anim.SetFloat("Walking", 0);
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponentInChildren<PlayerMovement>().enabled = false;
        player.GetComponentInChildren<MouseView>().enabled = false;
    }

   /// <summary>
   /// Saves the game
   /// </summary>
    public void SaveGame()
    {
        SaveSystem.instance.SaveData();
    }

    /// <summary>
    /// Continues the game by locking the cursor and enabling the player's movement
    /// </summary>
    public void ContinueGame()
    {       
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponentInChildren<PlayerMovement>().enabled = true;
        player.GetComponentInChildren<MouseView>().enabled = true;
    }


    public void SetTarget(Transform transform)
    {
        currentLookAtObj = transform;
    }
    public void Look()
    {       
        rotate = true;
        StartCoroutine(StopRotation());
    }

    private IEnumerator StopRotation()
    {
        yield return new WaitForSeconds(2);
        rotate = false;
    }
    
    /// <summary>
    /// Deactivates/activates rooms in levels based on the r0, r1, r2 and r3 parameters
    /// </summary>
    /// <param name="r0"></param>
    /// <param name="r1"></param>
    /// <param name="r2"></param>
    /// <param name="r3"></param>
    public void ChangeLevel(bool r0, bool r1, bool r2, bool r3)
    {
        if (currentLevelsActive[0] == r0 && currentLevelsActive[1] == r1 && currentLevelsActive[2] == r2 && currentLevelsActive[3] == r3)
            return;
        SetActiveRooms(r0, r1, r2, r3);
        for (int i = 0; i < rooms0.Length; i++)
        {
            rooms0[i].SetActive(r0);
        }
        for (int i = 0; i < rooms1.Length; i++)
        {
            rooms1[i].SetActive(r1);
        }
        for (int i = 0; i < rooms2.Length; i++)
        {
            rooms2[i].SetActive(r2);
        }
        for (int i = 0; i < rooms3.Length; i++)
        {
            rooms3[i].SetActive(r3);
        }
        
        foreach(GravityObjects gravityObject in FindObjectsOfType<GravityObjects>())
        {
            gravityObject.StartAgain();
        }
    }

    /// <summary>
    /// Continues the game by clicking Continue button
    /// </summary>
    public void ContinueGameFromMenu()
    {
        isMenu = false;
        menu.SetActive(false);
        if(!isDialogue)
            ContinueGame();
        Time.timeScale = 1;
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Loads the Menu Scene
    /// </summary>
    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    /// Loads the game by restarting the Main Scene
    /// </summary>
    public void LoadGame()
    {
        
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// Updates the room in which the player currently is
    /// </summary>
    /// <param name="room"></param>
    /// <param name="upWorld"></param>
    public void SetRoom(string room, bool upWorld)
    {
        currentRoom = room;
        SaveSystem.instance.SaveData();
    }

    /// <summary>
    /// Sets which level is currently active
    /// </summary>
    /// <param name="r0"></param>
    /// <param name="r1"></param>
    /// <param name="r2"></param>
    /// <param name="r3"></param>
    public void SetActiveRooms(bool r0, bool r1, bool r2, bool r3)
    {
        currentLevelsActive[0] = r0;
        currentLevelsActive[1] = r1;
        currentLevelsActive[2] = r2;
        currentLevelsActive[3] = r3;
    }

    /// <summary>
    /// Finishes the game by Pausing the game, showing Congratulations text and loading Menu Scene after 3 seconds
    /// </summary>
    public void GameFinish()
    {
        PauseGame();
        finishSign.gameObject.SetActive(true);
        StartCoroutine(MenuAfterFinish());
    }

    IEnumerator MenuAfterFinish()
    {
        yield return new WaitForSeconds(3);
        LoadMenu();
    }
}
