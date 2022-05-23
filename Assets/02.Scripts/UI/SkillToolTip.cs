using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject skillToolTip;
    [SerializeField]
    private Text SkillName;
    [SerializeField]
    private Text SkillLevel;
    [SerializeField]
    private Text SkillKinds;
    [SerializeField]
    private Text SkillCoolTime;
    [SerializeField]
    private Text SkillAbillity;
    [SerializeField]
    private Text SkillText;
    [SerializeField]
    private PlayerStat playerStat;
    [SerializeField]
    private AttackDamage attackDamage; 


    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        attackDamage = FindObjectOfType<AttackDamage>();
        skillToolTip = transform.GetChild(0).gameObject;
        SkillName = skillToolTip.transform.GetChild(0).GetComponent<Text>();
        SkillLevel = skillToolTip.transform.GetChild(1).GetComponent<Text>();
        SkillKinds = skillToolTip.transform.GetChild(2).GetComponent<Text>();
        SkillCoolTime = skillToolTip.transform.GetChild(3).GetComponent<Text>();
        SkillAbillity = skillToolTip.transform.GetChild(4).GetComponent<Text>();
        SkillText = skillToolTip.transform.GetChild(5).GetComponent<Text>();
    }


    public void ToolTipOn(SkillUI skill, Vector2 toolTipPoint)
    {
        transform.position = toolTipPoint;


        Name(skill);
        Level(skill);
        Type(skill);
        cooltime(skill);
        SkillAbility(skill);
        SkillExp(skill);


        skillToolTip.SetActive(true);
    }

    public void ToolTipOff()
    {
        skillToolTip.SetActive(false);

    }

    private void Name(SkillUI skill)
    {
        SkillName.text = "<color=#9ACD32>" + skill.skillName.ToString() + "</color>"; 
    }
    private void Level(SkillUI skill)
    {
        if (skill.Level != 0f)
        {
            SkillLevel.enabled = true;
            if (skill.Level > playerStat.Level)
            {
                SkillLevel.text = "<color=#FF0000>���� : " + skill.Level.ToString() + "</color>";
            }
            else
            {
                SkillLevel.text = "���� : " + skill.Level.ToString();
            }
        }
        else
            SkillLevel.enabled = false;
    }
    private void Type(SkillUI skill)
    {
        if (skill.skillType == SkillUI.SkillType.Attack)
        {
            SkillKinds.text = "Ÿ�� : ����";
        }
        else if(skill.skillType == SkillUI.SkillType.Buff)
        {
            SkillKinds.text = "Ÿ�� : ����";
        }
        else if (skill.skillType == SkillUI.SkillType.Move)
        {
            SkillKinds.text = "Ÿ�� : �̵�";
        }

    }
    private void cooltime(SkillUI skill)
    {
        #region ���� ��ų
        if (skill.skillCharacter == SkillUI.SkillCharacter.Warrior)
        {
            if (skill.skillNum == 1) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnSkill_1_Cool().ToString("F1") + "��"; }
            else if (skill.skillNum == 2) {  SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnBuffCool().ToString("F1") + "��"; }
            else if (skill.skillNum == 3) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnSkill_2_Cool().ToString("F1") + "��"; }
            else if (skill.skillNum == 4) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnSkill_3_Cool().ToString("F1") + "��"; }

        }
        #endregion
        #region �ü� ��ų
        if (skill.skillCharacter == SkillUI.SkillCharacter.Archer)
        {
            if (skill.skillNum == 1) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnSkill_1_Cool().ToString("F1") + "��"; }
            else if (skill.skillNum == 2) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnBuffCool().ToString("F1") + "��"; }
            else if (skill.skillNum == 3) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnSkill_2_Cool().ToString("F1") + "��"; }
            else if (skill.skillNum == 4) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnSkill_3_Cool().ToString("F1") + "��"; }

        }
        #endregion
        #region ���� ��ų
        if (skill.skillCharacter == SkillUI.SkillCharacter.Mage)
        {
            if (skill.skillNum == 1) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnTeleportCool().ToString("F1") + "��"; }
            else if (skill.skillNum == 2) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnSkill_1_Cool().ToString("F1") + "��"; }
            else if (skill.skillNum == 3) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnSkill_2_Cool().ToString("F1") + "��"; }
            else if (skill.skillNum == 4) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnSkill_3_Cool().ToString("F1") + "��"; }

        }
        #endregion

        #region ���� ��ų
        if (skill.skillCharacter == SkillUI.SkillCharacter.Common)
        {
            if (skill.skillNum == 0) { SkillCoolTime.text = "��Ÿ�� : " + attackDamage.ReturnDodgeCool().ToString() + "��"; }


        }
        #endregion

    }
    private void SkillAbility(SkillUI skill)
    {
        #region ���� ��ų
        if (skill.skillCharacter == SkillUI.SkillCharacter.Warrior)
        {
            if (skill.skillNum == 1)
            {
                if (attackDamage.Skill_1_per_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  " + attackDamage.Skill_1_fixed_dam;
                }
                else if (attackDamage.Skill_1_fixed_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_1_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_1_per_dam * 100 + "%  +  " + attackDamage.Skill_1_fixed_dam;
                }
            }
            else if (skill.skillNum == 2) { SkillAbillity.text = "���ӽð� : " + attackDamage.Skill_Buff_duration.ToString("F1"); }
            else if (skill.skillNum == 3) {
                if (attackDamage.Skill_2_per_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  " + attackDamage.Skill_2_fixed_dam;
                }
                else if (attackDamage.Skill_2_fixed_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_2_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_2_per_dam * 100 + "%  +  " + attackDamage.Skill_2_fixed_dam;
                }
            }
            else if (skill.skillNum == 4) {
                if (attackDamage.Skill_3_per_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  " + attackDamage.Skill_3_fixed_dam;
                }
                else if (attackDamage.Skill_3_fixed_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_3_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_3_per_dam * 100 + "%  +  " + attackDamage.Skill_3_fixed_dam;
                }
            }

        }
        #endregion
        #region �ü� ��ų
        if (skill.skillCharacter == SkillUI.SkillCharacter.Archer)
        {
            if (skill.skillNum == 1)
            {
                if (attackDamage.Skill_1_per_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  " + attackDamage.Skill_1_fixed_dam;
                }
                else if (attackDamage.Skill_1_fixed_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_1_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_1_per_dam * 100 + "%  +  " + attackDamage.Skill_1_fixed_dam;
                }
            }
            else if (skill.skillNum == 2) { SkillAbillity.text = "���ӽð� : " + attackDamage.Skill_Buff_duration.ToString("F1"); }
            else if (skill.skillNum == 3)
            {
                if (attackDamage.Skill_2_per_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  " + attackDamage.Skill_2_fixed_dam;
                }
                else if (attackDamage.Skill_2_fixed_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_2_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_2_per_dam * 100 + "%  +  " + attackDamage.Skill_2_fixed_dam;
                }
            }
            else if (skill.skillNum == 4)
            {
                if (attackDamage.Skill_3_per_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  " + attackDamage.Skill_3_fixed_dam;
                }
                else if (attackDamage.Skill_3_fixed_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_3_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_3_per_dam * 100 + "%  +  " + attackDamage.Skill_3_fixed_dam;
                }
            }

        }
        #endregion
        #region ���� ��ų
        if (skill.skillCharacter == SkillUI.SkillCharacter.Mage)
        {
            if (skill.skillNum == 1) { SkillAbillity.text = "�ڷ���Ʈ �̵� "; }
            else if (skill.skillNum == 2) {
                if (attackDamage.Skill_1_per_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  " + attackDamage.Skill_1_fixed_dam;
                }
                else if (attackDamage.Skill_1_fixed_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_1_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_1_per_dam * 100 + "%  +  " + attackDamage.Skill_1_fixed_dam;
                }
            }
            else if (skill.skillNum == 3)
            {
                if (attackDamage.Skill_2_per_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  " + attackDamage.Skill_2_fixed_dam;
                }
                else if (attackDamage.Skill_2_fixed_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_2_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_2_per_dam * 100 + "%  +  " + attackDamage.Skill_2_fixed_dam;
                }
            }
            else if (skill.skillNum == 4)
            {
                if (attackDamage.Skill_3_per_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  " + attackDamage.Skill_3_fixed_dam;
                }
                else if (attackDamage.Skill_3_fixed_dam == 0f)
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_3_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "��ų ������ :  ���ݷ� * " + attackDamage.Skill_3_per_dam * 100 + "%  +  " + attackDamage.Skill_3_fixed_dam;
                }
            }

        }
        #endregion

        #region ���� ��ų
        if (skill.skillCharacter == SkillUI.SkillCharacter.Common)
        {
            if (skill.skillNum == 0) { SkillAbillity.text = "������ "; }


        }
        #endregion
    }
    private void SkillExp(SkillUI skill)
    {
        SkillText.text = skill.skillText;

    }
}
