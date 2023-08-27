using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private GameObject volumeContents;

    [SerializeField]
    private GameObject keyContents;

    [SerializeField]
    private GameObject volumeTab;

    [SerializeField]
    private GameObject keyTab;

    private int currentUI = 0;    //0: ���� â 1: ����ȭ������ ������ â 2: ���� â
    private bool isOnPause = false;
    private bool isOnExit = false;
    private bool isOnSetting = false;
    public bool isOnKeySetting = false;
    private Image volumeTabImage;
    private Image keyTabImage;

    private void Start()
    {
        keyContents.SetActive(false);
        pauseCanvas.SetActive(false);
        volumeTabImage = volumeTab.GetComponent<Image>();
        keyTabImage = keyTab.GetComponent<Image>();
    }
    public void OnOffPauseButton()
    {
        switch(currentUI)
        {
            case 0:
                isOnPause = !isOnPause;
                pauseCanvas.SetActive(isOnPause);
                exitCheck.SetActive(false);
                settingBackImage.SetActive(false);
                if (!pauseBackImage.GetComponent<CanvasGroup>().interactable)  //interactable�� false�̸�, 
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
            currentUI = 1;
        else
            currentUI = 0;
    }
    public void OnOffSettingButton()
    {
        isOnSetting = !isOnSetting;
        isOnPause = !isOnPause;
        if (isOnSetting)
            currentUI = 2;
        else
            currentUI = 0;
        settingBackImage.SetActive(isOnSetting);
        pauseBackImage.SetActive(isOnPause);
    }
    public void OnOffVolumeTab()  //�ش� �ǿ� ������ ��, ���� �����ϴ� ����� ���� �Լ�
    {
        volumeTabImage.color = new Color(volumeTabImage.color.r, volumeTabImage.color.g, volumeTabImage.color.b, 7 / 255.0f);
        keyTabImage.color = new Color(keyTabImage.color.r, keyTabImage.color.g, keyTabImage.color.b, 25 / 255.0f);

        volumeContents.SetActive(true);
        keyContents.SetActive(false);
    }
    public void OnOffKeyTab()
    {
        volumeTabImage.color = new Color(volumeTabImage.color.r, volumeTabImage.color.g, volumeTabImage.color.b, 25 / 255.0f);
        keyTabImage.color = new Color(keyTabImage.color.r, keyTabImage.color.g, keyTabImage.color.b, 7 / 255.0f);

        keyContents.SetActive(true);
        volumeContents.SetActive(false);

    }  //�ش� �ǿ� ������ ��, ���� �����ϴ� ����� ���� �Լ�
    public bool ReturnBoolPause()
    {
        return isOnPause;
    }
    public bool ReturnBoolSetting()
    {
        return isOnSetting;
    }
    public bool ReturnBoolKeySetting()
    {
        return isOnKeySetting;
    }
}
