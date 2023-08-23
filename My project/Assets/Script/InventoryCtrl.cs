using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

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

    [SerializeField]
    private Transform weaponBoxTr;

    [SerializeField]
    private Transform armorBoxTr;

    [SerializeField]
    private Transform ring1BoxTr;

    [SerializeField]
    private Transform ring2BoxTr;

    [SerializeField]

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

    public void SetContent(int _index)
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
                        PotionCtrl potionCtrlInstance = temp_UIBox.AddComponent<PotionCtrl>();
                        potionCtrlInstance.myPotionNum = i;
                        temp_UIBox.GetComponent<Button>().onClick.AddListener(potionCtrlInstance.DrinkPotion);
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
                            WeaponCtrl weaponCtrlInstance = temp_UIBox.AddComponent<WeaponCtrl>();
                            weaponCtrlInstance.myWeaponNum = i;
                            weaponCtrlInstance.isInEquip = false;
                            temp_UIBox.GetComponent<Button>().onClick.AddListener(weaponCtrlInstance.OnOffEquipWeapon);
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
                            ArmorCtrl armorCtrlInstance = temp_UIBox.AddComponent<ArmorCtrl>();
                            armorCtrlInstance.myArmorNum = i;
                            armorCtrlInstance.isInEquip = false;
                            temp_UIBox.GetComponent<Button>().onClick.AddListener(armorCtrlInstance.OnOffEquipArmor);
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
                            RingCtrl ringCtrlInstance = temp_UIBox.AddComponent<RingCtrl>();
                            ringCtrlInstance.myRingNum = i;
                            ringCtrlInstance.whereIsItEquiped = 0;
                            temp_UIBox.GetComponent<Button>().onClick.AddListener(ringCtrlInstance.OnoffEquipRing);
                        }
                    }
                }
                break;
            case 1:

                break;
        }
    }

    public void SetEquipment(int _EquipmentType, int myEquipmentNum)
    {
        switch(_EquipmentType)
        {
            //무기
            case 0:
                temp_UIBox = Instantiate(inventorySlot_EquipmentPrefab, weaponBoxTr).GetComponent<UIBOX>();
                temp_UIBox.images[0].sprite = DataCtrl.instance.weapons[myEquipmentNum].sprite;
                WeaponCtrl weaponCtrlInstance = temp_UIBox.AddComponent<WeaponCtrl>();
                weaponCtrlInstance.myWeaponNum = myEquipmentNum;
                weaponCtrlInstance.isInEquip = true;
                temp_UIBox.GetComponent<Button>().onClick.AddListener(weaponCtrlInstance.OnOffEquipWeapon);
                temp_UIBox.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                temp_UIBox.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                break;
            //방어구
            case 1:
                temp_UIBox = Instantiate(inventorySlot_EquipmentPrefab, armorBoxTr).GetComponent<UIBOX>();
                temp_UIBox.images[0].sprite = DataCtrl.instance.armors[myEquipmentNum].sprite;
                ArmorCtrl armorCtrlInstance = temp_UIBox.AddComponent<ArmorCtrl>();
                armorCtrlInstance.myArmorNum = myEquipmentNum;
                armorCtrlInstance.isInEquip = true;
                temp_UIBox.GetComponent<Button>().onClick.AddListener(armorCtrlInstance.OnOffEquipArmor);
                temp_UIBox.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                temp_UIBox.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                break;
            //반지1
            case 2:
                temp_UIBox = Instantiate(inventorySlot_EquipmentPrefab, ring1BoxTr).GetComponent<UIBOX>();
                temp_UIBox.images[0].sprite = DataCtrl.instance.rings[myEquipmentNum].sprite;
                RingCtrl ring1CtrlInstance = temp_UIBox.AddComponent<RingCtrl>();
                ring1CtrlInstance.myRingNum = myEquipmentNum;
                ring1CtrlInstance.whereIsItEquiped = 1;
                temp_UIBox.GetComponent<Button>().onClick.AddListener(ring1CtrlInstance.OnoffEquipRing);
                temp_UIBox.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                temp_UIBox.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                break;
            //반지2
            case 3:
                temp_UIBox = Instantiate(inventorySlot_EquipmentPrefab, ring2BoxTr).GetComponent<UIBOX>();
                temp_UIBox.images[0].sprite = DataCtrl.instance.rings[myEquipmentNum].sprite;
                RingCtrl ring2CtrlInstance = temp_UIBox.AddComponent<RingCtrl>();
                ring2CtrlInstance.myRingNum = myEquipmentNum;
                ring2CtrlInstance.whereIsItEquiped = 2;
                temp_UIBox.GetComponent<Button>().onClick.AddListener(ring2CtrlInstance.OnoffEquipRing);
                temp_UIBox.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                temp_UIBox.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                break;
        }
    }
    public bool ReturnBoolInventory()
    {
        return isOnInventory;
    }
}
