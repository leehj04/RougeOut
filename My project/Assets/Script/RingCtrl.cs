using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RingCtrl : MonoBehaviour
{
    GameObject inventoryCanvas;
    GameObject ring1Box;
    private void Awake()
    {
        inventoryCanvas = GameObject.Find("InventoryCanvas");
        ring1Box = GameObject.Find("Ring1Box");
    }
    public int whereIsItEquiped; //0: 인벤토리 1: 반지1칸, 2: 반지2칸 에 위치하고 있음을 나타냄
    public int myRingNum;        //어떤 반지 아이템인지 확인. InventoryCtrl에서 이 스크립트가 생성 될 때 값이 지정됨.
    
    private enum ItisEquipingIn
    {
        Inventory = 0,
        Ring1 = 1,
        Ring2 = 2

    }
    public void OnoffEquipRing()
    {
        //반지 1번 칸이 비었을 때
        if(!DataCtrl.isRing1Equiped)
        {
            //모두 비었을 때
            if(!DataCtrl.isRing2Equiped)
            {
                if(whereIsItEquiped == (int)ItisEquipingIn.Inventory)      //반지를 장착 하는 경우
                {
                    DataCtrl.isRing1Equiped = true;
                    DataCtrl.instance.playerData.rings[myRingNum]--;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetEquipment(2, myRingNum);
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    whereIsItEquiped = 1;
                }
            }
            //반지 1번 칸만 비었을 때
            else if(DataCtrl.isRing2Equiped)
            {
                if(whereIsItEquiped == (int)ItisEquipingIn.Inventory)      //반지를 장착 하는 경우
                {
                    DataCtrl.isRing1Equiped = true;
                    DataCtrl.instance.playerData.rings[myRingNum]--;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetEquipment(2, myRingNum);
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    whereIsItEquiped = 1;
                }
                else if(whereIsItEquiped == (int)ItisEquipingIn.Ring2) //두 번째 반지를 해제하는 경우
                {
                    DataCtrl.isRing2Equiped = false;
                    DataCtrl.instance.playerData.rings[myRingNum]++;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    Destroy(gameObject);
                }
            }
        }
        //반지 1번칸이 차있을 때
        else if(DataCtrl.isRing1Equiped)
        {
            //반지 2번 칸만 비었을 때 => 예만 2번째 칸에 넣으면 됨
            if (!DataCtrl.isRing2Equiped)
            {
                if(whereIsItEquiped == (int)ItisEquipingIn.Inventory)      //반지를 장착하는 경우
                {
                    DataCtrl.isRing2Equiped = true;
                    DataCtrl.instance.playerData.rings[myRingNum]--;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetEquipment(3, myRingNum);
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    whereIsItEquiped = 2;
                }
                else if(whereIsItEquiped == (int)ItisEquipingIn.Ring1) //첫 번째 반지를 해제하는 경우
                {
                    DataCtrl.isRing1Equiped = false;
                    DataCtrl.instance.playerData.rings[myRingNum]++;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    Destroy(gameObject);
                }
            }
            //모두 차 있을 때
            else
            {
                if (whereIsItEquiped == (int)ItisEquipingIn.Inventory)    //반지를 교체하는 경우
                {
                    ring1Box.GetComponentInChildren<RingCtrl>().OnoffEquipRing();
                    OnoffEquipRing();
                }
                else if(whereIsItEquiped == (int)ItisEquipingIn.Ring1) //첫 번째 반지를 해제하는 경우
                {
                    DataCtrl.isRing1Equiped = false;
                    DataCtrl.instance.playerData.rings[myRingNum]++;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    Destroy(gameObject);
                }
                else                           //두 번째 반지를 해제하는 경우
                {
                    DataCtrl.isRing2Equiped = false;
                    DataCtrl.instance.playerData.rings[myRingNum]++;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    Destroy(gameObject);
                }
            }
        }
    }
}
