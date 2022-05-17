using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerST : MonoBehaviour
{
    public enum Type { Warrior, Archer, Mage };
    public Type CharacterType; //원래 앞에 static이 붙어있었는데 테스트할때 인스펙터창에 타입이 안떠서 임시로 뻈어요
    Transform _transform;
    Rigidbody rigid;

    public float jump = 5.0f; //점프력
    public float speed = 5.0f;  //플레이어 이동속도
    public GameObject cam; //플레이어 카메라
    public CapsuleCollider SelectPlayer; //제어할 플레이어
    public Animator anim; //애니메이션
    public Weapons[] equipWeapon;    //현재 무기. 나중에 배열로 여러무기를 등록하려고함
    public int NowWeapon; //현재 무기
    public enum SwordNames { Sword1, Sword5_normal, Sword5_rare, Sword10_normal, Sword10_rare, None }; //무기이름 위의 배열의 순서에 따라.
    public SwordNames basicSword = 0;

    public float bowMinPower = 0.2f;
    public float bowPower; // 화살 충전 데미지
    public float bowChargingTime = 1.0f; //화살 최대 충전시간
    public bool isSootReady = true;


    float h; //X값 좌표
    float v; //Z값 좌표
    float fireDelay; //공격속도 계산용

    private bool fDown; //마우스 왼쪽버튼을 눌렀다면 true
    private bool fDowning; //마우스 왼쪽버튼을 눌르고 있다면 true
    private bool fUp;
    private bool f2Down; //마우스 우클 눌렀다면
    public bool isFireReady = true;  //무기 rate가 fireDelay보다 작다면 true로 공격가능상태
    public bool isDamage; //무적타임. 연속으로 다다닥 맞을수있기때문에
    private bool sDown; //점프입력
    private bool Rdown;//알트입력
    private bool Ddown; //쉬프트입력 
    private bool Key1; //키보드 1번입력
    private bool Key2; //키보드 2번입력
    private bool Key3; //키보드 3번입력
    public bool ImWar; //나는 워리어다

    public static bool isJump; //현재 점프중?
    public bool archerattack = false; //현재 궁수공격중
    public static bool isStun; //현재 스턴상태
    public bool isRun; //현재 달리는상태?

    Vector3 moveVec;
    Vector3 dodgeVec;

    public Weapons weapons;
    public AttackDamage attackdamage;
    public PlayerStat playerstat;
    public GameObject Skillarea; //켜지면 데미지만
    public GameObject Skillarea2; //켜지면 데미지만
    public GameObject CCarea;  //켜지면 CC기 


    public static bool isFall; //공중에 떠있는상태? 몬스터들의 룩엣을 조정하기위함.
    //======================전사 스킬========================//

    public bool isDodge; //현재 회피중?
    private bool isBlock; //현재 방패치기중? ㅣ마우스 우클릭
    public static bool isBuff; //현재 폭주상태? ㅣ키보드 1번
    private bool isRush; //현재 돌진상태? ㅣ키보드 2번
    private bool isAura; //현재 검기날리는 상태? ㅣ 키보드 3번
    private bool isYes; //돌진 벽에서 쓰는행위 막는용도

    

    [Header("전사 관련")]
    public GameObject BuffEff;
    public GameObject RushEff;
    public Transform Aurapos;
    public GameObject SwordAura; //검기스킬 투사체

    //======================궁수 스킬========================// 
    [Header("궁수 관련")]
    public GameObject BackStepEff;
    public Transform BackStepPos;

    public bool isBackStep; //현재 백스텝상태?
    public static bool isPoison; //현재 독화살버프상태?  나중에 몬스터 충돌판정에서 if걸고 추가데미지를 줄예정

    //======================마법사 스킬========================// 
    [Header("마법사 관련")]
    public MeshRenderer mesh;           //순간이동때 투명화
    public SkinnedMeshRenderer smesh;   //순간이동때 투명화
    public GameObject FlashEff;  //순간이동이펙트
    public bool isFlash; //현재 순간이동중?


    //쿨타임 돌아가게 하기
    public static bool isCool1;
    public static bool isCool2;
    public static bool isCool3;
    public static bool isCool4;
    public static bool isCooldodge;
    public static bool isCoolTeleport;

    void Start()
    {
        bowPower = bowMinPower;
        _transform = GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Anima() //애니메이션 
    {
        anim.SetBool("isRun", Rdown);
        if (v >= 0.1f) //앞
        {
            anim.SetBool("forword", true);
            anim.SetBool("back", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
        else if (v <= -0.1f) //뒤
        {
            anim.SetBool("back", true);
            anim.SetBool("forword", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
        else if (h >= 0.1f) //오른쪽
        {
            anim.SetBool("right", true);
            anim.SetBool("forword", false);
            anim.SetBool("back", false);
            anim.SetBool("left", false);
        }
        else if (h <= -0.1f) //왼쪽
        {
            anim.SetBool("left", true);
            anim.SetBool("forword", false);
            anim.SetBool("back", false);
            anim.SetBool("right", false);
        }
        else
        {
            anim.SetBool("forword", false);
            anim.SetBool("back", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
    }

    void Attack()   //공격
    {
        if (CharacterType == Type.Warrior&& !isDodge && !isFlash && !weapons.isLightning &&
           !weapons.isIceage && !Weapons.isMeteo && !isJump && !isRun && !isBlock && !isRush && !isAura && !isStun)
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
        else if (CharacterType == Type.Mage&& !isDodge && !isFlash && !weapons.isLightning &&
           !weapons.isIceage && !Weapons.isMeteo && !isJump && !isRun && !isBlock && !isRush && !isAura && !isStun)
        {
            fireDelay += Time.deltaTime;     //공격속도 계산
            isFireReady = equipWeapon[NowWeapon].rate < fireDelay;  //공격 가능 타임

            if (fDown)
            {
                if (isFireReady)  //공격할수있을때
                {
                    equipWeapon[NowWeapon].Use();
                    fireDelay = 0;
                }
            }
        }

        else if (CharacterType == Type.Archer && !isDodge && !isJump && !isRun && !isStun && !isBackStep && !weapons.isEnergyReady)
        {
            fireDelay += Time.deltaTime;

            if (fDowning && bowPower < bowChargingTime)
            {

                bowPower += Time.deltaTime;
            }

            if (fDowning && isFireReady && equipWeapon[NowWeapon].rate < fireDelay)
            {
                archerattack = true;
                bowPower = bowMinPower;
                anim.SetTrigger("doSwing");
                isSootReady = false;
                isFireReady = false;
                fireDelay = 0f;
            }
            else if (fUp && !isSootReady)
            {
                archerattack = true;
                anim.SetBool("doShot", true);

                equipWeapon[NowWeapon].Use();
            }
        }
    }
    void Jump()
    {
        if (sDown && !isJump && !isDodge && !isBlock && !isRush && !isAura && !isBackStep && !weapons.isEnergyReady &&
            !weapons.isLightning && !weapons.isIceage && !Weapons.isMeteo && !isFlash && !isStun
            )
        {
            rigid.AddForce(Vector3.up * jump, ForceMode.Impulse); //애드포스 : 힘을주다/ 포스모드,임펄스 : 즉발적
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    public void Dodge()
    {
        if ( !isStun && !isJump && !isBlock && !isBackStep && !weapons.isEnergyReady && !isRush && !isAura && !isFlash &&
           !weapons.isLightning && !weapons.isIceage && !Weapons.isMeteo && attackdamage.Usable_Dodge)
        {
            isCooldodge = false;
            attackdamage.Usable_Dodge = false;
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;
            isDamage = true;

            Invoke("DodgeOut", 0.4f); //구르기를 하면 0.4초후에 이동속도가 정상으로돌아옴
           
        }
    }

    void DodgeOut()
    {
        isCooldodge = true;
        attackdamage.Skill_Dodge_Cool();
        speed *= 0.5f;
        isDodge = false;
        isDamage = false;
    }

    //==================================여기서부터 전사스킬=======================================
    public void Block() //방패 치기
    {
        if (!isRush && !isAura && !isJump && !isDodge && !isStun && !isRun &&
             attackdamage.Usable_Skill1)
        {
            StartCoroutine(BlockPlay());
        }
    }
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
        ArrowSkill arrow = Skillarea2.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow.damage = attackdamage.Skill_1_Damamge();
        yield return new WaitForSeconds(0.2f);
        Skillare.enabled = false;
        yield return new WaitForSeconds(0.5f);
        isCool1 = true;
        anim.SetBool("isBlock", false);
        isBlock = false;
        isDamage = false;
      
        attackdamage.Skill_1_Cool();  //방패치기쿨타임
    }
    public void Buff()
    {
        if (!isRush && !isAura && !isJump && !isDodge && !isBlock && !isStun && !isRun &&
            attackdamage.Usable_Buff)
        {
            attackdamage.Skill_Buff_Cool();
            BuffEff.SetActive(true);
        }
    }
    public void Rush()
    {
        if (!isYes && !isJump && !isDodge && !isBlock && !isAura && !isStun && !isRun &&
            attackdamage.Usable_Skill2)
        {
            
            StartCoroutine(RushPlay());
        }
    }
    IEnumerator RushPlay()
    {
        isCool2 = false;
        attackdamage.Usable_Skill2 = false;

        isRush = true;
        isFall = true;
        anim.SetBool("isRush", true);
        yield return new WaitForSeconds(0.5f);
        rigid.AddForce(transform.forward * 40 + transform.up * 20, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
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
        isRush = false;
        isFall = false;
        isCool2 = true;
        attackdamage.Skill_2_Cool();  //돌진쿨타임
        yield return new WaitForSeconds(0.5f);


        RushEff.SetActive(false);

    }
    public void Aura()
    {
        if (!isRun && !isJump && !isDodge && !isBlock && !isRush && !isStun &&
            attackdamage.Usable_Skill3)
        {
            
            StartCoroutine(AuraPlay());
        }
    }
    IEnumerator AuraPlay()
    {
        isCool3 = false;
        attackdamage.Usable_Skill3 = false;
        isAura = true;
        //AuraTimePrev = Time.time;
        
        anim.SetBool("isAura", true);
        yield return new WaitForSeconds(0.7f);

        GameObject swordaura = Instantiate(SwordAura, Aurapos.position, Aurapos.rotation);
        Rigidbody aurarigid = swordaura.GetComponent<Rigidbody>();
        aurarigid.velocity = Aurapos.forward * 20;
        ArrowSkill arrow = swordaura.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow.damage = attackdamage.Skill_3_Damamge();

        Destroy(swordaura, 1f);
        isCool3 = true;
        attackdamage.Skill_3_Cool();
        yield return new WaitForSeconds(0.8f);
        isAura = false;
        anim.SetBool("isAura", false);
    }
    //==================================여기서부터 궁수스킬=======================================
    public void Smoke() //마우스 우클릭 연막
    {
        if ( !isJump && !isDodge && !isStun && !isRun && attackdamage.Usable_Skill1)
        {
            StartCoroutine(SmokePlay());
        }
    }
    IEnumerator SmokePlay()
    {
        isCool1 = true;
        attackdamage.Skill_1_Cool();
        gameObject.layer = LayerMask.NameToLayer("Back");
        isFall = true;
        isBackStep = true;
        rigid.AddForce(transform.forward * -23 + transform.up * 10, ForceMode.Impulse);
        GameObject arceff = Instantiate(BackStepEff, BackStepPos.position, BackStepPos.rotation); //이펙트
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

        gameObject.layer = LayerMask.NameToLayer("Player");
        anim.SetBool("isSmoke", false);
        isBackStep = false;
        isFall = false;
    }
    public void PoisonArrow()
    {
        if (!isStun && !isRun && attackdamage.Usable_Buff)
        {
            attackdamage.Skill_Buff_Cool();
        }
    }
    //==================================여기서부터 마법사스킬=======================================
    public void Flash()
    {
        if (!isDodge && !isJump && !isRun && !isStun && !weapons.isLightning && !weapons.isIceage && !Weapons.isMeteo &&
            attackdamage.Usable_Teleport)
        {
            StartCoroutine(FlashStart());
        }
    }
    IEnumerator FlashStart()
    {
        isCoolTeleport = false;//쿨타임
        attackdamage.Usable_Teleport = false;
        
       
        gameObject.layer = LayerMask.NameToLayer("Back");
        isFlash = true;
        isFall = true;
        mesh.enabled = false;
        smesh.enabled = false;
        FlashEff.SetActive(true);
        rigid.AddForce(transform.forward * 40 + transform.up * 10, ForceMode.Impulse);
        yield return new WaitForSeconds(0.3f);
        gameObject.layer = LayerMask.NameToLayer("Player");
        isFlash = false;
        isFall = false;
        FlashEff.SetActive(false);
        mesh.enabled = true;
        smesh.enabled = true;

        isCoolTeleport = true;//쿨타임
        attackdamage.Skill_Mage_Teleport_Cool();
       
    }
    void InputManager()
    {
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
        ImWar = CharacterType == Type.Warrior;
        if (inventory.iDown || SkillWindow.kDown)
            return;
        if (!NPC.isNPCRange)
            Cursor.lockState = CursorLockMode.Locked;//마우스커서 고정

        if (attackdamage.Duration_Buff && CharacterType == Type.Warrior)  //전사 폭주상태면 공격 애니메이션 속도증가
        {
            anim.SetFloat("Attack1Speed", 1.5f);
            anim.SetFloat("Attack2Speed", 1.5f);
            anim.SetFloat("Attack3Speed", 1.5f);
        }
        else if (!attackdamage.Duration_Buff && CharacterType == Type.Warrior)
        {
            anim.SetFloat("Attack1Speed", 1f);
            anim.SetFloat("Attack2Speed", 1f);
            anim.SetFloat("Attack3Speed", 1f);
        }

        if (fDowning && CharacterType == Type.Archer) //화살땡길때 이속감소
            speed = 2.5f;
        else if (!fDowning && CharacterType == Type.Archer)
            speed = 6f;

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
        if (!isStun && !weapons.isLightning && !weapons.isIceage && !Weapons.isMeteo
            && !weapons.isEnergyReady && CharacterType == Type.Mage)
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
        if (!isStun && !isBackStep && !weapons.isEnergyReady && CharacterType == Type.Archer)
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
        if (!isStun && !isBlock && !isRush && !isAura && !isBackStep && CharacterType == Type.Warrior)
        {
            if (isDodge) //회피중이면 다른방향으로 전환이 느리게
                moveVec = dodgeVec;
            moveVec = (Vector3.forward * v) + (Vector3.right * h); //전 후진과 좌우 이동값 저장
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
    void FreezeVelocity()  //카메라 버그 안생기게하는거
    {
        if (!isJump)
        {

            rigid.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        //attackdamage.SkillPassedTimeFucn(); (weapons스크립트의 117줄, attackDamage의125줄 , playerST의 553줄에 총 3개가 있어 쿨타임이 3배로 돌아갑니다. 그래서 2개 주석처리 해놨습니다. 문제시에 알려주세요)
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other) //충돌감지
    {
        if (other.gameObject.tag == "EnemyRange")  //적에게 맞았다면
        {
            if (!isDamage) //무적타이밍이 아닐때만 실행
            {
                Debug.Log("아야!");
                EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                PlayerStat.playerstat.DamagedHp(enemyRange.damage);
                StartCoroutine(OnDamage());

            }
        }
        else if (other.gameObject.tag == "Boss1Skill")  //1보스 스턴스킬
        {
            if (!isDamage) //무적타이밍이 아닐때만 실행
            {
                EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                PlayerStat.playerstat.DamagedHp(enemyRange.damage);

                StartCoroutine(OnDamageNuck());
            }
        }
        else if (other.gameObject.tag == "Boss2Skill")  //최종보스 1.5초스턴스킬
        {
            if (!isDamage) //무적타이밍이 아닐때만 실행
            {
                EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                PlayerStat.playerstat.DamagedHp(enemyRange.damage);

                StartCoroutine(OnDamageNuck2());
            }

        }
        else if (other.gameObject.tag == "Boss3Skill")  //최종보스 5초스턴스킬
        {
            if (!isDamage) //무적타이밍이 아닐때만 실행
            {
                EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                PlayerStat.playerstat.DamagedHp(enemyRange.damage);

                StartCoroutine(OnDamageNuck3());
            }

        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            rigid.velocity = Vector3.zero;
            anim.SetBool("isJump", false);
            isJump = false;
        }
        if (collision.gameObject.tag == "WALL")
        {
            isYes = true; //벽쪽에서 돌진스킬못쓰게
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "WALL")
        {
            isYes = false;
        }
    }



    IEnumerator OnDamage() //무적타임
    {
        isDamage = true; //무적타임 true

        yield return new WaitForSeconds(1f);

        isDamage = false;

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

    public void WeaponChange(SwordNames WeaponNum) //무기를 바꿨을때 캐릭터에 적용시키기 위해 사용하는 함수
    {
        equipWeapon[NowWeapon].gameObject.SetActive(false);
        NowWeapon = (int)WeaponNum;
        equipWeapon[NowWeapon].gameObject.SetActive(true);

    }

}
