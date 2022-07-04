using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NET_PartyRecieve : MonoBehaviourPun
{
    [SerializeField]
    public  GameObject partyRecieveWindow;
    public static bool ispartyRecieveWindow;
    AllUI allUI;
    NET_PartyPlayer NET_partyPlayer;
    private void Start()
    {
        allUI = FindObjectOfType<AllUI>();
        NET_PartyPlayer[] net_partyplayes = GameObject.FindObjectsOfType<NET_PartyPlayer>();
        foreach (NET_PartyPlayer mypartPlaye in net_partyplayes)
        {
            if (mypartPlaye.GetComponent<PhotonView>().IsMine)
            {
                NET_partyPlayer = mypartPlaye;
                break;
            }
        }
    }

    public void PartyRecieveWindowOn()
    {

        partyRecieveWindow.SetActive(true);
        ispartyRecieveWindow = true;
        allUI.CheckCursorLock();
    }

    public void PartyRecieveWindowOff() //°ÅÀý
    {
        partyRecieveWindow.SetActive(false);
        ispartyRecieveWindow = false;
        NET_partyPlayer.isParty = false;
        NET_partyPlayer.partyFail();
        allUI.CheckCursorLock();
    }

    public void Recieve()
    {
        NET_partyPlayer.PartyRecieve();
        partyRecieveWindow.SetActive(false);
        ispartyRecieveWindow = false;
        allUI.CheckCursorLock();
    }
}
