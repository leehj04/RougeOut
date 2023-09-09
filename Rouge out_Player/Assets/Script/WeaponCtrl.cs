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

    public bool isInEquip; //���� ������Ʈ�ν� ���â �ȿ� �ִ��� Ȯ���ϴ� ����
    public int myWeaponNum;//� ���� ���������� Ȯ��. InventoryCtrl���� �� ��ũ��Ʈ�� ���� �� �� ���� ������.

    public void OnOffEquipWeapon()
    {
        //���â�� ��� ����Ǿ����� ���� ��
        if (!DataCtrl.isWeaponEquiped)
        {
            //�� ��ũ��Ʈ�� ���� ��� �κ��丮�� ���� ��(��� ����)
            if (!isInEquip)
            {
                DataCtrl.isWeaponEquiped = true;
                DataCtrl.instance.playerData.weapons[myWeaponNum]--;
                inventoryCanvas.GetComponent<InventoryCtrl>().SetEquipment(0, myWeaponNum);
                inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                isInEquip = true;
            }
        }
        //��� �̹� ����Ǿ� ���� ��
        else if (DataCtrl.isWeaponEquiped)
        {
            //�� ��ũ��Ʈ�� ���� ��� �κ��丮�� ���� ��(��� ��ü)
            if (!isInEquip)
            { 
                weaponBox.GetComponentInChildren<WeaponCtrl>().OnOffEquipWeapon();
                OnOffEquipWeapon();
            }
            //�� ��ũ��Ʈ�� ���� ��� ���â�� ���� ��(��� ����)
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
