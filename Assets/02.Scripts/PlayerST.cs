using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerST : MonoBehaviourPunCallbacks, IPunObservable
{


    public enum Type { Warrior, Archer, Mage };
    public Type CharacterType; //원래 앞에 static이 붙어있었는데 테스트할때 인스펙터창에 타입이 안떠서 임시로 뻈어요
    Transform _transform;
    public Rigidbody rigid;
    ComboHit comboHit;

    public float jump = 5.0f; //점프력
    public float speed = 5.0f;  //플레이어 이동속도
    public GameObject cam; //플레이어 카메라
    public CapsuleCollider SelectPlayer; //제어할 플레이어
    public Animator anim; //애니메이션
    public Weapons[] equipWeapon;    //현재 무기. 나중에 배열로 여러무기를 등록하려고함
    public Item.SwordNames swordNames;  //무기이름 위의 배열의 순서에 따라.
    public Item.SwordNames basicSword = 0;
    public int NowWeapon; //현재 무기



    public float bowMinPower = 0.2f;
    public float bowPower = 0.1f; // 화살 충전 데미지
    public float bowChargingTime = 1.0f; //화살 최대 충전시간
    public bool isSootReady = true;
    public bool FullChargeing; //일반공격 풀차징


    float h; //X값 좌표
    float v; //Z값 좌표
    float fireDelay; //공격속도 계산용

    private bool fDown; //마우스 왼쪽버튼을 눌렀다면 true
    private bool fDowning; //마우스 왼쪽버튼을 눌르고 있다면 true
    public bool fUp;
    private bool f2Down; //마우스 우클 눌렀다면
    public bool isFireReady = true;  //무기 rate가 fireDelay보다 작다면 true로 공격가능상태
    public bool isDamage; //무적타임. 연속으로 다다닥 맞을수있기때문에
    private bool sDown; //점프입력
    public bool Rdown;//알트입력
    private bool Ddown; //쉬프트입력 
    private bool Key1; //키보드 1번입력
    private bool Key2; //키보드 2번입력
    private bool Key3; //키보드 3번입력
    public bool ImWar; //나는 워리어다

    public bool isJump; //현재 점프중?
    public bool archerattack = false; //현재 궁수공격중
    public bool isStun; //현재 스턴상태
    public bool isRun; //현재 달리는상태?

    Vector3 moveVec;
    Vector3 dodgeVec;

    public AttackDamage attackdamage;
    public PlayerStat playerstat;
    public GameObject Skillarea; //켜지면 데미지만
    public GameObject Skillarea2; //켜지면 데미지만
    public GameObject CCarea;  //켜지면 CC기 

    public PlayerST playerST;
    public bool HorseMode; //말타고있는지
    public GameObject Horsee; //말 안장
    public GameObject HorseSpawn; //말 부모객체
    public Transform horsepos1; //말 소환시 나오는곳
    public Transform horsepos2; //말 소환시 나오는곳

    public bool DunjeonBossArena; //현재 위치가 보스아레나
    public bool NoMove; //벽뚫방지

    public bool isDie; //플레이어 사망
    public DieUI dieui;
    public BGM bgm;

    public int horsecount = 0;



    

    public bool isFall; //공중에 떠있는상태? 몬스터들의 룩엣을 조정하기위함.
    //======================전사 스킬========================//

    public bool isDodge; //현재 회피중?
    private bool isBlock; //현재 방패치기중? ㅣ마우스 우클릭
    public bool isBuff; //현재 폭주상태? ㅣ키보드 1번
    private bool isRush; //현재 돌진상태? ㅣ키보드 2번
    private bool isAura; //현재 검기날리는 상태? ㅣ 키보드 3번
    private bool isYes; //돌진 벽에서 쓰는행위 막는용도


    [Header("전사 관련")]
    public ParticleSystem BuffEff;
    public GameObject RushEff;
    public Transform Aurapos;
    public GameObject SwordAura; //검기스킬 투사체

    //======================궁수 스킬========================// 
    [Header("궁수 관련")]
    public GameObject BackStepEff;
    public Transform BackStepPos;

    public bool isBackStep; //현재 백스텝상태?
    public bool isPoison; //현재 독화살버프상태?  나중에 몬스터 충돌판정에서 if걸고 추가데미지를 줄예정

    //======================마법사 스킬========================// 
    [Header("마법사 관련")]
    public MeshRenderer mesh;           //순간이동때 투명화
    public SkinnedMeshRenderer[] smesh;   //순간이동때 투명화
    public GameObject FlashEff;  //순간이동이펙트
    public bool isFlash; //현재 순간이동중?
    [SerializeField]
    QuestStore questStore;

    //쿨타임 돌아가게 하기
    public bool isCool1;
    public bool isCool2;
    public bool isCool3;
    public bool isCool4;
    public bool isCooldodge;
    public bool isCoolTeleport;

    public Weapons weapons;
    public Image healthbar;
    public Text nickname;
    public Image helathbarBack;
    private SaveManager saveManager;


    private void Awake()
    {

        if (!photonView.IsMine)
        {
            this.enabled = false;
        }

        comboHit = GetComponent<ComboHit>();
        anim = GetComponentInChildren<Animator>();
        smesh = GetComponentsInChildren<SkinnedMeshRenderer>();
        photonView.RPC("Setting", RpcTarget.AllBuffered);
        saveManager = FindObjectOfType<SaveManager>();
        nickname = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        healthbar = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        helathbarBack = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponent<Image>();
    }

    [PunRPC]
    void NicknameSerching() //닉넴찾기 다른플레이어
    {
        PlayerMine[] ownerplayers = FindObjectsOfType<PlayerMine>();
        foreach (PlayerMine ownerplayer in ownerplayers)
        {
            if (!ownerplayer.GetComponent<PhotonView>().IsMine)
            {
                ownerplayer.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.
                    GetComponent<Text>().text = ownerplayer.GetComponent<PhotonView>().Owner.NickName;
                if (ownerplayer == null)
                    break;
            }
        }
    }
    void Start()
    {
        nickname.text = PhotonNetwork.LocalPlayer.NickName;
        healthbar.CrossFadeAlpha(0, 0, true);  //자기자신 HP바 가리기
        //nickname.CrossFadeAlpha(0, 0, true);
        helathbarBack.CrossFadeAlpha(0, 0, true);
        healthbar.fillAmount = playerstat._Hp / playerstat._MAXHP;
        bgm = GameObject.Find("Sounds").transform.GetChild(3).GetComponent<BGM>();
        playerstat = GetComponent<PlayerStat>();
        bowPower = bowMinPower;
        _transform = GetComponent<Transform>();



        playerST = this;
        dieui = GameObject.Find("DieUI").GetComponent<DieUI>();
        weapons = FindObjectOfType<Weapons>();
        rigid = GetComponent<Rigidbody>();
        // HorseSpawn = FindObjectOfType<Horse>().gameObject;
        GameObject[] horses = GameObject.FindGameObjectsWithTag("Horse");
        foreach (GameObject horse1 in horses)
        {
            if (horse1.GetComponent<PhotonView>().IsMine)
            {
                HorseSpawn = horse1;
                break;
            }
        }
        Horsee = HorseSpawn.transform.GetChild(1).transform.GetChild(0).transform.GetChild(10).transform.GetChild(6).transform.GetChild(0).gameObject; //안장

    }

    [PunRPC]
    void Setting() //변수 동기화 이유: 무기낄때 퀘스트쪽에서 에러
    {
        questStore = FindObjectOfType<QuestStore>();
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트라면 쓰기 부분이 실행됨
        if (stream.IsWriting)
        {
            // 보우파워 값 보내기
            stream.SendNext(bowPower);
            stream.SendNext(isCool1);
            stream.SendNext(isCool2);
            stream.SendNext(isCool3);
            stream.SendNext(isCool4);
            stream.SendNext(isCooldodge);
            stream.SendNext(isCoolTeleport);
            stream.SendNext(healthbar.fillAmount);
        }
        else
        {
            // 리모트 오브젝트라면 읽기 부분이 실행됨         

            // 네트워크를 통해 보우파워값 받기
            bowPower = (float)stream.ReceiveNext();
            isCool1 = (bool)stream.ReceiveNext();
            isCool2 = (bool)stream.ReceiveNext();
            isCool3 = (bool)stream.ReceiveNext();
            isCool4 = (bool)stream.ReceiveNext();
            isCooldodge = (bool)stream.ReceiveNext();
            isCoolTeleport = (bool)stream.ReceiveNext();
            healthbar.fillAmount = (float)stream.ReceiveNext();
        }
    }



    void Anima() //애니메이션 
    {

        if (!photonView.IsMine)
            return;

        #region 전사 이동 애니메이션
        if (CharacterType == Type.Warrior)
        {
            if (Rdown)
            {
                anim.SetBool("isRun", true);
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.FootRunSound();
            }
            else if (!Rdown)
            {
                anim.SetBool("isRun", false);
            }
            if (v >= 0.1f) //앞
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.War_F_FootSound();

                anim.SetBool("forword", true);
                anim.SetBool("back", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
            }
            else if (v <= -0.1f) //뒤
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.War_B_FootSound();
                anim.SetBool("back", true);
                anim.SetBool("forword", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
            }
            else if (h >= 0.1f) //오른쪽
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.War_L_R_FootSound();
                anim.SetBool("right", true);
                anim.SetBool("forword", false);
                anim.SetBool("back", false);
                anim.SetBool("left", false);
            }
            else if (h <= -0.1f) //왼쪽
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.War_L_R_FootSound();
                anim.SetBool("left", true);
                anim.SetBool("forword", false);
                anim.SetBool("back", false);
                anim.SetBool("right", false);
            }
            else
            {
                FootSound.footSound.audioSource.Stop();
                anim.SetBool("forword", false);
                anim.SetBool("back", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
            }

        }
        #endregion
        #region 궁수 이동 애니메이션
        if (CharacterType == Type.Archer)
        {
            if (Rdown)
            {
                anim.SetBool("isRun", true);
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.FootRunSound();
            }
            else if (!Rdown)
            {
                anim.SetBool("isRun", false);
            }

            if (v >= 0.1f) //앞
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.Arc_F_B_FootSound();

                anim.SetBool("forword", true);
                anim.SetBool("back", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
            }
            else if (v <= -0.1f) //뒤
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.Arc_F_B_FootSound();
                anim.SetBool("back", true);
                anim.SetBool("forword", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
            }
            else if (h >= 0.1f) //오른쪽
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.Arc_L_R_FootSound();
                anim.SetBool("right", true);
                anim.SetBool("forword", false);
                anim.SetBool("back", false);
                anim.SetBool("left", false);
            }
            else if (h <= -0.1f) //왼쪽
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.Arc_L_R_FootSound();
                anim.SetBool("left", true);
                anim.SetBool("forword", false);
                anim.SetBool("back", false);
                anim.SetBool("right", false);
            }
            else
            {
                if (FootSound.footSound)
                    FootSound.footSound.audioSource.Stop();
                anim.SetBool("forword", false);
                anim.SetBool("back", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
            }
        }
        #endregion
        #region 법사 이동 애니메이션
        if (CharacterType == Type.Mage)
        {
            if (Rdown)
            {
                anim.SetBool("isRun", true);
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.FootRunSound();
            }
            else if (!Rdown)
            {
                anim.SetBool("isRun", false);
            }

            if (v >= 0.1f) //앞
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.Mag_F_FootSound();

                anim.SetBool("forword", true);
                anim.SetBool("back", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
            }
            else if (v <= -0.1f) //뒤
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.Mag_B_FootSound();
                anim.SetBool("back", true);
                anim.SetBool("forword", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
            }
            else if (h >= 0.1f) //오른쪽
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.Mag_L_R_FootSound();
                anim.SetBool("right", true);
                anim.SetBool("forword", false);
                anim.SetBool("back", false);
                anim.SetBool("left", false);
            }
            else if (h <= -0.1f) //왼쪽
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.Mag_L_R_FootSound();
                anim.SetBool("left", true);
                anim.SetBool("forword", false);
                anim.SetBool("back", false);
                anim.SetBool("right", false);
            }
            else
            {
                FootSound.footSound.audioSource.Stop();
                anim.SetBool("forword", false);
                anim.SetBool("back", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
            }
        }
        #endregion
    }


    void Attack()   //공격
    {
        if (!photonView.IsMine)
            return;

        if (CharacterType == Type.Warrior && !isDodge && !isFlash && !weapons.isLightning &&
           !weapons.isIceage && !weapons.isMeteo && !isJump && !isRun && !isBlock && !isRush && !isAura && !isStun)
        {
            WAttack();
            photonView.RPC("WAttack", RpcTarget.Others);
        }
        else if (CharacterType == Type.Mage && !isDodge && !isFlash && !weapons.isLightning &&
          !weapons.isIceage && !weapons.isMeteo && !isJump && !isRun && !isBlock && !isRush && !isAura && !isStun)
        {
            MAttack();
            photonView.RPC("MAttack", RpcTarget.Others);
        }

        else if (CharacterType == Type.Archer && !isDodge && !isJump && !isRun && !isStun && !isBackStep && !weapons.isEnergyReady
            && !weapons.isBombArrow)
        {
            AAttack();
            photonView.RPC("AAttack", RpcTarget.Others);

        }
    }
    [PunRPC]
    void WAttack()
    {
        fireDelay += Time.deltaTime;     //공격속도 계산
        isFireReady = equipWeapon[NowWeapon].rate < fireDelay;  //공격 가능 타임

        if (fDown)
        {
            if (isFireReady)  //공격할수있을때
            {

                weapons.damage = attackdamage.Attack_Dam(); //기본공격 데미지
                                                            //equipWeapon[NowWeapon].Use();
                fireDelay = 0;
            }
        }
    }
    [PunRPC]
    void AAttack()
    {
        fireDelay += Time.deltaTime;

        if (fDowning && bowPower < bowChargingTime)
        {
            bowPower += Time.deltaTime;
            if (bowPower > 0.31f && StopSoundManager.stopSoundManager && !StopSoundManager.stopSoundManager.audioSource.isPlaying)
            {
                StopSoundManager.stopSoundManager.ArcherChargeSound();
            }
            if (bowPower >= bowChargingTime) //풀차징되면
            {
                FullChargeing = true;
            }
        }

        if (fDowning && isFireReady && equipWeapon[NowWeapon].rate < fireDelay)
        {
            archerattack = true;
            bowPower = bowMinPower;
            anim.SetBool("DefaultShot", true);
            isSootReady = false;
            isFireReady = false;
            fireDelay = 0f;
        }
        else if (fUp && !isSootReady)
        {
            archerattack = true;
            anim.SetBool("DefaultShot", false);
            anim.SetBool("doShot", true);
            StopSoundManager.stopSoundManager.audioSource.Stop();
            if (!attackdamage.Duration_Buff)
                SoundManager.soundManager.ArcherAttackSound();
            else if (attackdamage.Duration_Buff)
                SoundManager.soundManager.ArcherSkill1ShotSound();
            //equipWeapon[NowWeapon].photonView.RPC("Use", RpcTarget.All);
            equipWeapon[NowWeapon].Use();
        }
    }
    [PunRPC]
    void MAttack()
    {
        fireDelay += Time.deltaTime;     //공격속도 계산
        isFireReady = equipWeapon[NowWeapon].rate < fireDelay;  //공격 가능 타임

        if (fDown)
        {
            if (isFireReady)  //공격할수있을때
            {

                equipWeapon[NowWeapon].Use();
                //equipWeapon[NowWeapon].photonView.RPC("Use", RpcTarget.All);
                fireDelay = 0;
            }
        }
    }
    void Jump()
    {

        if (!photonView.IsMine)
            return;

        if (sDown && !isJump && !isDodge && !isBlock && !isRush && !isAura && !isBackStep && !weapons.isEnergyReady &&
           !weapons.isLightning && !weapons.isIceage && !weapons.isMeteo && !isFlash && !isStun && !isRun
            )
        {
            if (!archerattack && CharacterType == Type.Archer)
                photonView.RPC("JumpPlay", RpcTarget.All);
            else if (CharacterType == Type.Warrior || CharacterType == Type.Mage)
                photonView.RPC("JumpPlay", RpcTarget.All);
        }
    }
    [PunRPC]
    void JumpPlay()
    {
        FootSound.footSound.audioSource.Stop();

        if (CharacterType == Type.Archer)
            SoundManager.soundManager.ArcherJump();

        if (CharacterType == Type.Warrior)
            SoundManager.soundManager.WarriorAttackVoice();

        if (CharacterType == Type.Mage)
            SoundManager.soundManager.MageJump();

        if (!photonView.IsMine)
            return;
        rigid.AddForce(Vector3.up * jump, ForceMode.Impulse); //애드포스 : 힘을주다/ 포스모드,임펄스 : 즉발적
        anim.SetBool("isJump", true);
        if (CharacterType == Type.Warrior)
        {
            Invoke("WaJumpEnd", 0.2f); //전사 애니메이션 꼬임 방지용
        }
        isJump = true;
    }

    void WaJumpEnd()
    {
        anim.SetBool("isAttack4", false);
        anim.SetBool("isAttack3", false);
        anim.SetBool("isAttack2", false);
        anim.SetBool("isAttack", false);
        comboHit.noOfClicks = 0;
    }

    public void Dodge()
    {

        if (!photonView.IsMine)
            return;

        if (!isStun && !isJump && !isBlock && !isBackStep && !weapons.isEnergyReady && !isRush && !isAura && !isFlash &&
           !weapons.isLightning && !weapons.isIceage && !weapons.isMeteo && attackdamage.Usable_Dodge && !isRun)
        {
            DodgePlay();
            photonView.RPC("DodgePlay", RpcTarget.Others);
        }
    }
    [PunRPC]
    void DodgePlay()
    {

        FootSound.footSound.audioSource.Stop();
        if (CharacterType == Type.Archer)
            SoundManager.soundManager.ArcherJump();

        if (CharacterType == Type.Warrior)
            SoundManager.soundManager.WarriorAttackVoice();

        if (CharacterType == Type.Mage)
            SoundManager.soundManager.MageJump();

        isCooldodge = false;
        attackdamage.Usable_Dodge = false;
        if (photonView.IsMine)
        {
            dodgeVec = moveVec;
            speed *= 2;
        }
        anim.SetBool("isDodge", true);
        isDodge = true;
        isDamage = true;

        Invoke("DodgeOut", 0.4f); //구르기를 하면 0.4초후에 이동속도가 정상으로돌아옴

    }

    void DodgeOut()
    {
        isCooldodge = true;
        if (photonView.IsMine)
            attackdamage.Skill_Dodge_Cool();
        if (photonView.IsMine)
            speed *= 0.5f;
        isDodge = false;
        isDamage = false;
        anim.SetBool("isDodge", false);
        if (CharacterType == Type.Warrior)
        {
            anim.SetBool("isAttack4", false);
            anim.SetBool("isAttack3", false);
            anim.SetBool("isAttack2", false);
            anim.SetBool("isAttack", false);
            if (photonView.IsMine)
                comboHit.noOfClicks = 0;
        }
        if (!questStore.MainSuccess)
        {
            questStore.MainQuestSuccess(1);
        }

    }

    //==================================여기서부터 전사스킬=======================================
    public void Block() //방패 치기
    {
        if (!photonView.IsMine)
            return;

        if (!isRush && !isAura && !isJump && !isDodge && !isStun && !isRun &&
             attackdamage.Usable_Skill1)
        {
            StartCoroutine(BlockPlay());
            photonView.RPC("BlockPlay", RpcTarget.Others);
        }
    }
    [PunRPC]
    IEnumerator BlockPlay()
    {
        isCool1 = false;
        attackdamage.Usable_Skill1 = false;
        anim.SetBool("isBlock", true);
        isBlock = true;
        isDamage = true;
        yield return new WaitForSeconds(0.3f);
        BoxCollider Skillare = Skillarea2.GetComponent<BoxCollider>(); // 데미지 콜라이더 활성화
        Skillare.enabled = true;
        SoundManager.soundManager.WarriorShieldSound();
        ArrowSkill arrow = Skillarea2.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow.damage = attackdamage.Skill_1_Damamge();
        yield return new WaitForSeconds(0.2f);
        Skillare.enabled = false;

        yield return new WaitForSeconds(0.5f);
        isCool1 = true;
        anim.SetBool("isBlock", false);
        anim.SetBool("isAttack4", false);
        anim.SetBool("isAttack3", false);
        anim.SetBool("isAttack2", false);
        anim.SetBool("isAttack", false);
        if (photonView.IsMine)
            comboHit.noOfClicks = 0;
        isBlock = false;
        isDamage = false;
        if (photonView.IsMine)
            attackdamage.Skill_1_Cool();  //방패치기쿨타임

    }
    public void Buff()
    {
        if (!photonView.IsMine)
            return;
        if (!isRush && !isAura && !isJump && !isDodge && !isBlock && !isStun && !isRun &&
            attackdamage.Usable_Buff)
        {
            BuffPlay();
            photonView.RPC("BuffPlay", RpcTarget.Others);
        }
    }
    [PunRPC]
    void BuffPlay()
    {
        SoundManager.soundManager.WarriorBuffSound();
        if (photonView.IsMine)
            attackdamage.Skill_Buff_Cool();
        BuffEff.Play();

    }
    public void Rush()
    {
        if (!photonView.IsMine)
            return;

        if (!isYes && !isJump && !isDodge && !isBlock && !isAura && !isStun && !isRun &&
            attackdamage.Usable_Skill2)
        {
            StartCoroutine(RushPlay());
            photonView.RPC("RushPlay", RpcTarget.Others);
        }
    }
    [PunRPC]
    IEnumerator RushPlay()
    {
        isCool2 = false;
        attackdamage.Usable_Skill2 = false;

        isRush = true;
        isFall = true;
        anim.SetBool("isRush", true);
        //yield return new WaitForSeconds(0.5f);
        SoundManager.soundManager.WarriorRushVoice();
        if (photonView.IsMine)
            rigid.AddForce(transform.forward * 40 + transform.up * 20, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        SoundManager.soundManager.WarriorRushSound();
        BoxCollider Skillare = Skillarea.GetComponent<BoxCollider>(); //돌진착지지점 데미지 콜라이더 활성화
        Skillare.enabled = true;
        ArrowSkill arrow = Skillarea.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow.damage = attackdamage.Skill_2_Damamge();
        BoxCollider CCare = CCarea.GetComponent<BoxCollider>(); //돌진착지지점 cc기 콜라이더 활성화
        CCare.enabled = true;
        RushEff.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        CCare.enabled = false;
        Skillare.enabled = false;
        anim.SetBool("isRush", false);
        anim.SetBool("isAttack4", false);
        anim.SetBool("isAttack3", false);
        anim.SetBool("isAttack2", false);
        anim.SetBool("isAttack", false);
        if (photonView.IsMine)
            comboHit.noOfClicks = 0;
        isRush = false;
        isFall = false;
        isCool2 = true;
        if (photonView.IsMine)
            attackdamage.Skill_2_Cool();  //돌진쿨타임
        yield return new WaitForSeconds(0.5f);

        RushEff.SetActive(false);

    }
    public void Aura()
    {
        if (!photonView.IsMine)
            return;
        if (!isRun && !isJump && !isDodge && !isBlock && !isRush && !isStun &&
            attackdamage.Usable_Skill3)
        {
            StartCoroutine(AuraPlay());
            photonView.RPC("AuraPlay", RpcTarget.Others);
        }
    }
    [PunRPC]
    IEnumerator AuraPlay()
    {

        isCool3 = false;
        attackdamage.Usable_Skill3 = false;
        isAura = true;
        //AuraTimePrev = Time.time;

        anim.SetBool("isAura", true);
        SoundManager.soundManager.WarriorRushVoice();
        yield return new WaitForSeconds(0.7f);
        SoundManager.soundManager.WarriorAuraSound();
        GameObject swordaura = Instantiate(SwordAura, Aurapos.position, Aurapos.rotation);
        Rigidbody aurarigid = swordaura.GetComponent<Rigidbody>();
        aurarigid.velocity = Aurapos.forward * 20;
        ArrowSkill arrow = swordaura.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow.damage = attackdamage.Skill_3_Damamge();

        Destroy(swordaura, 1f);
        isCool3 = true;
        if (photonView.IsMine)
            attackdamage.Skill_3_Cool();
        yield return new WaitForSeconds(0.8f);
        isAura = false;
        anim.SetBool("isAura", false);
        anim.SetBool("isAttack4", false);
        anim.SetBool("isAttack3", false);
        anim.SetBool("isAttack2", false);
        anim.SetBool("isAttack", false);
        if (photonView.IsMine)
            comboHit.noOfClicks = 0;

    }
    //==================================여기서부터 궁수스킬=======================================
    public void Smoke() //마우스 우클릭 연막
    {
        if (!photonView.IsMine)
            return;

        if (!isJump && !isDodge && !isStun && !isRun && attackdamage.Usable_Skill1)
        {
            StartCoroutine(SmokePlay());
            photonView.RPC("SmokePlay", RpcTarget.Others);
        }
    }
    [PunRPC]
    IEnumerator SmokePlay()
    {
        isCool1 = true;
        if (photonView.IsMine)
            attackdamage.Skill_1_Cool();
        if (photonView.IsMine)
            gameObject.layer = LayerMask.NameToLayer("Back");
        isFall = true;
        isBackStep = true;
        if (photonView.IsMine)
            rigid.AddForce(transform.forward * -23 + transform.up * 10, ForceMode.Impulse);
        GameObject arceff = Instantiate(BackStepEff, BackStepPos.position, BackStepPos.rotation); //이펙트
        SoundManager.soundManager.ArcherBackStepSound();
        BoxCollider Skillare = Skillarea.GetComponent<BoxCollider>(); // 데미지 콜라이더 활성화
        Skillare.enabled = true;
        BoxCollider CCare = CCarea.GetComponent<BoxCollider>(); // cc기 콜라이더 활성화
        CCare.enabled = true;
        ArrowSkill arrow = Skillare.GetComponent<ArrowSkill>();
        arrow.damage = attackdamage.Skill_1_Damamge();
        Destroy(arceff, 2f);
        anim.SetBool("isSmoke", true);

        yield return new WaitForSeconds(0.2f);
        Skillare.enabled = false;
        CCare.enabled = false;
        yield return new WaitForSeconds(0.2f);
        if (photonView.IsMine)
            gameObject.layer = LayerMask.NameToLayer("Player");
        anim.SetBool("isSmoke", false);
        isBackStep = false;
        isFall = false;
        yield return new WaitForSeconds(0.5f);
    }
    public void PoisonArrow()
    {
        if (!photonView.IsMine)
            return;

        if (!isStun && !isRun && attackdamage.Usable_Buff)
        {
            PoisonPlay();
            photonView.RPC("PoisonPlay", RpcTarget.Others);
        }
    }

    [PunRPC]
    void PoisonPlay()
    {

        SoundManager.soundManager.ArcherSkill1Sound();
        if (photonView.IsMine)
            attackdamage.Skill_Buff_Cool();

    }
    //==================================여기서부터 마법사스킬=======================================
    public void Flash()
    {
        if (!photonView.IsMine)
            return;

        if (!isDodge && !isJump && !isRun && !isStun && !weapons.isLightning && !weapons.isIceage && !weapons.isMeteo &&
            attackdamage.Usable_Teleport)
        {
            StartCoroutine(FlashStart());
            photonView.RPC("FlashStart", RpcTarget.Others);
        }
    }
    [PunRPC]
    IEnumerator FlashStart()
    {

        SoundManager.soundManager.MageTeleportSound();
        if (photonView.IsMine)
            attackdamage.Skill_Mage_Teleport_Cool();
        isCoolTeleport = false;//쿨타임
        attackdamage.Usable_Teleport = false;

        if (photonView.IsMine)
            gameObject.layer = LayerMask.NameToLayer("Back");
        isFlash = true;
        isFall = true;
        mesh.enabled = false;
        for (int i = 0; i < smesh.Length; i++)
        {
            smesh[i].enabled = false;
        }
        FlashEff.SetActive(true);
        if (photonView.IsMine)
            rigid.AddForce(transform.forward * 40 + transform.up * 10, ForceMode.Impulse);
        yield return new WaitForSeconds(0.3f);
        if (photonView.IsMine)
            gameObject.layer = LayerMask.NameToLayer("Player");
        isFlash = false;
        isFall = false;
        FlashEff.SetActive(false);
        mesh.enabled = true;
        for (int i = 0; i < smesh.Length; i++)
        {
            smesh[i].enabled = true;
        }
        yield return new WaitForSeconds(0.2f);
        if (photonView.IsMine)
            rigid.velocity = Vector3.zero;

        isCoolTeleport = true;//쿨타임
        if (photonView.IsMine)
            attackdamage.Skill_Mage_Teleport_Cool();

    }
    void InputManager()
    {
        if (!photonView.IsMine)
            return;

        h = Input.GetAxisRaw("Horizontal");    //X좌표 입력받기
        v = Input.GetAxisRaw("Vertical"); //Z좌표 입력받기

        fDown = Input.GetButtonDown("Fire1"); //마우스1번키 입력
        fDowning = Input.GetButton("Fire1");
        fUp = Input.GetButtonUp("Fire1");
        f2Down = Input.GetButtonDown("Fire2"); //마우스 2번키입력
        sDown = Input.GetButtonDown("Jump"); //점프사용 스페이스바
        Rdown = Input.GetButton("Run"); //달리기  알트키 
        Ddown = Input.GetButtonDown("Dodge"); //구르기 쉬프트키
        Key1 = Input.GetButtonDown("Key1"); //1번키
        Key2 = Input.GetButtonDown("Key2"); //2번키
        Key3 = Input.GetButtonDown("Key3"); //3번키
    }


    private void Update()
    {
        if (this != null)
        {
            NicknameSerching(); //다른플레이어들 닉네임갱신
            photonView.RPC("NicknameSerching", RpcTarget.OthersBuffered);
        }

        if (!photonView.IsMine)
            return;




        if (Input.GetKeyDown(KeyCode.H) && !HorseMode) //말 아이템이 없어서 이걸로 테스트했어요
        {
            HorseRide();
        }

        if (AllUI.isUI)
        {
            if (FootSound.footSound)
                FootSound.footSound.audioSource.Stop();
            anim.SetBool("forword", false);
            anim.SetBool("back", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
            if (CharacterType == Type.Warrior)
            {
                anim.SetFloat("SpeedX", 0f);
                anim.SetFloat("SpeedY", 0f);
            }

        }

        if (AllUI.isUI || HorseMode || isDie)
            return;
        if (!NPC.isNPCRange)
            Cursor.lockState = CursorLockMode.Locked;//마우스커서 고정

        if (attackdamage.Duration_Buff && CharacterType == Type.Warrior)  //전사 폭주상태면 공격 애니메이션 속도증가
        {
            anim.SetFloat("Attack1Speed", 1.5f);
            anim.SetFloat("Attack2Speed", 1.5f);
            anim.SetFloat("Attack3Speed", 1.5f);
            anim.SetFloat("Attack4Speed", 1.5f);
        }
        else if (!attackdamage.Duration_Buff && CharacterType == Type.Warrior)
        {
            anim.SetFloat("Attack1Speed", 1f);
            anim.SetFloat("Attack2Speed", 1f);
            anim.SetFloat("Attack3Speed", 1f);
            anim.SetFloat("Attack4Speed", 1f);
        }

        if (attackdamage.Duration_Buff && CharacterType == Type.Warrior)
        {
            weapons.rate = 0.45f;
        }
        else if (!attackdamage.Duration_Buff && CharacterType == Type.Warrior)
        {
            weapons.rate = 0.6f;
        }

        if (fDowning && CharacterType == Type.Archer) //화살땡길때 이속감소
            speed = 2.5f;
        else if (!fDowning && CharacterType == Type.Archer)
            speed = 6f;

        //말 관련//
        //if (!HorseMode)
        //{
        //    transform.parent = null;
        //}
        if (HorseMode) //말탔을때 플레이어의 프리즈포지션 체크
        {
            rigid.constraints = RigidbodyConstraints.FreezePositionX;
            rigid.constraints = RigidbodyConstraints.FreezePositionY;
            rigid.constraints = RigidbodyConstraints.FreezePositionZ;
        }




        //Debug.Log("shield");
        InputManager(); //입력매니저
        Anima(); //애니메이션
        Attack(); //근접 공격
        Jump(); //점프
        //Dodge(); //구르기
        WarriorMove(); //전사이동제한
        ArcherMove(); //궁수이동제한
        MageMove(); //마법사이동제한

        //SkillClass(); //스킬직업제한
    }
    public void HorseRide()
    {

        if (!HorseMode) // 말 소환&소환해제
        {
            //if (horsecount == 0)
            //{
            //    SoundManager.soundManager.Horse();
            //    HorseSpawn.gameObject.SetActive(false);
            //    HorseSpawn.gameObject.SetActive(true);
            //    HorseSpawn.gameObject.transform.position = horsepos1.position;
            //    HorseSpawn.gameObject.transform.DOMove(horsepos2.position, 1.5f).SetEase(Ease.Linear);
            //    HorseSpawn.transform.parent = null;
            //    horsecount = 1;
            //}
            if (horsecount == 0) //소환 
            {
                HorseSpawn.GetComponent<Rigidbody>().velocity = Vector3.zero;
                HorseSpawn.transform.parent = null;
                SoundManager.soundManager.Horse();
                HorseSpawn.transform.position = horsepos1.position;
                HorseSpawn.transform.DOMove(horsepos2.position, 1.5f).SetEase(Ease.Linear);
                horsecount = 1;
            }
            else if (horsecount == 1) //소환 해제
            {
                SoundManager.soundManager.Horse2();
                HorseSpawn.transform.position = GameManager.gameManager.HorsePoint.position;
                //HorseSpawn.transform.parent = transform;
                horsecount = 0;
            }
        }

    }

    //void SkillClass()
    //{
    //    if (CharacterType == Type.Warrior)
    //    {
    //        Block(); //방패치기
    //        Buff(); //폭주스킬
    //        Rush(); //돌진스킬
    //        Aura(); //검기스킬
    //    }
    //    else if (CharacterType == Type.Archer)
    //    {
    //        Smoke(); //섬광탄백스텝
    //        PoisonArrow(); //독화살버프
    //    }
    //    else if (CharacterType == Type.Mage)
    //    {
    //        Flash(); //점멸
    //    }
    //}
    void MageMove()
    {


        if (!isStun && !weapons.isIceage && !weapons.isMeteo
            && !weapons.isEnergyReady && CharacterType == Type.Mage && !NoMove)
        {
            if (isDodge)
                moveVec = dodgeVec;
            moveVec = (Vector3.forward * v) + (Vector3.right * h);

            if (Rdown)
            {
                isRun = true;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed * 1.4f, Space.Self);
            }
            else
            {
                isRun = false;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed, Space.Self);
            }
        }
    }
    void ArcherMove()
    {
        if (!isStun && !isBackStep && !weapons.isEnergyReady && CharacterType == Type.Archer && !NoMove)
        {
            if (isDodge)
                moveVec = dodgeVec;
            moveVec = (Vector3.forward * v) + (Vector3.right * h);
            if (Rdown)
            {
                isRun = true;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed * 1.4f, Space.Self);
            }
            else
            {
                isRun = false;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed, Space.Self);
            }
        }
    }
    void WarriorMove()
    {
        if (!isStun && !isBlock && !isRush && !isAura && !isBackStep && CharacterType == Type.Warrior && !NoMove)
        {
            if (isDodge) //회피중이면 다른방향으로 전환이 느리게
                moveVec = dodgeVec;

            moveVec = (Vector3.forward * v) + (Vector3.right * h); //전 후진과 좌우 이동값 저장
            anim.SetFloat("SpeedX", h, 0.1f, Time.deltaTime);
            anim.SetFloat("SpeedY", v, 0.1f, Time.deltaTime);
            if (Rdown)//달리는 중이면 1.4배 이속증가
            {
                isRun = true;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed * 1.4f, Space.Self);//이동 처리를 편하게 하게해줌
            }
            else
            {
                isRun = false;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed, Space.Self);
            }
        }
    }
    void StopMove() //벽뚫기방지 벽앞에가면 멈춤
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        NoMove = Physics.Raycast(transform.position, transform.forward, 1.5f, LayerMask.GetMask("WALL"));
    }
    void FreezeVelocity()  //카메라 버그 안생기게하는거
    {
        if (!isJump)
        {

            rigid.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        //attackdamage.SkillPassedTimeFucn(); (weapons스크립트의 117줄, attackDamage의125줄 , playerST의 553줄에 총 3개가 있어 쿨타임이 3배로 돌아갑니다. 그래서 2개 주석처리 해놨습니다. 문제시에 알려주세요)
        FreezeVelocity();
        StopMove();
    }
    public void PlayerDie() //사망
    {

        if (!photonView.IsMine)
            return;
        SelectPlayer.enabled = false;
        rigid.useGravity = false;
        DunjeonBossArena = false;
        DiePRC(true, true); //죽음을 알림.
        anim.SetBool("isDie", true);
        if (CharacterType == Type.Warrior || CharacterType == Type.Mage)
            SoundManager.soundManager.MaleDieSound();
        else if (CharacterType == Type.Archer)
            SoundManager.soundManager.FeMaleDieSound();
        dieui.DieOn();
    }

    [PunRPC]
    public void DiePRC(bool _isDie, bool local)
    {
        if (local)
        {
            photonView.RPC("DiePRC", RpcTarget.Others, _isDie, false);
        }
        isDie = _isDie;
    }

    public void PlayerResurrection() //죽고살아날때 갱신
    {
        playerstat._Hp = playerstat._MAXHP;
        playerstat._Mp = playerstat._MAXMP;
    }


    void OnTriggerEnter(Collider other) //충돌감지
    {
        if (photonView.IsMine)
        {
            if (other.tag == "DeathZone")
            {
                SendMessage("TownResurrection");
            }

            if (other.tag == "DeathZoneDun")
            {
                SendMessage("DunjeonResurrection");
            }

            if (other.name == "Boss2Arena")
            {
                DunjeonBossArena = true;
                Debug.Log("여기????");
            }
            if (other.name == "Boss2ArenaOut")
            {
                DunjeonBossArena = false;
                Debug.Log("아웃");
            }

            if (other.gameObject.tag == "EnemyRange")  //적에게 맞았다면
            {
                if (other.gameObject.GetComponent<Attacking>().isAttacking && !isDamage)
                {
                    EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                    playerstat.DamagedHp(enemyRange.damage);
                    healthbar.fillAmount = playerstat._Hp / playerstat._MAXHP;
                    other.gameObject.GetComponent<Attacking>().isAttacking = false;
                    if (playerstat._Hp <= 0)
                    {
                        PlayerDie();
                    }
                }
            }
            else if (other.gameObject.tag == "Boss1Skill")  //1보스 스턴스킬
            {
                if (!isDamage) //무적타이밍이 아닐때만 실행
                {

                    EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                    playerstat.DamagedHp(enemyRange.damage);
                    healthbar.fillAmount = playerstat._Hp / playerstat._MAXHP;
                    StartCoroutine(OnDamageNuck());
                    if (playerstat._Hp <= 0)
                    {
                        PlayerDie();
                    }
                }
            }
            else if (other.gameObject.tag == "Boss2Skill")  //최종보스 1.5초스턴스킬
            {
                if (!isDamage) //무적타이밍이 아닐때만 실행
                {

                    EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                    playerstat.DamagedHp(enemyRange.damage);
                    healthbar.fillAmount = playerstat._Hp / playerstat._MAXHP;
                    StartCoroutine(OnDamageNuck2());
                    if (playerstat._Hp <= 0)
                    {
                        PlayerDie();
                    }
                }

            }
            else if (other.gameObject.tag == "Boss3Skill")  //최종보스 5초스턴스킬
            {
                if (!isDamage) //무적타이밍이 아닐때만 실행
                {
                    EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                    playerstat.DamagedHp(enemyRange.damage);
                    healthbar.fillAmount = playerstat._Hp / playerstat._MAXHP;
                    StartCoroutine(OnDamageNuck3());
                    if (playerstat._Hp <= 0)
                    {
                        PlayerDie();
                    }
                }
            }

            if (other.tag == "DunjeonPortal")
            {
                gameObject.transform.position = Portal.portal.WorldPortal.position + Portal.portal.WorldPortal.transform.forward * -2;
            }

            if (other.tag == "WorldPortal")
            {
                gameObject.transform.position = Portal.portal.DunjeonPortal.position + Portal.portal.DunjeonPortal.transform.forward * -2;
            }

            if (other.tag == "BossPortalIN")
            {
                gameObject.transform.position = Portal.portal.BossPortalOUT.position + Portal.portal.BossPortalOUT.transform.forward * 2;
            }

            if (other.tag == "BossPortalOUT")
            {
                gameObject.transform.position = Portal.portal.BossPortalIN.position + Portal.portal.BossPortalIN.transform.forward * -2;
            }



            if (other.tag == "BGM1")   //마을입장
            {
                // BGM.bgm.audioSource.Stop();
                bgm.audioSource.clip = Sounds.sounds.TownBGM;
                //  BGM.bgm.audioSource.Play();
            }
            if (other.tag == "BGM2")  //슬라임사냥터 입장
            {
                //  BGM.bgm.audioSource.Stop();
                bgm.audioSource.clip = Sounds.sounds.BeginnerGroundBGM;
                // BGM.bgm.audioSource.Play();
            }
            if (other.tag == "BGM3") //고블린마을 입장
            {
                //  BGM.bgm.audioSource.Stop();
                bgm.audioSource.clip = Sounds.sounds.MiddleGroundBGM;
                // BGM.bgm.audioSource.Play();
            }
            if (other.tag == "BGM4") //던전입장
            {
                // BGM.bgm.audioSource.Stop();
                bgm.audioSource.clip = Sounds.sounds.DunjeonBGM;
                // BGM.bgm.audioSource.Play();
            }
            if (other.tag == "BGM5") //골렘존입장
            {
                bgm.audioSource.clip = Sounds.sounds.BossBGM;
            }

        }

    }



    private void OnTriggerStay(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.tag == "Horse")
            {
                if (Input.GetKeyDown(KeyCode.C)) //말탑승
                {
                    HorseMode = true;
                    rigid.constraints = RigidbodyConstraints.FreezeAll;

                    anim.SetBool("isHorse", true);
                    rigid.useGravity = false;
                    transform.parent = Horsee.transform;
                    transform.position = Horsee.transform.position + Vector3.up * -0.5f;
                    transform.rotation = HorseSpawn.transform.rotation;
                    //Quaternion.LookRotation(new Vector3(h, 0f, v));
                }
            }

            if (other.tag == "BOSS2_RAZOR")
            {
                if (!isDamage)
                {
                    Debug.Log("응애");
                    EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                    playerstat.DamagedHp(enemyRange.damage);
                    healthbar.fillAmount = playerstat._Hp / playerstat._MAXHP;
                    if (playerstat._Hp <= 0)
                    {
                        PlayerDie();
                    }
                }
            }
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine)
        {


        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "GROUND") //이 태그가 붙은곳에서만 점프착지가능
            {
                anim.SetBool("isJump", false);
                isJump = false;
            }
            //if (collision.gameObject.tag == "WALL")
            //{
            //    isYes = true; //벽쪽에서 돌진스킬못쓰게
            //}
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //if (photonView.IsMine)
        //{
        //    if (collision.gameObject.tag == "WALL")
        //    {
        //        isYes = false;
        //    }
        //}
    }



    IEnumerator OnDamageNuck() //무적타임
    {
        anim.SetBool("isStun", true);
        isStun = true;

        yield return new WaitForSeconds(3f);
        anim.SetBool("isStun", false);
        isStun = false;

    }
    IEnumerator OnDamageNuck2() //무적타임
    {
        anim.SetBool("isStun", true);
        isStun = true;

        yield return new WaitForSeconds(1.5f);
        anim.SetBool("isStun", false);
        isStun = false;

    }
    IEnumerator OnDamageNuck3() //무적타임
    {
        anim.SetBool("isStun", true);
        isStun = true;

        yield return new WaitForSeconds(5f);
        anim.SetBool("isStun", false);
        isStun = false;

    }

    [PunRPC]
    public void WeaponChange(Item.SwordNames WeaponNum) //무기를 바꿨을때 캐릭터에 적용시키기 위해 사용하는 함수
    {
        if (WeaponNum != basicSword)
        {
            if (!questStore.MainSuccess)
            {
                questStore.MainQuestSuccess(2);
            }
        }
        equipWeapon[NowWeapon].gameObject.SetActive(false);
        NowWeapon = (int)WeaponNum;
        equipWeapon[NowWeapon].gameObject.SetActive(true);
        if (CharacterType == Type.Mage)
        {
            mesh = GetComponentInChildren<Weapons>().GetComponent<MeshRenderer>();
        }
        //QuikSlot.quikSlot.weapons = FindObjectOfType<Weapons>();
    }
}
