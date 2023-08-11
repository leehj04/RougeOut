using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextSceneChanger : MonoBehaviour
{
    public string nextSceneName;

    // UI Text를 클릭했을 때 호출될 함수
    public void OnUITextClick()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
