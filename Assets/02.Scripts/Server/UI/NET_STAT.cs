using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NET_STAT : MonoBehaviour
{
    [SerializeField]
    GameObject StatWindowDesign;
    [SerializeField]
    AllUI allUI;
    [SerializeField]
    private Image CharacterImage;
    [SerializeField]
    private Text Character;
    [SerializeField]
    private Text level;
    [SerializeField]
    private Text STR;
    [SerializeField]
    private Text DEX;
    [SerializeField]
    private Text INT;
    [SerializeField]
    private Text DAMAGE;
    [SerializeField]
    private Text DEFENCE;
    [SerializeField]
    private Text MOVESPEED;
    [SerializeField]
    private Text CoolTIme;
    [SerializeField]
    private Text SkillAddDamage;
    [SerializeField]
    private Text Critical_Probable;
    [SerializeField]
    private Text Critical_Damage;

    [SerializeField]
    NET_UIPlayer net_UIPlayer;
    void Start()
    {
        StatWindowDesign = transform.GetChild(0).gameObject;
        CharacterImage = transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>();
        Character = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>();
        level = transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Text>();
        STR = transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<Text>();
        DEX = transform.GetChild(0).GetChild(0).GetChild(5).GetComponent<Text>();
        INT = transform.GetChild(0).GetChild(0).GetChild(6).GetComponent<Text>();
        DAMAGE = transform.GetChild(0).GetChild(0).GetChild(7).GetComponent<Text>();
        DEFENCE = transform.GetChild(0).GetChild(0).GetChild(8).GetComponent<Text>();
        MOVESPEED = transform.GetChild(0).GetChild(0).GetChild(9).GetComponent<Text>();
        CoolTIme = transform.GetChild(0).GetChild(0).GetChild(10).GetComponent<Text>();
        SkillAddDamage = transform.GetChild(0).GetChild(0).GetChild(11).GetComponent<Text>();
        Critical_Probable = transform.GetChild(0).GetChild(0).GetChild(12).GetComponent<Text>();
        Critical_Damage = transform.GetChild(0).GetChild(0).GetChild(13).GetComponent<Text>();


        allUI = FindObjectOfType<AllUI>();

        NET_UIPlayer[] net_uiplayes = GameObject.FindObjectsOfType<NET_UIPlayer>();


        foreach (NET_UIPlayer myUIPlaye in net_uiplayes)
        {
            if (myUIPlaye.GetComponent<PhotonView>().IsMine)
            {
                net_UIPlayer = myUIPlaye;
                break;
            }
        }
    }

    
    void Update()
    {
        
    }
}
