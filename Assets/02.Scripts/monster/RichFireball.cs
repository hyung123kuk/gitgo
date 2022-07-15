using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichFireball : MonoBehaviour
{
    public float size;


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, size);
    }
}
