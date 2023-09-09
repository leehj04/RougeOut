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
        //금화 계산
        totalCoin += stageCoin;
        stageCoin = 0;

        //스테이지 전환
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
        }

        //게임 클리어
        else
        {
            Time.timeScale = 0;
            Debug.Log("게임 클리어!");
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, 0);
        player.VelocityZero();
    }
}