using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    public float minX = -100f;  // X ��ǥ�� �ּҰ�
    public float maxX = 100f;   // X ��ǥ�� �ִ밪
    public float minY = -100f;  // Y ��ǥ�� �ּҰ�
    public float maxY = 100f;   // Y ��ǥ�� �ִ밪

    void Start()
    {
        // ������ X�� Y ��ǥ�� ����
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // ������Ʈ�� ��ġ�� ������ ��ǥ�� ����
        transform.position = new Vector3(randomX, randomY, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}