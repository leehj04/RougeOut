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
    private GameObject inventorySlot_PotionPrefab;

    [SerializeField]
    private GameObject inventorySlot_EquipmentPrefab;

    [SerializeField]
    private Transform inventoryContentTr;

    private int menuIndex;
    private bool isOnInventory;

    UIBOX temp_UIBox;
    UIBOX[] temp_UIBoxes;

    private void Start()
    {
        inventoryCanvas.SetActive(false);
    }
    public void OnOffInventoryButton()
    {
        isOnInventory = !isOnInventory;
        inventoryCanvas.SetActive(isOnInventory);

        if (isOnInventory) 
        {
            Time.timeScale = 1.0f;

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
                //temp_UIBOxes에 inventoryContentTr아래에 있는 'UIBOX를 컴포넌트로 가지고 있는 모든 부모 오브젝트'를 저장
                temp_UIBoxes = inventoryContentTr.GetComponentsInChildren<UIBOX>();

                for (int i = 0; i < temp_UIBoxes.Length; i++)
                    //기존에 인벤토리에 있던 아이템 갯수 만큼 파괴
                    Destroy(temp_UIBoxes[i].gameObject);
                
                for (int i = 0; i < Potion.PotionNum; i++)
                {
                    if (DataCtrl.instance.playerData.potions[i] > 0)
                    {
                        temp_UIBox = Instantiate(inventorySlot_PotionPrefab, inventoryContentTr).GetComponent<UIBOX>();
                        temp_UIBox.images[0].sprite = DataCtrl.instance.potions[i].sprite;
                        temp_UIBox.texts[0].text = "x" + DataCtrl.instance.playerData.potions[i];
                    }
                }
                for (int i = 0; i < Weapon.WeaponNum; i++)
                {
                    if (DataCtrl.instance.playerData.weapons[i] > 0)
                    {
                        for (int j = 0; j < DataCtrl.instance.playerData.weapons[i]; j++)
                        {
                            temp_UIBox = Instantiate(inventorySlot_EquipmentPrefab, inventoryContentTr).GetComponent<UIBOX>();
                            temp_UIBox.images[0].sprite = DataCtrl.instance.weapons[i].sprite;
                        }
                    }
                }
                for (int i = 0; i < Armor.ArmorNum; i++)
                {
                    if (DataCtrl.instance.playerData.armors[i] > 0)
                    {
                        for (int j = 0; j < DataCtrl.instance.playerData.armors[i]; j++)
                        {
                            temp_UIBox = Instantiate(inventorySlot_EquipmentPrefab, inventoryContentTr).GetComponent<UIBOX>();
                            temp_UIBox.images[0].sprite = DataCtrl.instance.armors[i].sprite;
                        }
                    }
                }
                for (int i = 0; i < Ring.RingNum; i++)
                {
                    if (DataCtrl.instance.playerData.rings[i] > 0)
                    {
                        for (int j = 0; j < DataCtrl.instance.playerData.rings[i]; j++)
                        {
                            temp_UIBox = Instantiate(inventorySlot_EquipmentPrefab, inventoryContentTr).GetComponent<UIBOX>();
                            temp_UIBox.images[0].sprite = DataCtrl.instance.rings[i].sprite;
                        }
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
