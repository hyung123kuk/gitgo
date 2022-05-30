using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSkill : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Build" || other.gameObject.tag == "WALL")
        {
            Destroy(gameObject, 0.1f);
        }
            
    }
}
