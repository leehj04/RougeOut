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

    [SerializeField]
    private GameObject settingBackImage;

    private int currentImage = 0;
    private bool isOnPause = false;
    private bool isOnExit = false;
    private bool isOnOption = false;
    

    private void Start()
    {
        pauseCanvas.SetActive(false);
    }

    public void OnOffPauseButton()
    {
        switch(currentImage)
        {
            case 0:
                isOnPause = !isOnPause;
                pauseCanvas.SetActive(isOnPause);
                exitCheck.SetActive(false);
                settingBackImage.SetActive(false);
                if (!pauseBackImage.GetComponent<CanvasGroup>().interactable)
                    pauseBackImage.GetComponent<CanvasGroup>().interactable = !pauseBackImage.GetComponent<CanvasGroup>().interactable;
                if (isOnPause)
                    Time.timeScale = 0f;
                else
                    Time.timeScale = 1.0f;
                break;

            case 1:
                OnOffExitCheck();
                break;

            case 2:
                OnOffSettingButton();
                break;
        }
    }
    public void OnOffExitCheck()
    {
        isOnExit = !isOnExit;
        pauseBackImage.GetComponent<CanvasGroup>().interactable = !pauseBackImage.GetComponent<CanvasGroup>().interactable;
        exitCheck.SetActive(isOnExit);
        if (isOnExit)
            currentImage = 1;
        else
            currentImage = 0;
    }

    public void OnOffSettingButton()
    {
        isOnOption = !isOnOption;
        isOnPause = !isOnPause;
        if (isOnOption)
            currentImage = 2;
        else
            currentImage = 0;
        settingBackImage.SetActive(isOnOption);
        pauseBackImage.SetActive(isOnPause);
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
