using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryListManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject inventoryList;
    [SerializeField] private GameObject openButton;
    [SerializeField] private GameObject closeButton;

    [SerializeField] private Transform inventoryContentTransform;

    //---------------------//
    public void OpenCloseInventory(int value)
    //---------------------//
    {
        if (value == 0)
        {
            openButton.SetActive(false);
            closeButton.SetActive(true);
            inventoryList.SetActive(true);
        }
        else if (value == 1)
        {
            openButton.SetActive(true);
            closeButton.SetActive(false);
            inventoryList.SetActive(false);
        }

    }//END PlayMusic




}//END InventoryListManager
