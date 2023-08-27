using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeySetting : MonoBehaviour
{
    public PauseCtrl pauseCtrlInstance;

    public KeyCode defaultKeyCode;         //인스펙터에서 지정해 줘야 함.
    public GameObject keyText;             //인스펙터에서 지정해 줘야 함.
    private EventSystem eventSystem;       //새로운 키를 받을 때 UI가 클릭 안 되게 하기 위해.
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
        Debug.Log("버튼을 누르셨습니다. 아무 키를 누르세요.");

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
                //Escape키를 누르거나 해서 None이 되었을 때.
                if (newKeyCode == KeyCode.None)
                    newKeyCode = defaultKeyCode;
                Debug.Log(newKeyCode);
                keyText.GetComponent<TextMeshProUGUI>().text = System.Enum.GetName(typeof(KeyCode), newKeyCode);
                waitingForInput = false;
            }

            yield return null;
        }

        Debug.Log("입력 기다리기 종료");
        eventSystem.enabled = true;
        pauseCtrlInstance.GetComponent<PauseCtrl>().isOnKeySetting = false;
        
    }
}
