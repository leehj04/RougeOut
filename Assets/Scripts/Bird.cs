using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : BaseEnemy
{
    private Animator anim;
    Rigidbody2D rigid;
    Vector3 startPosition;
    private int maxHealth = 70;
    float verticalAmplitude = 1.0f;
    float verticalFrequency = 1.0f;
    float horizontalSpeed = 2.0f;
    protected override void Start()
    {
        base.Start();

        currentHealth = maxHealth;
    }
    protected override void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();
    }
    void FixedUpdate()
    {
      
        float newY = startPosition.y + Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        Vector2 verticalPosition = new Vector2(transform.position.x, newY);

        float horizontalDistance = 4.0f;
        float newX = startPosition.x + Mathf.PingPong(Time.time * horizontalSpeed, horizontalDistance) - (horizontalDistance / 2);
        Vector2 horizontalPosition = new Vector2(newX, verticalPosition.y);

        rigid.MovePosition(horizontalPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("death");
            gameObject.SetActive(false);
        }
    }
}
