using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* [ClassINFO : ItemObject]
   @ Description : This class is used to hold a scriptable object itemData data in an itemData prefab.
   @ Attached at : Item_Name(prefab)
   @ Methods : ============================================
               [public]
               - GetInteractionPrompt() : Returns the interaction prompt for the itemData.
               - WhenInteracted() : Handles the interaction with the itemData when interacted with it.
               ============================================
               [private]
               - None
               ============================================
*/

/* [InterfaceINFO : IInteractable]
   @ Description: This interface is used to define the interaction methods for interactable objects.
   @ Methods : ============================================
               -GetInteractionPrompt() : Returns the interaction prompt for the object.
               - WhenInteracted() : Handles the interaction with the object when interacted with it.
               ============================================
*/

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
    #region [Reference to ItemData]
    [Header("Item Data")]
    public ItemData itemData;
    #endregion

    // =========================== //
    //   [IInteractable Methods]
    // =========================== //
    #region [IInteractable Methods]
    public string GetInteractionPrompt()
    {
        string prompt = $"<b>{itemData.itemName}</b>\n{itemData.itemDescription}";
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
