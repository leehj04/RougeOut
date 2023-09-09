using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Slime : MonoBehaviour
{

    public Player player;
    private Transform target;
    private Animator Enemy_GreenSlime;
    private Rigidbody2D rb;
    public int currentHealth;

    protected float distanceToTarget;
    private int maxHealth = 100;
    private float chaseDistance = 4f;
    public int jumpForce = 70; //점프 힘
    public int speed = 30; //좌우 이동속도

    private bool isJumping = false;

    private void Start()
    {

        currentHealth = maxHealth;
    }

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Enemy_GreenSlime = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (distanceToTarget <= chaseDistance && Mathf.Abs(target.position.y - transform.position.y) > 1f ){
            if (!isJumping)
            {
                isJumping = true;
                StartCoroutine( Jump() );
            }
        }
    }

    private IEnumerator Jump()
    {
        yield return new WaitForSecondsRealtime(3.0f);

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        Enemy_GreenSlime.SetTrigger("Jump"); //Animation에 Jump 트리거 없음. 추가 요망.

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player")) //Ground 충돌 여부를 CompareTag로 수정하였음.
        {
            isJumping = false;
        }
        
        if (collision.gameObject.name.Contains("Player"))
            if (player != null)
            {
                player.getDamage(10);
            }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
        Enemy_GreenSlime.SetTrigger("Hurt");
    }

    private void Die()
    {
        Enemy_GreenSlime.SetTrigger("Death");
        Destroy(gameObject);
    }
}
