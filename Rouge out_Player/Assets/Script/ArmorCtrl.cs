using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorCtrl : MonoBehaviour
{
    GameObject inventoryCanvas;
    GameObject armorBox;
    private void Awake()
    {
        inventoryCanvas = GameObject.Find("InventoryCanvas");
        armorBox = GameObject.Find("ArmorBox");
        if (isInEquip) DataCtrl.instance.ArmorCtrl( false, (float)((myArmorNum + 1) * 10) );
    }

    public bool isInEquip; //���� ������Ʈ�ν� ���â �ȿ� �ִ��� Ȯ���ϴ� ����
    public int myArmorNum; //� �� ���������� Ȯ��. InventoryCtrl���� �� ��ũ��Ʈ�� ���� �� �� ���� ������.

    public void OnOffEquipArmor()
    {
        //���â�� ��� ����Ǿ����� ���� ��
        if (!DataCtrl.isArmorEquiped)
        {
            //�� ��ũ��Ʈ�� ���� ��� �κ��丮�� ���� ��(��� ����)
            if (!isInEquip)
            {
                DataCtrl.isArmorEquiped = true;
                DataCtrl.instance.playerData.armors[myArmorNum]--;
                inventoryCanvas.GetComponent<InventoryCtrl>().SetEquipment(1, myArmorNum);
                inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                isInEquip = true;
            }
        }
        //��� �̹� ����Ǿ� ���� ��
        else if (DataCtrl.isArmorEquiped)
        {
            //�� ��ũ��Ʈ�� ���� ��� �κ��丮�� ���� ��(��� ��ü)
            if (!isInEquip)
            {
                armorBox.GetComponentInChildren<ArmorCtrl>().OnOffEquipArmor();
                OnOffEquipArmor();
            }
            //�� ��ũ��Ʈ�� ���� ��� ���â�� ���� ��(��� ����)
            else if (isInEquip)
            {
                DataCtrl.isArmorEquiped = false;
                DataCtrl.instance.playerData.armors[myArmorNum]++;
                inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                Destroy(gameObject);
            }
        }
    }
}
