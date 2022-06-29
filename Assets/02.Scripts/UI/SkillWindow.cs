using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class SkillWindow : MonoBehaviourPun, IPointerClickHandler
{
    [SerializeField]
    public static SkillWindow skillwindow;

    [SerializeField]
    private GameObject skillWindow;
    [SerializeField]
    private AllUI allUI;
    [SerializeField]
    private GameObject warriorSkillWIndow;
    [SerializeField]
    private GameObject acherSkillWIndow;
    [SerializeField]
    private GameObject mageSkillWIndow;
    [SerializeField]
    public PlayerST playerST;
    public PlayerStat playerStat;
    [SerializeField]
    private SkillSlot[] skillslots;
    [SerializeField]
    private SkillToolTip skillToolTip;


    public static bool kDown = false;

    private void Awake()
    {
        skillwindow = this;
        allUI = FindObjectOfType<AllUI>();
        warriorSkillWIndow = transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
        acherSkillWIndow = transform.GetChild(0).GetChild(0).GetChild(3).gameObject;
        mageSkillWIndow = transform.GetChild(0).GetChild(0).GetChild(4).gameObject;

        skillslots = GetComponentsInChildren<SkillSlot>();
        skillToolTip = FindObjectOfType<SkillToolTip>();


    }
    private void Start()
    {
        
        PlayerST[] playerSts = FindObjectsOfType<PlayerST>();


        foreach (PlayerST myplayerSt in playerSts)
        {
            if (myplayerSt.GetComponent<PhotonView>().IsMine)
            {
                playerST = myplayerSt;
                break;
            }
        }

        if (playerST.CharacterType == PlayerST.Type.Warrior)
        {
            warriorSkillWIndow.SetActive(true);
            acherSkillWIndow.SetActive(false);
            mageSkillWIndow.SetActive(false);
        }
        else if (playerST.CharacterType == PlayerST.Type.Archer)
        {
            warriorSkillWIndow.SetActive(false);
            acherSkillWIndow.SetActive(true);
            mageSkillWIndow.SetActive(false);
        }
        else if (playerST.CharacterType == PlayerST.Type.Mage)
        {
            warriorSkillWIndow.SetActive(false);
            acherSkillWIndow.SetActive(false);
            mageSkillWIndow.SetActive(true);
        }

        StartCoroutine(Setting());


    }

    IEnumerator Setting()
    {
        SkillWindowOn();
        yield return new WaitForSeconds(0.1f);
        SkillWindowOff();
        SkillToolTipOff();

    }

    public void SkillToolTipOff()
    {
        skillToolTip.ToolTipOff();
    }


    public void SkillWindowOn()
    {
        UiSound.uiSound.UiOptionSound();
        skillWindow.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(true);
        kDown = true;

    }

    public void SkillWindowOff()
    {
        UiSound.uiSound.UiOptionSound();
        skillWindow.SetActive(false);

        kDown = false;
        AllUI.allUI.CheckCursorLock();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        allUI.SkillWindowTop();
    }

    public void AllSkillSlotColor()
    {
        for (int i = 0; i < skillslots.Length; i++)
        {
            skillslots[i].SetSkillColor();

        }
    }
}
