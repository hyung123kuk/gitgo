using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuikSlot : MonoBehaviour
{
    [SerializeField]
    QuestStore questStore;
    [SerializeField]
    public Slot slot;
    [SerializeField]
    public SkillSlot skill;
    [SerializeField]
    public Image CoolTimeImage;
    [SerializeField]
    private PlayerST playerST;
    [SerializeField]
    public Weapons weapons;

    [SerializeField]
    private AttackDamage attckDamage;
    [SerializeField]
    private BuffSkillUI[] buffSkillUI;

    public static QuikSlot quikSlot;

    void Start()
    {
        slot=gameObject.GetComponent<Slot>();
        skill = gameObject.GetComponent<SkillSlot>();
        playerST = FindObjectOfType<PlayerST>();
        weapons = FindObjectOfType<Weapons>();
        quikSlot = this;
        attckDamage = FindObjectOfType<AttackDamage>();
        buffSkillUI = FindObjectsOfType<BuffSkillUI>();
        questStore = FindObjectOfType<QuestStore>();
    }


    public void setCoolImage()
    {
        if(skill.skill == null)
        {
            SetCollColor(0);
        }


        if (skill.skill != null)
        {
            CoolTimeImage.sprite = skill.skill.SkillImage;
            SetCollColor(1);


        }

    }

    
    void Update()
    {
        weapons = FindObjectOfType<Weapons>(); //무기 교체될때마다 갱신되어야해서 여기다 우선 뒀습니다 Playerst에서 찾으면 첫번째Q퀵슬롯은 되는데
        //그다음 퀵슬롯들은 바뀌지않습니다 ㅠㅠ
        if (inventory.iDown || SkillWindow.kDown || StatWindow.tDown)
            return;


        if (slot.item!=null && Input.GetButtonDown(gameObject.tag)) //아이템 사용
        {
            Debug.Log(slot.item.itemName + " 을 사용했습니다.");
            slot.UseItem();
            slot.SetSlotCount(-1);
        }

        else if(skill.skill != null && skill.skill.skillCharacter==SkillUI.SkillCharacter.Archer && skill.skill.skillNum == 4) // 궁수 충전형 스킬
        {
            if(Input.GetButton(gameObject.tag))
            {
                weapons.archer_skill4_Order = 0;
                weapons.EnergyArrow();
            }
            else if(Input.GetButtonUp(gameObject.tag))
            {
                weapons.archer_skill4_Order = 1;
                weapons.EnergyArrow();
                StartCoroutine(Skill3());
            }
        }

        else if (skill.skill != null && skill.skill.skillCharacter == SkillUI.SkillCharacter.Mage && skill.skill.skillNum == 4) // 마법사 충전형 스킬
        {
            if (Input.GetButton(gameObject.tag))
            {
                weapons.mage_skill4_Order = 0;
                weapons.Meteo();
            }
            else if (Input.GetButtonUp(gameObject.tag))
            {
                weapons.mage_skill4_Order = 1;
                weapons.Meteo();
                StartCoroutine(Skill3());
            }
        }


        else if(skill.skill!=null && Input.GetButtonDown(gameObject.tag))
        {
            
            #region 전사 스킬
            if(skill.skill.skillCharacter == SkillUI.SkillCharacter.Warrior)
            {
                if(skill.skill.skillNum == 1 && attckDamage.Usable_Skill1 ) {                    
                    playerST.Block();
                    playerST.StartCoroutine(Skill1());
                }
                else if (skill.skill.skillNum == 2 && attckDamage.Usable_Buff) { playerST.Buff(); StartCoroutine(Buff()) ; buffSkillUI[0].BuffOn(BuffSkillUI.BuffSkills.WarriorBuff1, CoolTimeImage.sprite); }
                else if (skill.skill.skillNum == 3 && attckDamage.Usable_Skill2) { playerST.Rush(); StartCoroutine(Skill2()); }
                else if (skill.skill.skillNum == 4 && attckDamage.Usable_Skill3) { playerST.Aura(); StartCoroutine(Skill3()); }

            }
            #endregion
            #region 궁수 스킬
            if (skill.skill.skillCharacter == SkillUI.SkillCharacter.Archer)
            {
                if (skill.skill.skillNum == 1 && attckDamage.Usable_Skill1) { playerST.Smoke(); StartCoroutine(Skill1()); }
                else if (skill.skill.skillNum == 2 && attckDamage.Usable_Buff) { playerST.PoisonArrow(); StartCoroutine(Buff()); buffSkillUI[0].BuffOn(BuffSkillUI.BuffSkills.ArcherBuff1 , CoolTimeImage.sprite); }
                else if (skill.skill.skillNum == 3 && attckDamage.Usable_Skill2) { weapons.BombArrow(); StartCoroutine(Skill2()); }

            }
            #endregion
            #region 법사 스킬
            if (skill.skill.skillCharacter == SkillUI.SkillCharacter.Mage)
            {
                if (skill.skill.skillNum == 1 && attckDamage.Usable_Teleport) { playerST.Flash(); StartCoroutine(Telleport()); }
                else if (skill.skill.skillNum == 2 && attckDamage.Usable_Skill1) { weapons.LightningBall(); StartCoroutine(Skill1()); }
                else if (skill.skill.skillNum == 3 && attckDamage.Usable_Skill2) { weapons.IceAge(); StartCoroutine(Skill2()); }

            }
            #endregion

            #region 공용 스킬
            if (skill.skill.skillCharacter == SkillUI.SkillCharacter.Common)
            {
                if (skill.skill.skillNum == 0 && attckDamage.Usable_Dodge) { 
                    playerST.Dodge(); 
                    StartCoroutine(Dodge());
                    QuestStore.qustore.MainQuestSuccess(1);

                }              
            }
            #endregion

        }


    }

    public QuikSlot UseQuikSlot()
    {
        return this;
    }

    public void SetCollColor(float _alpha)
    {
        Color color = CoolTimeImage.color;
        color.a = _alpha;
        CoolTimeImage.color = color;
    }



    IEnumerator Skill1()
    {
              
        float fillmount = 1 - attckDamage.Skill1_passedTime / attckDamage.Skill1_time; 
        while (fillmount >= 0f)
        {

            fillmount = 1 - attckDamage.Skill1_passedTime / attckDamage.Skill1_time;
            if (fillmount != 1 && PlayerST.isCool1)      
                    CoolTimeImage.fillAmount = fillmount; 
            
            yield return new WaitForSeconds(0.02f);
            
        }
        CoolTimeImage.fillAmount = 0f;
        yield return null;
    }

    IEnumerator Skill2()
    {
        float fillmount2= 1 - attckDamage.Skill2_passedTime / attckDamage.Skill2_time;
        while (fillmount2 >= 0f)
        {
            fillmount2 = 1 - attckDamage.Skill2_passedTime / attckDamage.Skill2_time;
            if (fillmount2 != 1&& PlayerST.isCool2)
                CoolTimeImage.fillAmount = fillmount2;

            yield return new WaitForSeconds(0.02f);

        }
        CoolTimeImage.fillAmount = 0f;
        yield return null;
    }

    IEnumerator Skill3()
    {
        float fillmount3 = 1 - attckDamage.Skill3_passedTime / attckDamage.Skill3_time;
        while (fillmount3 >= 0f)
        {
            fillmount3 = 1 - attckDamage.Skill3_passedTime / attckDamage.Skill3_time;
            if (fillmount3 != 1 && PlayerST.isCool3)
                CoolTimeImage.fillAmount = fillmount3;

            yield return new WaitForSeconds(0.02f);

        }
        CoolTimeImage.fillAmount = 0f;
        yield return null;
    }

    IEnumerator Skill4()
    {
        float fillmount4 = 1 - attckDamage.Skill4_passedTime / attckDamage.Skill4_time;
        while (fillmount4 >= 0f)
        {
            fillmount4 = 1 - attckDamage.Skill4_passedTime / attckDamage.Skill4_time;
            if (fillmount4 != 1&& PlayerST.isCool4)
                CoolTimeImage.fillAmount = fillmount4;

            yield return new WaitForSeconds(0.02f);

        }
        CoolTimeImage.fillAmount = 0f;
        yield return null;
    }

    IEnumerator Buff()
    {
        float fillmount5 = 1 - attckDamage.SkillBuff_passedTime / attckDamage.SkillBuff_time;
        while (fillmount5 >= 0f)
        {
            fillmount5 = 1 - attckDamage.SkillBuff_passedTime / attckDamage.SkillBuff_time;
            if (fillmount5 != 1)
                CoolTimeImage.fillAmount = fillmount5;

            yield return new WaitForSeconds(0.02f);

        }
        CoolTimeImage.fillAmount = 0f;
        yield return null;
    }

    IEnumerator Telleport()
    {
        float fillmount5 = 1 - attckDamage.SkillTeleport_passedTime / attckDamage.SkillTeleport_time;
        while (fillmount5 >= 0f)
        {
            fillmount5 = 1 - attckDamage.SkillTeleport_passedTime / attckDamage.SkillTeleport_time;
            if (fillmount5 != 1 && PlayerST.isCoolTeleport)
                CoolTimeImage.fillAmount = fillmount5;

            yield return new WaitForSeconds(0.02f);

        }
        CoolTimeImage.fillAmount = 0f;
        yield return null;
    }

    IEnumerator Dodge()
    {
        float fillmount6 = 1 - attckDamage.SkillDodge_passedTime / attckDamage.SkillDodge_time;
        while (fillmount6 >= 0f)
        {
            fillmount6 = 1 - attckDamage.SkillDodge_passedTime / attckDamage.SkillDodge_time;
            if (fillmount6 != 1&& PlayerST.isCooldodge)
                CoolTimeImage.fillAmount = fillmount6;

            yield return new WaitForSeconds(0.02f);

        }
        CoolTimeImage.fillAmount = 0f;
        yield return null;
    }
}

