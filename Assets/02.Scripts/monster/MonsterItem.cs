using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterItem : MonoBehaviour
{
    [SerializeField]
    private Item[] item;




    public void Drop()
    {
        float rand=Random.Range(0f, 100f);
        if (rand < 100)
        {
            Instantiate(item[0].itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
