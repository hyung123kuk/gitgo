using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage;
    public bool isMelee;
    private void OnTriggerEnter(Collider other)
    {
        
        if (!isMelee&&other.tag == "Player")
        {
            Destroy(gameObject, 0.5f);
        }
    }
}
