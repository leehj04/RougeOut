using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static bool IsAttacking = false;
    
    public enum PlayerDirection
    {
        Left,
        Right
    }

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public float jumpForce = 10f;
    public bool isJump = false;
    private PlayerDirection direction = PlayerDirection.Right;
    private static int playerHealth=500;
    public bool isAttacking = false;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        // 키보드 입력 받기
        float horizontalInput = Input.GetAxis("Horizontal");

        // 플레이어 이동
        Move(horizontalInput);

        // 플레이어 방향 변경
        Flip(horizontalInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJump)
            {
                Jump();
            }
        }
    }

    private void Move(float horizontalInput)
    {
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = movement;
    }

    private void Flip(float horizontalInput)
    {
        if (horizontalInput > 0 && direction != PlayerDirection.Right)
        {
            // 오른쪽을 보도록 변경
            transform.localScale = new Vector3(1, 1, 1);
            direction = PlayerDirection.Right;
        }
        else if (horizontalInput < 0 && direction != PlayerDirection.Left)
        {
            // 왼쪽을 보도록 변경
            transform.localScale = new Vector3(-1, 1, 1);
            direction = PlayerDirection.Left;
        }
    }
    private void Jump()
    {
        isJump = true;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Enemy"))
        {
            isJump = false;
        }
    }
    public static void getDamage()
    {
        playerHealth = playerHealth- 100;
    }
}











