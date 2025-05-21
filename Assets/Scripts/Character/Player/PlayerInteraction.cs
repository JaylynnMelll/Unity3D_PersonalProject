using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
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
    public Camera camera;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]
    // ========================== //
    #region [Unity LifeCycle]
    void Start()
    {
        camera = Camera.main;
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
        Ray ray = camera.ScreenPointToRay(new Vector3((Screen.width / 2), (Screen.height / 2)));
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
    #endregion
}
