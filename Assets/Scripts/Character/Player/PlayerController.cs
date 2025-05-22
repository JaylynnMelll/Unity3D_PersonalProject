using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

/* [ClassINFO : PlayerController]
   @ Description : - This class is used to control the player's movement, looking around(through camera), jumping, and interaction with objects in the game world.
                   - The class holds methods to use in the Player Input System.
   @ Attached at : Player (gameObject)
   @ Methods : ============================================
               [public] Used in Input System
               - OnMove() : Handle player movement input.
               - OnLook() : Handle player looking around input.
               - OnJump() : Handle player jumping input.
               - OnInteraction() : Handle player interaction with objects.
               - OnInventory() : Handle player inventory input.
               ============================================
               [private]
               - Move() : Move the player based on input.
               - CameraLook() : Rotate the camera based on mouse movement.
               - IsOnGround() : Check if the player is on the ground.
               - ToggleCursor() : Toggle the cursor lock state and camera look ability.
               ============================================
*/

public class PlayerController : MonoBehaviour
{
    // ========================== //
    //     [InspectorWindow]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    public Rigidbody rigidBody;
    public PlayerInteraction playerInteraction;

    [Header("Movement Settings")]
    public float moveSpeed;
    private Vector2 currentMovement;
    public LayerMask groundLayerMask;

    [Header("Look Settings")]
    public Transform cameraContainer;
    public float minXLookAngle;
    public float maxXLookAngle;
    public float lookSensitivity;
    private float camCursorXRotation;
    private float camCursorYRotation;
    private Vector2 mouseDelta;
    public bool canLook = true;

    [Header("Jump Settings")]
    public float jumpForce;
    private int jumpCount = 0;
    private int maxJumpCount = 2;

    [Header("Interaction Settings")]
    public Action inventoryEvent;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]    
    // ========================== //
    #region [Unity LifeCycle]
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();

        if (IsOnGround())
        {
            jumpCount = 0;
        }
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }
    #endregion


    // ========================== //
    //      [Public Methods]
    // ========================== //
    #region [Public Methods]
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            currentMovement = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            currentMovement = Vector2.zero;
        }
    }
       
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (jumpCount < maxJumpCount)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 2, rigidBody.velocity.z);
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && playerInteraction.currentInteractableInfo != null)
        {
            playerInteraction.currentInteractableInfo.WhenInteracted();
            playerInteraction.currentInteractingGameObject = null;
            playerInteraction.currentInteractableInfo = null;
            playerInteraction.interactionPrompt.gameObject.SetActive(false);
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventoryEvent?.Invoke();
            ToggleCursor();
        }
    }
    #endregion


    // ========================== //
    //      [Private Methods]
    // ========================== //
    #region [Private Methods]
    private void Move()
    {
        // 01) This combines forward/backward and left/right movement into a single vector
        // 02) Scales the direction vector by the player's move speed
        // 03) Preserves current vertical (Y-axis) velocity -- like jumping or falling
        // 04) Sets the rigidbody's velocity to the new direction vector
        Vector3 direction = (transform.forward * currentMovement.y) + (transform.right * currentMovement.x);
        direction *= moveSpeed;
        direction.y = rigidBody.velocity.y;

        rigidBody.velocity = direction;
    }

    private void CameraLook()
    {
        // 01) This adds vertical mouse movement to the attachedCamera's X rotation accumulator
        // 02) This prevents the attachedCamera from rotating beyond a certain vertical limit.
        // 03) This sets the attachedCamera's X rotation to the new value
        // 04) This rotates the attachedCamera container (the parent of the attachedCamera) left / right based on horizontal mouse movement
        camCursorXRotation += mouseDelta.y * lookSensitivity;
        camCursorXRotation = Mathf.Clamp(camCursorXRotation, minXLookAngle, maxXLookAngle);
        cameraContainer.localEulerAngles = new Vector3(-camCursorXRotation, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private bool IsOnGround()
    {
        // 01) ray 만들어주기
        Ray[] rays = new Ray[4]
        {
            // new Ray(rayOriginPosition, rayDirection)
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // 02) ray에 충돌한 물체 확인
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.5f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    private void ToggleCursor()
    {
        bool isCursorLocked = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = isCursorLocked ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !isCursorLocked;
    }
    #endregion
}
