using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{


    
    // void Update()
    // {
    //     castTimer += Time.deltaTime;
    //     if(castTimer >= castInterval)
    //     {
            
    //         Collider[] hitInfo = Physics.OverlapBox(transform.position + boxOffset,boxSize,transform.root.rotation,mask);

    //         foreach(Collider collider in hitInfo)
    //         {
    //             EnemyAI enemyAI = collider.transform.root.GetComponent<EnemyAI>();
    //             if(enemyAI != null)
    //             {
    //                 enemyAI.Burn(fireDamage);
    //             }
    //         }

    //         castTimer = 0f;
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        EnemyAI enemyAI = other.transform.root.GetComponent<EnemyAI>();
        if(enemyAI != null)
        {
            enemyAI.Burn();
        }
    }

    void OnTriggerStay(Collider other)
    {
        EnemyAI enemyAI = other.transform.root.GetComponent<EnemyAI>();
        if(enemyAI != null)
        {
            enemyAI.Burn();
        }
    }
}
