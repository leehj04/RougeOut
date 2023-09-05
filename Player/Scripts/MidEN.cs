using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MidEN : MonoBehaviour
{
    public GameManager gameManager;
    public Player player;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    public int health = 60; //Àû Ã¼·Â

    public Transform posPlayer;
    public Vector2 boxSize;

    private float attackCoolDown = 3.0f;
    private float timeSinceLastAttack = 0.0f;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if(timeSinceLastAttack >= attackCoolDown) {
            AttackPlayer();
            timeSinceLastAttack = 0.0f;
        }
    }

    private void AttackPlayer()
    {
        
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(posPlayer.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            Player player = collider.gameObject.GetComponent<Player>();

            if (player != null)
            {
                gameManager.health -= 40;
            }
        }

        if (anim != null)
                anim.SetTrigger("isAttack");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(posPlayer.position, boxSize);
    }
}
