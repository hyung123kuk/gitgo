using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetEff : MonoBehaviour
{
    Transform target;
    NavMeshAgent navi;
    void Start()
    {
        navi = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navi.SetDestination(target.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject,0.3f);
        }
    }
}
