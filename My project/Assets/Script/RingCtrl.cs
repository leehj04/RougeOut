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
    public int whereIsItEquiped; //0: �κ��丮 1: ����1ĭ, 2: ����2ĭ �� ��ġ�ϰ� ������ ��Ÿ��
    public int myRingNum;        //� ���� ���������� Ȯ��. InventoryCtrl���� �� ��ũ��Ʈ�� ���� �� �� ���� ������.
    
    public void OnoffEquipRing()
    {
        //���� 1�� ĭ�� ����� ��
        if(!DataCtrl.isRing1Equiped)
        {
            //��� ����� ��
            if(!DataCtrl.isRing2Equiped)
            {
                if(whereIsItEquiped == 0)      //������ ���� �ϴ� ���
                {
                    DataCtrl.isRing1Equiped = true;
                    DataCtrl.instance.playerData.rings[myRingNum]--;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetEquipment(2, myRingNum);
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    whereIsItEquiped = 1;
                }
            }
            //���� 1�� ĭ�� ����� ��
            else if(DataCtrl.isRing2Equiped)
            {
                if(whereIsItEquiped == 0)      //������ ���� �ϴ� ���
                {
                    DataCtrl.isRing1Equiped = true;
                    DataCtrl.instance.playerData.rings[myRingNum]--;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetEquipment(2, myRingNum);
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    whereIsItEquiped = 1;
                }
                else if(whereIsItEquiped == 2) //�� ��° ������ �����ϴ� ���
                {
                    DataCtrl.isRing2Equiped = false;
                    DataCtrl.instance.playerData.rings[myRingNum]++;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    Destroy(gameObject);
                }
            }
        }
        //���� 1��ĭ�� ������ ��
        else if(DataCtrl.isRing1Equiped)
        {
            //���� 2�� ĭ�� ����� �� => ���� 2��° ĭ�� ������ ��
            if (!DataCtrl.isRing2Equiped)
            {
                if(whereIsItEquiped == 0)      //������ �����ϴ� ���
                {
                    DataCtrl.isRing2Equiped = true;
                    DataCtrl.instance.playerData.rings[myRingNum]--;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetEquipment(3, myRingNum);
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    whereIsItEquiped = 2;
                }
                else if(whereIsItEquiped == 1) //ù ��° ������ �����ϴ� ���
                {
                    DataCtrl.isRing1Equiped = false;
                    DataCtrl.instance.playerData.rings[myRingNum]++;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    Destroy(gameObject);
                }
            }
            //��� �� ���� ��
            else
            {
                if (whereIsItEquiped == 0)    //������ ��ü�ϴ� ���
                {
                    ring1Box.GetComponentInChildren<RingCtrl>().OnoffEquipRing();
                    OnoffEquipRing();
                }
                else if(whereIsItEquiped == 1) //ù ��° ������ �����ϴ� ���
                {
                    DataCtrl.isRing1Equiped = false;
                    DataCtrl.instance.playerData.rings[myRingNum]++;
                    inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
                    Destroy(gameObject);
                }
                else                           //�� ��° ������ �����ϴ� ���
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
