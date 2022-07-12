using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().name != "NewChSel") //캐릭터선택창 씬에서는 적용X
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
    public void WarriorTalk1() //캐릭터창 전사 대사1
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorTalk1);
    }
    public void WarriorTalk2() //캐릭터창 전사 대사2
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorTalk2);
    }
    public void SelWarriorAttackSound() //캐릭터창 전사 공격소리
    {
        audioSource.PlayOneShot(Sounds.sounds.WarriorAttackVoice);
    }
    public void ArcherTalk1() //캐릭터창 궁수 대사1
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherTalk1);
    }
    public void ArcherTalk2() //캐릭터창 궁수 대사2
    {
        audioSource.PlayOneShot(Sounds.sounds.ArcherTalk2);
    }
    public void SelArcherAttackSound() //캐릭터창 궁수 공격소리
    {
        audioSource.PlayOneShot(Sounds.sounds.SelArcherAttackSound);
    }
    public void MageTalk1() //캐릭터창 법사 대사1
    {
        audioSource.PlayOneShot(Sounds.sounds.MageTalk1);
    }
    public void MageTalk2() //캐릭터창 법사 대사2
    {
        audioSource.PlayOneShot(Sounds.sounds.MageTalk2);
    }
    public void SelMageAttackSound() //캐릭터창 법사 공격소리
    {
        audioSource.PlayOneShot(Sounds.sounds.SelMageAttackSound);
    }
    public void SelMakeSound() //캐릭터 만들때 소리
    {
        audioSource.PlayOneShot(Sounds.sounds.SelMakeSound);
    }
}
