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

    IEnumerator DamageDelay() //1초마다 이펙트의 범위안에있으면 데미지
    {
       
        Collider[] colliders =
                    Physics.OverlapSphere(transform.position, size, LayerMask.GetMask("Player"));

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerST livingEntity = colliders[i].GetComponent<PlayerST>();
            if (livingEntity != null && !livingEntity.isDie && !livingEntity.isDamage)
            {
                livingEntity.GetComponent<PlayerStat>()._Hp -= 100;
                Debug.Log("데미지 들어감" + livingEntity.GetComponent<PlayerStat>()._Hp);
                livingEntity.GetComponent<PlayerST>().healthbar.fillAmount = livingEntity.GetComponent<PlayerStat>()._Hp /
                    livingEntity.GetComponent<PlayerStat>()._MAXHP;
                Debug.Log("데미지 들어감" + livingEntity.GetComponent<PlayerStat>()._Hp);
                if (livingEntity.GetComponent<PlayerStat>()._Hp <= 0)
                {
                    livingEntity.GetComponent<PlayerST>().PlayerDie();
                }
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(DamageDelay());
    }
}
