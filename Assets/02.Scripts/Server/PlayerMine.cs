using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMine : MonoBehaviourPun
{
    public GameObject Player;
    public GameObject Horse;
    

    private void Start()
    {
       
        Player = this.gameObject;

        if(photonView.IsMine)
        Horse = GameObject.FindGameObjectWithTag("Horse");

    }

    [PunRPC]
    void Playerfind()
    {
        if (!photonView.IsMine)
            return;

        GameObject[] HorseCheck = GameObject.FindGameObjectsWithTag("Horse"); //플레이어의 말 찾기.
        GameObject[] PlayerCheck = GameObject.FindGameObjectsWithTag("Player"); //플레이어 찾기.
        Horse = HorseCheck[0];

        if (PlayerCheck.Length > 1) //플레이어
        {
            for (int i = 1; i < PlayerCheck.Length; i++)
            {
                if ((PlayerCheck[i].GetComponent<PlayerMine>().photonView.ViewID != //자신 거르고 ismine이 true가 된 대상 찾기
                    Player.GetComponent<PlayerMine>().photonView.ViewID) && PlayerCheck[i].GetComponent<PlayerMine>().photonView.IsMine == true)
                {
                    PhotonNetwork.Destroy(PlayerCheck[i]);
                    PlayerCheck[i] = null;
                }
            }
        }

        if (HorseCheck.Length > 1) //플레이어의 말
        {
            for (int i = 1; i < HorseCheck.Length; i++)
            {
                if ((HorseCheck[i].GetComponent<Horse>().photonView.ViewID != //자신 거르고 ismine이 true가 된 대상 찾기
                   Horse.GetComponent<Horse>().photonView.ViewID) && HorseCheck[i].GetComponent<Horse>().photonView.IsMine == true)
                {
                    PhotonNetwork.Destroy(HorseCheck[i]);
                    HorseCheck[i] = null;
                }
            }
        }
    }

    private void Update()
    {
        Playerfind();
    }

    
}
