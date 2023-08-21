using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    [SerializeField]
    public float speed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    Transform pos;
    [SerializeField]
    float checkRadius;
    [SerializeField]
    LayerMask islayer;

    public int jumpCount;
    int jumpCnt;
    float plyYVel;

    bool isGrounded;

    public bool isLadder;
    public bool isClimbing;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        jumpCnt = jumpCount;
    }

    void Update()
    {
        //����
        bool isJumping = false;
        bool isFalling = false;
        isGrounded = Physics2D.OverlapCircle(pos.position, checkRadius, islayer);

        if( Input.GetKeyDown(KeyCode.Space) && jumpCnt > 0 && !isLadder ) {
            rigid.velocity = Vector2.up * jumpPower;
            jumpCnt--;
            isJumping = true;
        }
        else {
            if (isGrounded) { isFalling = false; }
            else if (isLadder) { isFalling = false; }
            else { isFalling = true; }
        }

        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFalling", isFalling);

        plyYVel = Mathf.Abs(rigid.velocity.y); //�÷��̾� ���� ������ �ӵ�
        if ( isGrounded && plyYVel <= 0 ) { 
            jumpCnt = jumpCount;
        }
    }

    void FixedUpdate()
    {
        // �¿� �̵�
        float hor = Input.GetAxisRaw(("Horizontal"));
        bool ishpLow;
        rigid.velocity = new Vector2(hor * speed, rigid.velocity.y);

        //�÷��̾� �涱�� Anim
        if( gameManager.health <= 100 ){ ishpLow = true; }
        else{ ishpLow = false; }

        anim.SetFloat("Speed", Mathf.Abs(hor));
        anim.SetBool("isHpLow", ishpLow);

        if (hor != 0)
        {
            spriteRenderer.flipX = hor < 0;
        }

        //��ٸ� ����
        if (isLadder)
        {
            float ver = Input.GetAxisRaw("Vertical");
            if(ver > 0)
            {
                isClimbing = true;
                rigid.gravityScale = 0;
                rigid.velocity = new Vector2(rigid.velocity.x, ver * speed);
            }
            else if (ver < 0)
            {
                isClimbing = true;
                rigid.gravityScale = 0;
                rigid.velocity = new Vector2(rigid.velocity.x * (-1), ver * speed);
            }

            else
            {
                isClimbing = false;
                rigid.gravityScale = 0;  //��ٸ����� ���� �� �߷� ����
                rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isClimbing)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            }
        }
        else
        {
            isClimbing = false;
            rigid.gravityScale = 3;  //��ٸ����� ����� �� �߷� ����
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //��ȭ ȹ��
        if(collision.gameObject.tag == "Item")
        {
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if (isSilver)
                gameManager.stageCoin += 1;  //�ʵ� ���� óġ �� ��ȭ
            else if(isGold) 
                gameManager.stageCoin += 3;  //���� ���� óġ �� ��ȭ

            collision.gameObject.SetActive(false);  //��ȭ �����
        }

        //��ٸ�Ÿ��
        if (collision.gameObject.tag == "Ladder")
        {
            isLadder = true;
        }

        //�����()
        else if(collision.gameObject.tag == "Finish")
        {
            gameManager.NextStage();
        }
    }

    //��ٸ� Ÿ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isLadder = false;
        }
    }

    //�÷��̾� �ǰ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
            OnDamged(collision.transform.position);
    }

    void OnDamged(Vector2 targetPos)
    {
        //�÷��̾� ���̾� ����
        gameObject.layer = 11;

        //���� �ð�ȭ
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //�ǰݽ� �÷��̾� ƨ��� ���
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 8, ForceMode2D.Impulse);

        //�ִϸ��̼�
        anim.SetTrigger("Take Hit");

        Invoke("OffDamaged", 1.5f);
    }

    void OffDamaged()
    {
        gameObject.layer = 10;  //�÷��̾� ���̾� ����

        spriteRenderer.color = new Color(1, 1, 1, 1);  //�������� �ð�ȭ
    }

    //�������� ������ �� �÷��̾� ����
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}