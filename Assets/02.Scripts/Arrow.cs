using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Enemy")
        {
            Destroy(gameObject,0.1f);  
        }
    }
}
