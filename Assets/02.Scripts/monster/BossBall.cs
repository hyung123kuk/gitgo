using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBall : EnemyAttack
{

    public Transform target;
    NavMeshAgent navi;
    public GameObject eff;
    private Rigidbody rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navi = GetComponent<NavMeshAgent>();
    }

   
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navi.SetDestination(target.position);
        rigid.AddTorque(transform.right * 5, ForceMode.Force);
    }
    
}
