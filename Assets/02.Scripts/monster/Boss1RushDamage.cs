using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1RushDamage : MonoBehaviour
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
        yield return new WaitForSeconds(1f);
        Collider[] colliders =
                    Physics.OverlapSphere(transform.position, size, LayerMask.GetMask("Player"));

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerST livingEntity = colliders[i].GetComponent<PlayerST>();
            if (livingEntity != null && !livingEntity.isDie && !livingEntity.isDamage)
            {
                livingEntity.GetComponent<PlayerStat>().DamagedHp(100);
                livingEntity.GetComponent<PlayerST>().healthbar.fillAmount = livingEntity.GetComponent<PlayerStat>().playerstat._Hp /
                    livingEntity.GetComponent<PlayerStat>().playerstat._MAXHP;
                Debug.Log("������ ��" + livingEntity.GetComponent<PlayerStat>()._Hp);
                if (livingEntity.GetComponent<PlayerStat>()._Hp <= 0)
                {
                    livingEntity.GetComponent<PlayerST>().PlayerDie();
                }
            }
        }
        StartCoroutine(DamageDelay());
    }
}
