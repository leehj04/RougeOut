using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseCanvas;

    [SerializeField]
    private GameObject exitCheck;

    [SerializeField]
    public GameObject pauseBackImage;

    private bool isOnPause = false;
    private bool isOnExit = false;

    private void Start()
    {
        pauseCanvas.SetActive(false);
    }

    public void OnOffPauseButton()
    {
        isOnPause = !isOnPause;
        pauseCanvas.SetActive(isOnPause);
        exitCheck.SetActive(false);

        if (isOnPause)
        {
            Time.timeScale = 0f;
        }
        else
            Time.timeScale = 1.0f;
    }

    public void OnOffExitCheck()
    {
        isOnExit = !isOnExit;
        pauseBackImage.GetComponent<CanvasGroup>().interactable = !pauseBackImage.GetComponent<CanvasGroup>().interactable;
        exitCheck.SetActive(isOnExit);
    }
   
    public bool ReturnBoolPause()
    {
        return isOnPause;
    }
    public bool ReturnBoolExit()
    {
        return isOnExit;
    }
}
