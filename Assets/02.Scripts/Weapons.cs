using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class Weapons : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum Type { Melee, Range, Mage }; //근접무기와 원거리무기 구분
    public Type type;
    public float damage; //근접무기 공격력 / 원거리무기는 Arrow 스크립트
    public float rate; //공격속도

    bool Key1; //키보드 1번입력
    bool Key2; //키보드 2번입력
    public bool Key3; //키보드 3번입력
    public bool Key3Up;


    public Animator anim;
    private Rigidbody rigid;
    public BoxCollider meleeArea; //근딜범위
    public bool isSootReady = true;


    public PlayerST playerST;
    public AttackDamage attackdamage;
    public PlayerStat playerstat;


    /*=========================궁수 스킬 관련===================================*/
    public bool isBombArrow; //2스킬 쓰고나서 공격딜레이
    public bool isEnergyReady; //3스킬쓰고있는상태
    public bool isEnergy1; //3스킬 1.5초장전상태
    public bool isEnergy2; //3스킬 5초 풀장전상태
    public float EnergyReady; //3스킬 1.5초장전 시간재기
    public float EnergyReady2; //3스킬 5초장전 시간재기
    public float EnergyChargingTime = 5f; //풀차징 제한시간

    public int archer_skill4_Order = 0; //궁수 기모아 쏘기 스킬 차례

    [Header("궁수 관련")]
    public Transform arrowPos; //화살나가는위치
    public Transform Arc3SkillPos; //3스킬 나가는 위치
    public GameObject ArcDefaultAttack; //기본화살
    public GameObject Arc1Skilarrow; //독화살
    public GameObject Arc2Skilarrow; //폭탄화살
    public GameObject Arc3SkillBuff1;  //기모을때 버프
    public GameObject Arc3SkillBuff2;  //기모을때 버프2차
    public GameObject Arc3SkillBuff3;  //기모을때 버프3차
    public GameObject Arc3SkillArrow1; //1화살 발사이펙트
    public GameObject Arc3SkillArrow2; //2화살 발사이펙트


    /*=========================마법사 스킬 관련===================================*/
    private float MeteoCasting; //3스킬 캐스팅시간
    private float MeteoMaxCasting = 4f; //3스킬 최대 캐스팅시간


    public bool isLightning;  //현재 스킬사용중?
    public bool isIceage;
    public bool isMeteo;
    public bool isMeteoShot;

    public int mage_skill4_Order = 0; //마법사 메테오 스킬 차례

    [Header("마법사 관련")]
    public Transform MagicPos; //마법나가는위치
    public GameObject MageDefaultAttack; //마법사 기본공격
    public GameObject Mage1SkillEff; //마법사1스킬
    public Transform Mage1SkillPos1; //1스킬 도착위치
    public Transform Mage1SkillPos2;
    public Transform Mage1SkillPos3;
    public GameObject Mage2SkillEff; //2스킬 이펙트
    public GameObject Mage2SkillReadyEff; //2스킬 준비동작이펙트
    public Transform Mage3SkillPos1; //3스킬 시작위치
    public Transform Mage3SkillPos2; //3스킬 도착위치
    public GameObject Mage3SkillPosEff; //3스킬 도착위치 이펙트
    public GameObject Mage3SkillEff; //3스킬 이펙트
    public GameObject Mage3SkillPlayerEff; //3스킬 이펙트 플레이어쪽
    public GameObject Skillarea; //켜지면 데미지만
    public GameObject Skillarea2; //켜지면 데미지만
    public GameObject CCarea;  //켜지면 CC기 

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            if (type == Type.Range)
            {
                stream.SendNext(archer_skill4_Order);
                stream.SendNext(ShotFull);
                stream.SendNext(EnergyReady);
                stream.SendNext(EnergyReady2);
                stream.SendNext(isEnergy1);
                stream.SendNext(isEnergy2);
                stream.SendNext(isEnergyReady);
            }
            else if (type == Type.Mage)
            {
                stream.SendNext(mage_skill4_Order);
                stream.SendNext(MeteoCasting);
                stream.SendNext(isMeteo);
                stream.SendNext(isIceage);
            }

        }
        else
        {
            if (type == Type.Range)
            {
                archer_skill4_Order = (int)stream.ReceiveNext();
                ShotFull = (bool)stream.ReceiveNext();
                EnergyReady = (float)stream.ReceiveNext();
                EnergyReady2 = (float)stream.ReceiveNext();
                isEnergy1 = (bool)stream.ReceiveNext();
                isEnergy2 = (bool)stream.ReceiveNext();
                isEnergyReady = (bool)stream.ReceiveNext();
            }
            else if (type == Type.Mage)
            {
                mage_skill4_Order = (int)stream.ReceiveNext();
                MeteoCasting = (float)stream.ReceiveNext();
                isMeteo = (bool)stream.ReceiveNext();
                isIceage = (bool)stream.ReceiveNext();
            }

        }
    }
    private void Start()
    {
        //if (!photonView.IsMine)
        //    this.enabled = false;

        anim = GetComponentInParent<Animator>();
    }
    private void Update()
    {


        Key1 = Input.GetButtonDown("Key1"); //1번키
        Key2 = Input.GetButtonDown("Key2"); //2번키
        Key3 = Input.GetButton("Key3"); //3번키 꾹
        Key3Up = Input.GetKeyUp(KeyCode.Alpha3);

        /*if (type == Type.Range)
        {
            BombArrow();
            EnergyArrow();
        }
        else if (type == Type.Mage)
        {
            LightningBall();
            IceAge();
            Meteo();
        }*/


    }
    private void FixedUpdate()
    {
        //attackdamage.SkillPassedTimeFucn(); (weapons스크립트의 117줄, attackDamage의125줄 , playerST의 553줄에 총 3개가 있어 쿨타임이 3배로 돌아갑니다. 그래서 2개 주석처리 해놨습니다. 문제시에 알려주세요)
    }
    //============================궁수스킬========================================
    public void BombArrow() //폭탄화살 궁수용
    {
        if (!photonView.IsMine)
            return;

        if (!playerST.isDodge && !isEnergyReady && !playerST.isJump
            && !playerST.isStun && !playerST.isRun && attackdamage.Usable_Skill2)
        {
            StartCoroutine(BombArrowPlay());
            photonView.RPC("BombArrowPlay", RpcTarget.Others);
        }
    }
    [PunRPC]
    IEnumerator BombArrowPlay()
    {
        isBombArrow = true;
        playerST.isCool2 = true;
        if (photonView.IsMine)
            attackdamage.Skill_2_Cool();
        SoundManager.soundManager.ArcherSkill2_1Sound();
        anim.SetBool("isBomb", true);
        GameObject bombarrow = Instantiate(Arc2Skilarrow, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = bombarrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward * 20;
        Arrow arrow = bombarrow.GetComponent<Arrow>(); //스킬데미지설정
        arrow.damage = attackdamage.Skill_2_Damamge();

        Destroy(bombarrow, 1f);

        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isBomb", false);
        yield return new WaitForSeconds(0.2f);
        isBombArrow = false;

    }

    public void EnergyArrow()
    {
        if (!photonView.IsMine)
            return;

        if ((archer_skill4_Order == 0 || archer_skill4_Order == 1) && !playerST.isDodge && !playerST.isStun && !playerST.isJump && !playerST.isRun &&
            attackdamage.Usable_Skill3)
        {
            photonView.RPC("EnergyArrowPlay", RpcTarget.All);
        }
    }
    public bool energyfullCharging = false;

    [PunRPC]
    public void EnergyArrowPlay()
    {
        if (archer_skill4_Order == 0 && !playerST.isDodge && !playerST.isStun && !playerST.isJump && !playerST.isRun &&
        attackdamage.Usable_Skill3)
        {
            if (!StopSoundManager.stopSoundManager.audioSource.isPlaying)
                StopSoundManager.stopSoundManager.ArcherSkill3ChargeSound();

            playerST.isCool3 = true;
            isEnergyReady = true;
            anim.SetBool("isReady", true);
            Arc3SkillBuff1.SetActive(true);

            if (archer_skill4_Order == 0 && EnergyReady < EnergyChargingTime)
            {
                EnergyReady += Time.deltaTime;
                EnergyReady2 += Time.deltaTime;
            }

            if (archer_skill4_Order == 0 && EnergyReady2 >= 5f)
            {
                Arc3SkillBuff3.SetActive(true);
                isEnergy1 = false;
                isEnergy2 = true;
                Arc3SkillBuff2.SetActive(false);
                Arc3SkillBuff1.SetActive(false);
            }
            else if (archer_skill4_Order == 0 && EnergyReady > 1.5f)
            {
                isEnergy1 = true;
                Arc3SkillBuff1.SetActive(false);
                Arc3SkillBuff2.SetActive(true);
            }
        }

        else if (archer_skill4_Order == 1)
        {
            isEnergyReady = false;

            StopSoundManager.stopSoundManager.audioSource.Stop();

            anim.SetBool("isReady", false);
            anim.SetBool("isShot", true);
            if (isEnergy1)
            {
                photonView.RPC("EnergyShot1", RpcTarget.All);
            }
            else if (isEnergy2)
            {
                photonView.RPC("EnergyShot2", RpcTarget.All);
            }

            EnergyReady = 0;
            EnergyReady2 = 0;
            isEnergy1 = false;
            isEnergy2 = false;

            photonView.RPC("EnergyArrowOut", RpcTarget.All);
            photonView.RPC("EnergyAnimOut", RpcTarget.All);
        }
    }
    [PunRPC]
    void EnergyShot1()
    {
        attackdamage.Skill_3_Cool();
        SoundManager.soundManager.ArcherSkill3Sound();
        GameObject intantArrow = Instantiate(Arc3SkillArrow1, Arc3SkillPos.position, Arc3SkillPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = Arc3SkillPos.forward * 20;
        ArrowSkill arrowskill = intantArrow.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrowskill.damage = attackdamage.Skill_3_Damamge();
        energyfullCharging = false;
        Destroy(intantArrow, 2f);

    }
    [PunRPC]
    void EnergyShot2()
    {
        attackdamage.Skill_3_Cool();
        SoundManager.soundManager.ArcherSkill3Sound();
        GameObject intantArrow = Instantiate(Arc3SkillArrow2, Arc3SkillPos.position, Arc3SkillPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = Arc3SkillPos.forward * 20;
        ArrowSkill arrowskill = intantArrow.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrowskill.damage = 1.5f * attackdamage.Skill_3_Damamge();
        energyfullCharging = true;
        Destroy(intantArrow, 2f);

    }
    [PunRPC]
    IEnumerator EnergyArrowOut()
    {
        yield return new WaitForSeconds(0.5f);
        Arc3SkillBuff1.SetActive(false);
        Arc3SkillBuff2.SetActive(false);
        Arc3SkillBuff3.SetActive(false);

    }
    [PunRPC]
    IEnumerator EnergyAnimOut()
    {

        yield return new WaitForSeconds(1f);
        anim.SetBool("isShot", false);

    }
    //============================================마법사 스킬=====================================================
    public void LightningBall()
    {
        if (!photonView.IsMine)
            return;

        if (!playerST.isDodge && !playerST.isStun && !playerST.isJump && !playerST.isRun && !playerST.isFlash && !isIceage &&
           !isMeteo && attackdamage.Usable_Skill1)
        {
            StartCoroutine(LightningBallStart());
            photonView.RPC("LightningBallStart", RpcTarget.Others);
        }
    }
    [PunRPC]
    IEnumerator LightningBallStart()
    {
        playerST.isCool1 = false;
        attackdamage.Usable_Skill1 = false;

        isLightning = true;
        anim.SetBool("Skill1", true);
        SoundManager.soundManager.MageSkill1Voice();
        yield return new WaitForSeconds(1.2f);
        SoundManager.soundManager.MageSkill1Sound();
        GameObject darkball1 = Instantiate(Mage1SkillEff, MagicPos.position, MagicPos.rotation);
        GameObject darkball2 = Instantiate(Mage1SkillEff, MagicPos.position, MagicPos.rotation);
        GameObject darkball3 = Instantiate(Mage1SkillEff, MagicPos.position, MagicPos.rotation);
        darkball1.transform.DOMove(Mage1SkillPos1.position, 1f).SetEase(Ease.Linear);
        darkball2.transform.DOMove(Mage1SkillPos2.position, 1f).SetEase(Ease.Linear);
        darkball3.transform.DOMove(Mage1SkillPos3.position, 1f).SetEase(Ease.Linear);
        ArrowSkill arrow1 = darkball1.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow1.damage = attackdamage.Skill_1_Damamge();
        ArrowSkill arrow2 = darkball2.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow2.damage = attackdamage.Skill_1_Damamge();
        ArrowSkill arrow3 = darkball3.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow3.damage = attackdamage.Skill_1_Damamge();
        Destroy(darkball1, 1.1f);
        Destroy(darkball2, 1.1f);
        Destroy(darkball3, 1.1f);

        playerST.isCool1 = true;
        if (photonView.IsMine)
            attackdamage.Skill_1_Cool();


        yield return new WaitForSeconds(0.5f);
        isLightning = false;
        anim.SetBool("Skill1", false);

    }
    public void IceAge()
    {
        if (!photonView.IsMine)
            return;

        if (!playerST.isDodge && !playerST.isStun && !playerST.isJump && !playerST.isRun && !isLightning && !playerST.isFlash &&
           !isMeteo && attackdamage.Usable_Skill2)
        {
            StartCoroutine(IceAgeStart());
            photonView.RPC("IceAgeStart", RpcTarget.Others);
        }
    }
    [PunRPC]
    IEnumerator IceAgeStart()
    {
        SoundManager.soundManager.MageSkill2Voice();
        playerST.isCool2 = false;
        attackdamage.Usable_Skill2 = false;

        Mage2SkillReadyEff.SetActive(true);
        anim.SetBool("Skill2", true);
        isIceage = true;
        yield return new WaitForSeconds(1.8f);
        SoundManager.soundManager.MageSkill2Sound();
        BoxCollider Skillare = Skillarea.GetComponent<BoxCollider>(); //데미지 콜라이더 활성화
        Skillare.enabled = true;
        ArrowSkill arrow = Skillarea.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow.damage = attackdamage.Skill_2_Damamge();
        BoxCollider CCare = CCarea.GetComponent<BoxCollider>(); //cc기 콜라이더 활성화
        CCare.enabled = true;

        Mage2SkillReadyEff.SetActive(false);
        Mage2SkillEff.SetActive(true);

        playerST.isCool2 = true;
        if (photonView.IsMine)
            attackdamage.Skill_2_Cool();

        yield return new WaitForSeconds(0.2f);
        CCare.enabled = false;
        Skillare.enabled = false;
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("Skill2", false);
        isIceage = false;


        yield return new WaitForSeconds(1f);
        Mage2SkillEff.SetActive(false);

    }
    public void Meteo()
    {
        if (!photonView.IsMine)
            return;

        if ((mage_skill4_Order == 0 || mage_skill4_Order == 1) && !playerST.isDodge && !playerST.isStun && !playerST.isJump && !playerST.isRun && !playerST.isFlash
            && !isLightning && !isIceage && attackdamage.Usable_Skill3)
        {
            photonView.RPC("MeteoPlay", RpcTarget.All);
        }
    }
    [PunRPC]
    public void MeteoPlay()
    {
        if ((mage_skill4_Order == 0 || mage_skill4_Order == 1) && !playerST.isDodge && !playerST.isStun && !playerST.isJump && !playerST.isRun && !playerST.isFlash
            && !isLightning && !isIceage && attackdamage.Usable_Skill3)
        {
            playerST.isCool3 = true;
            if (mage_skill4_Order == 0 && MeteoCasting < MeteoMaxCasting)
            {
                if (!StopSoundManager.stopSoundManager.audioSource.isPlaying)
                    StopSoundManager.stopSoundManager.MageSkill3CastSound();

                isMeteo = true;
                Mage3SkillPlayerEff.SetActive(true);
                anim.SetBool("Skill3", true);
                anim.SetBool("Skill3Cut", false);
                Mage3SkillPosEff.SetActive(true);
                MeteoCasting += Time.deltaTime;
            }
            else if (mage_skill4_Order == 1 && !isMeteoShot)
            {

                StopSoundManager.stopSoundManager.audioSource.Stop();

                MeteoCasting = 0f;
                isMeteo = false;
                anim.SetBool("Skill3", false);
                anim.SetBool("Skill3Cut", true);
                photonView.RPC("MeteoEffRemove", RpcTarget.All);
            }
            if (MeteoCasting > MeteoMaxCasting)
            {
                StopSoundManager.stopSoundManager.audioSource.Stop();
                SoundManager.soundManager.MageSkill3Sound();
                Debug.Log("메테오!!");
                isMeteoShot = true;
                anim.SetBool("Skill31", true);
                GameObject meteo = Instantiate(Mage3SkillEff, Mage3SkillPos2.position, Mage3SkillPos2.rotation);
                meteo.transform.DOMove(Mage3SkillPos1.position, 1.5f).SetEase(Ease.Linear);
                ArrowSkill arrow1 = meteo.GetComponent<ArrowSkill>(); //스킬데미지설정
                arrow1.damage = attackdamage.Skill_3_Damamge();
                Destroy(meteo, 1.6f);
                MeteoCasting = 0f;
                attackdamage.Skill_3_Cool();

                photonView.RPC("MeteoEnd", RpcTarget.All);
                photonView.RPC("MeteoEnd2", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    void MeteoEffRemove()
    {
        Mage3SkillPlayerEff.SetActive(false);
        Mage3SkillPosEff.SetActive(false);
    }

    [PunRPC]
    IEnumerator MeteoEnd()
    {
        yield return new WaitForSeconds(0.8f);
        Mage3SkillPlayerEff.SetActive(false);
        anim.SetBool("Skill3", false);

    }
    [PunRPC]
    IEnumerator MeteoEnd2()
    {
        yield return new WaitForSeconds(1.5f);
        Mage3SkillPosEff.SetActive(false);
        anim.SetBool("Skill31", false);
        isMeteoShot = false;
        isMeteo = false;
    }
    //====================================================================================

    public void Use()//무기 사용
    {
        //if (type == Type.Melee) //근접무기일때 실행
        //{
        //    StopCoroutine("Swing");  //현재 공격중일시 멈춤
        //    StartCoroutine("Swing"); //공격실행
        //}
        //if (!photonView.IsMine)
        //    return;

        //if (type == Type.Range)
        //{
        //    photonView.RPC("Shot", RpcTarget.All);
        //}
        //else if (type == Type.Mage)
        //{
        //    photonView.RPC("MagicShot", RpcTarget.All);
        //}
    }

    IEnumerator Swing()
    {


        yield return new WaitForSeconds(0.1f); // 대기
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = false;


    }
    public bool ShotFull;

    [PunRPC]
    public IEnumerator Shot()
    {
        playerST.isSootReady = true;
        yield return new WaitForSeconds(0.2f); //애니메이션과 화살나가는속도와 맞추기위함
        if (!attackdamage.Duration_Buff)
        {
            if (photonView.IsMine)
            {
                GameObject intantArrow = Instantiate(ArcDefaultAttack, arrowPos.position, arrowPos.rotation);
                Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
                arrowRigid.velocity = arrowPos.forward * playerST.bowPower * 150;
                Arrow arrow = intantArrow.GetComponent<Arrow>(); //스킬데미지설정
                Destroy(intantArrow, 1f);

                if (playerST.FullChargeing)
                {
                    arrow.damage = 1.3f * attackdamage.Attack_Dam();
                    playerST.FullChargeing = false;
                    ShotFull = true;
                }
                else if (!playerST.FullChargeing)
                {
                    arrow.damage = attackdamage.Attack_Dam();
                    ShotFull = false;
                }
            }


        }
        else if (attackdamage.Duration_Buff)
        {
            GameObject intantArrow = Instantiate(Arc1Skilarrow, arrowPos.position, arrowPos.rotation);
            Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
            arrowRigid.velocity = arrowPos.forward * playerST.bowPower * 150;
            Arrow arrow = intantArrow.GetComponent<Arrow>(); //스킬데미지설정
            arrow.damage = 1.3f * attackdamage.Attack_Dam();
            ShotFull = true;
            Destroy(intantArrow, 1f);
        }

        playerST.anim.SetBool("doShot", false);



        yield return new WaitForSeconds(0.25f);
        playerST.bowPower = 0;
        playerST.isFireReady = true;
        yield return new WaitForSeconds(0.1f);
        playerST.archerattack = false;

        yield return null;

    }

    [PunRPC]
    public IEnumerator MagicShot()
    {
        if (playerST.CharacterType == PlayerST.Type.Mage)
            anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.3f); //애니메이션과 화살나가는속도와 맞추기위함
        SoundManager.soundManager.MageAttackSound();
        if (photonView.IsMine)
        {
            GameObject intantArrow = Instantiate(MageDefaultAttack, MagicPos.position, MagicPos.rotation);
            Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
            arrowRigid.velocity = MagicPos.forward * 20;
            Arrow arrow = intantArrow.GetComponent<Arrow>(); //스킬데미지설정
            arrow.damage = attackdamage.Attack_Dam();
            Destroy(intantArrow, 0.7f);
        }
        yield return new WaitForSeconds(0.3f);
        if (playerST.CharacterType == PlayerST.Type.Mage)
            anim.SetBool("isAttack", false);
        yield return null;
    }


}