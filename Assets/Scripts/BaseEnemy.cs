using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{



    protected int currentHealth;
    protected float distanceToTarget;
    protected Transform target;
    protected virtual void Start()
    {
        
    }
    protected virtual void Awake()
    {

        target = GameObject.FindGameObjectWithTag("Player").transform; //플레이어에 Player 태그 추가
    }
    protected virtual void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
