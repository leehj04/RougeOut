using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HyunmuFinal : MonoBehaviour
{
    public Player player;

    private Animator anim;
    private Rigidbody2D rb;

    public int currentHP;
    public float attackDistance = 1.2f;
    public float closeAttackDistance = 0.8f;
    public float chaseDistance = 5.0f;
    public float moveSpeed = 2.0f;
    public float chaseSpeed = 4.0f;
    private int maxHealth = 500;
    public int randomAttack1;
    public int randomAttack2;

    public Transform pos;
    public Vector2 boxSize;
    public GameObject bullet;
    public Transform pos1;
    protected Transform target;

    private bool isRunning = false;
    private bool isHurt = false;
    private bool hasExecuted = false;
    private bool isDefending = false;



    private enum EnemyState
    {
        Default,
        Hurt,
        Attacking,
        Defending,
        Dead
    }
    private EnemyState currentState = EnemyState.Default;

    private void Start()
    {
        currentHP = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(RandomAttack());
        StartCoroutine(InstantMove());
    }

    private void Update()
    {
        if (currentHP <= 0 && currentState != EnemyState.Dead)
        {
            Die();
        }

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case EnemyState.Default:
                isHurt = false;
                if (distanceToPlayer <= chaseDistance)
                {
                    currentState = EnemyState.Attacking;
                }
                MoveTowards(target.position, chaseSpeed);
                FlipSpriteBasedOnDirection();
                break;

            case EnemyState.Hurt:
                isHurt = true;
                // Handle hurt animation and logic
                break;

            case EnemyState.Attacking:
                if (distanceToPlayer > attackDistance)
                {
                    currentState = EnemyState.Default;
                }
                else if (distanceToPlayer < closeAttackDistance)
                {
                    randomAttack1 = Random.Range(0, 2);
                    hasExecuted = false;
                    // Randomly select and trigger close range attack animation
                    if (randomAttack1==1)
                    {
                        anim.SetTrigger("underattack");

                    }
                    else
                    {
                        anim.SetTrigger("frontattack");
                    }
                }
                else 
                {
                    randomAttack2 = Random.Range(0, 2);
                    hasExecuted = false;
                    // Randomly select and trigger close range attack animation
                    if (randomAttack2 == 1)
                    {
                        anim.SetTrigger("sideattack");
                    }
                    else
                    {
                        anim.SetTrigger("frontattack");
                    }
                }
                break;

            case EnemyState.Defending:
                // 합치고 추가 예정
                if (Random.Range(0, 100) < 20)
                {
                    anim.SetTrigger("defense");
                    isDefending = true;
                }
                break;

            case EnemyState.Dead:
                break;
        }
        Vector3 velocity = rb.velocity;
   
        float speed = velocity.magnitude;

        float thresholdSpeed = 0.1f; 
        if (speed > thresholdSpeed)
        {
            anim.SetBool("running", true);
            isRunning = true;
        }
        else
        {
            anim.SetBool("running", false);
            isRunning = false;
        }
    }

    private void MoveTowards(Vector3 targetPosition, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void Die()
    {
        currentState = EnemyState.Dead;
        anim.SetTrigger("die");
        Destroy(gameObject, 1.0f); 
    }

    private IEnumerator RandomAttack()
    {
        while (true)
        {
            float randomTime = Random.Range(17.0f, 28.0f);
            yield return new WaitForSeconds(randomTime);

            if (currentState != EnemyState.Dead)
            {
                int randomBullet = Random.Range(4, 8);
                anim.SetBool("longfrontattack", true);
                for (; randomBullet > 0; randomBullet--)
                {
                    Instantiate(bullet, pos1.position, transform.rotation);
                }
                anim.SetBool("longfrontattack", false);
            }
        }
    }

    private IEnumerator InstantMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(30.0f, 50.0f));
            if (currentState != EnemyState.Dead)
            {
                anim.SetTrigger("instantmove");
                yield return new WaitForSeconds(1.3f);

                Vector2 newPosition = target.position + (Vector3)Random.insideUnitCircle.normalized * 1.0f;
                transform.position = newPosition;

                anim.SetTrigger("Instantmove2");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDefending)
        {
            isDefending = false;
        }
        else
        {
            currentHP -= damage;
            // Handle hurt animation and logic
            if (damage >= 40)
            {
                anim.SetTrigger("hurt");
                currentState = EnemyState.Hurt;
            }
        }
    }
    private void FlipSpriteBasedOnDirection()
    {
        if (target.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    private void IsAttackFrame()
    {
        if (!hasExecuted)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Player")
                {
                    player.damage = 70;
                }
            }
            hasExecuted = true;
        }
    }
}
