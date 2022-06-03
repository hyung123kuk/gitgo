using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] //자동으로 오디오소스 부착
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
    //==============================도중에 끊어야할 사운드=========================================//
    private void Update()
    {
        audioSource.volume = soundbar.CharacterVolume;
    }
    public void ArcherChargeSound() //궁수 활시위 당기는소리
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherChargeSound);
    }
    public void ArcherSkill3ChargeSound() //궁수 3스킬 차징소리
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherSkill3ChargeSound);
    }
    public void MageSkill3CastSound() //법사 3스킬 캐스팅소리
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill3CastSound);
    }
}
