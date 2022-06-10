using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    [SerializeField]
    private PlayerStat playerStat;
    [Header("스킬 기본 쿨타임")]
    [Header("       버프 스킬")]
    [SerializeField]
    private float Skill_Buff_cooltime = 5;    
    public bool Usable_Buff = true;
    [Header("스킬 지속 시간")]
    [SerializeField]
    public float Skill_Buff_duration = 3;
    public bool Duration_Buff = false;
    [Header("스킬 사용 마나")]
    [SerializeField]
    public float Skill_Buff_use_Mp = 50;


    [SerializeField]
    [Header("스킬 기본 쿨타임")]
    [Header("       마법사 텔레포트")]
    private float Skill_Teleport_cooltime = 5;    
    public bool Usable_Teleport = true;
    [Header("스킬 사용 마나")]
    [SerializeField]
    public float Skill_TelePort_Mp = 50;


    [SerializeField]
    [Header("스킬 기본 쿨타임")]
    [Header("       닷지 스킬")]
    private float Skill_Dodge_cooltime = 10;    
    public bool Usable_Dodge = true;
    [Header("스킬 사용 마나")]
    [SerializeField]
    public float Skill_Dodge_Mp = 50;


    [Header("스킬 퍼뎀")]
    [Header("       1번 스킬")]
    [SerializeField]
    public float Skill_1_per_dam=1f;
    [Header("스킬 고정뎀")]
    [SerializeField]
    public float Skill_1_fixed_dam;
    [Header("스킬 기본 쿨타임")]
    [SerializeField]
    private float Skill_1_cooltime=5;    
    public bool Usable_Skill1 = true;
    [Header("스킬 사용 마나")]
    [SerializeField]
    public float Skill_1_use_Mp = 50;


    [Header("스킬 퍼뎀")]
    [Header("       2번 스킬")]
    [SerializeField]
    public float Skill_2_per_dam=1f;
    [Header("스킬 고정뎀")]
    [SerializeField]
    public float Skill_2_fixed_dam;
    [Header("스킬 기본 쿨타임")]
    [SerializeField]
    private float Skill_2_cooltime = 5;
    public bool Usable_Skill2 = true;
    [Header("스킬 사용 마나")]
    [SerializeField]
    public float Skill_2_use_Mp = 50;

    [Header("스킬 퍼뎀")]
    [Header("       3번 스킬")]
    [SerializeField]
    public float Skill_3_per_dam=1f;
    [Header("스킬 고정뎀")]
    [SerializeField]
    public float Skill_3_fixed_dam;
    [Header("스킬 기본 쿨타임")]
    [SerializeField]
    private float Skill_3_cooltime = 5;
    public bool Usable_Skill3 = true;
    [Header("스킬 사용 마나")]
    [SerializeField]
    public float Skill_3_use_Mp = 50;

    [Header("스킬 퍼뎀")]
    [Header("       4번 스킬")]
    [SerializeField]
    public float Skill_4_per_dam=1f;
    [Header("스킬 고정뎀")]
    [SerializeField]
    public float Skill_4_fixed_dam;
    [Header("스킬 기본 쿨타임")]
    [SerializeField]
    private float Skill_4_cooltime = 5;
    public bool Usable_Skill4 = true;
    [Header("스킬 사용 마나")]
    [SerializeField]
    public float Skill_4_use_Mp = 50;

    public float SkillBuff_time;
    public float SkillBuff_passedTime;
    public float SkillTeleport_time;
    public float SkillTeleport_passedTime;
    public float SkillDodge_time;
    public float SkillDodge_passedTime;

    public float SkillBuff_passedDurationgTime;

    public float Skill1_time;
    public float Skill1_passedTime;

    public float Skill2_time;
    public float Skill2_passedTime;
    public float Skill3_time;
    public float Skill3_passedTime;
    public float Skill4_time;
    public float Skill4_passedTime;


    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        Skill1_time = Skill_1_cooltime;
        Skill2_time = Skill_2_cooltime;
        Skill3_time = Skill_3_cooltime;
        Skill4_time = Skill_4_cooltime;
        SkillBuff_time = Skill_Buff_cooltime;
        SkillDodge_time = Skill_Dodge_cooltime;
        SkillTeleport_time = Skill_Teleport_cooltime;

    }
    private void FixedUpdate()
    {
        SkillPassedTimeFucn();
        
    }




    public  float attackDamage=0;
    public  bool critical=false;
    public  int DamageNum;

    public float Attack_Dam()
    {
        DamageNum = 0;
        float DamRange = Random.Range(0.95f, 1.05f);
        if(Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) 
        {
            critical = true;
            attackDamage = (playerStat._DAMAGE + (playerStat._DAMAGE * playerStat._CRITICAL_ADD_DAMAGE_PER / 100f)) * DamRange;
            return attackDamage; 
        }
        else
        {
            critical = false;
            attackDamage = (playerStat._DAMAGE) * DamRange;
            return attackDamage;
        }
        
    }

    
    public float Skill_1_Damamge()
    {
        DamageNum = 1;
        float DamRange = Random.Range(0.95f, 1.05f);
        float Skill_1_basedamage = (playerStat._DAMAGE * Skill_1_per_dam + Skill_1_fixed_dam) + (playerStat._DAMAGE * Skill_1_per_dam + Skill_1_fixed_dam) * playerStat._SKILL_ADD_DAMAGE_PER / 100;
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) 
        {
            critical = true;
            attackDamage = (Skill_1_basedamage + (Skill_1_basedamage * playerStat._CRITICAL_ADD_DAMAGE_PER / 100f)) * DamRange;
            return attackDamage; 
        }
        else
        {
            critical = false;
            attackDamage = Skill_1_basedamage * DamRange;
            return attackDamage;
        }

    }
    




    public float Skill_2_Damamge()
    {
        DamageNum = 2;
        float DamRange = Random.Range(0.95f, 1.05f);
        float Skill_2_basedamage = (playerStat._DAMAGE * Skill_2_per_dam + Skill_2_fixed_dam) + (playerStat._DAMAGE * Skill_2_per_dam + Skill_2_fixed_dam) * playerStat._SKILL_ADD_DAMAGE_PER / 100;
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 �� ũ��Ƽ�� Ȯ�������� �������̸� ũ��Ƽ�õ����� ����.
        {
            critical = true;
            attackDamage = (Skill_2_basedamage + (Skill_2_basedamage * playerStat._CRITICAL_ADD_DAMAGE_PER / 100f)) * DamRange;
            return attackDamage; 
        }
        else
        {
            critical = false;
            attackDamage = Skill_2_basedamage * DamRange;
            return attackDamage;
        }

    }


    public float Skill_3_Damamge()
    {
        DamageNum = 3;
        float DamRange = Random.Range(0.95f, 1.05f);
        float Skill_3_basedamage = (playerStat._DAMAGE * Skill_3_per_dam + Skill_3_fixed_dam) + (playerStat._DAMAGE * Skill_3_per_dam + Skill_3_fixed_dam) * playerStat._SKILL_ADD_DAMAGE_PER / 100;
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 �� ũ��Ƽ�� Ȯ�������� �������̸� ũ��Ƽ���� ������.
        {
            critical = true;
            attackDamage = (Skill_3_basedamage + (Skill_3_basedamage * playerStat._CRITICAL_ADD_DAMAGE_PER / 100f)) * DamRange;
            return attackDamage;
   
        }
        else
        {
            critical = false;
            attackDamage = Skill_3_basedamage * DamRange;
            return attackDamage;

        }

    }
 

    public float Skill_4_Damamge()
    {
        DamageNum = 4;
        float DamRange = Random.Range(0.95f, 1.05f);
        float Skill_4_basedamage = (playerStat._DAMAGE * Skill_4_per_dam + Skill_4_fixed_dam) + (playerStat._DAMAGE * Skill_4_per_dam + Skill_4_fixed_dam) * playerStat._SKILL_ADD_DAMAGE_PER / 100;
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 �� ũ��Ƽ�� Ȯ�������� �������̸� ũ��Ƽ���� ������.
        {
            critical = true;
            attackDamage = (Skill_4_basedamage + (Skill_4_basedamage * playerStat._CRITICAL_ADD_DAMAGE_PER / 100f)) * DamRange;
            return attackDamage;

        }
        else
        {
            critical = false;
            attackDamage = Skill_4_basedamage * DamRange;
            return attackDamage;

        }

    }

    public float ReturnBuffCool()
    {
        return Skill_Buff_cooltime - Skill_Buff_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
    }

    public void Skill_Buff_Cool()
    {
        playerStat.SkillMp(Skill_Buff_use_Mp);
        SkillBuff_time = Skill_Buff_cooltime - Skill_Buff_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        SkillBuff_passedTime = 0f;
        SkillBuff_passedDurationgTime=0f;
        Duration_Buff = true;
        Usable_Buff = false;
    }

    public float ReturnTeleportCool()
    {
        return Skill_Teleport_cooltime - Skill_Teleport_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
    }
 
    public void Skill_Mage_Teleport_Cool()
    {
        playerStat.SkillMp(Skill_TelePort_Mp);
        SkillTeleport_time = Skill_Teleport_cooltime - Skill_Teleport_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        SkillTeleport_passedTime = 0f;
        Usable_Teleport = false;
    }

    public float ReturnDodgeCool()
    {
        return Skill_Dodge_cooltime - Skill_Dodge_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
    }
    public void Skill_Dodge_Cool()
    {
        playerStat.SkillMp(Skill_Dodge_Mp);
        SkillDodge_time = Skill_Dodge_cooltime - Skill_Dodge_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        SkillDodge_passedTime = 0f;
        Usable_Dodge = false;
    }

    public float ReturnSkill_1_Cool()
    {
        return Skill_1_cooltime - Skill_1_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
    }


    public void Skill_1_Cool()
    {
        playerStat.SkillMp(Skill_1_use_Mp);
        Skill1_time = Skill_1_cooltime - Skill_1_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        Skill1_passedTime = 0f;
        Usable_Skill1 = false;
    }

    public float ReturnSkill_2_Cool()
    {
        return Skill_2_cooltime - Skill_2_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
    }
    public void Skill_2_Cool()
    {
        playerStat.SkillMp(Skill_2_use_Mp);
        Skill2_time = Skill_2_cooltime - Skill_2_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        Skill2_passedTime = 0f;
        Usable_Skill2 = false;
    }

    public float ReturnSkill_3_Cool()
    {
        return Skill_3_cooltime - Skill_3_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
    }
    public void Skill_3_Cool()
    {
        playerStat.SkillMp(Skill_3_use_Mp);
        Skill3_time = Skill_3_cooltime - Skill_3_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        Skill3_passedTime = 0f;
        Usable_Skill3 = false;
    }

    public float ReturnSkill_4_Cool()
    {
        return Skill_4_cooltime - Skill_4_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
    }
    public void Skill_4_Cool()
    {
        playerStat.SkillMp(Skill_4_use_Mp);
        Skill4_time = Skill_4_cooltime - Skill_4_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        Skill4_passedTime = 0f;
        Usable_Skill4 = false;
    }




    public void SkillPassedTimeFucn()
    {

        if (!Usable_Buff)
        {
            SkillBuff_passedTime += Time.deltaTime;
            if (SkillBuff_time < SkillBuff_passedTime)
            {
                Debug.Log(SkillBuff_passedTime);
                SkillBuff_passedTime = 0f;
                Usable_Buff = true;

            }
        }

        if (!Usable_Teleport)
        {
            SkillTeleport_passedTime += Time.deltaTime;
            if (SkillTeleport_time < SkillTeleport_passedTime)
            {
                Debug.Log(SkillTeleport_passedTime);
                SkillTeleport_passedTime = 0f;
                Usable_Teleport = true;

            }
        }
        if (!Usable_Dodge)
        {
            SkillDodge_passedTime += Time.deltaTime;
            if (SkillDodge_time < SkillDodge_passedTime)
            {
                Debug.Log(SkillDodge_passedTime);
                SkillDodge_passedTime = 0f;
                Usable_Dodge = true;

            }
        }

        if (Duration_Buff)
        {
            SkillBuff_passedDurationgTime += Time.deltaTime;
            if(Skill_Buff_duration < SkillBuff_passedDurationgTime)
            {
                Debug.Log(SkillBuff_passedDurationgTime);
                SkillBuff_passedDurationgTime = 0f;
                Duration_Buff = false;
            }
        }


        if (!Usable_Skill1)
        {
            Skill1_passedTime += Time.deltaTime;
            if (Skill1_time < Skill1_passedTime)
            {
                Debug.Log(Skill1_passedTime);
                Skill1_passedTime = 0f;
                Usable_Skill1 = true;
            }
        }
        if (!Usable_Skill2)
        {
            Skill2_passedTime += Time.deltaTime;
            if (Skill2_time < Skill2_passedTime)
            {
                Debug.Log(Skill2_passedTime);
                Skill2_passedTime = 0f;
                Usable_Skill2 = true;
            }
        }
        if (!Usable_Skill3)
        {
            Skill3_passedTime += Time.deltaTime;
            if (Skill3_time < Skill3_passedTime)
            {
                Debug.Log(Skill3_passedTime);
                Skill3_passedTime = 0f;
                Usable_Skill3 = true;
            }
        }
        if (!Usable_Skill4)
        {
            Skill4_passedTime += Time.deltaTime;
            if (Skill4_time < Skill4_passedTime)
            {
                Debug.Log(Skill4_passedTime);
                Skill4_passedTime = 0f;
                Usable_Skill4 = true;
            }
        }
    }

}
