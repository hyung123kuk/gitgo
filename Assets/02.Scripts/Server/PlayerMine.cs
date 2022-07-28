using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMine : MonoBehaviourPun
{
    public GameObject Player; //자기꺼
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

        GameObject[] HorseCheck = GameObject.FindGameObjectsWithTag("Horse"); //플레이어의 말 찾기.
        foreach (GameObject horse1 in HorseCheck)
        {
            if ((horse1.GetComponent<PhotonView>().ViewID != //자신 거르고 ismine이 true가 된 대상 찾기
                   Horse.GetComponent<PhotonView>().ViewID) && horse1.GetComponent<PhotonView>().IsMine == true)
            {
                Debug.Log("말 삭제");
                PhotonNetwork.Destroy(horse1);
            }
        }

        GameObject[] PlayerCheck = GameObject.FindGameObjectsWithTag("Player"); //플레이어의 말 찾기.
        foreach (GameObject player1 in PlayerCheck)
        {
            if ((player1.GetComponent<PhotonView>().ViewID != //자신 거르고 ismine이 true가 된 대상 찾기
                   Player.GetComponent<PhotonView>().ViewID) && player1.GetComponent<PhotonView>().IsMine == true)
            {
                Debug.Log("플레이어 삭제");
                PhotonNetwork.Destroy(player1);
                //if (PhotonNetwork.IsMasterClient)
                //{
                //    //Debug.Log("응애");
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
