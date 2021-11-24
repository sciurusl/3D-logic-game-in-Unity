using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages pushing and pulling movable objects by player
/// </summary>
public class ColliderController : MonoBehaviour
{
    private float pushPower = 3.0f;
    public float weight = 6.0f;
    public Rigidbody body;
    public bool pulling = false;
    public bool hitted = false;
    ControllerColliderHit hitTemp;
    public GameObject pullingText;
    public GameObject crouchingText;

    /// <summary>
    /// Pushes movable rigidbody objects when hit by calling AddForce() method of the hit rigidbody
    /// </summary>
    /// <param name="hit"></param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Platform"))
            return;
        body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic) 
            return;
        hitted = true;
        hitTemp = hit;
        if (hit.moveDirection.y < -0.3)
            return;

            var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            body.freezeRotation = true;


            body.velocity = pushDir * pushPower;
            body.AddForce(pushDir * pushPower);
        if (Mathf.Abs(pushDir.x) + Mathf.Abs(pushDir.z) > 0.1)
        {
            if(body.GetComponent<AudioSource>() != null && !body.GetComponent<AudioSource>().isPlaying)
                body.GetComponent<AudioSource>().Play();
        }
            if (body.gameObject.GetComponent<MirroringObjectInfo>() != null)
            {
                body.gameObject.GetComponent<MirroringObjectInfo>().UpdateMirObjectPosition(pushDir * pushPower);              
            }
            
    }

    /// <summary>
    /// Manages pulling hitted rigidbody objects when pulling feature is activated
    /// Almost the same as OnControllerColliderHit but in the opposite direction
    /// </summary>
    public void Update()
    {
        if (hitted)
        {
            if (Vector3.Distance(hitTemp.transform.position, transform.position) > 2f)
            {
                if (Vector3.Distance(hitTemp.transform.position, transform.position) > 3.5f)
                {
                    pulling = false;
                    pullingText.SetActive(false);
                   
                    transform.GetComponentInChildren<PlayerMovement>().currentPlayerSpeed = transform.GetComponentInChildren<PlayerMovement>().playerSpeed;
                    transform.GetComponentInChildren<PlayerMovement>().pulling = false;
                    hitted = false;
                }
                if (pulling)
                {
                    body = hitTemp.collider.attachedRigidbody;
                    if (body == null || body.isKinematic)
                        return;

                    var pushDir = -new Vector3(hitTemp.moveDirection.x, 0, hitTemp.moveDirection.z);

                    body.freezeRotation = true;
               
                    body.velocity = pushDir * pushPower;
                    body.AddForce(pushDir * pushPower);
                    if (Mathf.Abs(pushDir.x) + Mathf.Abs(pushDir.z) > 0.1)
                    {
                        if (body.GetComponent<AudioSource>() != null && !body.GetComponent<AudioSource>().isPlaying)
                            body.GetComponent<AudioSource>().Play();
                    }
                    if (body.gameObject.GetComponent<MirroringObjectInfo>() != null)
                    {
                        body.gameObject.GetComponent<MirroringObjectInfo>().UpdateMirObjectPosition(pushDir * pushPower);
                    }
                }
 
            }
        }
    }

    /// <summary>
    /// When pulling feature is activated, it also manages pulling
    /// Otherwise, it stops audio source of the last hitted rigidbody movable object
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit(Collider collision)
    {
        if(collision.GetComponent<Rigidbody>() && collision.GetComponent<Rigidbody>().Equals(body) && collision.GetComponent<AudioSource>())
        {
            body.GetComponent<AudioSource>().Stop();
        }
        if (pulling && collision.gameObject.GetComponent<Rigidbody>())
        {
            var pushDir = collision.transform.position - transform.position;

            body.freezeRotation = true;


            body.velocity = pushDir * pushPower;
            body.AddForce(pushDir * pushPower);
            if (body.GetComponent<AudioSource>() != null)
                body.GetComponent<AudioSource>().Stop();
            if (body.gameObject.GetComponent<MirroringObjectInfo>() != null)
            {
                body.gameObject.GetComponent<MirroringObjectInfo>().UpdateMirObjectPosition(pushDir * pushPower);
            }
        }
        hitted = false;
        
    }
}
