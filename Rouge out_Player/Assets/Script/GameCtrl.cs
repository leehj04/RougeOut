using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static KeySetting;

public class GameCtrl : MonoBehaviour
{
    private static GameCtrl Instance;
    public static GameCtrl instance
    {
        set
        {
            if (Instance == null)
                Instance = value;
        }
        get { return Instance; }
    }
    public InventoryCtrl inventoryCtrlInstance;
    public PauseCtrl pauseCtrlInstance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(DataCtrl.instance.mappedKey.Inventory))
        {
            if(!pauseCtrlInstance.ReturnBoolPause() && !pauseCtrlInstance.ReturnBoolSetting())
                inventoryCtrlInstance.OnOffInventoryButton();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DataCtrl.instance.playerData.potions[Random.Range(0, 9)]++;
            DataCtrl.instance.playerData.weapons[Random.Range(0, 4)]++;
            DataCtrl.instance.playerData.armors[Random.Range(0, 3)]++;
            DataCtrl.instance.playerData.rings[Random.Range(0, 3)]++;
        }
            
        
        if (Input.GetKeyDown(DataCtrl.instance.mappedKey.Menu))
        {
            if (!inventoryCtrlInstance.ReturnBoolInventory() && !pauseCtrlInstance.ReturnBoolKeySetting())
                pauseCtrlInstance.OnOffPauseButton();
        }
    }
}
