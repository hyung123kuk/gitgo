using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject player;
    public Transform WorldPortal;
    public Transform DunjeonPortal;
    public Transform BossPortalIN;
    public Transform BossPortalOUT;
    public static Portal portal;
    void Start()
    {
        portal = this;
        WorldPortal = transform.GetChild(0);
        DunjeonPortal = transform.GetChild(1);
        BossPortalIN = transform.GetChild(2);
        BossPortalOUT = transform.GetChild(3);
    }

}
