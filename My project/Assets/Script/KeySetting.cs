using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeySetting : MonoBehaviour
{
    public PauseCtrl pauseCtrlInstance;

    private KeyCode currentKeyCode;
    public GameObject keyText;             //�ν����Ϳ��� ������ ��� ��.
    private EventSystem eventSystem;       //���ο� Ű�� ���� �� UI�� Ŭ�� �� �ǰ� �ϱ� ���� ���.
    private bool waitingForInput = false;  //�ڷ�ƾ �� �ݺ��� Ż������

    private KeyCode newKeyCode;
    public int targetKey;
    
    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    public void StartWaitingForInput()
    {
        pauseCtrlInstance.GetComponent<PauseCtrl>().isOnKeySetting = true;  //isonKeySetting�� ���� �ٲٸ�, GameCtrl���� ReturnBoolKeySetting�� false�� �Ǽ� EscŰ�� ����� ������ �ȵǰ� ����� ����
        eventSystem.enabled = false;
        StartCoroutine(WaitForPlayerInput());
    }

    private System.Collections.IEnumerator WaitForPlayerInput()
    {
        switch (targetKey)
        {
            case 0:
                currentKeyCode = DataCtrl.instance.mappedKey.Skill_1_KeyCode;
                break;
            case 1:
                currentKeyCode = DataCtrl.instance.mappedKey.Skill_2_KeyCode;
                break;
            case 2:
                currentKeyCode = DataCtrl.instance.mappedKey.Skill_3_KeyCode;
                break;
            case 3:
                currentKeyCode = DataCtrl.instance.mappedKey.Skill_4_KeyCode;
                break;
            case 4:
                currentKeyCode = DataCtrl.instance.mappedKey.Item_1_KeyCode;
                break;
            case 5:
                currentKeyCode = DataCtrl.instance.mappedKey.Item_2_KeyCode;
                break;
            case 6:
                currentKeyCode = DataCtrl.instance.mappedKey.MoveForward;
                break;
            case 7:
                currentKeyCode = DataCtrl.instance.mappedKey.MoveBackward;
                break;
            case 8:
                currentKeyCode = DataCtrl.instance.mappedKey.MoveUp;
                break;
            case 9:
                currentKeyCode = DataCtrl.instance.mappedKey.MoveDown;
                break;
            case 10:
                currentKeyCode = DataCtrl.instance.mappedKey.Jump;
                break;
            case 11:
                currentKeyCode = DataCtrl.instance.mappedKey.Attack;
                break;
            case 12:
                currentKeyCode = DataCtrl.instance.mappedKey.Inventory;
                break;
            default:
                break;
        }
        waitingForInput = true;

        while (waitingForInput)
        {
            if (Input.anyKey)
            {
                foreach (KeyCode privateKeyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(privateKeyCode) && privateKeyCode != KeyCode.Escape)
                    {
                        newKeyCode = privateKeyCode;
                        break;
                    }
                }

                //EscapeŰ�� �����ų� �ؼ� None�� �Ǿ��� �� || ���콺 ���ʹ�ư�� ������ ��
                if (newKeyCode == KeyCode.None || newKeyCode == KeyCode.Mouse0)
                    newKeyCode = currentKeyCode;

                waitingForInput = false;
            }

            yield return null;
        }
        switch(targetKey)
        {
            case 0:
                DataCtrl.instance.mappedKey.Skill_1_KeyCode = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.Skill_1_KeyCode = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 1:
                DataCtrl.instance.mappedKey.Skill_2_KeyCode = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.Skill_2_KeyCode = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 2:
                DataCtrl.instance.mappedKey.Skill_3_KeyCode = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.Skill_3_KeyCode = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 3:
                DataCtrl.instance.mappedKey.Skill_4_KeyCode = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.Skill_4_KeyCode = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 4:
                DataCtrl.instance.mappedKey.Item_1_KeyCode = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.Item_1_KeyCode = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 5:
                DataCtrl.instance.mappedKey.Item_2_KeyCode = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.Item_2_KeyCode = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 6:
                DataCtrl.instance.mappedKey.MoveForward = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.MoveForward = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 7:
                DataCtrl.instance.mappedKey.MoveBackward = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.MoveBackward= currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 8:
                DataCtrl.instance.mappedKey.MoveUp = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.MoveUp = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 9:
                DataCtrl.instance.mappedKey.MoveDown = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.MoveDown = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 10:
                DataCtrl.instance.mappedKey.Jump = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.Jump = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 11:
                DataCtrl.instance.mappedKey.Attack = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.Attack = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            case 12:
                DataCtrl.instance.mappedKey.Inventory = newKeyCode;
                DataCtrl.instance.mappedKey.AddList();
                if (DataCtrl.instance.mappedKey.FindDuplication())
                {
                    DataCtrl.instance.mappedKey.Inventory = currentKeyCode;
                    newKeyCode = currentKeyCode;
                }
                break;
            default:
                break;
        }
        keyText.GetComponent<TextMeshProUGUI>().text = System.Enum.GetName(typeof(KeyCode), newKeyCode);
        eventSystem.enabled = true;
        pauseCtrlInstance.GetComponent<PauseCtrl>().isOnKeySetting = false;
        
    }
}
