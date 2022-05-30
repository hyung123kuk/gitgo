using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Enemy" || other.gameObject.tag == "Build" || other.gameObject.tag == "WALL")
        {
            Destroy(gameObject,0.1f);  
        }
    }
}
