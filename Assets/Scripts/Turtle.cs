using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : BaseEnemy
{
    public float chaseSpeed = 3.0f;    
    public float returnSpeed = 1.0f;   
    public float detectionDistance = 3.0f;   
    public float maxChaseDistance = 5.0f;  
    public float jumpOffset = 0.5f;
    private Vector3 initialPosition;   
    private bool isChasing = false;
    private int maxHealth = 180;

    private Animator TurtleAnim;
    protected override void Awake()
    {
        base.Awake();
        TurtleAnim = GetComponent<Animator>();
        initialPosition = transform.position;
        TurtleAnim.SetBool("IsMoving", false);
        currentHealth = maxHealth;
    }

    protected override void Update()
    {
        base.Update();
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        Vector3 directionToPlayer = (target.position - transform.position).normalized;

        if (distanceToPlayer <= detectionDistance)
        {
            isChasing = true;
            TurtleAnim.SetBool("IsMoving", true);
        }
        else if (distanceToPlayer > maxChaseDistance)
        {
            isChasing = false;
            TurtleAnim.SetBool("IsMoving", false);
            ReturnToInitialPosition();
            return; // 추가된 부분
        }

        if (isChasing)
        {
            ChasePlayer();
        }

        if (directionToPlayer.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); 
        }
        else if (directionToPlayer.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); 
        }
    }


    private void ChasePlayer()
    {
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        transform.position += new Vector3(directionToPlayer.x, directionToPlayer.y + jumpOffset, 0) * chaseSpeed * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerMove.getDamage();
        }
    }
    private void ReturnToInitialPosition()
    {
        Vector3 directionToInitial = (initialPosition - transform.position).normalized;
        transform.position += directionToInitial * returnSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
        {
            isChasing = false;
            TurtleAnim.SetBool("IsMoving", false);
            transform.position = initialPosition; 
        }
    }

}
