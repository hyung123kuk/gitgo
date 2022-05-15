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
                SkillLevel.text = "<color=#FF0000>레벨 : " + skill.Level.ToString() + "</color>";
            }
            else
            {
                SkillLevel.text = "레벨 : " + skill.Level.ToString();
            }
        }
        else
            SkillLevel.enabled = false;
    }
    private void Type(SkillUI skill)
    {
        if (skill.skillType == SkillUI.SkillType.Attack)
        {
            SkillKinds.text = "타입 : 공격";
        }
        else if(skill.skillType == SkillUI.SkillType.Buff)
        {
            SkillKinds.text = "타입 : 버프";
        }
        else if (skill.skillType == SkillUI.SkillType.Move)
        {
            SkillKinds.text = "타입 : 이동";
        }

    }
    private void cooltime(SkillUI skill)
    {
        #region 전사 스킬
        if (skill.skillCharacter == SkillUI.SkillCharacter.Warrior)
        {
            if (skill.skillNum == 1) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnSkill_1_Cool().ToString("F1") + "초"; }
            else if (skill.skillNum == 2) {  SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnBuffCool().ToString("F1") + "초"; }
            else if (skill.skillNum == 3) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnSkill_2_Cool().ToString("F1") + "초"; }
            else if (skill.skillNum == 4) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnSkill_3_Cool().ToString("F1") + "초"; }

        }
        #endregion
        #region 궁수 스킬
        if (skill.skillCharacter == SkillUI.SkillCharacter.Archer)
        {
            if (skill.skillNum == 1) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnSkill_1_Cool().ToString("F1") + "초"; }
            else if (skill.skillNum == 2) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnBuffCool().ToString("F1") + "초"; }
            else if (skill.skillNum == 3) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnSkill_2_Cool().ToString("F1") + "초"; }
            else if (skill.skillNum == 4) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnSkill_3_Cool().ToString("F1") + "초"; }

        }
        #endregion
        #region 법사 스킬
        if (skill.skillCharacter == SkillUI.SkillCharacter.Mage)
        {
            if (skill.skillNum == 1) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnTeleportCool().ToString("F1") + "초"; }
            else if (skill.skillNum == 2) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnSkill_1_Cool().ToString("F1") + "초"; }
            else if (skill.skillNum == 3) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnSkill_2_Cool().ToString("F1") + "초"; }
            else if (skill.skillNum == 4) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnSkill_3_Cool().ToString("F1") + "초"; }

        }
        #endregion

        #region 공용 스킬
        if (skill.skillCharacter == SkillUI.SkillCharacter.Common)
        {
            if (skill.skillNum == 0) { SkillCoolTime.text = "쿨타임 : " + attackDamage.ReturnDodgeCool().ToString() + "초"; }


        }
        #endregion

    }
    private void SkillAbility(SkillUI skill)
    {
        #region 전사 스킬
        if (skill.skillCharacter == SkillUI.SkillCharacter.Warrior)
        {
            if (skill.skillNum == 1)
            {
                if (attackDamage.Skill_1_per_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  " + attackDamage.Skill_1_fixed_dam;
                }
                else if (attackDamage.Skill_1_fixed_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_1_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_1_per_dam * 100 + "%  +  " + attackDamage.Skill_1_fixed_dam;
                }
            }
            else if (skill.skillNum == 2) { SkillAbillity.text = "지속시간 : " + attackDamage.Skill_Buff_duration.ToString("F1"); }
            else if (skill.skillNum == 3) {
                if (attackDamage.Skill_2_per_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  " + attackDamage.Skill_2_fixed_dam;
                }
                else if (attackDamage.Skill_2_fixed_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_2_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_2_per_dam * 100 + "%  +  " + attackDamage.Skill_2_fixed_dam;
                }
            }
            else if (skill.skillNum == 4) {
                if (attackDamage.Skill_3_per_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  " + attackDamage.Skill_3_fixed_dam;
                }
                else if (attackDamage.Skill_3_fixed_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_3_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_3_per_dam * 100 + "%  +  " + attackDamage.Skill_3_fixed_dam;
                }
            }

        }
        #endregion
        #region 궁수 스킬
        if (skill.skillCharacter == SkillUI.SkillCharacter.Archer)
        {
            if (skill.skillNum == 1)
            {
                if (attackDamage.Skill_1_per_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  " + attackDamage.Skill_1_fixed_dam;
                }
                else if (attackDamage.Skill_1_fixed_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_1_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_1_per_dam * 100 + "%  +  " + attackDamage.Skill_1_fixed_dam;
                }
            }
            else if (skill.skillNum == 2) { SkillAbillity.text = "지속시간 : " + attackDamage.Skill_Buff_duration.ToString("F1"); }
            else if (skill.skillNum == 3)
            {
                if (attackDamage.Skill_2_per_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  " + attackDamage.Skill_2_fixed_dam;
                }
                else if (attackDamage.Skill_2_fixed_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_2_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_2_per_dam * 100 + "%  +  " + attackDamage.Skill_2_fixed_dam;
                }
            }
            else if (skill.skillNum == 4)
            {
                if (attackDamage.Skill_3_per_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  " + attackDamage.Skill_3_fixed_dam;
                }
                else if (attackDamage.Skill_3_fixed_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_3_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_3_per_dam * 100 + "%  +  " + attackDamage.Skill_3_fixed_dam;
                }
            }

        }
        #endregion
        #region 법사 스킬
        if (skill.skillCharacter == SkillUI.SkillCharacter.Mage)
        {
            if (skill.skillNum == 1) { SkillAbillity.text = "텔레포트 이동 "; }
            else if (skill.skillNum == 2) {
                if (attackDamage.Skill_1_per_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  " + attackDamage.Skill_1_fixed_dam;
                }
                else if (attackDamage.Skill_1_fixed_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_1_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_1_per_dam * 100 + "%  +  " + attackDamage.Skill_1_fixed_dam;
                }
            }
            else if (skill.skillNum == 3)
            {
                if (attackDamage.Skill_2_per_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  " + attackDamage.Skill_2_fixed_dam;
                }
                else if (attackDamage.Skill_2_fixed_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_2_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_2_per_dam * 100 + "%  +  " + attackDamage.Skill_2_fixed_dam;
                }
            }
            else if (skill.skillNum == 4)
            {
                if (attackDamage.Skill_3_per_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  " + attackDamage.Skill_3_fixed_dam;
                }
                else if (attackDamage.Skill_3_fixed_dam == 0f)
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_3_per_dam * 100 + "%";
                }
                else
                {
                    SkillAbillity.text = "스킬 데미지 :  공격력 * " + attackDamage.Skill_3_per_dam * 100 + "%  +  " + attackDamage.Skill_3_fixed_dam;
                }
            }

        }
        #endregion

        #region 공용 스킬
        if (skill.skillCharacter == SkillUI.SkillCharacter.Common)
        {
            if (skill.skillNum == 0) { SkillAbillity.text = "구르기 "; }


        }
        #endregion
    }
    private void SkillExp(SkillUI skill)
    {
        SkillText.text = skill.skillText;

    }
}
