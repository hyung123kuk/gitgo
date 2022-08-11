using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : MonoBehaviourPun
{
    public float damage;

    private void Start()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Enemy" || other.gameObject.tag == "Build" || other.gameObject.tag == "WALL")
        {
            Destroy(gameObject,0.1f);  
        }
    }
}
