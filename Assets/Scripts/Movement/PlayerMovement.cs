using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Using the CharacterController component, it manages the movement of the player
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float currentPlayerSpeed;
    public float playerSpeed = 8.0f;
    public float playerHeight = 4.2f;
    public float crounchHeight = 2.0f;
    public float crouchSpeed = 3.0f;
    private float pullSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private float cameraPosition;
    public bool teleported = false;
    public bool gravity = true;
    public Camera mainCamera;
    public Animator anim;
    private int idleHash;
    private int runHash;
    public bool crouching;
    public bool pulling = false;

    private void Start()
    {
        cameraPosition = mainCamera.transform.localPosition.y;
        controller = transform.parent.GetComponent<CharacterController>();
        anim = transform.parent.GetComponentInChildren<Animator>();
        idleHash = Animator.StringToHash("Idle");
        currentPlayerSpeed = playerSpeed;
    }

    /// <summary>
    /// Checks the input from the keyboard. Based on the key, it activates crouching or pulling
    /// </summary>
    private void Update()
    {
        if (GameManager.instance.teleportationEnded)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                    crouching = true;
                    SetCrouching();
                    transform.parent.GetComponent<ColliderController>().crouchingText.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {

                    crouching = false;
                    UnsetCrouching();
                    transform.parent.GetComponent<ColliderController>().crouchingText.SetActive(false);
            }


            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("B clicked");
                if (pulling)
                {
                    currentPlayerSpeed = playerSpeed;
                    pulling = false;
                    transform.parent.GetComponent<ColliderController>().pulling = false;
                    transform.parent.GetComponent<ColliderController>().pullingText.SetActive(false);
                }
                else
                {
                    if (transform.parent.GetComponent<ColliderController>().hitted)
                    {
                        Debug.Log("pulling");
                        currentPlayerSpeed = pullSpeed;
                        pulling = true;
                        transform.parent.GetComponent<ColliderController>().pulling = true;
                        transform.parent.GetComponent<ColliderController>().pullingText.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("nothing to pull");
                    }
                }
            }

        }
    }

    /// <summary>
    /// Activates crouching
    /// </summary>
    private void SetCrouching()
    {
        controller.height = crounchHeight;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.95f, transform.localPosition.z);
        currentPlayerSpeed = crouchSpeed;
    }

    /// <summary>
    /// Deactivates crouching
    /// </summary>
    private void UnsetCrouching()
    {
        controller.height = playerHeight;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.95f, transform.localPosition.z);
        currentPlayerSpeed = playerSpeed;
    }

    /// <summary>
    /// Moves the player in specific direction.
    /// Also manages jumping and falling
    /// </summary>
    void FixedUpdate()
    {
        if (gravity)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            if (GameManager.instance.teleportationEnded)
            {
                
                Vector3 move = Input.GetAxis("Horizontal") * transform.right + transform.forward * Input.GetAxis("Vertical");

                controller.Move(move * Time.deltaTime * currentPlayerSpeed);
              
                anim.SetFloat("Walking", Mathf.Abs(move.x) + Mathf.Abs(move.z));
               

                if (Input.GetKey("space") && groundedPlayer)
                {
                    Debug.Log("jumping " + Time.deltaTime);
                    playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
                }
                
                playerVelocity.y += gravityValue * Time.deltaTime;
                controller.Move(playerVelocity * Time.deltaTime);
               
               
            }
            
        }
    }

}

