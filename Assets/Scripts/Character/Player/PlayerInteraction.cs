using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/* [ClassINFO : PlayerInteraction]
   @ Description : This class is used to handle player interactions with objects in the game world.
   @ Attached at : Player (gameObject)
   @ Methods : ============================================
               [public]
               - None
               ============================================
               [private]
               - DetectInteractables() : Check for interactable objects within a certain distance from the player.
               - SetPromptText() : Update the interaction prompt text and visibility based on the current interactable object.
               ============================================
*/

public class PlayerInteraction : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    private Rigidbody rigidbody;

    [Header("Player Interaction Settings")]
    public float checkRate = 0.05f;
    public float lastCheckTime;
    public float checkDistance;
    public LayerMask layerMask;

    [Header("Interactable Object Settings")]
    public GameObject currentInteractingGameObject;
    public IInteractable currentInteractableInfo;
    public GameObject interactionPrompt;
    public TextMeshProUGUI promptText;
    public Camera attachedCamera;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]
    // ========================== //
    #region [Unity LifeCycle]
    void Start()
    {
        attachedCamera = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
        Debug.Log(rigidbody);
    }
    void Update()
    {
        if ((Time.time - lastCheckTime) > checkRate)
        {
            lastCheckTime = Time.time;
            DetectInteractables();
        }    
    }
    #endregion


    // ========================== //
    //     [Private Methods]
    // ========================== //
    #region [Private Methods]
    private void DetectInteractables()
    {
        Ray ray = attachedCamera.ScreenPointToRay(new Vector3((Screen.width / 2), (Screen.height / 2)));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, checkDistance, layerMask))
        {
            // 레이가 맞은 오브젝트가 null이 아닐 경우
            if (hit.collider.gameObject != currentInteractingGameObject) 
            {
                currentInteractingGameObject = hit.collider.gameObject;
                currentInteractableInfo = hit.collider.GetComponent<IInteractable>();
                SetPromptText();
            }
        }
        else // 빈 공간에 레이를 쐈을 경우
        {
            currentInteractingGameObject = null;
            currentInteractableInfo = null;
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    private void SetPromptText()
    {
        interactionPrompt.gameObject.SetActive(true);
        promptText.text = currentInteractableInfo.GetInteractionPrompt();
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        // ---- for moving platforms -----
        if (collision.gameObject.tag == "MovingPlatform")
        {
            transform.SetParent(collision.transform);
        }

        // ---- for jumping platforms -----
        if (collision.gameObject.tag == "JumpPanel")
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 1f, rigidbody.velocity.z);

            float jumpForce = 500f;
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            transform.SetParent(null);
        }
    }
    #endregion
}

