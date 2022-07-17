using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage;
    public bool isMelee;
    public bool isRazor;
    private void OnTriggerEnter(Collider other)
    {
        
        if (!isMelee&& !isRazor && other.tag == "Player")
        {
            Destroy(gameObject, 0.5f);
        }
        if(!isMelee && !isRazor && other.tag == "WALL")
        {
            Destroy(gameObject);
        }
    }
}
