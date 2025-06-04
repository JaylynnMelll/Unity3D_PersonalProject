using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    private int placeHolder = 0;

    [Header("Resource Info")]
    public ItemData ItemToGive;
    public int quantityPerHit = 1;
    public int hitCapacity;
    #endregion

    // ========================== //
    //     [Unity Lifecycle]
    // ========================== //
    #region [Unity Lifecycle]
    #endregion

    // ========================== //
    //     [public Methods]
    // ========================== //
    #region [public Methods]
    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (hitCapacity <= 0) break;
            hitCapacity--;
            Instantiate(ItemToGive.itemPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }
    }
    #endregion  

    // ========================== //
    //     [private Methods]
    // ========================== //
    #region [private Methods]
    #endregion
}
