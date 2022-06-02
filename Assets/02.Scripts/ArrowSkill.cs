using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSkill : MonoBehaviour
{
    public static ArrowSkill arrowSkill;
    public float damage;
    public bool NoDestroy; //투사체가 아닌 전사의 돌진스킬같은것도 콜라이더가 벽에닿으면 삭제되기때문에 오류를일으킵니다.


    private void Start()
    {
        arrowSkill = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if((!NoDestroy&&other.gameObject.tag == "Build") || (!NoDestroy && other.gameObject.tag == "WALL"))
        {
            Destroy(gameObject, 0.1f);
        }
            
    }
}
