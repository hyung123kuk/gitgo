using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBoss : Monster
{
    [SerializeField]
    public GameObject[] item;
   

   public void StartBossMonster()
    {
        StartMonster();
    }


    


    public void BossDrop()
    {
        if (!isSpread)
        {
            int rand = Random.Range(0, item.Length);

            for (int i = 0; i < item.Length; i++)
            {

                if (rand == i)
                {
                    int point = Random.Range(-1, 2);
                    Instantiate(item[i], transform.position+ Vector3.up +new Vector3(point * 0.5f, itemUpPoint, point * 0.5f), Quaternion.identity);
                }
            }
            
        }
    }
}
