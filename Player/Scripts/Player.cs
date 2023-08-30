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
    private float flashInterval = 0.2f;  //�����̴� ����
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
            //����
            isOnGround = Physics2D.OverlapCircle(pos.position, checkRadius/2, isLayer);
            anim.SetBool("isOnGround", isOnGround);

            plyYVel = Mathf.Round(rigid.velocity.y); //�÷��̾� ���� ������ �ӵ�
            anim.SetFloat("plyYVel", plyYVel);

            if (isOnGround && plyYVel == 0 && jumpCnt != jumpCount) { jumpCnt = jumpCount; } //���鿡 ������ ����Ƚ�� ����

            if ( Input.GetKeyDown(KeyCode.Space) && jumpCnt > 0 && !isLadder ) {
                rigid.velocity = Vector2.up * jumpPower;
                jumpCnt--;
            }

            //���� ��Ÿ H
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

            //���� �ð�ȭ ������
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
            // �¿� �̵�
            float hor = Input.GetAxisRaw(("Horizontal"));
            bool ishpLow;
            rigid.velocity = new Vector2(hor * speed, rigid.velocity.y);

            //�÷��̾� �涱�� Anim
            if (gameManager.health <= 100) { ishpLow = true; }
            else { ishpLow = false; }

            anim.SetFloat("Speed", Mathf.Abs(hor));
            anim.SetBool("isHpLow", ishpLow);

            if (hor != 0) { spriteRenderer.flipX = hor < 0; }

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

                if (Input.GetKeyDown(KeyCode.Space) && !isClimbing)
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
                rigid.gravityScale = 3;
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
                    gameManager.stageCoin += 1;  //�ʵ� ���� óġ �� ��ȭ
                else if (isGold)
                    gameManager.stageCoin += 3;  //���� ���� óġ �� ��ȭ

                collision.gameObject.SetActive(false);  //��ȭ �����
            }

            //�����()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead) {
            //�÷��̾� �ǰ�
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
            //�÷��̾� ü�� ����
            gameManager.health -= 10;
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

    //�������� ������ �� �÷��̾� ����
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}