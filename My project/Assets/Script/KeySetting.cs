using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeySetting : MonoBehaviour
{
    public PauseCtrl pauseCtrlInstance;

    public KeyCode defaultKeyCode;         //�ν����Ϳ��� ������ ��� ��.
    public GameObject keyText;             //�ν����Ϳ��� ������ ��� ��.
    private EventSystem eventSystem;       //���ο� Ű�� ���� �� UI�� Ŭ�� �� �ǰ� �ϱ� ����.
    private bool waitingForInput = false;
    KeyCode newKeyCode;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }
    public void StartWaitingForInput()
    {
        pauseCtrlInstance.GetComponent<PauseCtrl>().isOnKeySetting = true;
        eventSystem.enabled = false;
        StartCoroutine(WaitForPlayerInput());
    }

    private System.Collections.IEnumerator WaitForPlayerInput()
    {
        waitingForInput = true;
        Debug.Log("��ư�� �����̽��ϴ�. �ƹ� Ű�� ��������.");

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
                Debug.Log(newKeyCode);
                //EscapeŰ�� �����ų� �ؼ� None�� �Ǿ��� ��.
                if (newKeyCode == KeyCode.None)
                    newKeyCode = defaultKeyCode;
                Debug.Log(newKeyCode);
                keyText.GetComponent<TextMeshProUGUI>().text = System.Enum.GetName(typeof(KeyCode), newKeyCode);
                waitingForInput = false;
            }

            yield return null;
        }

        Debug.Log("�Է� ��ٸ��� ����");
        eventSystem.enabled = true;
        pauseCtrlInstance.GetComponent<PauseCtrl>().isOnKeySetting = false;
        
    }
}
