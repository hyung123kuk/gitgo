using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMine : MonoBehaviourPun
{
    public GameObject Player; //�ڱⲨ
    public GameObject Horse;



    private void Start()
    {
        //if (!photonView.IsMine)
        //    return;

        Player = this.gameObject;
        Horse = FindObjectOfType<Horse>().gameObject;

        //Horse[] horses = FindObjectsOfType<Horse>();
        //foreach (Horse horse1 in horses)
        //{
        //    if (horse1.GetComponent<PhotonView>().IsMine)
        //    {
        //        Debug.Log(horse1);
        //        Horse = horse1.gameObject;
        //        break;
        //    }
        //}

    }

    void Playerfind()
    {
        if (!photonView.IsMine)
            return;

        GameObject[] HorseCheck = GameObject.FindGameObjectsWithTag("Horse"); //�÷��̾��� �� ã��.
        foreach (GameObject horse1 in HorseCheck)
        {
            if ((horse1.GetComponent<PhotonView>().ViewID != //�ڽ� �Ÿ��� ismine�� true�� �� ��� ã��
                   Horse.GetComponent<PhotonView>().ViewID) && horse1.GetComponent<PhotonView>().IsMine == true)
            {
                Debug.Log("�� ����");
                PhotonNetwork.Destroy(horse1);
            }
        }

        GameObject[] PlayerCheck = GameObject.FindGameObjectsWithTag("Player"); //�÷��̾��� �� ã��.
        foreach (GameObject player1 in PlayerCheck)
        {
            if ((player1.GetComponent<PhotonView>().ViewID != //�ڽ� �Ÿ��� ismine�� true�� �� ��� ã��
                   Player.GetComponent<PhotonView>().ViewID) && player1.GetComponent<PhotonView>().IsMine == true)
            {
                Debug.Log("�÷��̾� ����");
                PhotonNetwork.Destroy(player1);
                //if (PhotonNetwork.IsMasterClient)
                //{
                //    //Debug.Log("����");
                //    //SpawnManager.spawnManager.RemoteMonsterReset();
                //}
            }
        }
    }

    private void Update()
    {
        Playerfind();
    }


}
