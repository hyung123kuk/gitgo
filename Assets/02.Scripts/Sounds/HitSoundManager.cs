using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] //자동으로 오디오소스 부착
public class HitSoundManager : MonoBehaviour
{
    public static HitSoundManager hitsoundManager;

    public AudioSource audioSource;
    SoundBar soundbar;
    private void Awake()
    {
        if (hitsoundManager == null)
        {
            hitsoundManager = this;
        }
    }

    private void Start()
    {
        soundbar = FindObjectOfType<SoundBar>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        audioSource.volume = soundbar.CharacterVolume;
    }
    //==============================몬스터 사운드=========================================//
    public void SlimeHitSound() //슬라임류 피격음
    {
        audioSource.PlayOneShot(Sounds.sounds.SlimeHitSound);
    }
    public void GoblinHitSound() //고블린류 피격음
    {
        audioSource.PlayOneShot(Sounds.sounds.GoblinHitSound);
    }
    public void SkeletonHitSound() //스켈레톤 피격음
    {
        audioSource.PlayOneShot(Sounds.sounds.SkeletonHitSound);
    }
    public void GolemHitSound() //골렘 피격음
    {
        audioSource.PlayOneShot(Sounds.sounds.GolemHitSound);
    }


}
