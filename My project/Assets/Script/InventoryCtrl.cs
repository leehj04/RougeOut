using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryCanvas;

    [SerializeField]
    private GameObject inventorySlotPrefab;

    [SerializeField]
    private Transform inventoryContentTr;

    private int menuIndex;
    private bool isOnInventory;

    UIBOX temp_UIBox;
    UIBOX[] temp_UIBoxes;

    private void Start()
    {
        inventoryCanvas.SetActive(false);
        Debug.Log(isOnInventory);
    }
    public void OnOffInventoryButton()
    {
        isOnInventory = !isOnInventory;
        inventoryCanvas.SetActive(isOnInventory);

        if (isOnInventory) 
        {
            Time.timeScale = 0f;

            menuIndex = 0;
            SetContent(menuIndex);
        }
        else
            Time.timeScale = 1.0f;
    }

    private void SetContent(int _index)
    {
        switch(_index)
        {
            case 0:
                temp_UIBoxes = inventoryContentTr.GetComponentsInChildren<UIBOX>();

                for (int i = 0; i < temp_UIBoxes.Length; i++)
                    Destroy(temp_UIBoxes[i].gameObject);

                for (int i = 0; i < Potion.PotionNum; i++)
                {
                    if (DataCtrl.instance.playerData.potions[i] > 0)
                    {
                        temp_UIBox = Instantiate(inventorySlotPrefab, inventoryContentTr).GetComponent<UIBOX>();
                        temp_UIBox.images[0].sprite = DataCtrl.instance.potions[i].sprite;
                        temp_UIBox.texts[0].text = "x" + DataCtrl.instance.playerData.potions[i];
                    }
                }
                break;
            case 1:
                break;
        }
    }
    public bool ReturnBoolInventory()
    {
        return isOnInventory;
    }
}
