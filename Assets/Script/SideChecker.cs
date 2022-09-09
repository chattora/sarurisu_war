using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideChecker : MonoBehaviour
{

    private bool isSide = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    //    Debug.Log(isSide);
    }


    public bool IsSide()
    {
        return isSide;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            isSide = true;
        }
        else
        {
            Debug.Log("プレイヤー");
            isSide = false;
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isSide = false;

    }

}
