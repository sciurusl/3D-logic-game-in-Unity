using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    Transform playerCamera;
    float eulerAnglesX;
    private void Start()
    {
        playerCamera = Camera.main.transform;
        eulerAnglesX = transform.eulerAngles.x;
    }

    /// <summary>
    /// Rotates so that it always look at the main camera
    /// </summary>
    void Update()
    {

        transform.LookAt(transform.position + playerCamera.rotation * Vector3.forward,
              playerCamera.transform.rotation * Vector3.up);
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.x = eulerAnglesX;
        transform.eulerAngles = eulerAngles;
    }
}
