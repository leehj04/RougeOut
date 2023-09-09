using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public Slime slime;
    public HyunmuMiddle hyunmuMid;

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

    private Transform plyTransform;

    private float curTimeShift;
    private static float coolTimeShift = 1.2f; //평타 Shift 쿨타임 1.5초
    public Transform posEnemy;
    public Vector2 boxSize;

    private float curTimeQ;
    private static float coolTimeQ = 10f; //Q스킬 쿨타임 60초

    private bool isFlashing = false;
    private float flashInterval = 0.2f;  //깜빡이는 간격
    private float flashTimer = 0f;

    public int damage;

    private bool isDead = false;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        jumpCnt = jumpCount;
        originalColor = spriteRenderer.color;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            plyTransform = player.transform;
        }
    }

    public void Update()
    {
        if (!isDead) {
            //점프
            isOnGround = Physics2D.OverlapCircle(pos.position, checkRadius, isLayer);
            anim.SetBool("isOnGround", isOnGround);

            plyYVel = Mathf.Round(rigid.velocity.y); //플레이어 상하 움직임 속도
            anim.SetFloat("plyYVel", plyYVel);

            if (isOnGround && plyYVel == 0 && jumpCnt != jumpCount) { jumpCnt = jumpCount; } //지면에 닿을시 점프횟수 리셋

            if ( Input.GetKeyDown(DataCtrl.instance.mappedKey.Jump) && jumpCnt > 0 && !isLadder ) {
                rigid.velocity = Vector2.up * jumpPower;
                jumpCnt--;
            }

            //공격 평타 SHIFT -- 공격력 20
            if (curTimeShift <= 0)
            {
                if (Input.GetKey(DataCtrl.instance.mappedKey.Attack)) //LeftShift로 해야되는데 시현할 때 고정키 알림창 팝업으로 인하여 H로 변경한 상태임.
                {
                    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(posEnemy.position, boxSize, 0);
                    foreach (Collider2D collider in collider2Ds)
                    {
                        Slime slime = collider.gameObject.GetComponent<Slime>();
                        HyunmuMiddle hyunmuMid = collider.gameObject.GetComponent<HyunmuMiddle>();
                        Turtle turtle = collider.gameObject.GetComponent<Turtle>();
                        Snake snake = collider.gameObject.GetComponent<Snake>();
                        Bird bird = collider.gameObject.GetComponent<Bird>();
                        HyunmuFinal hyunmuFinal = collider.gameObject.GetComponent<HyunmuFinal>();

                        //슬라임 피격
                        if (slime != null)
                            slime.TakeDamage(20);

                        //거북이 피격
                        else if (turtle != null)
                            turtle.TakeDamage(20);

                        //뱀 피격
                        else if (snake != null)
                            snake.TakeDamage(20);

                        //새 피격
                        else if (bird != null)
                            bird.TakeDamage(20);

                        //중간보스 피격
                        else if (hyunmuMid != null && hyunmuMid.name == "MidEN")
                            hyunmuMid.TakeDamage(20);

                        //최종보스 피격
                        else if (hyunmuFinal != null && hyunmuFinal.name == "Hyunmu_final")
                            hyunmuFinal.TakeDamage(20);
                    }
                    anim.SetTrigger("isAttack");
                    curTimeShift = coolTimeShift;
                }
            }
            else
            {
                curTimeShift -= Time.deltaTime;
            }

            //공격 스킬 Q
            if(curTimeQ <= 0)
            {
                if(Input.GetKey(DataCtrl.instance.mappedKey.Skill_1_KeyCode))
                {
                    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(posEnemy.position, boxSize, 0);
                    foreach (Collider2D collider in collider2Ds)
                    {
                        Debug.Log(collider.tag);
                    }
                    anim.SetTrigger("isSkill");
                    curTimeQ = coolTimeQ;
                }
            }
            else
            {
                curTimeQ -= Time.deltaTime;
            }

            //무적 시각화 깜빡임
            if (isFlashing)
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
            rigid.velocity = new Vector2(hor * speed, rigid.velocity.y);

            //플레이어 헐떡임 Anim
            bool ishpLow;
            if (DataCtrl.instance.playerData.health <= 100.0f) { ishpLow = true; }
            else { ishpLow = false; }

            anim.SetFloat("Speed", Mathf.Abs(hor));
            anim.SetBool("isHpLow", ishpLow);

            if (hor != 0) { 
                spriteRenderer.flipX = hor < 0;
                
                Vector3 newPosEN = posEnemy.position;
                newPosEN.x = plyTransform.position.x - (hor * -1.0f);
                posEnemy.position = newPosEN;
            }

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

                if (Input.GetKeyDown(DataCtrl.instance.mappedKey.Jump) && !isClimbing)
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
                rigid.gravityScale = 10;
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
                    gameManager.stageCoin += 1;  //필드 몬스터(슬라임) 처치 후 금화
                else if (isGold)
                    gameManager.stageCoin += 3;  //던전 몬스터(잡몹) 처치 후 금화

                collision.gameObject.SetActive(false);  //금화 사라짐
            }

            //회복 포션 -- public void OnTriggerEnter2D(Collider2D collision)에서 다른 함수로 이동시켜야됨.
            if (collision.gameObject.tag == "Potion")
            {
                bool isHealthPotion = collision.gameObject.name.Contains("HealthPotion");

                if (isHealthPotion)
                    DataCtrl.instance.playerData.health += 20.0f;

                collision.gameObject.SetActive(false); //포션 사라짐
            }

            //회복의 반지
            if(collision.gameObject.tag == "Ring")
            {
                bool isHealthRing = collision.gameObject.name.Contains("HealthRing");

                if (isHealthRing)
                {
                    DataCtrl.instance.playerData.health += 20.0f; //TODO 잡몹 5마리 처치 시 추가
                }

                collision.gameObject.SetActive(false);
            }

            //결승점
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DownFloor")
        {
            if (Input.GetAxisRaw("Vertical") < 0) { GetComponent<CapsuleCollider2D>().isTrigger = true; }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead) {
            //플레이어 피격
            if (collision.gameObject.tag == "Enemy")
            {
                if (collision.gameObject.name.Contains("Slime") || collision.gameObject.name.Contains("MidEN"))
                    OnDamaged(collision.transform.position);
                if (collision.gameObject.name.Contains("DeadFloor"))
                {
                    DataCtrl.instance.playerData.health -= 20.0f;
                    OnDamaged(collision.transform.position);
                }
            }

        }
    }

    public void getDamage(int damage)
    {
        DataCtrl.instance.playerData.health -= damage * DataCtrl.instance.playerData.finalArmor;
    }

    public void OnDamaged(Vector2 targetPos)
    {
        if (!isDead) {
            getDamage(damage);
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

    //플레이어 죽음
    public void Death()
    {
        if (DataCtrl.instance.playerData.health <= 0.0f)
        {
            DataCtrl.instance.playerData.health = 0.0f;
            anim.SetTrigger("Dead");
            gameObject.layer = 11;
            spriteRenderer.color = new Color(0, 0, 0);
            isDead = true;
        }
    }

    //스테이지 끝났을 때 플레이어 멈춤
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}