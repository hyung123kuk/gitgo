using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public CharacterSel CharSel;
    public GameObject Wor;
    public GameObject Arc;
    public GameObject mage;
    public Vector3 StartPosition = new Vector3(30, 3, 50);
    public Transform startPoint;
    public Transform HorsePoint;
    private SaveManager saveManager;

    public static GameManager gameManager;

   
    void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
        gameManager = this;
        CharSel = GameObject.FindGameObjectWithTag("SelManager").GetComponent<CharacterSel>();
        CharSel.audioListener.enabled = false;

        //CharSel.sel.SetActive(false);
        if (CharSel.charSel == 1)
        {
            if (CharSel.character1 == CharacterSel.Type.Warrior)
            {
                GameObject character = PhotonNetwork.Instantiate("gameWarrior1", startPoint.position, Quaternion.identity);
                GameObject Horseee = PhotonNetwork.Instantiate("Horse", GameManager.gameManager.HorsePoint.position, Quaternion.identity);
            }
            else if (CharSel.character1 == CharacterSel.Type.Archer)
            {
                GameObject character = PhotonNetwork.Instantiate("gameArcher1", startPoint.position, Quaternion.identity);
                GameObject Horseee = PhotonNetwork.Instantiate("Horse", GameManager.gameManager.HorsePoint.position, Quaternion.identity);

            }
            else if (CharSel.character1 == CharacterSel.Type.Mage)
            {
                GameObject character = PhotonNetwork.Instantiate("gameMage1", startPoint.position, Quaternion.identity);
                GameObject Horseee = PhotonNetwork.Instantiate("Horse", GameManager.gameManager.HorsePoint.position, Quaternion.identity);

            }
        }
        else if (CharSel.charSel == 2)
        {
            if (CharSel.character2 == CharacterSel.Type.Warrior)
            {
                GameObject character = PhotonNetwork.Instantiate("gameWarrior1", startPoint.position, Quaternion.identity);
                GameObject Horseee = PhotonNetwork.Instantiate("Horse", GameManager.gameManager.HorsePoint.position, Quaternion.identity);

            }
            else if (CharSel.character2 == CharacterSel.Type.Archer)
            {
                GameObject character = PhotonNetwork.Instantiate("gameArcher1", startPoint.position, Quaternion.identity);
                GameObject Horseee = PhotonNetwork.Instantiate("Horse", GameManager.gameManager.HorsePoint.position, Quaternion.identity);

            }
            else if (CharSel.character2 == CharacterSel.Type.Mage)
            {
                GameObject character = PhotonNetwork.Instantiate("gameMage1", startPoint.position, Quaternion.identity);
                GameObject Horseee = PhotonNetwork.Instantiate("Horse", GameManager.gameManager.HorsePoint.position, Quaternion.identity);

            }
        }

    }





}
