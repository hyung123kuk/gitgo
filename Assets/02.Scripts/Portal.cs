using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject player;
    public Transform InPortal;
    public Transform OutPortal;
    public static Portal portal;
    void Start()
    {
        portal = this;
        InPortal = GameObject.Find("In-Portal").transform;
        OutPortal = GameObject.Find("Out-Portal").transform;
    }

    public void InDunJeonPortal(GameObject player)
    {
        player.transform.position = OutPortal.transform.position;
    }

    public void OutDunJeonPortal(GameObject player)
    {
        player.transform.position = InPortal.transform.position;
    }
}
