using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NET_TradeRecieve : MonoBehaviourPun
{
   

    [SerializeField]
    GameObject NetWorkRecive;
    [SerializeField]
    AllUI allUI;

    [SerializeField]
    NET_UIPlayer net_UIPlayer;

    public static bool isNET_Trade_ReicieveWindow;

    private void Start()
    {
       
        allUI = FindObjectOfType<AllUI>();
        NET_UIPlayer[] net_uiplayes = GameObject.FindObjectsOfType<NET_UIPlayer>();
        foreach (NET_UIPlayer myUIPlaye in net_uiplayes)
        {
            if (myUIPlaye.GetComponent<PhotonView>().IsMine)
            {
                net_UIPlayer = myUIPlaye;
                break;
            }
        }
    }

    public void Net_RecieveOn()
    {
        isNET_Trade_ReicieveWindow = true;
        NetWorkRecive.SetActive(true);
       
        allUI.CheckCursorLock();
    }

    public void Cancle()
    {
        NetWorkRecive.SetActive(false);
        isNET_Trade_ReicieveWindow = false;
        allUI.CheckCursorLock();
    }

    public void Recieve()
    {
        if (!NET_UIPlayer.TradeOn)
        {
            net_UIPlayer.TradeConnect();
        }
    }
    
}
