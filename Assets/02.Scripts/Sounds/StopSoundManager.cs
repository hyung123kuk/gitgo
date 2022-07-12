using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))] //�ڵ����� ������ҽ� ����
public class StopSoundManager : MonoBehaviour
{
    public static StopSoundManager stopSoundManager;

    public AudioSource audioSource;

    SoundBar soundbar;
    private void Awake()
    {
        if (stopSoundManager == null)
        {
            stopSoundManager = this;
        }
    }

    private void Start()
    {
        soundbar = FindObjectOfType<SoundBar>();
        audioSource = GetComponent<AudioSource>();
    }
    //==============================���߿� ������� ����=========================================//
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "NewChSel") //ĳ���ͼ���â �������� ����X
            audioSource.volume = soundbar.CharacterVolume;
    }
    public void ArcherChargeSound() //�ü� Ȱ���� ���¼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherChargeSound);
    }
    public void ArcherSkill3ChargeSound() //�ü� 3��ų ��¡�Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherSkill3ChargeSound);
    }
    public void MageSkill3CastSound() //���� 3��ų ĳ���üҸ�
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill3CastSound);
    }
    public void WarriorTalk1() //ĳ����â ���� ���1
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorTalk1);
    }
    public void WarriorTalk2() //ĳ����â ���� ���2
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorTalk2);
    }
    public void SelWarriorAttackSound() //ĳ����â ���� ���ݼҸ�
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorAttackVoice);
    }
    public void ArcherTalk1() //ĳ����â �ü� ���1
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherTalk1);
    }
    public void ArcherTalk2() //ĳ����â �ü� ���2
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherTalk2);
    }
    public void SelArcherAttackSound() //ĳ����â �ü� ���ݼҸ�
    {
        audioSource.PlayOneShot(Sounds.sounds.SelArcherAttackSound);
    }
    public void MageTalk1() //ĳ����â ���� ���1
    {
        audioSource.PlayOneShot(Sounds.sounds.MageTalk1);
    }
    public void MageTalk2() //ĳ����â ���� ���2
    {
        audioSource.PlayOneShot(Sounds.sounds.MageTalk2);
    }
    public void SelMageAttackSound() //ĳ����â ���� ���ݼҸ�
    {
        audioSource.PlayOneShot(Sounds.sounds.SelMageAttackSound);
    }
    public void SelMakeSound() //ĳ���� ���鶧 �Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.SelMakeSound);
    }
}
