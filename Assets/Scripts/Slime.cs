using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseEnemy
{

    private Animator Enemy_GreenSlime;
    private Rigidbody2D rb;



    private float moveSpeed = 1.5f;
    private int maxHealth = 100;
    private int  damage= 10;
    private float chaseDistance = 4f;
    public float jumpForce = 5f;

    private bool isJumping = false;

    protected override void Start()
    {
        base.Start(); 

        currentHealth = maxHealth; 
    }

    protected override void Awake()
    {
        base.Awake();
        Enemy_GreenSlime = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {

        base.Update();
        if (distanceToTarget <= chaseDistance && Mathf.Abs(target.position.y - transform.position.y) > 1f)
        {
            Jump();
        }
        else if (distanceToTarget <= chaseDistance)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {

        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

    }

    private void Jump()
    {
        if (!isJumping)
        {
            Enemy_GreenSlime.SetTrigger("Jump");
            isJumping = true; 
           
            StartCoroutine(WaitForJumpAnimation());

        }
    }

    private IEnumerator WaitForJumpAnimation()
    {
        yield return new WaitForSeconds(1.12f);
        Enemy_GreenSlime.SetTrigger("Jump2");
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
        StartCoroutine(WaitForRealJump());

    }
    private IEnumerator WaitForRealJump()
    {
        yield return new WaitForSeconds(0.64f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            isJumping = false;
        }
        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerMove.getDamage();
        }
    }


    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        Enemy_GreenSlime.SetTrigger("Hurt");
    }

    public override void Die()
    {
        base.Die();
        Enemy_GreenSlime.SetTrigger("Death");
    }
}
