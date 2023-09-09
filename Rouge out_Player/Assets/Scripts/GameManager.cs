using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalCoin;
    public int stageCoin;
    public int stageIndex;
    
    public Player player;
    public GameObject[] Stages;

    public void NextStage()
    {
        //��ȭ ���
        totalCoin += stageCoin;
        stageCoin = 0;

        //�������� ��ȯ
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
        }

        //���� Ŭ����
        else
        {
            Time.timeScale = 0;
            Debug.Log("���� Ŭ����!");
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, 0);
        player.VelocityZero();
    }
}