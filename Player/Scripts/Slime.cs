using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public GameManager gameManager;
    public Player player;

    public int health = 60;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�÷��̾� ����
        if (collision.gameObject.name.Contains("Player"))
            if (player != null)
            {
                gameManager.health -= 10;

            }
    }
}
        
