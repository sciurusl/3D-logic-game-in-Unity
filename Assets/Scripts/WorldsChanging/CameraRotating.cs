using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
///  Manages the updating of the objects during the rotation
/// </summary>
public class CameraRotating : MonoBehaviour
{
    public Transform otherWorldParent;
    public float cameraOffset = 0.75f;
    public float cameraRotationViewOffset = 3.5f;
    public float animTime;// = 2f;
    public Transform tempParent;
    public Transform upWorldParent;
    private Transform portalParent;
    private Vector3 origLocalPos;
    private Transform portalTmp;

    private List<Transform> rooms = new List<Transform>();

    public void Start()
    {
        origLocalPos = transform.localPosition;
    }

    /// <summary>
    /// Calls UpWorld and ParallelWorld objects to update their objects (rigidbodies, doors, mirroring objects, pressure plates, and lights
    /// It also starts the animation of the camera
    /// </summary>
    /// <param name="portal"></param>
    public void StartRotation(Transform portal)
    {

        transform.parent.GetComponent<CharacterController>().enabled = false;
        transform.parent.GetComponentInChildren<PlayerMovement>().enabled = false;
        GameManager.instance.teleportationEnded = false;

        rooms.Clear();
        rooms.Add(GameManager.instance.playerRoomLocation);
        rooms.Add(GameManager.instance.playerParallelRoomLocation);
        foreach (Transform room in rooms)
        {
            //if (GameManager.instance.inParallelWorld)
            room.GetComponent<GravityObjects>().UpdateDoor(true);
            room.GetComponent<GravityObjects>().UpdateRigidbody(true);
            room.GetComponent<GravityObjects>().UpdateMirroringObejcts();
            room.GetComponent<GravityObjects>().UpdatePressurePlates();
            room.GetComponent<GravityObjects>().UpdateLights();
        }
        portalTmp = portal;
        transform.parent.GetComponentInChildren<PlayerMovement>().anim.SetFloat("Walking", 0);
        StartCoroutine(LerpFromTo());
    }

    /// <summary>
    /// MSmoothly moves the camera back and slightly up
    /// </summary>
    /// <returns></returns>
    private IEnumerator LerpFromTo()
    {
        float elapsedTime = 0f;
        float waitTime = 1f;
        Vector3 curPos = transform.position;
        Vector3 playersCurPos = transform.parent.position;

        Vector3 dir = Vector3.Normalize(portalTmp.position - playersCurPos);
        dir.y = 0;
        transform.parent.position = playersCurPos + dir;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(curPos, curPos - 4*Vector3.Normalize((transform.forward-transform.up/4.5f)), (elapsedTime / waitTime));            
            elapsedTime += Time.deltaTime;

            yield return null;
        }

    }

    /// <summary>
    /// Returns camera to its default position
    /// </summary>
    /// <returns></returns>
    private IEnumerator LerpFromToBack()
    {
        float elapsedTime = 0f;
        float waitTime = 1f;
       
        Vector3 curPos = transform.localPosition;

        while (elapsedTime < waitTime)
        {
            transform.localPosition = Vector3.Lerp(curPos, origLocalPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    /// <summary>
    /// Calls UpWorld and ParallelWorld objects to update their objects 
    /// (rigidbodies, doors, mirroring objects, pressure plates, and lights) at the end of the rotation
    /// It also starts the returning animation of the camera
    /// </summary>
    /// <param name="portal"></param>
    public void ChangeBackToNormal(Transform portal)
    {
        StartCoroutine(LerpFromToBack());
        
        GetComponent<MouseView>().playerBody = transform.parent;
      
        
        foreach (Transform room in rooms)
        {

            room.GetComponent<GravityObjects>().UpdateRigidbody(false);
            room.GetComponent<GravityObjects>().UpdateMirroringObejcts();
            room.GetComponent<GravityObjects>().UpdatePressurePlates();
            room.GetComponent<GravityObjects>().UpdateDoor(false);
            room.GetComponent<GravityObjects>().UpdateLights();
        }

        transform.parent.GetComponent<CharacterController>().enabled = true;
        transform.parent.GetComponentInChildren<PlayerMovement>().enabled = true;
     

        StartCoroutine(StopTeleporting());
        GameManager.instance.teleportationEnded = true;

        GameManager.instance.inParallelWorld = !GameManager.instance.inParallelWorld;
        GameManager.instance.rotated = !GameManager.instance.rotated;
       
    }

    /// <summary>
    /// Manages updating of objects after changing of worlds when the game is loaded
    /// </summary>
    public void ChangeBackToNormal()
    {
        GameManager.instance.playerRoomLocation.GetComponent<GravityObjects>().UpdateRigidbody(false);
       
        GameManager.instance.playerRoomLocation.GetComponent<GravityObjects>().UpdateLights();

        GameManager.instance.playerParallelRoomLocation.GetComponent<GravityObjects>().UpdateRigidbody(false);
       
        GameManager.instance.playerParallelRoomLocation.GetComponent<GravityObjects>().UpdateLights();

  
        

        transform.parent.GetComponent<CharacterController>().enabled = true;
        transform.parent.GetComponentInChildren<PlayerMovement>().enabled = true;
      
        GameManager.instance.teleportationEnded = true;

        GameManager.instance.inParallelWorld = !GameManager.instance.inParallelWorld;
        GameManager.instance.rotated = !GameManager.instance.rotated;

        if (GameManager.instance.currentLevelsActive[0])
        {
            foreach (MainDoorOpening door in FindObjectsOfType<MainDoorOpening>())
            {
                if (!door.tutorialDoor)
                    continue;
                door.Open();
            }
        }

    }



    IEnumerator StopTeleporting()
    {
        yield return new WaitForSeconds(1);
        transform.parent.GetComponentInChildren<PlayerMovement>().teleported = false;
    }

   

}
