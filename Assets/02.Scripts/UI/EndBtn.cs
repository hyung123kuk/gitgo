using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EndBtn : MonoBehaviourPun
{
    [SerializeField]
    private PlayerST playerst;

    public Button btn1, btn2;
        

    private void Awake()
    {
        PlayerST[] playerSts = FindObjectsOfType<PlayerST>();
        foreach (PlayerST myPlayerst in playerSts)
        {
            if (myPlayerst.photonView.IsMine)
            {
                playerst = myPlayerst;
                break;
            }
        }
    }

}
