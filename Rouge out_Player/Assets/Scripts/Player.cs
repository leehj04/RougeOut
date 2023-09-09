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
    private static float coolTimeShift = 1.2f; //��Ÿ Shift ��Ÿ�� 1.5��
    public Transform posEnemy;
    public Vector2 boxSize;

    private float curTimeQ;
    private static float coolTimeQ = 10f; //Q��ų ��Ÿ�� 60��

    private bool isFlashing = false;
    private float flashInterval = 0.2f;  //�����̴� ����
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
            //����
            isOnGround = Physics2D.OverlapCircle(pos.position, checkRadius, isLayer);
            anim.SetBool("isOnGround", isOnGround);

            plyYVel = Mathf.Round(rigid.velocity.y); //�÷��̾� ���� ������ �ӵ�
            anim.SetFloat("plyYVel", plyYVel);

            if (isOnGround && plyYVel == 0 && jumpCnt != jumpCount) { jumpCnt = jumpCount; } //���鿡 ������ ����Ƚ�� ����

            if ( Input.GetKeyDown(DataCtrl.instance.mappedKey.Jump) && jumpCnt > 0 && !isLadder ) {
                rigid.velocity = Vector2.up * jumpPower;
                jumpCnt--;
            }

            //���� ��Ÿ SHIFT -- ���ݷ� 20
            if (curTimeShift <= 0)
            {
                if (Input.GetKey(DataCtrl.instance.mappedKey.Attack)) //LeftShift�� �ؾߵǴµ� ������ �� ����Ű �˸�â �˾����� ���Ͽ� H�� ������ ������.
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

                        //������ �ǰ�
                        if (slime != null)
                            slime.TakeDamage(20);

                        //�ź��� �ǰ�
                        else if (turtle != null)
                            turtle.TakeDamage(20);

                        //�� �ǰ�
                        else if (snake != null)
                            snake.TakeDamage(20);

                        //�� �ǰ�
                        else if (bird != null)
                            bird.TakeDamage(20);

                        //�߰����� �ǰ�
                        else if (hyunmuMid != null && hyunmuMid.name == "MidEN")
                            hyunmuMid.TakeDamage(20);

                        //�������� �ǰ�
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

            //���� ��ų Q
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

            //���� �ð�ȭ ������
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
            // �¿� �̵�
            float hor = Input.GetAxisRaw(("Horizontal"));
            rigid.velocity = new Vector2(hor * speed, rigid.velocity.y);

            //�÷��̾� �涱�� Anim
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

            //��ٸ� ����
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
                    rigid.gravityScale = 0;  //��ٸ����� ���� �� �߷� ����
                    rigid.velocity = new Vector2(rigid.velocity.x, 0f);
                }

                if (Input.GetKeyDown(DataCtrl.instance.mappedKey.Jump) && !isClimbing)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                }

                else
                {
                    isClimbing = false;
                    rigid.gravityScale = 0;  //��ٸ����� ����� �� �߷� ����
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
            //��ٸ�Ÿ��
            if (collision.gameObject.tag == "Ladder") { isLadder = true; }

            //��ȭ ȹ��
            if (collision.gameObject.tag == "Item")
            {
                bool isSilver = collision.gameObject.name.Contains("Silver");
                bool isGold = collision.gameObject.name.Contains("Gold");

                if (isSilver)
                    gameManager.stageCoin += 1;  //�ʵ� ����(������) óġ �� ��ȭ
                else if (isGold)
                    gameManager.stageCoin += 3;  //���� ����(���) óġ �� ��ȭ

                collision.gameObject.SetActive(false);  //��ȭ �����
            }

            //ȸ�� ���� -- public void OnTriggerEnter2D(Collider2D collision)���� �ٸ� �Լ��� �̵����Ѿߵ�.
            if (collision.gameObject.tag == "Potion")
            {
                bool isHealthPotion = collision.gameObject.name.Contains("HealthPotion");

                if (isHealthPotion)
                    DataCtrl.instance.playerData.health += 20.0f;

                collision.gameObject.SetActive(false); //���� �����
            }

            //ȸ���� ����
            if(collision.gameObject.tag == "Ring")
            {
                bool isHealthRing = collision.gameObject.name.Contains("HealthRing");

                if (isHealthRing)
                {
                    DataCtrl.instance.playerData.health += 20.0f; //TODO ��� 5���� óġ �� �߰�
                }

                collision.gameObject.SetActive(false);
            }

            //�����
            else if (collision.gameObject.tag == "Finish")
            {
                gameManager.NextStage();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isDead) {
            //��ٸ� Ÿ��
            if (collision.gameObject.tag == "Ladder") { isLadder = false; }

            //�������� Ÿ��
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
            //�÷��̾� �ǰ�
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

            //�÷��̾� ���̾� ����
            gameObject.layer = 11;

            //���� �ð�ȭ ������
            spriteRenderer.color = new Color(1, 1, 1, 0.1f);
            isFlashing = true;

            //�ǰݽ� �÷��̾� ƨ��� ���
            int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc, 1) * 8, ForceMode2D.Impulse);

            //�ִϸ��̼�
            anim.SetTrigger("Take Hit");

            Invoke("OffDamaged", 1.5f);
        }
    }

    public void OffDamaged()
    {
        if (!isDead) {
            gameObject.layer = 10; //�÷��̾� ���̾� ����

            //���� �ð�ȭ ����
            isFlashing = false;
            spriteRenderer.color = originalColor;
        }
    }

    //�÷��̾� ����
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

    //�������� ������ �� �÷��̾� ����
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}