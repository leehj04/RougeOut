using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuit : MonoBehaviour
{
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 모드에서는 에디터를 종료
#else
        Application.Quit(); // 빌드된 애플리케이션에서는 애플리케이션을 종료
#endif
    }
}
