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
    public List<Weapon> weapons = new List<Weapon>();
    public List<Armor> armors = new List<Armor>();
    public List<Ring> rings = new List<Ring>();

    public static bool isWeaponEquiped;
    public static bool isArmorEquiped;
    public static bool isRing1Equiped;
    public static bool isRing2Equiped;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerData = new PlayerData(0);
        for (int i = 0; i < Potion.PotionNum; i++)
            potions.Add(new Potion(i));
        for (int i = 0; i < Weapon.WeaponNum; i++)
            weapons.Add(new Weapon(i));
        for (int i = 0; i < Armor.ArmorNum; i++)
            armors.Add(new Armor(i));
        for (int i = 0; i < Ring.RingNum; i++)
            rings.Add(new Ring(i));

        isWeaponEquiped = false;
        isArmorEquiped = false;
        isRing1Equiped = false;
        isRing2Equiped = false;
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
    //정수 배열 선언 => 인덱스별로 담긴 수는 해당 아이템을 가진 수를 뜻함
    public int[] potions;
    public int[] weapons;
    public int[] armors;
    public int[] rings;
    public float HP;

    public long gold;
    public PlayerData(long _gold)
    {
        gold = _gold;
        potions = new int[Potion.PotionNum + 1];
        weapons = new int[Weapon.WeaponNum + 1];
        armors = new int[Armor.ArmorNum + 1];
        rings = new int[Ring.RingNum + 1];
        HP = 100.0f;
    }
}
public class Potion
{
    //게임내에서 존재하는 포션의 수
    public static int PotionNum = 11;
    //potions 배열에서 쓰이는 potion인덱스
    public int potionCode;
    public Sprite sprite;
        
    public Potion(int _potionCode)
    {
        potionCode = _potionCode;
        sprite = Resources.LoadAll<Sprite>("Potion")[potionCode];
    }
}
public class Weapon
{
    public static int WeaponNum = 4;

    public int weaponCode;
    public Sprite sprite;

    public Weapon(int _weaponCode)
    {
        weaponCode = _weaponCode;
        sprite = Resources.LoadAll<Sprite>("Weapon")[weaponCode];
    }
}
public class Armor
{
    public static int ArmorNum = 3;

    public int armorCode;
    public Sprite sprite;

    public Armor(int _armorCode)
    {
        armorCode = _armorCode;
        sprite = Resources.LoadAll<Sprite>("Armor")[armorCode];
    }
}

public class Ring
{
    public static int RingNum = 3;

    public int ringCode;
    public Sprite sprite;

  
    public Ring(int _ringCode)
    {
        ringCode = _ringCode;
        sprite = Resources.LoadAll<Sprite>("Ring")[ringCode];
    }
}
