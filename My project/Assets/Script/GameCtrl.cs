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
    /*
    private KeyCode pauseKey;
    private KeyCode inventoryKey;
    */
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        /*
        KeySetting keySetting = GetComponent<KeySetting>();
        if (keySetting != null)
        {
            InputBinding inputBinding = new InputBinding(keySetting.Bindings);
            if (inputBinding != null)
            {
                // 원하는 바인딩 값에 접근하여 사용한다.
                inventoryKey = inputBinding.Bindings[KeySetting.UserAction.UI_Inventory];
                pauseKey = inputBinding.Bindings[KeySetting.UserAction.UI_Pause];
            }
        }
        */
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
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
            
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!inventoryCtrlInstance.ReturnBoolInventory())
                pauseCtrlInstance.OnOffPauseButton();
        }
        /*
            if (Input.anyKeyDown)
                {
                    string key = Input.inputString;
                    Debug.Log("Key pressed: " + key);
                }
        */
    }
}
