using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public List<Transform> Point = new List<Transform>();

    private void OnEnable()
    {
       
    }

    private void OnDrawGizmos()
    {
        Point.Clear();
        Point.AddRange(GetComponentsInChildren<Transform>());
        Point.RemoveAt(0);
        Gizmos.color = Color.red;
        for (int i = 0; i < Point.Count -1 ; i++)
        {
            
           
            Gizmos.DrawLine(Point[i].position, Point[i + 1].position);
        }
        

    }
}
