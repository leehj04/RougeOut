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
        //점프
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

        plyYVel = Mathf.Abs(rigid.velocity.y); //플레이어 상하 움직임 속도
        if ( isGrounded && plyYVel <= 0 ) { 
            jumpCnt = jumpCount;
        }
    }

    void FixedUpdate()
    {
        // 좌우 이동
        float hor = Input.GetAxisRaw(("Horizontal"));
        bool ishpLow;
        rigid.velocity = new Vector2(hor * speed, rigid.velocity.y);

        //플레이어 헐떡임 Anim
        if( gameManager.health <= 100 ){ ishpLow = true; }
        else{ ishpLow = false; }

        anim.SetFloat("Speed", Mathf.Abs(hor));
        anim.SetBool("isHpLow", ishpLow);

        if (hor != 0)
        {
            spriteRenderer.flipX = hor < 0;
        }

        //사다리 구현
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
                rigid.gravityScale = 0;  //사다리에서 멈출 때 중력 적용
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
            rigid.gravityScale = 3;  //사다리에서 벗어났을 때 중력 적용
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //금화 획득
        if(collision.gameObject.tag == "Item")
        {
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if (isSilver)
                gameManager.stageCoin += 1;  //필드 몬스터 처치 후 금화
            else if(isGold) 
                gameManager.stageCoin += 3;  //던전 몬스터 처치 후 금화

            collision.gameObject.SetActive(false);  //금화 사라짐
        }

        //사다리타기
        if (collision.gameObject.tag == "Ladder")
        {
            isLadder = true;
        }

        //결승점()
        else if(collision.gameObject.tag == "Finish")
        {
            gameManager.NextStage();
        }
    }

    //사다리 타기
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isLadder = false;
        }
    }

    //플레이어 피격
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
            OnDamged(collision.transform.position);
    }

    void OnDamged(Vector2 targetPos)
    {
        //플레이어 레이어 변경
        gameObject.layer = 11;

        //무적 시각화
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //피격시 플레이어 튕기는 모션
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 8, ForceMode2D.Impulse);

        //애니메이션
        anim.SetTrigger("Take Hit");

        Invoke("OffDamaged", 1.5f);
    }

    void OffDamaged()
    {
        gameObject.layer = 10;  //플레이어 레이어 변경

        spriteRenderer.color = new Color(1, 1, 1, 1);  //무적해제 시각화
    }

    //스테이지 끝났을 때 플레이어 멈춤
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}