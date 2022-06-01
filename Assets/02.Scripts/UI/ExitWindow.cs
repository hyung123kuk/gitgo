using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitWindow : MonoBehaviour
{
    [SerializeField]
    public static ExitWindow ExWindow;

    [SerializeField]
    GameObject exitWindow;
    [SerializeField]
    GameObject exitMenu;
    [SerializeField]
    GameObject soundMenu;

    public static bool isExitMenu = false;

    private void Awake()
    {
        ExWindow = this;
        exitWindow = transform.GetChild(0).gameObject;
        exitMenu = transform.GetChild(0).GetChild(3).gameObject;
        soundMenu = transform.GetChild(0).GetChild(4).gameObject;
    }


    public void ExitWindowOn()
    {
        exitWindow.SetActive(true);
        isExitMenu = true;
        ChangeExitMenu();
    }

    public void ExitWindowOff()
    {
        exitWindow.SetActive(false);
        isExitMenu = false;
    }

    public void ChangeExitMenu()
    {
        exitMenu.SetActive(true);
        soundMenu.SetActive(false);
    }

    public void ChangeSoundMenu()
    {
        exitMenu.SetActive(false);
        soundMenu.SetActive(true);
    }

    public void SelCharacterMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }



}
