using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractionPrompt();
    public void WhenInteracted();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    // =========================== //
    //   [Reference to ItemData]    
    // =========================== //
    #region
    [Header("Item Data")]
    public ItemData itemData;
    #endregion

    // =========================== //
    //   [IInteractable Methods]
    // =========================== //
    #region
    public string GetInteractionPrompt()
    {
        string prompt = $"{itemData.itemName}\n{itemData.itemDescription}";
        return prompt;
    }

    public void WhenInteracted()
    {
        CharacterManager.Instance.Player.itemData = itemData;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
    #endregion
}
