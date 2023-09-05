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

        target = GameObject.FindGameObjectWithTag("Player").transform; //�÷��̾ Player �±� �߰�
    }
    protected virtual void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //���� ������ �޴� �� �ϴ� �÷��̾ ���� �������� ������ �߰�, �浹�� ���� ���� �����ϰ� �ֱ� �ѵ� ��ġ�� �߰��ϴ� �� ���� �� �����ϴ�




    

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
