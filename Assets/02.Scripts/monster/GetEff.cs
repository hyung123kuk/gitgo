using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetEff : MonoBehaviour
{
    Transform target;
    NavMeshAgent navi;
    PlayerStat playerStat;
    public float exp;
    void Start()
    {
        navi = GetComponent<NavMeshAgent>();
        playerStat = FindObjectOfType<PlayerStat>();

    }

    public void SetExp(float _exp)
    {
        exp = _exp;
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
            playerStat.AddExp(exp);
            Destroy(gameObject,0.3f);
        }
    }
}
