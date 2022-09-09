using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{


    [SerializeField, Range(1, 10), Header("猿が投げる敵の最大数を設定しよう")]
    int EnemyMax = 3;
    [SerializeField, Range(1, 10), Header("猿が投げるどんぐりの最大数を設定しよう")]
    int DonguriMax = 2;
    [SerializeField, Range(1, 10), Header("猿が投げてからどんぐりが消える秒数を設定しよう")]
    int DonguriDeleteTime = 3;
    [SerializeField, Range(1, 5), Header("猿がものを投げる間隔（秒）を設定しよう")]
    int throwTime = 2;
    [SerializeField, Range(1, 5), Header("猿のHPを設定しよう")]
    int monkeyHP = 2;



    [SerializeField, Range(5, 50), Header("敵(虫）の速度を設定しよう")]
    float EnemySpeed = 15f;

    [SerializeField, Range(10, 100), Header("プレイヤーの速度を設定しよう")]
    float playerSpeed = 30f;

    [SerializeField, Range(300, 900), Header("プレイヤーのジャンプ力を設定しよう")]
    float playerJumpForce = 900f;

    [SerializeField, Header("主人公のHPを表示")]
    bool playerHP_DISP = false;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if( Input.GetKey(KeyCode.Escape) )
        {
            Debug.Log("終了");
            Application.Quit();
        }
    }

    //猿の設定
    public int _getEnemyMax()
    {
        return EnemyMax;
    }

    public int _getDonguriMax()
    {
        return DonguriMax;
    }
    public int _getDonguriDeleteTime()
    {
        return DonguriDeleteTime;
    }
    public int _getThrowTime()
    {
        return throwTime;
    }
    public int _getMonkeyHp()
    {
        return monkeyHP;
    }



    public float _getPlayerSpeed()
    {
        return playerSpeed;
    }

    public float _getPlayerJump()
    {
        return playerJumpForce;
    }

    public float _getEnemySpeed()
    {
        return EnemySpeed;
    }

    public bool _getPlayerHpDisp()
    {
        return playerHP_DISP;
    }
}
