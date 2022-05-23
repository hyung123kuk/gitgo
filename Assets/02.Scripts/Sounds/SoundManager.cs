using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] //�ڵ����� ������ҽ� ����
public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;

    public AudioSource audioSource;

    public bool isArcSkill2; //��ų2 ���߻��� �Ȱ�ġ�� 

    private void Awake()
    {
        if(soundManager == null)
        {
            soundManager = this;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    //==============================���� ����=========================================//
   

    //==============================���� ����=========================================//

    public void WarriorAttackSound() //���� �Ϲݰ���
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorAttackSound);
    }
    public void WarriorAttackVoice() //���� �Ϲݰ��� ���̽�
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorAttackVoice);
    }
    public void WarriorAttackVoice2() //���� �Ϲݰ��� 3Ÿ ���̽�
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorAttackVoice2);
    }
    public void WarriorShieldSound() //���� �Ϲݰ��� 3Ÿ ���̽�
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorShieldSound);
    }
    public void WarriorBuffSound() //���� ������ų ����
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorBuffSound);
    }
    public void WarriorRushSound() //���� 2��ų ���� ����
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorRushSound);
    }
    public void WarriorRushVoice() //���� 2��ų ���� ���̽�
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorRushVoice);
    }
    public void WarriorAuraSound() //���� 3��ų �˱� ����
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorAuraSound);
    }

    //==============================�ü� ����=========================================//
    public void ArcherAttackSound() //�ü� Ȱ��� �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherAttackSound);
    }
    public void ArcherBackStepSound() //�ü� �齺�� �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherBackStepSound);
    }
    public void ArcherSkill1Sound() //�ü� ��ȭ�� �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherSkill1Sound);
    }
    public void ArcherSkill1ShotSound() //�ü� ��ȭ�� �߻� �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherSkill1ShotSound);
    }
    public void ArcherSkill2_1Sound() //�ü� ��ų2 ����ȭ�� �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherSkill2Sound);
    }
    public void ArcherSkill2_2Sound() //�ü� ��ų2 ����ȭ�� ������ �Ҹ�
    {
        isArcSkill2 = true;
        audioSource.PlayOneShot(Sounds.sounds.BoomSound);
        Invoke("ArcherSkill2_2Sound_Out", 1f);
    }
    void ArcherSkill2_2Sound_Out()
    {
        isArcSkill2 = false;
    }
    public void ArcherSkill3Sound() //�ü� ��ų3 ���󰡴¼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherSkill3ShotSound);
    }
    public void ArcherJump() //�ü� �����Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherJump);
    }

    //=======================���� ����==============================
    public void MageAttackSound() //���� �⺻���� �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.MageAttackSound);
    }
    public void MageTeleportSound() //���� ���� �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.MageTeleportSound);
    }
    public void MageSkill1Sound() //���� 1��ų �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill1Sound);
    }
    public void MageSkill1Voice() //���� 1��ų �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill1Voice);
    }
    public void MageSkill2Voice() //���� 2��ų ���̽� �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill2Voice);
    }
    public void MageSkill2Sound() //���� 2��ų �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill2Sound);
    }
    public void MageSkill3Sound() //���� 3��ų �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill3Sound);
    }
    public void MageJump() //����������
    {
        audioSource.PlayOneShot(Sounds.sounds.MageJump);
    }

}
