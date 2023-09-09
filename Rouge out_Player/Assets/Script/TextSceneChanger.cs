using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextSceneChanger : MonoBehaviour
{
    public string nextSceneName;

    // UI Text�� Ŭ������ �� ȣ��� �Լ�
    public void OnUITextClick()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
