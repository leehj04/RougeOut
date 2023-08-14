using System.Collections;
using System.Collections.Generic;
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
    private float musicVolume;
    [SerializeField]
    private GameObject musicVolumeScrollbar;

    public void MusicVolumeSetting()
    {
        musicVolume = musicVolumeScrollbar.GetComponent<Scrollbar>().value;
        Debug.Log(musicVolume);
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

