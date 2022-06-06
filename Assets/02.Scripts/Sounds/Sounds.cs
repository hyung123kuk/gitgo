using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static Sounds sounds;

    [Header("배경 사운드")]
    public AudioClip TownBGM; //마을 배경음악
    public AudioClip BeginnerGroundBGM; //초보사냥터 배경음악 (슬라임)
    public AudioClip MiddleGroundBGM; //중급사냥터 배경음악 (고블린,슬라임보스)
    public AudioClip DunjeonBGM; //던전브금
    public AudioClip BossBGM; //보스존 브금

    [Header("공용 사운드")]
    public AudioClip FootRunSound; //달리는 발소리
    public AudioClip Horse; //말 소환소리
    public AudioClip Horse2; //말 소환해제소리
    public AudioClip HorseRunSound; //말 달리는소리
    public AudioClip MaleDieSound; //남자 죽음소리
    public AudioClip FeMaleDieSound; //여자 죽음소리


    [Header("캐릭터창 사운드")]
    public AudioClip WarriorTalk1; //전사대사
    public AudioClip WarriorTalk2; //전사대사2
    public AudioClip ArcherTalk1; //궁수대사1
    public AudioClip ArcherTalk2; //궁수대사2
    public AudioClip SelArcherAttackSound; //궁수 공격소리
    public AudioClip MageTalk1; //마법사대사1
    public AudioClip MageTalk2; //마법사대사2
    public AudioClip SelMageAttackSound; //마법사공격소리
    public AudioClip SelMakeSound; //캐릭터생성소리

    [Header("전사 사운드")]
    public AudioClip WarriorAttackSound;//전사 기본공격
    public AudioClip WarriorAttackVoice; //전사 공격보이스;
    public AudioClip WarriorAttackVoice2; //전사 공격보이스;
    public AudioClip WarriorShieldSound; //전사 방패치기 사운드
    public AudioClip WarriorBuffSound; //전사 폭주버프 사운드
    public AudioClip WarriorRushSound; //전사 돌진 사운드
    public AudioClip WarriorRushVoice; //전사 돌진 보이스
    public AudioClip WarriorAuraSound; //전사 검기 사운드
    public AudioClip War_L_R_FootSound; //전사 좌,우 발소리
    public AudioClip War_F_FootSound; //전사 앞 발소리
    public AudioClip War_B_FootSound; //전사 뒤 발소리

    [Header("궁수 사운드")]
    public AudioClip ArcherChargeSound; //궁수 활시위
    public AudioClip ArcherAttackSound; //궁수 일반화살
    public AudioClip ArcherSkill2Sound; //궁수 폭발화살
    public AudioClip BoomSound; //폭발터질때
    public AudioClip ArcherSkill1Sound; //궁수 독화살소리
    public AudioClip ArcherSkill1ShotSound; //궁수 독화살쏘는소리
    public AudioClip ArcherBackStepSound; //궁수 백스텝 사운드
    public AudioClip ArcherSkill3ChargeSound; //궁수 스킬3 차징사운드
    public AudioClip ArcherSkill3ShotSound; //궁수 스킬3 발사사운드;
    public AudioClip ArcherJump; //궁수 점프
    public AudioClip Arc_L_R_FootSound; //궁수 좌,우 발소리
    public AudioClip Arc_F_B_FootSound; //궁수 앞,뒤 발소리

    [Header("마법사 사운드")]
    public AudioClip MageAttackSound; //마법사 기본공격사운드
    public AudioClip MageTeleportSound; //마법사 텔포사운드
    public AudioClip MageSkill1Sound; //마법사 스킬1사운드
    public AudioClip MageSkill1Voice; //마법사 스킬1사운드
    public AudioClip MageSkill2Voice;//마법사 스킬2 보이스
    public AudioClip MageSkill2Sound;//마법사 스킬2 사운드
    public AudioClip MageSkill3Sound; //마법사 메테오떨어지는사운드
    public AudioClip MageSkill3CastSound; //마법사 메테오 캐스팅사운드
    public AudioClip Mag_L_R_FootSound; //마법사 좌,우 발소리
    public AudioClip MageJump; //마법사 점프
    public AudioClip Mag_F_FootSound; //마법사 앞 발소리
    public AudioClip Mag_B_FootSound; //마법사 뒤 발소리



    [Header("몬스터 사운드")]
    public AudioClip SlimeHitSound; //맞는소리
    public AudioClip GoblinHitSound; //맞는소리
    public AudioClip SkeletonHitSound; //맞는소리
    public AudioClip GolemHitSound; //맞는소리

    [Header("UI 사운드")]
    public AudioClip InventoryOpenSound; //인벤열때소리
    public AudioClip InventoryCloseSound; //인벤닫을때소리
    public AudioClip UiSound; //다른UI 소리
    public AudioClip BuySound; //구매할때소리
    public AudioClip BuyfailSound; //구매실패소리
    public AudioClip GetCoinSound; //판매&동전먹을때소리
    public AudioClip DrinkSound; //물약빨때소리
    public AudioClip EquipSound; //장비장착소리
    public AudioClip Quest1; //퀘스트 수락소리
    public AudioClip Quest2; //퀘스트 완료소리

    void Start()
    {
        sounds = this;
    }

}
