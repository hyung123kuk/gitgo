using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    public CharacterSel CharSel;
    public GameObject Wor;
    public GameObject Arc;
    public GameObject mage;
    public Vector3 StartPosition = new Vector3(30, 3, 50);
    public Transform startPoint;
    public Transform HorsePoint;
                                 
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
    // Update is called once per frame
    void Update()
    {
        //if (CharSel.charSel == 1)
        //{
        //    if (CharSel.character1 == CharacterSel.Type.Warrior)
        //    {
        //        GameObject character = PhotonNetwork.Instantiate("gameWarrior", startPoint.position, Quaternion.identity);
        //        GameObject Horse = PhotonNetwork.Instantiate("Horse", startPoint.position, Quaternion.identity);
        //        Horse.gameObject.SetActive(false);
        //        Horse.transform.parent = character.transform;
        //        //GameObject character=Instantiate(Wor, startPoint.position, Quaternion.identity);
        //        //character.transform.GetChild(0).position = StartPosition;
        //    }
        //    else if (CharSel.character1 == CharacterSel.Type.Archer)
        //    {
        //        GameObject character = PhotonNetwork.Instantiate("playerArcher", startPoint.position, Quaternion.identity);
        //        character.transform.GetChild(0).position = StartPosition;
        //    }
        //    else if (CharSel.character1 == CharacterSel.Type.Mage)
        //    {
        //        GameObject character = PhotonNetwork.Instantiate("playerMage", startPoint.position, Quaternion.identity);
        //        character.transform.GetChild(0).position = StartPosition;
        //    }
        //}
        //else if (CharSel.charSel == 2)
        //{
        //    if (CharSel.character2 == CharacterSel.Type.Warrior)
        //    {
        //        GameObject character = PhotonNetwork.Instantiate("PlayerWarrior", startPoint.position, Quaternion.identity);
        //        character.transform.GetChild(0).position = StartPosition;
        //        //GameObject character = Instantiate(Wor, startPoint.position, Quaternion.identity);
        //        //character.transform.GetChild(0).position = StartPosition;
        //    }
        //    else if (CharSel.character2 == CharacterSel.Type.Archer)
        //    {
        //        GameObject character = PhotonNetwork.Instantiate("playerArcher", startPoint.position, Quaternion.identity);
        //        character.transform.GetChild(0).position = StartPosition;
        //    }
        //    else if (CharSel.character2 == CharacterSel.Type.Mage)
        //    {
        //        GameObject character = PhotonNetwork.Instantiate("playerMage", startPoint.position, Quaternion.identity);
        //        character.transform.GetChild(0).position = StartPosition;
        //    }
        //}
    }


}
