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

        GameObject[] HorseCheck = GameObject.FindGameObjectsWithTag("Horse"); //�÷��̾��� �� ã��.
        GameObject[] PlayerCheck = GameObject.FindGameObjectsWithTag("Player"); //�÷��̾� ã��.
        Horse = HorseCheck[0];

        if (PlayerCheck.Length > 1) //�÷��̾�
        {
            for (int i = 1; i < PlayerCheck.Length; i++)
            {
                if ((PlayerCheck[i].GetComponent<PlayerMine>().photonView.ViewID != //�ڽ� �Ÿ��� ismine�� true�� �� ��� ã��
                    Player.GetComponent<PlayerMine>().photonView.ViewID) && PlayerCheck[i].GetComponent<PlayerMine>().photonView.IsMine == true)
                {
                    PhotonNetwork.Destroy(PlayerCheck[i]);
                    PlayerCheck[i] = null;
                }
            }
        }

        if (HorseCheck.Length > 1) //�÷��̾��� ��
        {
            for (int i = 1; i < HorseCheck.Length; i++)
            {
                if ((HorseCheck[i].GetComponent<Horse>().photonView.ViewID != //�ڽ� �Ÿ��� ismine�� true�� �� ��� ã��
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
