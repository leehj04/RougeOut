using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalCoin;
    public int stageCoin;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    public void NextStage()
    {
        //�������� ��ȯ
        if(stageIndex < Stages.Length - 1)
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

        //��ȭ ���
        totalCoin += stageCoin;
        stageCoin = 0;
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, 0);
        player.VelocityZero();
    }
    
    /*void Update()
    {
        
    }*/
}
