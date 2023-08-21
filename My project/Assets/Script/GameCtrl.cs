using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    private void Awake()
    {
        Instance = this;
    }

    public InventoryCtrl inventoryCtrlInstance;
    public PauseCtrl pauseCtrlInstance;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!pauseCtrlInstance.ReturnBoolPause())
                inventoryCtrlInstance.OnOffInventoryButton();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DataCtrl.instance.playerData.potions[Random.Range(0, 11)]++;
            DataCtrl.instance.playerData.weapons[Random.Range(0, 3)]++;
            DataCtrl.instance.playerData.armors[Random.Range(0, 2)]++;
            DataCtrl.instance.playerData.rings[Random.Range(0, 2)]++;
        }
            

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!inventoryCtrlInstance.ReturnBoolInventory())
                pauseCtrlInstance.OnOffPauseButton();
        }
    }
}
