using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterItem : MonoBehaviour
{
    [SerializeField]
    private GameObject[] item;




    public void Drop()
    {
        int rand=Random.Range(0, item.Length);
        Debug.Log(item.Length);
        for(int i = 0; i < item.Length; i++)
        {
            Debug.Log(rand);
            Debug.Log(i);
            if(rand == i)
            {
                Debug.Log(item[i]);
                Instantiate(item[i], transform.position, Quaternion.identity);
            }
        }

       
    }
}
