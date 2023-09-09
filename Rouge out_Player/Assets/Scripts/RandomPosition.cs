using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    public float minX = -100f;  // X 좌표의 최소값
    public float maxX = 100f;   // X 좌표의 최대값
    public float minY = -100f;  // Y 좌표의 최소값
    public float maxY = 100f;   // Y 좌표의 최대값

    void Start()
    {
        // 랜덤한 X와 Y 좌표를 생성
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // 오브젝트의 위치를 랜덤한 좌표로 설정
        transform.position = new Vector3(randomX, randomY, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}