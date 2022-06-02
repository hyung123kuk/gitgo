using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatWindow : MonoBehaviour ,IPointerClickHandler
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
    private Text  DAMAGE;
    [SerializeField]
    private Text  DEFENCE;
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
    private PlayerStat playerStat;
    [SerializeField]
    private PlayerST playerST;
    
    public static StatWindow statWindow;

    public static bool tDown;

    private void Awake()
    {
        statWindow = this;
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
        playerStat = FindObjectOfType<PlayerStat>();
        
        allUI = FindObjectOfType<AllUI>();
        tDown = false;
       
    }

    private void Start()
    {
        playerST = FindObjectOfType<PlayerST>();
        if (playerST.CharacterType == PlayerST.Type.Warrior)
        {
            Character.text = "직업 : 전사";
        }
        else if (playerST.CharacterType == PlayerST.Type.Archer)
        {
            Character.text = "직업 : 궁수";
        }
        else if (playerST.CharacterType == PlayerST.Type.Mage)
        {
            Character.text = "직업 : 법사";
        }
    }

    void Update()
    {
        
    }

    public void SetLevel()
    {
        level.text = "레벨 : " + playerStat.Level;
    }
    public void SetStat()
    {
        STR.text = "힘 : " + (int)playerStat._STR;
        DEX.text = "민첩 : " + (int)playerStat._DEX;
        INT.text = "지능 : " + (int)playerStat._INT;

        CoolTIme.text = "쿨타임 감소량 : " + (int)playerStat._SKILL_COOLTIME_DEC_PER ;
        SkillAddDamage.text = "스킬 추가 데미지 : " + (int)playerStat._SKILL_ADD_DAMAGE_PER ;
        DAMAGE.text = "공격력 : " + (int)playerStat._DAMAGE;
        DEFENCE.text = "방어력 : " + (int)playerStat._DEFENCE;

        MOVESPEED.text = "이동속도 : " + (int) playerST.speed*10; 
        Critical_Probable.text = "크리티컬 확률 : " + (int)playerStat._CRITICAL_PROBABILITY ;
        Critical_Damage.text = "크리티컬 데미지 : " + (int)playerStat._CRITICAL_ADD_DAMAGE_PER ;


    }

    public void StatWindowOn()
    {
        UiSound.uiSound.UiOptionSound();
        StatWindowDesign.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(true);
        tDown = true;

    }

    public void StatWindowOff()
    {
        UiSound.uiSound.UiOptionSound();
        StatWindowDesign.SetActive(false);

        tDown = false;
        AllUI.allUI.CheckCursorLock();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        allUI.StatWindowTop();
    }
}
