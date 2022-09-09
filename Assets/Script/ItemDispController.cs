using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDispController : MonoBehaviour
{
    private GameObject player = null;
    private SpriteRenderer spRenderer = null;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        spRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
       if(  player.GetComponent<PlayerController>()._getAttackItemUse() )
        {
            spRenderer.enabled = true;
        }
       else
        {
            spRenderer.enabled = false;
        }

      //  Debug.Log("AttackUse" + player.GetComponent<PlayerController>()._getAttackItemUse() );


    }
}
