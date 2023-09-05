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

    //적이 공격을 받는 건 일단 플레이어가 적의 방향으로 공격을 했고, 충돌을 했을 때를 생각하고 있긴 한데 합치고 추가하는 게 좋을 것 같습니당




    

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
