using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    [SerializeField]
    private PlayerStat playerStat;
    [Header("��ų �⺻ ��Ÿ��")]
    [Header("       ���� ��ų")]
    [SerializeField]
    private float Skill_Buff_cooltime = 5;    //��ų�� �⺻ ��Ÿ�� ���� ��Ÿ�� ���� ����Ǽ� �� ����
    public bool Usable_Buff = true; //��ų ��� ���� �Ҷ� true�� �ٲ�
    [Header("��ų ���� �ð�")]
    [SerializeField]
    private float Skill_Buff_duration = 3;
    public bool Duration_Buff = false;
    [Header("��ų ��� ����")]
    [SerializeField]
    private float Skill_Buff_use_Mp = 50;


    [SerializeField]
    [Header("��ų �⺻ ��Ÿ��")]
    [Header("       ������� ��Ŭ����ų")]
    private float Skill_Teleport_cooltime = 5;    //��ų�� �⺻ ��Ÿ�� ���� ��Ÿ�� ���� ����Ǽ� �� ����
    public bool Usable_Teleport = true;


    [SerializeField]
    [Header("��ų �⺻ ��Ÿ��")]
    [Header("       ������ �Դϴ�")]
    private float Skill_Dodge_cooltime = 10;    //��ų�� �⺻ ��Ÿ�� ���� ��Ÿ�� ���� ����Ǽ� �� ����
    public bool Usable_Dodge = true;

    //��ų �۵� : ĳ���� ���ݷ¿� ��� ex) 1.2 ������ ĳ���� ���ݷ��� 1.2���� �������� ����.
    //��ų ������ : ĳ���� ���ݷ¿� �߰��ȴٰ� �����ϸ� ��.
    //��ų �۵� + ��ų ������ : ��ų �۵��� 1.2 ��ų �������� 30 �̶�� ��������  (ĳ���Ͱ��ݷ�*1.2) + 30 �� ����.
    [Header("��ų �۵�")]
    [Header("       1�� ��ų")]
    [SerializeField]
    private float Skill_1_per_dam=1f;
    [Header("��ų ������")]
    [SerializeField]
    private float Skill_1_fixed_dam;
    [Header("��ų �⺻ ��Ÿ��")]
    [SerializeField]
    private float Skill_1_cooltime=5;    //��ų�� �⺻ ��Ÿ�� ���� ��Ÿ�� ���� ����Ǽ� �� ����
    public bool Usable_Skill1 = true; //��ų ��� ���� �Ҷ� true�� �ٲ�
    [Header("��ų ��� ����")]
    [SerializeField]
    private float Skill_1_use_Mp = 50;


    [Header("��ų �۵�")]
    [Header("       2�� ��ų")]
    [SerializeField]
    private float Skill_2_per_dam=1f;
    [Header("��ų ������")]
    [SerializeField]
    private float Skill_2_fixed_dam;
    [Header("��ų �⺻ ��Ÿ��")]
    [SerializeField]
    private float Skill_2_cooltime = 5;
    public bool Usable_Skill2 = true;
    [Header("��ų ��� ����")]
    [SerializeField]
    private float Skill_2_use_Mp = 50;

    [Header("��ų �۵�")]
    [Header("       3�� ��ų")]
    [SerializeField]
    private float Skill_3_per_dam=1f;
    [Header("��ų ������")]
    [SerializeField]
    private float Skill_3_fixed_dam;
    [Header("��ų �⺻ ��Ÿ��")]
    [SerializeField]
    private float Skill_3_cooltime = 5;
    public bool Usable_Skill3 = true;
    [Header("��ų ��� ����")]
    [SerializeField]
    private float Skill_3_use_Mp = 50;

    [Header("��ų �۵�")]
    [Header("       4�� ��ų")]
    [SerializeField]
    private float Skill_4_per_dam=1f;
    [Header("��ų ������")]
    [SerializeField]
    private float Skill_4_fixed_dam;
    [Header("��ų �⺻ ��Ÿ��")]
    [SerializeField]
    private float Skill_4_cooltime = 5;
    public bool Usable_Skill4 = true;
    [Header("��ų ��� ����")]
    [SerializeField]
    private float Skill_4_use_Mp = 50;

    float SkillBuff_time;
    float SkillBuff_passedTime;
    float SkillTeleport_time;
    float SkillTeleport_passedTime;
    float SkillDodge_time;
    float SkillDodge_passedTime;

    float SkillBuff_passedDurationgTime;

    float Skill1_time;         //��Ÿ�� ���� ���� ��ų Ÿ��
    float Skill1_passedTime;   //��ų 1 �귯�� �ð�
    float Skill2_time;
    float Skill2_passedTime;
    float Skill3_time;
    float Skill3_passedTime;
    float Skill4_time;
    float Skill4_passedTime;


    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
       // Skill_Buff_Cool();
        
    }
    private void Update()
    {
        SkillPassedTimeFucn();
        
    }

   


    // ���� => ���������� �ְ� �������� Atack_Dam() �� ������ �˴ϴ�.  ��ȯ���� float�Դϴ� (�������� float������ ���ϴ�)

    public float Attack_Dam()
    {
        float DamRange = Random.Range(0.95f, 1.05f);
        if(Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 �� ũ��Ƽ�� Ȯ�������� �������̸�ũ��Ƽ�õ����� ����.
        {
            return  ( playerStat._DAMAGE + (playerStat._DAMAGE * playerStat._CRITICAL_ADD_DAMAGE_PER /100f) ) *DamRange; //ũ��Ƽ�� �������� ���������� �ۼ�Ʈġ ����
        }
        else
        {
            return (playerStat._DAMAGE) * DamRange;
        }
        
    }

    // ��ų 1�� �������� �ְ� �������� Skill_1_Damage() �� ������ �˴ϴ�.  ��ȯ���� float�Դϴ� (�������� float������ ���ϴ�)
    public float Skill_1_Damamge()
    {
        float DamRange = Random.Range(0.95f, 1.05f);
        float Skill_1_basedamage = (playerStat._DAMAGE * Skill_1_per_dam + Skill_1_fixed_dam) + (playerStat._DAMAGE * Skill_1_per_dam + Skill_1_fixed_dam) * playerStat._SKILL_ADD_DAMAGE_PER / 100;
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 �� ũ��Ƽ�� Ȯ�������� �������̸� ũ��Ƽ�õ����� ����.
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
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 �� ũ��Ƽ�� Ȯ�������� �������̸� ũ��Ƽ�õ����� ����.
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
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 �� ũ��Ƽ�� Ȯ�������� �������̸� ũ��Ƽ���� ������.
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
        if (Random.Range(1.0f, 100.0f) <= playerStat._CRITICAL_PROBABILITY) //1~100 �� ũ��Ƽ�� Ȯ�������� �������̸� ũ��Ƽ���� ������.
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
    public void Skill_Mage_Teleport_Cool()
    {
        SkillTeleport_time = Skill_Teleport_cooltime - Skill_Teleport_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        SkillTeleport_passedTime = 0f;
        Usable_Teleport = false;
    }
    public void Skill_Dodge_Cool()
    {
        SkillDodge_time = Skill_Dodge_cooltime - Skill_Dodge_cooltime * playerStat._SKILL_COOLTIME_DEC_PER / 100;
        SkillDodge_passedTime = 0f;
        Usable_Dodge = false;
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
        Usable_Skill3 = false;
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


