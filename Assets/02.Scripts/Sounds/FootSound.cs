using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootSound : MonoBehaviour
{
    public static FootSound footSound;

    public AudioSource audioSource;


    private void Awake()
    {
        if (footSound == null)
        {
            footSound = this;
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FootRunSound() //�޸���
    {
        audioSource.PlayOneShot(Sounds.sounds.FootRunSound);
    }
    public void War_L_R_FootSound() //���� �¿� �߼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.War_L_R_FootSound);
    }
    public void War_F_FootSound() //���� �� �߼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.War_F_FootSound);
    }
    public void War_B_FootSound() //���� �� �߼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.War_B_FootSound);
    }
    public void Arc_L_R_FootSound() //�ü� �¿� �߼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.Arc_L_R_FootSound);
    }
    public void Arc_F_B_FootSound() //�ü� �յ� �߼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.Arc_F_B_FootSound);
    }
    public void Mag_L_R_FootSound() //������ �¿� �߼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.Mag_L_R_FootSound);
    }
    public void Mag_F_FootSound() //������ �� �߼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.Mag_F_FootSound);
    }
    public void Mag_B_FootSound() //������ �� �߼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.Mag_B_FootSound);
    }


    public void HorseRunSound() //�� �޸���
    {
        audioSource.PlayOneShot(Sounds.sounds.HorseRunSound);
    }

}
