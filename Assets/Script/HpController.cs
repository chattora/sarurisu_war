using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour
{

    [SerializeReference] GameObject LifeObj;

    GameObject[] Life = new GameObject[3];
    GameObject player = null;
    GameObject GameCtrl;

    // Start is called before the first frame update
    void Start()
    {

        //GameObject LifeTemp = Instantiate(Life[0],new Vector3(1,1,1),Quaternion.identity);
        GameCtrl = GameObject.Find("GameCtrl");


        for (int i =0; i< Life.Length;i++)
        {
            Life[i] = Instantiate(LifeObj, new Vector3(3.7f + ( -1.4f * i ) , 0.3f, 1.0f), Quaternion.identity);
        }

        LifeInit();

        player = GameObject.Find("Player");

     }

    // Update is called once per frame
    void Update()
    {
        bool dispFlg = GameCtrl.GetComponent<GameController>()._getPlayerHpDisp();

        if(dispFlg)
        {
            LifeInit();
            SetLife();
        }
        else
        {
            LifeOff();
        }
    
    }

    void LifeInit()
    {
        for (int i = 0; i < Life.Length; i++)
        {
            Life[i].transform.GetChild(0).gameObject.SetActive(false);
            Life[i].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    void SetLife()
    {
        int hp = player.GetComponent<PlayerController>()._getPlayerHP();

        for(int i = 0; i <hp; i++)
        {
            Life[i].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void LifeOff()
    {
        for (int i = 0; i < Life.Length; i++)
        {
            Life[i].transform.GetChild(0).gameObject.SetActive(false);
            Life[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

}
