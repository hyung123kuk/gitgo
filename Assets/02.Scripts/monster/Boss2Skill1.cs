using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Skill1 : MonoBehaviour
{
    public float size;

    void Awake()
    {
        StartCoroutine(DamageDelay());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, size);
    }

    IEnumerator DamageDelay() //1�ʸ��� ����Ʈ�� �����ȿ������� ������
    {
       
        Collider[] colliders =
                    Physics.OverlapSphere(transform.position, size, LayerMask.GetMask("Player"));

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerST livingEntity = colliders[i].GetComponent<PlayerST>();
            if (livingEntity != null && !livingEntity.isDie && !livingEntity.isDamage)
            {
                livingEntity.GetComponent<PlayerStat>()._Hp -= 100;
                Debug.Log("������ ��" + livingEntity.GetComponent<PlayerStat>()._Hp);
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(DamageDelay());
    }
}
