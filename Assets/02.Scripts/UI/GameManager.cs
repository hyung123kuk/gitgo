using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] communicators = new GameObject[4];//플레이어 담기
    public CharacterSel CharSel;
    public GameObject Wor;
    public GameObject Arc;
    public GameObject mage;
    public Vector3 StartPosition = new Vector3(30, 3, 50);
    public Transform startPoint;
    public Transform HorsePoint;
    public PlayerST[] PlayerSts = new PlayerST[4];


    public static GameManager gameManager;

   
    void Awake()
    {

        gameManager = this;
        CharSel = GameObject.FindGameObjectWithTag("SelManager").GetComponent<CharacterSel>();
        //CharSel.sel.SetActive(false);
        if (CharSel.charSel == 1)
        {
            if (CharSel.character1 == CharacterSel.Type.Warrior)
            {

                GameObject character = PhotonNetwork.Instantiate("gameWarrior", startPoint.position, Quaternion.identity);
                GameObject Horseee = PhotonNetwork.Instantiate("Horse", GameManager.gameManager.HorsePoint.position, Quaternion.identity);
                Horseee.transform.SetParent(character.transform, false);
                character.GetComponent<PlayerST>().photonView.RPC("synchronization", RpcTarget.AllBuffered);

                //변수할당 동기화

                //GameObject Horsee = PhotonNetwork.Instantiate("Horse", HorsePoint.position, Quaternion.identity);
                //Horsee.transform.SetParent(character.transform, false);

                //GameObject character=Instantiate(Wor, startPoint.position, Quaternion.identity);
                //character.transform.GetChild(0).position = StartPosition;
            }
            else if (CharSel.character1 == CharacterSel.Type.Archer)
            {
                GameObject character = PhotonNetwork.Instantiate("playerArcher", startPoint.position, Quaternion.identity);
                character.transform.GetChild(0).position = StartPosition;
            }
            else if (CharSel.character1 == CharacterSel.Type.Mage)
            {
                GameObject character = PhotonNetwork.Instantiate("playerMage", startPoint.position, Quaternion.identity);
                character.transform.GetChild(0).position = StartPosition;
            }
        }
        else if (CharSel.charSel == 2)
        {
            if (CharSel.character2 == CharacterSel.Type.Warrior)
            {
                GameObject character = PhotonNetwork.Instantiate("PlayerWarrior", startPoint.position, Quaternion.identity);
                character.transform.GetChild(0).position = StartPosition;
                //GameObject character = Instantiate(Wor, startPoint.position, Quaternion.identity);
                //character.transform.GetChild(0).position = StartPosition;
            }
            else if (CharSel.character2 == CharacterSel.Type.Archer)
            {
                GameObject character = PhotonNetwork.Instantiate("playerArcher", startPoint.position, Quaternion.identity);
                character.transform.GetChild(0).position = StartPosition;
            }
            else if (CharSel.character2 == CharacterSel.Type.Mage)
            {
                GameObject character = PhotonNetwork.Instantiate("playerMage", startPoint.position, Quaternion.identity);
                character.transform.GetChild(0).position = StartPosition;
            }
        }

    }

    void Update()
    {
        //우선 배열크기는 4로 했는데 나중에 더 늘리겠습니다
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");

        if (communicators[0] == null || communicators[1] == null || communicators[2] == null || communicators[3] == null)
            for (int i = 0; i < Players.Length; i++) //플레이어 차례대로 넣기
            {
                if (Players[i].GetComponent<PhotonView>().ViewID == 1001)
                    communicators[0] = Players[i];
                if (Players[i].GetComponent<PhotonView>().ViewID == 2001)
                    communicators[1] = Players[i];
                if (Players[i].GetComponent<PhotonView>().ViewID == 3001)
                    communicators[2] = Players[i];
                if (Players[i].GetComponent<PhotonView>().ViewID == 4001)
                    communicators[3] = Players[i];
            }

        if (Players.Length != 0) //플레이어 스크립트넣기
        {
            if (communicators[0] != null)
                PlayerSts[0] = communicators[0].GetComponent<PlayerST>();
            if (communicators[1] != null)
                PlayerSts[1] = communicators[1].GetComponent<PlayerST>();
            if (communicators[2] != null)
                PlayerSts[2] = communicators[2].GetComponent<PlayerST>();
            if (communicators[3] != null)
                PlayerSts[3] = communicators[3].GetComponent<PlayerST>();
        }

        if (PlayerSts[0] != null)   //PlayerST 스크립트에서 스위치가 켜지면 오브젝트 끄기
        {
            if (PlayerSts[0].endswitch)
            {
                communicators[0].SetActive(false);
            }
        }
        if (PlayerSts[1] != null)
        {
            if (PlayerSts[1].endswitch)
            {
                communicators[1].SetActive(false);
            }
        }
        if (PlayerSts[2] != null)
        {
            if (PlayerSts[2].endswitch)
            {
                communicators[2].SetActive(false);
            }
        }
        if (PlayerSts[3] != null)
        {
            if (PlayerSts[3].endswitch)
            {
                communicators[3].SetActive(false);
            }
        }

    }

}
