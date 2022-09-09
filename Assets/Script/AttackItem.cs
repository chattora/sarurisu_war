using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackItem : MonoBehaviour
{

    private float cntTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cntTime += Time.deltaTime;

        if (cntTime > 3) //３秒で消す
        {
            _AttackItemDestory();
        }

    }

    void _AttackItemDestory()
    {
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.tag == "Enemy")
       {
            _AttackItemDestory();
      }
    }
}
