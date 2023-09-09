using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCtrl : MonoBehaviour
{
    GameObject inventoryCanvas;
    public int myPotionNum;
    private void Awake()
    {
        inventoryCanvas = GameObject.Find("InventoryCanvas");
    }
    public void DrinkPotion()
    {
        DataCtrl.instance.playerData.potions[myPotionNum]--;
        /*
        DataCtrl.instance.playerData.HP += 40.0f;
        if (DataCtrl.instance.playerData.HP > 100.0f)
            DataCtrl.instance.playerData.HP = 100.0f;
        inventoryCanvas.GetComponent<InventoryCtrl>().SetContent(0);
        */
    }
}
