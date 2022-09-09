using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonguriController : MonoBehaviour
{

    private float cntTime;
    private int DeleteTime;
    private GameObject player = null;
    private GameObject monkey = null;
    private GameObject GameCtrl = null;


    // Start is called before the first frame update
    void Start()
    {
        cntTime = 0;
        player = GameObject.Find("Player");
        monkey = GameObject.Find("Monkey");
        GameCtrl = GameObject.Find("GameCtrl");

        DeleteTime = GameCtrl.GetComponent<GameController>()._getDonguriDeleteTime();
        monkey.GetComponent<monkeyController>().AddDonguriCnt(1);


    }

    // Update is called once per frame
    void Update()
    {
        cntTime += Time.deltaTime;

        if( cntTime > DeleteTime)
        {
            _donguriDestory();
        }

    }

    void _donguriDestory()
    {
        Destroy(this.gameObject);
        monkey.GetComponent<monkeyController>().AddDonguriCnt(-1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.tag == "Player" )
        {
            player.GetComponent<PlayerController>()._getAttackItem();
            _donguriDestory();
        }

    }
}
