using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float speed;

    public GroundCheck ground; //
    public SideChecker side;

    private Animator anim = null;
    private Rigidbody2D rb = null;
    private SpriteRenderer spRenderer = null;
    private GameObject player = null;
    private GameObject monkey = null;
    private GameObject GameCtrl = null;

    //各種フラグ
    private bool isGround = false;
    private bool isSide = false;
    private bool isDie = false;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();

        anim.SetBool("move", false);
        anim.SetBool("jump", true);
        player = GameObject.Find("Player");
        monkey = GameObject.Find("Monkey");
        GameCtrl = GameObject.Find("GameCtrl");


        speed = GameCtrl.GetComponent<GameController>()._getEnemySpeed();
        monkey.GetComponent<monkeyController>().AddEnemyCnt(1);

    }

    // Update is called once per frame
    void Update()
    {
        isGround = ground.IsGround();
        isSide = side.IsSide();

        float dirX = 1;

        if( this.transform.eulerAngles.y == 180 )
        {
            dirX =  -1;
        }
        else
        {
            dirX =  1;
        }

        //速度を取得
        float velX = rb.velocity.x;
        float velY = rb.velocity.y;

        //最高速度に制限をかける
        if (Mathf.Abs(velX) >3)
        {
            if (velX > 3.0f)
            {
                rb.velocity = new Vector2(3.0f, velY);
            }
            if (velX < -3.0f)
            {
                rb.velocity = new Vector2(-3.0f, velY);
            }

        }

        if ( isGround && !isDie )
        {
            anim.SetBool("move", true);
            anim.SetBool("jump", false);

            rb.AddForce(Vector2.right * dirX * speed);
        }


        if ( isGround && isSide )　//反転
        {
            if (this.transform.eulerAngles.y == 180)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if(isDie)
        {
            isDie = false;
            anim.SetBool("gone", true);
            GetComponent<CircleCollider2D>().enabled = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        { 
            isDie = true;
        }
    }

    //アニメーションでコール
    void _enemyDesroy()
    {
        Destroy(this.gameObject);
        monkey.GetComponent<monkeyController>().AddEnemyCnt(-1);

    }

}

