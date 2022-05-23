using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] //�ڵ����� ������ҽ� ����
public class StopSoundManager : MonoBehaviour
{
    public static StopSoundManager stopSoundManager;

    public AudioSource audioSource;

    private void Awake()
    {
        if (stopSoundManager == null)
        {
            stopSoundManager = this;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    //==============================���߿� ������� ����=========================================//
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
}
