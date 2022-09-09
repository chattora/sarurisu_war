using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class monkeyController : MonoBehaviour
{

    [SerializeField] GameObject ItemObj;
    [SerializeField] GameObject EnemyObj;
    private GameObject  GameCtrl;

    [SerializeField, Range(1, 10), Header("敵の数を設定しよう")]
    int EnemyMax = 5;
    [SerializeField, Range(1, 10), Header("ドングルの数を設定しよう")]
    int DonguriMax = 2;
    [SerializeField, Range(1, 5), Header("投げる間隔（秒）を設定しよう")]
    int throwTime = 2;


    private Rigidbody2D rb = null;
    private SpriteRenderer spRenderer = null;

    //投げるパラメーター
    private int power = 1;
    private int rotation = 0;
    private float torquePower = 0;

    private int EnemyCnt, DonguriCnt;
    private float cntTime;
    private Animator anim = null;
    private int kind;
    private int monkeyHP;

    //各種フラグ
    private bool isThrow;
    private bool isDamage;
    private bool isDie;

    // Start is called before the first frame update
    void Start()
    {
        cntTime = 0;
        isThrow = false;
        isDamage = false;
        isDie = false;

        DonguriCnt = 0;
        EnemyCnt = 0;

        GameCtrl = GameObject.Find("GameCtrl");
        EnemyMax = GameCtrl.GetComponent<GameController>()._getEnemyMax();
        DonguriMax = GameCtrl.GetComponent<GameController>()._getDonguriMax();
        throwTime = GameCtrl.GetComponent<GameController>()._getThrowTime();
        monkeyHP = GameCtrl.GetComponent<GameController>()._getMonkeyHp();


        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!isThrow && !isDamage && !isDie)
        {

            power = Random.Range(8, 22);
            rotation = Random.Range(-60, -40); 
            kind = Random.Range(1, 10);
            cntTime += Time.deltaTime;

            if (cntTime > throwTime && throwCheck() != 0 )
            {
                anim.SetBool("throw", true);
                cntTime = 0;
                isThrow = true;
            }
        }

        if( isDamage )　//ダメージ中の処理
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time * 20));

            GetComponent<CircleCollider2D>().enabled = false; //ダメージ中は無敵        
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, level);
        }
        else
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    void throwItem()
    {
        isThrow = false;
        anim.SetBool("throw", false);

        //最大値を超えてないか確認して超えてたら強制的に投げる物を変える
        int throwCheckRes = throwCheck();

        if (throwCheckRes == 1) kind = 1;
        else if (throwCheckRes == 2) kind = 10;

        if (kind > 5)
        {
            Transform newItem = Instantiate(ItemObj, transform.position, Quaternion.identity).transform;
            LaunchUtils.LaunchItem(ref newItem, power, transform.up, rotation, torquePower);
        }
        else
        {
            Transform newItem = Instantiate(EnemyObj, transform.position, Quaternion.identity).transform;
            LaunchUtils.LaunchItem(ref newItem, power, transform.up, rotation, torquePower);
        }
    }

    public void AddEnemyCnt(int cnt)
    {
        EnemyCnt += cnt;
    }

    public void AddDonguriCnt(int cnt)
    {
        DonguriCnt += cnt;
    }

    //投げてるアニメーションでコール
    int throwCheck()
    {
        if(DonguriCnt >= DonguriMax && EnemyCnt >= EnemyMax)
        {
            return 0;
        }
        else if(DonguriCnt >= DonguriMax && EnemyCnt < EnemyMax)
        {
            return 1;
        }
        else if (DonguriCnt < DonguriMax && EnemyCnt >= EnemyMax)
        {
            return 2;
        }
        else
        {
            return -1;
        }

    }

    public int _getMonkeyHP()
    {
        return monkeyHP;
    }

    public void _addMonkeyHP(int pra)
    {
        monkeyHP += pra;
    }

    void DamageEnd()
    {
        isDamage = false;
        anim.SetBool("damage", isDamage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            Destroy(collision.gameObject);
            anim.SetBool("damage", true);
            _addMonkeyHP(-1);

            if( _getMonkeyHP() <=0 )
            {
                isDie = true;
                StartCoroutine("Die");
            }
            else
            {
                isDamage = true;
                StartCoroutine("Damage");
            }
        }
    }

    //割り込み
    IEnumerator Damage()
    {
        yield return new WaitForSeconds(1.0f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    IEnumerator Die()
    {
        this.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        //rb.AddForce(Vector2.up * 300f);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Game");
    }
}
