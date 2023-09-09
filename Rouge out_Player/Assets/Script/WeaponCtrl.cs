using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    GameObject inventoryCanvas;
    GameObject weaponBox;
    private void Awake()
    {
        inventoryCanvas = GameObject.Find("InventoryCanvas");
        weaponBox = GameObject.Find("WeaponBox");
    }

    public bool isInEquip; //게임 오브젝트로써 장비창 안에 있는지 확인하는 변수
    public int myWeaponNum;//어떤 무기 아이템인지 확인. InventoryCtrl에서 이 스크립트가 생성 될 때 값이 지정됨.

    public void OnOffEquipWeapon()
    {
        //장비창에 장비가 착용되어있지 않을 때
        if (!DataCtrl.isWeaponEquiped)
        {
            //이 스크립트를 가진 장비가 인벤토리에 있을 때(장비 착용)
            if (!isInEquip)
            {
                DataCtrl.isWeaponEquiped = true;
                DataCtrl.instance.playerData.weapons[myWeaponNum]--;
                inventoryCanvas.GetComponent<InventoryCtrl>().SetEquipment(0, myWeaponNum);
                inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                isInEquip = true;
            }
        }
        //장비가 이미 착용되어 있을 때
        else if (DataCtrl.isWeaponEquiped)
        {
            //이 스크립트를 가진 장비가 인벤토리에 있을 때(장비 교체)
            if (!isInEquip)
            { 
                weaponBox.GetComponentInChildren<WeaponCtrl>().OnOffEquipWeapon();
                OnOffEquipWeapon();
            }
            //이 스크립트를 가진 장비가 장비창에 있을 때(장비 해제)
            else if(isInEquip)
            {
                DataCtrl.isWeaponEquiped = false;
                DataCtrl.instance.playerData.weapons[myWeaponNum]++;
                inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                Destroy(gameObject);
            }
        }
    }     
}
