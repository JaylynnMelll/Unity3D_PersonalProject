using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* [ClassINFO : ButtonTracker]
   @ Description : This class is used to track the currently selected button in the inventory UI and manage its outline.
   @ Attached at : UI -> InventoryUI -> InventoryBG -> ItemSlots
   @ Methods : =============================================
               [public]
               - TrackClickedButton() : Track the clicked button and manage its outline.
               =============================================
    */

public class ButtonTracker : MonoBehaviour
{
    private GameObject lastSelectedButton;
    private GameObject currentSelectedButton;

    public void TrackClickedButton()
    {
        // if last selected button is not null, disable its outline
        // Detect the currentSelected button through event system
        // turn on the outline of the current button if not null
        // make the current button the last selected button
        if (lastSelectedButton != null)
        {
            Outline lastButtonOutline = lastSelectedButton.transform.Find("Icon")?.GetComponent<Outline>();
            if (lastButtonOutline != null)
            {
                lastButtonOutline.enabled = false;
            }
        }

        GameObject currentSelectedButton = EventSystem.current.currentSelectedGameObject;

        if (currentSelectedButton != null)
        {
            Outline currentButtonOutline = currentSelectedButton.transform.Find("Icon")?.GetComponent<Outline>();
            if (currentButtonOutline != null)
            {
                currentButtonOutline.enabled = true;
            }
        }

        lastSelectedButton = currentSelectedButton;
    }
}
