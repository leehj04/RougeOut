using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator SnakeAnim;
    SpriteRenderer spriteRenderer;
    public int nextMove;
    private int maxHealth = 200;
    private int currentHealth;
    private void Start()
    {

        currentHealth = maxHealth;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        SnakeAnim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        Invoke("Think", 2);
    }


    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("ground"));


        if (rayHit.collider == null)
        {

            Turn();

        }
    }


    void Think()
    {

        //set next active
        nextMove = Random.Range(-1, 2); 

  
        SnakeAnim.SetInteger("WalkSpeed", nextMove);

   
        if (nextMove != 0)
        {
            spriteRenderer.flipX = (nextMove == 1); 
        }

        //Àç±Í 
        float nextThinkTime = Random.Range(2f, 5f); 

        Invoke("Think", nextThinkTime);




    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerMove.getDamage();
        }
    }
    void Turn()
    {
        nextMove = nextMove * (-1);
        spriteRenderer.flipX = (nextMove == 1); 


        CancelInvoke();
        Invoke("Think", 2);
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
