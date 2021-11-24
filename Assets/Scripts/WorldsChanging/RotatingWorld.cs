using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

/// <summary>
/// Manages the main rotation of worlds
/// </summary>
public class RotatingWorld : MonoBehaviour
{
    public Transform player;
    public Transform animUp;
    public Transform animParW;
    public float animTime = 1.5f;

    public bool playerIsOverlapping = false;

    public int rotAnglePerSecond = 60;
    public int rotAngle = 180;
    public bool teleporting = false;
    public bool leftTheTeleport = true;
    private float time;

    private Vector3 gravity;
    public Transform playerCamera;

    private Vector3 rotVector;
    private Vector3 cameraPos;
    private int[] coords = new int[3];

    void Start()
    {
        rotVector = transform.up;
        if (!transform.parent.parent.parent.CompareTag("NormalWorld"))
            gameObject.SetActive(false);
        leftTheTeleport = true;
    }

    /// <summary>
    /// Checks the value of the playerIsOverlapping variable. When true and if not already rotating, it starts the rotation of the worlds.
    /// Otherwise, it checks if the rotation has reached the desired angle and stops the rotation.
    /// It rotates all objects around the Camera so that the player does not move during the rotation.
    /// It uses the RotateAround function of the Transform component. 
    /// It takes the center of the rotation in Vector3 format, the direction of the rotation, and rotation angle. 
    /// The angle is calculated as the multiplication of Time.deltaTime and a rotation angle per
    /// second stored in the rotAnglePerSecond variable.
    /// It also calls the StartRotation method of the CameraRotating class
    /// </summary>
    void Update()
    {
        if (playerIsOverlapping)
        {
            if (leftTheTeleport)
            {
                if (!teleporting)
                {
                    cameraPos = playerCamera.transform.position;
                    Debug.Log("setting camera's position");
                    teleporting = true;
                    time = Time.time;
                    player.gameObject.GetComponentInChildren<PlayerMovement>().teleported = true;
                    player.gameObject.GetComponentInChildren<PlayerMovement>().gravity = false;

                    animUp.RotateAround(new Vector3(cameraPos.x, cameraPos.y - 4, cameraPos.z), rotVector, rotAnglePerSecond * Time.deltaTime);
                    if (player.GetComponentInChildren<CameraRotating>())
                        player.GetComponentInChildren<CameraRotating>().StartRotation(transform.parent);
                    gravity = Physics2D.gravity;
                    Physics2D.gravity = new Vector3(0, -1.0F, 0);
                }
                else
                {
                    if (Time.time - time >= rotAngle / rotAnglePerSecond)
                    {

                        leftTheTeleport = false;
                        teleporting = false;
                        playerIsOverlapping = false;
                        player.gameObject.GetComponentInChildren<PlayerMovement>().gravity = true;
                        Physics2D.gravity = gravity;
                        for (int i = 0; i < 3; i++)
                        {
                            if (animUp.localEulerAngles[i] > 100)
                            {
                                if (animUp.localEulerAngles[i] > 340)
                                    coords[i] = 0;
                                else
                                    coords[i] = 180;
                            }

                            else
                                coords[i] = 0;
                        }

                        animUp.localEulerAngles = new Vector3(coords[0], Mathf.Round(animUp.localEulerAngles.y), coords[2]);

                        if (player.GetComponentInChildren<CameraRotating>())
                            player.GetComponentInChildren<CameraRotating>().ChangeBackToNormal(transform.parent);
                    }
                    else
                    {

                        animUp.RotateAround(new Vector3(cameraPos.x, cameraPos.y - 4, cameraPos.z), rotVector, rotAnglePerSecond * Time.deltaTime);

                    }
                }
            }

        }
    }


    /// <summary>
    /// Checks if the other Collider component is assigned to a player by comparing its
    /// tag to the player tag. It sets the playerIsOverlappig variable to true
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            playerIsOverlapping = true;
            if (leftTheTeleport)
                transform.parent.GetComponentInChildren<AudioSource>().Play();
        }
    }

    /// <summary>
    /// Checks if the other Collider component is assigned to a player by comparing its
    /// tag to the player tag. It sets the playerIsOverlappig variable to false
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (teleporting)
            return;
        if (other.tag == "player")
        {
            leftTheTeleport = true;
            playerIsOverlapping = false;
            player.gameObject.GetComponentInChildren<PlayerMovement>().teleported = false;

        }
    }


}
