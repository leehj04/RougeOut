using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuit : MonoBehaviour
{
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ������ ��忡���� �����͸� ����
#else
        Application.Quit(); // ����� ���ø����̼ǿ����� ���ø����̼��� ����
#endif
    }
}
