using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UI;

public class DataCtrl : MonoBehaviour
{
    private static DataCtrl Instance;

    public static DataCtrl instance
    {
        set
        {
            if (Instance == null)
                Instance = value;
        }
        get { return Instance; }
    }

    public PlayerData playerData;
    public List<Potion> potions = new List<Potion>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerData = new PlayerData(0);
        for (int i = 0; i < Potion.PotionNum; i++)
            potions.Add(new Potion(i));
    }

    [SerializeField]
    private GameObject masterVolumeScrollbar;

    [SerializeField]
    private GameObject musicVolumeScrollbar;

    [SerializeField]
    private GameObject battleEffectVolumeScrollbar;

    [SerializeField]
    private GameObject masterVolumeText;

    [SerializeField]
    private GameObject musicVolumeText;

    [SerializeField]
    private GameObject battleEffectVolumeText;

    private float masterVolume;
    private float musicVolume;
    private float battleEffectVolume;

    public void MasterVolumeSetting()
    {
        masterVolume = masterVolumeScrollbar.GetComponent<Scrollbar>().value;
        masterVolumeText.GetComponent<TextMeshProUGUI>().text = "Master Volume: " + Mathf.RoundToInt(masterVolume*100.0f).ToString();
    }
    public void MusicVolumeSetting()
    {
        musicVolume = musicVolumeScrollbar.GetComponent<Scrollbar>().value;
        musicVolumeText.GetComponent<TextMeshProUGUI>().text = "Music Volume: " + Mathf.RoundToInt(musicVolume * 100.0f).ToString();

    }
    public void BattleEffectVolumeSetting()
    {
        battleEffectVolume = battleEffectVolumeScrollbar.GetComponent<Scrollbar>().value;
        battleEffectVolumeText.GetComponent<TextMeshProUGUI>().text = "Battle Effect Volume: " + Mathf.RoundToInt(battleEffectVolume*100.0f).ToString();
    }
}
public struct PlayerData
{
    public int[] potions;
    public long gold;
    public PlayerData(long _gold)

    {
        gold = _gold;
        potions = new int[Potion.PotionNum];
    }
}
public class Potion
{
    public static int PotionNum = 4;

    public int potionCode;
    public Sprite sprite;
        
    public Potion(int _potionCode)
    {
        potionCode = _potionCode;
        sprite = Resources.LoadAll<Sprite>("Potion")[potionCode];
    }
}

