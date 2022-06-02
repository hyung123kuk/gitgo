using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSkill : MonoBehaviour
{
    public static ArrowSkill arrowSkill;
    public float damage;
    public bool NoDestroy; //����ü�� �ƴ� ������ ������ų�����͵� �ݶ��̴��� ���������� �����Ǳ⶧���� ����������ŵ�ϴ�.


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
