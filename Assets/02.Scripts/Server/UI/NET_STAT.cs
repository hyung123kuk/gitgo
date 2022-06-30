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

    public static bool isNet_Stat;

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

    
    public void Net_StatWindowOn()
    {
        UiSound.uiSound.UiOptionSound();
        StatWindowDesign.SetActive(true);
        isNet_Stat = true;
        AllUI.allUI.CheckCursorLock();
    }

    public void Net_StatWindowOff()
    {
        UiSound.uiSound.UiOptionSound();
        StatWindowDesign.SetActive(false);
        isNet_Stat = false;
        AllUI.allUI.CheckCursorLock();

    }


    public void SetStat(int CharacterType, int Level, float _STR, float _DEX, float _INT, float _DAMAGE,
                            float _DEFENCE, float _MOVE_SPEED, float _SKILL_COOLTIME_DEC_PER, float _SKILL_ADD_DAMAGE_PER, float _CRITICAL_PROBABILITY, float _CRITICAL_ADD_DAMAGE_PER)
    {

        if (CharacterType == 0)
        {
            Character.text = "직업 : 전사";
        }
        else if (CharacterType == 1)
        {
            Character.text = "직업 : 궁수";
        }
        else if (CharacterType == 2)
        {
            Character.text = "직업 : 법사";
        }

        level.text = "레벨 : " + Level;

        STR.text = "힘 : " + (int)_STR;
        DEX.text = "민첩 : " + (int)_DEX;
        INT.text = "지능 : " + (int)_INT;

        CoolTIme.text = "쿨타임 감소량 : " + (int)_SKILL_COOLTIME_DEC_PER;
        SkillAddDamage.text = "스킬 추가 데미지 : " + (int)_SKILL_ADD_DAMAGE_PER;
        DAMAGE.text = "공격력 : " + (int)_DAMAGE;
        DEFENCE.text = "방어력 : " + (int)_DEFENCE;


        MOVESPEED.text = "이동속도 : " + (int)(5 + (5 * _MOVE_SPEED / 100)) * 10;
        Critical_Probable.text = "크리티컬 확률 : " + (int)_CRITICAL_PROBABILITY;
        Critical_Damage.text = "크리티컬 데미지 : " + (int)_CRITICAL_ADD_DAMAGE_PER;


    }

}
