using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject player;
    public Transform WorldPortal;
    public Transform DunjeonPortal;
    public static Portal portal;
    void Start()
    {
        portal = this;
        WorldPortal = transform.GetChild(0);
        DunjeonPortal = transform.GetChild(1);
    }

}
