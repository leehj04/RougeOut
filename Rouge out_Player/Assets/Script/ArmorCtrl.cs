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

    public bool isInEquip; //게임 오브젝트로써 장비창 안에 있는지 확인하는 변수
    public int myArmorNum; //어떤 방어구 아이템인지 확인. InventoryCtrl에서 이 스크립트가 생성 될 때 값이 지정됨.

    public void OnOffEquipArmor()
    {
        //장비창에 장비가 착용되어있지 않을 때
        if (!DataCtrl.isArmorEquiped)
        {
            //이 스크립트를 가진 장비가 인벤토리에 있을 때(장비 착용)
            if (!isInEquip)
            {
                DataCtrl.isArmorEquiped = true;
                DataCtrl.instance.playerData.armors[myArmorNum]--;
                inventoryCanvas.GetComponent<InventoryCtrl>().SetEquipment(1, myArmorNum);
                inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                isInEquip = true;
            }
        }
        //장비가 이미 착용되어 있을 때
        else if (DataCtrl.isArmorEquiped)
        {
            //이 스크립트를 가진 장비가 인벤토리에 있을 때(장비 교체)
            if (!isInEquip)
            {
                armorBox.GetComponentInChildren<ArmorCtrl>().OnOffEquipArmor();
                OnOffEquipArmor();
            }
            //이 스크립트를 가진 장비가 장비창에 있을 때(장비 해제)
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
