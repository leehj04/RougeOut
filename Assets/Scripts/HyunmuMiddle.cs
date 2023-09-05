using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyunmuMiddle : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float chaseSpeed = 4.0f;
    public float attackDistance = 1.5f;
    public float chaseDistance = 5.0f;
    public float returnDistance = 7.0f;
    private int maxHealth = 300;
    private bool isMoving = false;
    private bool hasAttacked = false;
    private int currentHealth;
    private Vector3 initialScale;
    public Transform pos;
    public Vector2 boxSize;
    private Transform target;

    private Vector3 initialPosition;
    private Animator AnimHyunmuMiddle;

    private enum EnemyState
    {
        Idle,
        Chasing,
        Attacking,
        Returning
    }

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform; //플레이어에 Player 태그 추가
        initialScale = transform.localScale;
        AnimHyunmuMiddle = GetComponent<Animator>();

    }
    private EnemyState currentState = EnemyState.Idle;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                MoveBetweenPoints(initialPosition + Vector3.left * 2.0f, initialPosition + Vector3.right * 2.0f, moveSpeed);
                if (distanceToPlayer <= chaseDistance)
                {
                    currentState = EnemyState.Chasing;
                }
                break;

            case EnemyState.Chasing:
                if (distanceToPlayer <= attackDistance)
                {
                    AnimHyunmuMiddle.SetTrigger("attack");
                    currentState = EnemyState.Attacking;
                    
                }
                else if (distanceToPlayer > chaseDistance)
                {
                    currentState = EnemyState.Returning;
                }
                else
                {
                    Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
                    MoveTowards(targetPosition, chaseSpeed);
                }
                break;

            case EnemyState.Attacking:
                
                if (distanceToPlayer > attackDistance)
                {
                    currentState = EnemyState.Chasing;
                }
                break;

            case EnemyState.Returning:
                MoveTowards(initialPosition, moveSpeed);
                if (Vector3.Distance(transform.position, initialPosition) <= 0.1f)
                {
                    currentState = EnemyState.Idle;
                }
                break;


        }
        if (currentState == EnemyState.Chasing || currentState == EnemyState.Returning)
        {
            isMoving = true;
            if (target.position.x < transform.position.x)
            {
                Flip(false); 
            }
            else
            {
                Flip(true); 
            }
        }
        else
        {
            isMoving = false;
        }
        AnimHyunmuMiddle.SetBool("walk", isMoving);
        
        
    }

    private void Flip(bool facingRight)
    {
        if (facingRight)
        {
            transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }
    }


    private void MoveBetweenPoints(Vector3 pointA, Vector3 pointB, float speed)
    {
        transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time * speed, 1));
    }

    private void MoveTowards(Vector3 targetPosition, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    private void TakeDamage(int amount)
    {
        AnimHyunmuMiddle.SetTrigger("hurt");
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        AnimHyunmuMiddle.SetTrigger("death");
        Destroy(gameObject, 1.0f);
    }
    private void IsAttackFrame()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                Debug.Log("attacked");
            }
        }
    }

}