using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuikSlot : MonoBehaviour
{
    
    [SerializeField]
    public Slot slot;
    [SerializeField]
    public SkillSlot skill;
    [SerializeField]
    public Image CoolTimeImage;
    [SerializeField]
    private PlayerST playerST;
    [SerializeField]
    private PlayerStat playerStat;
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
        playerStat = FindObjectOfType<PlayerStat>();
        weapons = FindObjectOfType<Weapons>();
        quikSlot = this;
        attckDamage = FindObjectOfType<AttackDamage>();
        buffSkillUI = FindObjectsOfType<BuffSkillUI>();

        
    }


    public void addItem(QuikSlot quikSlot,Item _item, int _itemCount)
    {

        quikSlot.GetComponent<Slot>().AddItem(_item, _itemCount);
        
    }

    public void addSkill(QuikSlot quikSlot,SkillUI _skill)
    {
        quikSlot.GetComponent<SkillSlot>().skill = _skill;
        quikSlot.GetComponent<SkillSlot>().Setimage();
        quikSlot.GetComponent<SkillSlot>().SetColor(1);
        setCoolImage();
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
        weapons = FindObjectOfType<Weapons>(); //���� ��ü�ɶ����� ���ŵǾ���ؼ� ����� �켱 �׽��ϴ� Playerst���� ã���� ù��°Q�������� �Ǵµ�
        //�״��� �����Ե��� �ٲ����ʽ��ϴ� �Ф�
        if (AllUI.isUI)
            return;


        if (slot.item!=null && slot.item.itemType== Item.ItemType.Used && Input.GetButtonDown(gameObject.tag)) //������ ���
        {
            Debug.Log(slot.item.itemName + " �� ����߽��ϴ�.");
            slot.UseItem();
            slot.SetSlotCount(-1);
        }
        else if (slot.item != null && slot.item.itemType == Item.ItemType.Ride && Input.GetButtonDown(gameObject.tag)) //�� ���
        {
            playerST.HorseRide();

        }

        else if(skill.skill != null && skill.skill.skillCharacter==SkillUI.SkillCharacter.Archer && skill.skill.skillNum == 4) // �ü� ������ ��ų
        {
            if (attckDamage.Skill_3_use_Mp > playerStat._Mp)
            {
                LogManager.logManager.Log("������ �����ϴ�.", true);
                return;
            }
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

        else if (skill.skill != null && skill.skill.skillCharacter == SkillUI.SkillCharacter.Mage && skill.skill.skillNum == 4) // ������ ������ ��ų
        {
            if (attckDamage.Skill_3_use_Mp > playerStat._Mp)
            {
                LogManager.logManager.Log("������ �����ϴ�.", true);
                return;
            }
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
            
            #region ���� ��ų
            if(skill.skill.skillCharacter == SkillUI.SkillCharacter.Warrior)
            {

                if(skill.skill.skillNum == 1 && attckDamage.Usable_Skill1 ) {
                    if (attckDamage.Skill_1_use_Mp > playerStat._Mp)
                    {
                        LogManager.logManager.Log("������ �����ϴ�.", true);
                        return;
                    }
                    playerST.Block();
                    playerST.StartCoroutine(Skill1());
                }

                else if (skill.skill.skillNum == 2 && attckDamage.Usable_Buff) 
                {
                    if (attckDamage.Skill_Buff_use_Mp > playerStat._Mp)
                    {
                        LogManager.logManager.Log("������ �����ϴ�.", true);
                        return;
                    }
                    playerST.Buff(); StartCoroutine(Buff()) ; 
                    buffSkillUI[0].BuffOn(BuffSkillUI.BuffSkills.WarriorBuff1, CoolTimeImage.sprite); }

                else if (skill.skill.skillNum == 3 && attckDamage.Usable_Skill2) {
                    if (attckDamage.Skill_2_use_Mp > playerStat._Mp)
                    {
                        LogManager.logManager.Log("������ �����ϴ�.", true);
                        return;
                    }
                    playerST.Rush(); StartCoroutine(Skill2()); }

                else if (skill.skill.skillNum == 4 && attckDamage.Usable_Skill3) { playerST.Aura();
                    if (attckDamage.Skill_3_use_Mp > playerStat._Mp)
                    {
                        LogManager.logManager.Log("������ �����ϴ�.", true);
                        return;
                    }
                    StartCoroutine(Skill3()); }

            }
            #endregion
            #region �ü� ��ų
            if (skill.skill.skillCharacter == SkillUI.SkillCharacter.Archer)
            {
                if (skill.skill.skillNum == 1 && attckDamage.Usable_Skill1) {
                    if (attckDamage.Skill_1_use_Mp > playerStat._Mp)
                    {
                        LogManager.logManager.Log("������ �����ϴ�.", true);
                        return;
                    }
                    playerST.Smoke(); StartCoroutine(Skill1()); }
                else if (skill.skill.skillNum == 2 && attckDamage.Usable_Buff) {
                    if (attckDamage.Skill_Buff_use_Mp > playerStat._Mp)
                    {
                        LogManager.logManager.Log("������ �����ϴ�.", true);
                        return;
                    }
                    playerST.PoisonArrow(); StartCoroutine(Buff()); buffSkillUI[0].BuffOn(BuffSkillUI.BuffSkills.ArcherBuff1 , CoolTimeImage.sprite); }
                else if (skill.skill.skillNum == 3 && attckDamage.Usable_Skill2) {
                    if (attckDamage.Skill_2_use_Mp > playerStat._Mp)
                    {
                        LogManager.logManager.Log("������ �����ϴ�.", true);
                        return;
                    }
                    weapons.BombArrow(); StartCoroutine(Skill2()); }

            }
            #endregion
            #region ���� ��ų
            if (skill.skill.skillCharacter == SkillUI.SkillCharacter.Mage)
            {
                if (skill.skill.skillNum == 1 && attckDamage.Usable_Teleport) {
                    if (attckDamage.Skill_TelePort_Mp > playerStat._Mp)
                    {
                        LogManager.logManager.Log("������ �����ϴ�.", true);
                        return;
                    }
                    playerST.Flash(); StartCoroutine(Telleport()); }
                else if (skill.skill.skillNum == 2 && attckDamage.Usable_Skill1) {
                    if (attckDamage.Skill_1_use_Mp > playerStat._Mp)
                    {
                        LogManager.logManager.Log("������ �����ϴ�.", true);
                        return;
                    }
                    weapons.LightningBall(); StartCoroutine(Skill1()); }
                else if (skill.skill.skillNum == 3 && attckDamage.Usable_Skill2) {
                    if (attckDamage.Skill_2_use_Mp > playerStat._Mp)
                    {
                        LogManager.logManager.Log("������ �����ϴ�.", true);
                        return;
                    }
                    weapons.IceAge(); StartCoroutine(Skill2()); }

            }
            #endregion

            #region ���� ��ų
            if (skill.skill.skillCharacter == SkillUI.SkillCharacter.Common)
            {
                if (skill.skill.skillNum == 0 && attckDamage.Usable_Dodge) {
                    if (attckDamage.Skill_Dodge_Mp > playerStat._Mp)
                    {
                        LogManager.logManager.Log("������ �����ϴ�.", true);
                        return;
                    }
                    playerST.Dodge(); 
                    StartCoroutine(Dodge());
                    
                   

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
            if (fillmount != 1 && playerST.isCool1)      
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
            if (fillmount2 != 1&& playerST.isCool2)
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
            if (fillmount3 != 1 && playerST.isCool3)
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
            if (fillmount4 != 1&& playerST.isCool4)
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
            if (fillmount5 != 1 && playerST.isCoolTeleport)
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
            if (fillmount6 != 1&& playerST.isCooldodge)
                CoolTimeImage.fillAmount = fillmount6;

            yield return new WaitForSeconds(0.02f);

        }
        CoolTimeImage.fillAmount = 0f;
        
        yield return null;
    }
}

