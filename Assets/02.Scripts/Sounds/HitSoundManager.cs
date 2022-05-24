using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] //�ڵ����� ������ҽ� ����
public class HitSoundManager : MonoBehaviour
{
    public static HitSoundManager hitsoundManager;

    public AudioSource audioSource;

    private void Awake()
    {
        if (hitsoundManager == null)
        {
            hitsoundManager = this;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    //==============================���� ����=========================================//
    public void SlimeHitSound() //�����ӷ� �ǰ���
    {
        audioSource.PlayOneShot(Sounds.sounds.SlimeHitSound);
    }
    public void GoblinHitSound() //����� �ǰ���
    {
        audioSource.PlayOneShot(Sounds.sounds.GoblinHitSound);
    }
    public void SkeletonHitSound() //���̷��� �ǰ���
    {
        audioSource.PlayOneShot(Sounds.sounds.SkeletonHitSound);
    }
    public void GolemHitSound() //�� �ǰ���
    {
        audioSource.PlayOneShot(Sounds.sounds.GolemHitSound);
    }


}
