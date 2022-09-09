using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float speed; //プレイヤーの速度
    public float jumpForce;　//ジャンプ力

    public GroundCheck ground; //地面に触れているかを確認する
    public GroundCheck head; 　//頭が触れているかを確認する
    public SideChecker side;　 //横が触れているかを確認する

    public GameObject AttackItem = null; //アイテム用オブジェ
    public Transform AttackPoint = null;　//アイテムを出現させる位置
    　
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private SpriteRenderer spRenderer = null;

    private GameObject GameCtrl = null; //設定を取得する

    //各種フラグ
    private bool isGround = false;
    private bool isJump = false;
    private bool isThrow = false;
    private bool isHead = false;
    private bool isSide = false;
    private bool AttackItemUse = false;
    private bool isDie = false;
    private bool isDamage = false;
    private bool isStop = false;


    //プレイヤーのHP
    private int PlayerHP;

    // Start is called before the first frame update
    void Start()
    {
        GameCtrl = GameObject.Find("GameCtrl");
        speed = GameCtrl.GetComponent<GameController>()._getPlayerSpeed();
        jumpForce = GameCtrl.GetComponent<GameController>()._getPlayerJump();

        PlayerHP = 3;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        spRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        isGround = ground.IsGround();
        isHead = head.IsGround();
        isSide = side.IsSide();

        float moveX = Input.GetAxis("Horizontal");

        if (!isDie && !isSide && !isDie )
        {
            anim.SetFloat("speed", Mathf.Abs(moveX * speed));
            rb.AddForce(Vector2.right * moveX * speed);

        }
        else
        {
            anim.SetFloat("speed", 0);
        }

        if (moveX < 0)
        {
            transform.localScale = new Vector3(0.7f, transform.localScale.y, 1);

        }
        else if( moveX > 0)
        {
            transform.localScale = new Vector3(-0.7f, transform.localScale.y, 1);
        }


        if(Input.GetButtonDown("Jump") && isGround && transform.position.y < 4 && !isDie )
        {
            rb.AddForce(Vector2.up * jumpForce);
            anim.SetBool("jump", true);
           
        }

        //Xボタンでどんぐり発射（どんぐりを所持している時のみ）
        if (Input.GetKeyDown(KeyCode.X) && !isThrow && AttackItemUse && !isDie)
        {
            isThrow = true;
            AttackItemUse = false;
            anim.SetBool("throw", true);
            anim.SetBool("jump", false);
            anim.SetBool("fall", false);

        }

        //速度を取得
        float velX = rb.velocity.x;
        float velY = rb.velocity.y;
        float velX_MAX = 5.0f;
        float velY_MAX = 20.0f;
       
        //最高速度に制限をかける
        if( Mathf.Abs(velX) > velX_MAX)
        {
            if(velX > velX_MAX)
            {
                rb.velocity = new Vector2(velX_MAX, velY);
            }
            if(velX < -1 * velX_MAX)
            {
                rb.velocity = new Vector2(-1 * velX_MAX, velY);
            }
         }
        if(Mathf.Abs(velY) > velY_MAX)
        {
            if (velY > velY_MAX)
            {
                rb.velocity = new Vector2(velX, 0.0f); //速度０にする
            }
        }

        //ジャンプ中のアニメ設定
        if(velY > 0.5f)
        {
            anim.SetBool("jump", true);
        }
        if(velY < -0.1)
        {
            anim.SetBool("fall", true);
        }

        //地上にいる時はジャンプアニメはリセット
        if( isGround  && !isThrow )
        {
            anim.SetBool("jump", false);
            anim.SetBool("fall", false);
        }

        //ダメージを受けているときはブリンクさせる
        if(isDamage)
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time *20));
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, level);
        }

         //Debug.Log("V"+velY);
     }

    //衝突判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && !isDamage )
        {
            _addPlayerHP(-1);

            //PlayerのHPが0になったら
            if (_getPlayerHP() <= 0)
            {
                Debug.Log("リス死亡");
                isDie = true;
                StartCoroutine("Die");
            }
            else
            {
                isDamage = true;
                gameObject.layer = LayerMask.NameToLayer("PlayerDamage"); //ブリンク中は敵との衝突判定はなくす
                StartCoroutine("Damage");
            }


        }
    }

    //投げアニメの終わりで呼ぶ関数
    void OnCompleteAnimation()
    {
        isThrow = false;
        anim.SetBool("throw", false);
    }

    //投げアニメの途中で攻撃処理をいれる
    void OnAttack()
    {
        GameObject newAttackItem = Instantiate(AttackItem, AttackPoint.position, Quaternion.identity) as GameObject;

        newAttackItem.gameObject.tag = "Attack";

        if(transform.localScale.x > 0)
        {
            newAttackItem.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 1000f);
        }
        else
        {
            newAttackItem.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 1000f);
        }
    }

    //どんぐり取得時に呼ばれる
    public void _getAttackItem()
    {
        AttackItemUse = true;
    }

    //どんぐりの有無を返す
    public bool _getAttackItemUse()
    {
        return AttackItemUse;
    }

    public int _getPlayerHP()
    {
        return PlayerHP;
    }

    public void _addPlayerHP(int pra)
    {
        PlayerHP += pra;
    }

    //割り込み
    IEnumerator Damage()
    {
        yield return new WaitForSeconds(2.0f);
        isDamage = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    IEnumerator Die()
    {
        anim.SetBool("die", isDie);
        GetComponent<CapsuleCollider2D>().enabled = false;
        rb.AddForce(Vector2.up * jumpForce);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Game");
    }

}
