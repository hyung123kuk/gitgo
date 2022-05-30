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

    public void FootRunSound() //달리기
    {
        audioSource.PlayOneShot(Sounds.sounds.FootRunSound);
    }
    public void War_L_R_FootSound() //전사 좌우 발소리
    {
        audioSource.PlayOneShot(Sounds.sounds.War_L_R_FootSound);
    }
    public void War_F_FootSound() //전사 앞 발소리
    {
        audioSource.PlayOneShot(Sounds.sounds.War_F_FootSound);
    }
    public void War_B_FootSound() //전사 뒤 발소리
    {
        audioSource.PlayOneShot(Sounds.sounds.War_B_FootSound);
    }
    public void Arc_L_R_FootSound() //궁수 좌우 발소리
    {
        audioSource.PlayOneShot(Sounds.sounds.Arc_L_R_FootSound);
    }
    public void Arc_F_B_FootSound() //궁수 앞뒤 발소리
    {
        audioSource.PlayOneShot(Sounds.sounds.Arc_F_B_FootSound);
    }
    public void Mag_L_R_FootSound() //마법사 좌우 발소리
    {
        audioSource.PlayOneShot(Sounds.sounds.Mag_L_R_FootSound);
    }
    public void Mag_F_FootSound() //마법사 앞 발소리
    {
        audioSource.PlayOneShot(Sounds.sounds.Mag_F_FootSound);
    }
    public void Mag_B_FootSound() //마법사 뒤 발소리
    {
        audioSource.PlayOneShot(Sounds.sounds.Mag_B_FootSound);
    }


    public void HorseRunSound() //말 달리기
    {
        audioSource.PlayOneShot(Sounds.sounds.HorseRunSound);
    }

}
