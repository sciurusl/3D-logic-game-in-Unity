using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates the player based on the mouse movement
/// </summary>
public class MouseView : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;

    void Start()
    {
        playerBody = transform.parent;     
    }

    /// <summary>
    /// Rotates the player based on the mouse movement
    /// </summary>
    void Update()
    {
        if (GameManager.instance.teleportationEnded)
        {

            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);
            }
            
        }
    }
}
