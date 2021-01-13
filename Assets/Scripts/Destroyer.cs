using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    //[SerializeField] private float deactivationTime;
    //private float timer = 0;
    //private bool deActivate = false;
    private void Update()
    {
        if (transform.position.y < -10)
        {
            //gameObject.SetActive(false);
            //Pool.singleton.pooledItems.Remove(gameObject);
            Destroy(gameObject);
        }
            
        
        // if(deActivate)
        //     DeActivate();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6)
        {
            //deActivate = true;
            Destroy(gameObject, 2f);
        }
            
    }

    // private void DeActivate()
    // {
    //     timer += Time.deltaTime;
    //     if (timer >= deactivationTime)
    //     {
    //         timer -= timer;
    //         gameObject.SetActive(false);
    //         deActivate = false;
    //     }
    // }
}
