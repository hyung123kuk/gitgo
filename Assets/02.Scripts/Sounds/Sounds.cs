using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static Sounds sounds;

    [Header("��� ����")]
    public AudioClip TownBGM; //���� �������
    public AudioClip BeginnerGroundBGM; //�ʺ������ ������� (������)
    public AudioClip MiddleGroundBGM; //�߱޻���� ������� (���,�����Ӻ���)
    public AudioClip DunjeonBGM; //�������
    public AudioClip BossBGM; //������ ���

    [Header("���� ����")]
    public AudioClip FootRunSound; //�޸��� �߼Ҹ�
    public AudioClip Horse; //�� ��ȯ�Ҹ�
    public AudioClip Horse2; //�� ��ȯ�����Ҹ�
    public AudioClip HorseRunSound; //�� �޸��¼Ҹ�
    public AudioClip MaleDieSound; //���� �����Ҹ�
    public AudioClip FeMaleDieSound; //���� �����Ҹ�


    [Header("ĳ����â ����")]
    public AudioClip WarriorTalk1; //������
    public AudioClip WarriorTalk2; //������2
    public AudioClip ArcherTalk1; //�ü����1
    public AudioClip ArcherTalk2; //�ü����2
    public AudioClip SelArcherAttackSound; //�ü� ���ݼҸ�
    public AudioClip MageTalk1; //��������1
    public AudioClip MageTalk2; //��������2
    public AudioClip SelMageAttackSound; //��������ݼҸ�
    public AudioClip SelMakeSound; //ĳ���ͻ����Ҹ�

    [Header("���� ����")]
    public AudioClip WarriorAttackSound;//���� �⺻����
    public AudioClip WarriorAttackVoice; //���� ���ݺ��̽�;
    public AudioClip WarriorAttackVoice2; //���� ���ݺ��̽�;
    public AudioClip WarriorShieldSound; //���� ����ġ�� ����
    public AudioClip WarriorBuffSound; //���� ���ֹ��� ����
    public AudioClip WarriorRushSound; //���� ���� ����
    public AudioClip WarriorRushVoice; //���� ���� ���̽�
    public AudioClip WarriorAuraSound; //���� �˱� ����
    public AudioClip War_L_R_FootSound; //���� ��,�� �߼Ҹ�
    public AudioClip War_F_FootSound; //���� �� �߼Ҹ�
    public AudioClip War_B_FootSound; //���� �� �߼Ҹ�

    [Header("�ü� ����")]
    public AudioClip ArcherChargeSound; //�ü� Ȱ����
    public AudioClip ArcherAttackSound; //�ü� �Ϲ�ȭ��
    public AudioClip ArcherSkill2Sound; //�ü� ����ȭ��
    public AudioClip BoomSound; //����������
    public AudioClip ArcherSkill1Sound; //�ü� ��ȭ��Ҹ�
    public AudioClip ArcherSkill1ShotSound; //�ü� ��ȭ���¼Ҹ�
    public AudioClip ArcherBackStepSound; //�ü� �齺�� ����
    public AudioClip ArcherSkill3ChargeSound; //�ü� ��ų3 ��¡����
    public AudioClip ArcherSkill3ShotSound; //�ü� ��ų3 �߻����;
    public AudioClip ArcherJump; //�ü� ����
    public AudioClip Arc_L_R_FootSound; //�ü� ��,�� �߼Ҹ�
    public AudioClip Arc_F_B_FootSound; //�ü� ��,�� �߼Ҹ�

    [Header("������ ����")]
    public AudioClip MageAttackSound; //������ �⺻���ݻ���
    public AudioClip MageTeleportSound; //������ ��������
    public AudioClip MageSkill1Sound; //������ ��ų1����
    public AudioClip MageSkill1Voice; //������ ��ų1����
    public AudioClip MageSkill2Voice;//������ ��ų2 ���̽�
    public AudioClip MageSkill2Sound;//������ ��ų2 ����
    public AudioClip MageSkill3Sound; //������ ���׿��������»���
    public AudioClip MageSkill3CastSound; //������ ���׿� ĳ���û���
    public AudioClip Mag_L_R_FootSound; //������ ��,�� �߼Ҹ�
    public AudioClip MageJump; //������ ����
    public AudioClip Mag_F_FootSound; //������ �� �߼Ҹ�
    public AudioClip Mag_B_FootSound; //������ �� �߼Ҹ�



    [Header("���� ����")]
    public AudioClip SlimeHitSound; //�´¼Ҹ�
    public AudioClip GoblinHitSound; //�´¼Ҹ�
    public AudioClip SkeletonHitSound; //�´¼Ҹ�
    public AudioClip GolemHitSound; //�´¼Ҹ�

    [Header("UI ����")]
    public AudioClip InventoryOpenSound; //�κ������Ҹ�
    public AudioClip InventoryCloseSound; //�κ��������Ҹ�
    public AudioClip UiSound; //�ٸ�UI �Ҹ�
    public AudioClip BuySound; //�����Ҷ��Ҹ�
    public AudioClip BuyfailSound; //���Ž��мҸ�
    public AudioClip GetCoinSound; //�Ǹ�&�����������Ҹ�
    public AudioClip DrinkSound; //���໡���Ҹ�
    public AudioClip EquipSound; //��������Ҹ�
    public AudioClip Quest1; //����Ʈ �����Ҹ�
    public AudioClip Quest2; //����Ʈ �Ϸ�Ҹ�

    void Start()
    {
        sounds = this;
    }

}
