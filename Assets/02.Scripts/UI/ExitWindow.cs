using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ExitWindow : MonoBehaviourPun
{
    [SerializeField]
    public static ExitWindow ExWindow;

    [SerializeField]
    GameObject exitWindow;
    [SerializeField]
    GameObject exitMenu;
    [SerializeField]
    GameObject soundMenu;
    [SerializeField]
    CharacterSel characterSel;

    public static bool isExitMenu = false;

    private void Awake()
    {
        ExWindow = this;
        exitWindow = transform.GetChild(0).gameObject;
        exitMenu = transform.GetChild(0).GetChild(3).gameObject;
        soundMenu = transform.GetChild(0).GetChild(4).gameObject;
        
        characterSel = FindObjectOfType<CharacterSel>();
    }


    public void ExitWindowOn()
    {
        exitWindow.SetActive(true);
        isExitMenu = true;
        ChangeExitMenu();
    }

    public void ExitWindowOff()
    {
        UiSound.uiSound.UiOptionSound();
        exitWindow.SetActive(false);
        isExitMenu = false;
    }

    public void ChangeExitMenu()
    {
        UiSound.uiSound.UiOptionSound();
        exitMenu.SetActive(true);
        soundMenu.SetActive(false);
    }

    public void ChangeSoundMenu()
    {
        UiSound.uiSound.UiOptionSound();
        exitMenu.SetActive(false);
        soundMenu.SetActive(true);
    }

    public void SelCharacterMenu()
    {
        
        AllUI.allUI.AllWindowEnd();
        PhotonNetwork.LeaveRoom();
    }
    

    public void ExitGame()
    {
        Application.Quit();
    }



}
