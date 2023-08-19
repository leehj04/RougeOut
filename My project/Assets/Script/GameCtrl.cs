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
            DataCtrl.instance.playerData.potions[0]++;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!inventoryCtrlInstance.ReturnBoolInventory())
                pauseCtrlInstance.OnOffPauseButton();
        }
    }
}
