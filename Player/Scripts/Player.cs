using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Color originalColor;

    [SerializeField]
    public float speed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    Transform pos;
    [SerializeField]
    float checkRadius;
    [SerializeField]
    LayerMask isLayer;

    public int jumpCount;
    int jumpCnt;
    float plyYVel;

    bool isOnGround;

    public bool isLadder;
    public bool isClimbing;

    public bool isDownFloor;

    private float curTime;
    private static float coolTime = 0.5f;
    public Transform posEnemy;
    public Vector2 boxSize;

    private bool isFlashing = false;
    private float flashInterval = 0.2f;  //깜빡이는 간격
    private float flashTimer = 0f;

    private bool isDead = false;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        jumpCnt = jumpCount;
        originalColor = spriteRenderer.color;
    }

    public void Update()
    {
        if (!isDead) {
            //점프
            isOnGround = Physics2D.OverlapCircle(pos.position, checkRadius/2, isLayer);
            anim.SetBool("isOnGround", isOnGround);

            plyYVel = Mathf.Round(rigid.velocity.y); //플레이어 상하 움직임 속도
            anim.SetFloat("plyYVel", plyYVel);

            if (isOnGround && plyYVel == 0 && jumpCnt != jumpCount) { jumpCnt = jumpCount; } //지면에 닿을시 점프횟수 리셋

            if ( Input.GetKeyDown(KeyCode.Space) && jumpCnt > 0 && !isLadder ) {
                rigid.velocity = Vector2.up * jumpPower;
                jumpCnt--;
            }

            //공격 평타 H
            if (curTime <= 0)
            {
                if (Input.GetKey(KeyCode.H))
                {
                    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(posEnemy.position, boxSize, 0);
                    foreach (Collider2D collider in collider2Ds)
                    {
                        Debug.Log(collider.tag);
                    }
                    anim.SetTrigger("isAttack");
                    curTime = coolTime;
                }
            }
            else
            {
                curTime -= Time.deltaTime;
            }

            //무적 시각화 깜빡임
            if(isFlashing)
            {
                flashTimer += Time.deltaTime;
                if(flashTimer >= flashInterval)
                {
                    flashTimer = 0f;
                    spriteRenderer.color = spriteRenderer.color == originalColor ? new Color(1, 1, 1, 0.1f) : originalColor;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(posEnemy.position, boxSize);
    }

    public void FixedUpdate()
    {
        if (!isDead) {
            // 좌우 이동
            float hor = Input.GetAxisRaw(("Horizontal"));
            bool ishpLow;
            rigid.velocity = new Vector2(hor * speed, rigid.velocity.y);

            //플레이어 헐떡임 Anim
            if (gameManager.health <= 100) { ishpLow = true; }
            else { ishpLow = false; }

            anim.SetFloat("Speed", Mathf.Abs(hor));
            anim.SetBool("isHpLow", ishpLow);

            if (hor != 0) { spriteRenderer.flipX = hor < 0; }

            //사다리 구현
            if (isLadder)
            {
                float ver = Input.GetAxisRaw("Vertical");
                if (ver > 0)
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
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                }

                else
                {
                    isClimbing = false;
                    rigid.gravityScale = 0;  //사다리에서 벗어났을 때 중력 적용
                }
            }
            else { 
                isClimbing = false;
                rigid.gravityScale = 3;
            }
            anim.SetBool("isLadder", isLadder);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead) {
            //사다리타기
            if (collision.gameObject.tag == "Ladder") { isLadder = true; }



            //금화 획득
            if (collision.gameObject.tag == "Item")
            {
                bool isSilver = collision.gameObject.name.Contains("Silver");
                bool isGold = collision.gameObject.name.Contains("Gold");

                if (isSilver)
                    gameManager.stageCoin += 1;  //필드 몬스터 처치 후 금화
                else if (isGold)
                    gameManager.stageCoin += 3;  //던전 몬스터 처치 후 금화

                collision.gameObject.SetActive(false);  //금화 사라짐
            }

            //결승점()
            else if (collision.gameObject.tag == "Finish")
            {
                gameManager.NextStage();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isDead) {
            //사다리 타기
            if (collision.gameObject.tag == "Ladder") { isLadder = false; }

            //내려가는 타일
            if (collision.gameObject.tag == "DownFloor") { GetComponent<CapsuleCollider2D>().isTrigger = false; }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead) {
            //플레이어 피격
            if (collision.gameObject.tag == "Enemy")
                OnDamged(collision.transform.position);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DownFloor")
        {
            if (Input.GetAxisRaw("Vertical") < 0) { GetComponent<CapsuleCollider2D>().isTrigger = true; }
        }
    }

    public void OnDamged(Vector2 targetPos)
    {
        if (!isDead) {
            //플레이어 체력 깍임
            gameManager.health -= 10;
            Death();

            //플레이어 레이어 변경
            gameObject.layer = 11;

            //무적 시각화 깜빡임
            spriteRenderer.color = new Color(1, 1, 1, 0.1f);
            isFlashing = true;

            //피격시 플레이어 튕기는 모션
            int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc, 1) * 8, ForceMode2D.Impulse);

            //애니메이션
            anim.SetTrigger("Take Hit");

            Invoke("OffDamaged", 1.5f);
        }
    }

    public void OffDamaged()
    {
        if (!isDead) {
            gameObject.layer = 10; //플레이어 레이어 변경

            //무적 시각화 종료
            isFlashing = false;
            spriteRenderer.color = originalColor;
        }
    }

    public void Death()
    {
        if (gameManager.health <= 0)
        {
            gameManager.health = 0;
            anim.SetTrigger("Dead");
            gameObject.layer = 11;
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
            isDead = true;
        }
    }

    //스테이지 끝났을 때 플레이어 멈춤
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}