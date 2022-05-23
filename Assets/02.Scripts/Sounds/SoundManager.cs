using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] //자동으로 오디오소스 부착
public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;

    public AudioSource audioSource;

    public bool isArcSkill2; //스킬2 폭발사운드 안겹치게 

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
    //==============================공용 사운드=========================================//
   

    //==============================전사 사운드=========================================//

    public void WarriorAttackSound() //전사 일반공격
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorAttackSound);
    }
    public void WarriorAttackVoice() //전사 일반공격 보이스
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorAttackVoice);
    }
    public void WarriorAttackVoice2() //전사 일반공격 3타 보이스
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorAttackVoice2);
    }
    public void WarriorShieldSound() //전사 일반공격 3타 보이스
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorShieldSound);
    }
    public void WarriorBuffSound() //전사 버프스킬 사운드
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorBuffSound);
    }
    public void WarriorRushSound() //전사 2스킬 돌진 사운드
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorRushSound);
    }
    public void WarriorRushVoice() //전사 2스킬 돌진 보이스
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorRushVoice);
    }
    public void WarriorAuraSound() //전사 3스킬 검기 사운드
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorAuraSound);
    }

    //==============================궁수 사운드=========================================//
    public void ArcherAttackSound() //궁수 활쏘는 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherAttackSound);
    }
    public void ArcherBackStepSound() //궁수 백스텝 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherBackStepSound);
    }
    public void ArcherSkill1Sound() //궁수 독화살 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherSkill1Sound);
    }
    public void ArcherSkill1ShotSound() //궁수 독화살 발사 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherSkill1ShotSound);
    }
    public void ArcherSkill2_1Sound() //궁수 스킬2 폭발화살 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherSkill2Sound);
    }
    public void ArcherSkill2_2Sound() //궁수 스킬2 폭발화살 터지는 소리
    {
        isArcSkill2 = true;
        audioSource.PlayOneShot(Sounds.sounds.BoomSound);
        Invoke("ArcherSkill2_2Sound_Out", 1f);
    }
    void ArcherSkill2_2Sound_Out()
    {
        isArcSkill2 = false;
    }
    public void ArcherSkill3Sound() //궁수 스킬3 날라가는소리
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherSkill3ShotSound);
    }
    public void ArcherJump() //궁수 점프소리
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherJump);
    }

    //=======================법사 사운드==============================
    public void MageAttackSound() //법사 기본공격 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.MageAttackSound);
    }
    public void MageTeleportSound() //법사 텔포 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.MageTeleportSound);
    }
    public void MageSkill1Sound() //법사 1스킬 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill1Sound);
    }
    public void MageSkill1Voice() //법사 1스킬 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill1Voice);
    }
    public void MageSkill2Voice() //법사 2스킬 보이스 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill2Voice);
    }
    public void MageSkill2Sound() //법사 2스킬 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill2Sound);
    }
    public void MageSkill3Sound() //법사 3스킬 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.MageSkill3Sound);
    }
    public void MageJump() //마법사점프
    {
        audioSource.PlayOneShot(Sounds.sounds.MageJump);
    }

}
