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
    private float Skill_Buff_cooltime = 5;    //스킬의 기본 쿨타임 이후 쿨타임 감소 적용되서 값 나옴
    public bool Usable_Buff = true; //스킬 사용 가능 할때 true로 바뀜
    [Header("스킬 지속 시간")]
    [SerializeField]
    private float Skill_Buff_duration = 3;
    public bool Duration_Buff = false;
    [Header("스킬 사용 마나")]
    [SerializeField]
    private float Skill_Buff_use_Mp = 50;


    //스킬 퍼뎀 : 캐릭터 공격력에 배수 ex) 1.2 넣으면 캐릭터 공격력의 1.2배의 데미지가 들어간다.
    //스킬 고정뎀 : 캐릭터 공격력에 추가된다고 생각하면 됨.
    //스킬 퍼뎀 + 스킬 고정뎀 : 스킬 퍼뎀이 1.2 스킬 고정뎀이 30 이라면 데미지는  (캐릭터공격력*1.2) + 30 이 들어간다.
    [Header("스킬 퍼뎀")]
    [Header("       1번 스킬")]
    [SerializeField]
    private float Skill_1_per_dam=1f;
    [Header("스킬 고정뎀")]
    [SerializeField]
    private float Skill_1_fixed_dam;
    [Header("스킬 기본 쿨타임")]
    [SerializeField]
    private float Skill_1_cooltime=5;    //스킬의 기본 쿨타임 이후 쿨타임 감소 적용되서 값 나옴
    public bool Usable_Skill1 = true; //스킬 사용 가능 할때 true로 바뀜
    [Header("스킬 사용 마나")]
    [SerializeField]
    private float Skill_1_use_Mp = 50;


    [Header("스킬 퍼뎀")]
    [Header("       2번 스킬")]
    [SerializeField]
    private float Skill_2_per_dam=1f;
    [Header("스킬 고정뎀")]
    [SerializeField]
    private float Skill_2_fixed_dam;
    [Header("스킬 기본 쿨타임")]
    [SerializeField]
    private float Skill_2_cooltime = 5;
    public bool Usable_Skill2 = true;
    [Header("스킬 사용 마나")]
    [SerializeField]
    private float Skill_2_use_Mp = 50;

    [Header("스킬 퍼뎀")]
    [Header("       3번 스킬")]
    [SerializeField]
    private float Skill_3_per_dam=1f;
    [Header("스킬 고정뎀")]
    [SerializeField]
    private float Skill_3_fixed_dam;
    [Header("스킬 기본 쿨타임")]
    [SerializeField]
    private float Skill_3_cooltime = 5;
    public bool Usable_Skill3 = true;
    [Header("스킬 사용 마나")]
    [SerializeField]
    private float Skill_3_use_Mp = 50;

    [Header("스킬 퍼뎀")]
    [Header("       4번 스킬")]
    [SerializeField]
    private float Skill_4_per_dam=1f;
    [Header("스킬 고정뎀")]
    [SerializeField]
    private float Skill_4_fixed_dam;
    [Header("스킬 기본 쿨타임")]
    [SerializeField]
    private float Skill_4_cooltime = 5;
    public bool Usable_Skill4 = true;
    [Header("스킬 사용 마나")]
    [SerializeField]
    private float Skill_4_use_Mp = 50;

    float SkillBuff_time;
    float SkillBuff_passedTime;

   
    float SkillBuff_passedDurationgTime;

    float Skill1_time;         //쿨타임 감소 적용 스킬 타임
    float Skill1_passedTime;   //스킬 1 흘러간 시간
    float Skill2_time;
    float Skill2_passedTime;
    float Skill3_time;
    float Skill3_passedTime;
    float Skill4_time;
    float Skill4_passedTime;


    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        Skill_Buff_Cool();
        
    }
    private void Update()
    {
        SkillPassedTimeFucn();
        
    }

   


    // 사용법 => 데미지값을 넣고 싶은곳에 Atack_Dam() 를 넣으면 됩니다.  반환형이 float입니다 (데미지가 float형으로 들어갑니다)

    public float Attack_Dam()
    {
        float DamRange = Random.Range(0.95f, 1.05f);
        if(Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 중 크리티컬 확률값보다 작은값이면크리티컬데미지 리턴.
        {
            return  ( playerStat._DAMAGE + (playerStat._DAMAGE * playerStat._CRITICAL_ADD_DAMAGE_PER /100f) ) *DamRange; //크리티컬 터졌을때 데미지값에 퍼센트치 더함
        }
        else
        {
            return (playerStat._DAMAGE) * DamRange;
        }
        
    }

    // 스킬 1번 데미지를 넣고 싶은곳에 Skill_1_Damage() 를 넣으면 됩니다.  반환형이 float입니다 (데미지가 float형으로 들어갑니다)
    public float Skill_1_Damamge()
    {
        float DamRange = Random.Range(0.95f, 1.05f);
        float Skill_1_basedamage = (playerStat._DAMAGE * Skill_1_per_dam + Skill_1_fixed_dam) + (playerStat._DAMAGE * Skill_1_per_dam + Skill_1_fixed_dam) * playerStat._SKILL_ADD_DAMAGE_PER / 100;
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 중 크리티컬 확률값보다 작은값이면 크리티컬데미지 리턴.
        {
            return (Skill_1_basedamage + (Skill_1_basedamage * playerStat._CRITICAL_ADD_DAMAGE_PER / 100f)) * DamRange; 
        }
        else
        {
            return Skill_1_basedamage * DamRange;
        }

    }




    public float Skill_2_Damamge()
    {
        float DamRange = Random.Range(0.95f, 1.05f);
        float Skill_2_basedamage = (playerStat._DAMAGE * Skill_2_per_dam + Skill_2_fixed_dam) + (playerStat._DAMAGE * Skill_2_per_dam + Skill_2_fixed_dam) * playerStat._SKILL_ADD_DAMAGE_PER / 100;
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 중 크리티컬 확률값보다 작은값이면 크리티컬데미지 리턴.
        {
            return (Skill_2_basedamage + (Skill_2_basedamage * playerStat._CRITICAL_ADD_DAMAGE_PER / 100f)) * DamRange; 
        }
        else
        {
            return Skill_2_basedamage * DamRange;
        }

    }


    public float Skill_3_Damamge()
    {
        float DamRange = Random.Range(0.95f, 1.05f);
        float Skill_3_basedamage = (playerStat._DAMAGE * Skill_3_per_dam + Skill_3_fixed_dam) + (playerStat._DAMAGE * Skill_3_per_dam + Skill_3_fixed_dam) * playerStat._SKILL_ADD_DAMAGE_PER / 100;
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 중 크리티컬 확률값보다 작은값이면 크리티컬이 터진다.
        {
            return (Skill_3_basedamage + (Skill_3_basedamage * playerStat._CRITICAL_ADD_DAMAGE_PER / 100f)) * DamRange;
        }
        else
        {
            return Skill_3_basedamage * DamRange;
        }

    }
 

    public float Skill_4_Damamge()
    {
        float DamRange = Random.Range(0.95f, 1.05f);
        float Skill_4_basedamage = (playerStat._DAMAGE * Skill_4_per_dam + Skill_4_fixed_dam) + (playerStat._DAMAGE * Skill_4_per_dam + Skill_4_fixed_dam) * playerStat._SKILL_ADD_DAMAGE_PER / 100;
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 중 크리티컬 확률값보다 작은값이면 크리티컬이 터진다.
        {
            return (Skill_4_basedamage + (Skill_4_basedamage * playerStat._CRITICAL_ADD_DAMAGE_PER / 100f)) * DamRange;
        }
        else
        {
            return Skill_4_basedamage * DamRange;
        }

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


    public void Skill_1_Cool()
    {
        playerStat.SkillMp(Skill_1_use_Mp);
        Skill1_time = Skill_1_cooltime - Skill_1_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        Skill1_passedTime = 0f;
        Usable_Skill1 = false;
    }
    public void Skill_2_Cool()
    {
        playerStat.SkillMp(Skill_2_use_Mp);
        Skill2_time = Skill_2_cooltime - Skill_2_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        Skill2_passedTime = 0f;
        Usable_Skill2 = false;
    }
    public void Skill_3_Cool()
    {
        playerStat.SkillMp(Skill_3_use_Mp);
        Skill3_time = Skill_3_cooltime - Skill_3_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        Skill3_passedTime = 0f;
        Usable_Skill1 = false;
    }
    public void Skill_4_Cool()
    {
        playerStat.SkillMp(Skill_4_use_Mp);
        Skill4_time = Skill_4_cooltime - Skill_4_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        Skill4_passedTime = 0f;
        Usable_Skill1 = false;
    }


    private void SkillPassedTimeFucn()
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

        if(Duration_Buff)
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
                Usable_Skill1 = true;
            }
        }
        if (!Usable_Skill3)
        {
            Skill3_passedTime += Time.deltaTime;
            if (Skill3_time < Skill3_passedTime)
            {
                Debug.Log(Skill3_passedTime);
                Skill3_passedTime = 0f;
                Usable_Skill1 = true;
            }
        }
        if (!Usable_Skill4)
        {
            Skill4_passedTime += Time.deltaTime;
            if (Skill4_time < Skill4_passedTime)
            {
                Debug.Log(Skill4_passedTime);
                Skill4_passedTime = 0f;
                Usable_Skill1 = true;
            }
        }
    }

}


