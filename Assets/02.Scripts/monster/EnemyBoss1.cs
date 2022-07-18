using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using DG.Tweening;
public class EnemyBoss1 : MonsterBoss
{

    public BoxCollider meleeArea; //���� ���ݹ���
    public BoxCollider nuckArea; //���Ͻ�ų 
    public SphereCollider nuckarea;
    public bool isChase; //�������� ����
    public bool isAttack; //���� ������
    public bool isRush;
    public bool isStun;
    public bool isThrow;
    public bool isDie;
    public Transform respawn;

    public LayerMask whatIsTarget; // 공격 대상 레이어


    private Light stunarea;
    Transform target;
    Rigidbody rigid;
    CapsuleCollider boxCollider;
    NavMeshAgent nav;
    Animator anim;
    QuestStore questStore;
    public ParticleSystem Hiteff; //피격이펙
    public ParticleSystem Hiteff2;
    public bool isDamage;

    public bool Patterning; //현재 패턴진행중?
    public bool isSkill; //현재 스킬사용중?

    [SerializeField]
    Attacking attacking;
    float targetRange = 3f; //몬스터 공격사정거리
    public Transform ThrowPoint;
    public Transform ThrowEndPoint;

    [SerializeField]
    ParticleSystem RushEff; //돌진스킬 이펙트
    [SerializeField]
    GameObject ThrowEff;
    [SerializeField]
    SkinnedMeshRenderer mat;

    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (target != null && !target.GetComponent<PlayerST>().isDie)
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }
    void Awake()
    {
        mat = transform.GetChild(7).transform.GetComponent<SkinnedMeshRenderer>();
        stunarea = GetComponentInChildren<Light>();
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        attacking = transform.GetChild(2).GetComponent<Attacking>();
        questStore = FindObjectOfType<QuestStore>();
        RushEff = GameObject.Find("EffectPool").transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    public override void BossHpBarSettting()
    {
        bossHpBar = GameObject.Find("BossMonsterHp").transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        isDamage = false;
        boxCollider.enabled = true;
        isAttack = false;
        nav.isStopped = false;
        isDie = false;
        curHealth = maxHealth;
        isStun = false;
        anim = GetComponent<Animator>();

        StartBossMonster();
        BossItemSet();
        Monstername.text = "이끼 골렘";
        level.text = "5";
        coin = 30;
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        BossMonsterHpBarSet();

        if (PhotonNetwork.IsMasterClient) //호스트에서만 추적
        {
            StartCoroutine(UpdatePath());
        }

    }


    public void BossItemSet()
    {

        item = Resources.LoadAll<GameObject>("DROP/BOSS1");

    }


    void Update()
    {
        if (isDie)
        {
            StopAllCoroutines();
        }
        if (isChase || isAttack) //룩엣
            if (!isDie && !playerST.isJump && !playerST.isFall && !isStun)
                transform.LookAt(target);

    }
    private IEnumerator UpdatePath()
    {

        // 살아있는 동안 무한 루프
        while (!isDie)
        { 
            if (hasTarget)
            {
                // 추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
                Targerting();
                PatternStart();
                nav.SetDestination(target.position);
                
                if(!isThrow)
                nav.speed = 4f;
                if (!isAttack && !isSkill)
                {
                    isChase = true;
                    if (!isDie)
                        nav.isStopped = false;
                    anim.SetBool("isRun", true);
                }
                if (Vector3.Distance(target.position, transform.position) > 40f)
                {
                    EnemyReset();
                    target = null;
                }


            }
            else
            {
                // 추적 대상 없음 : AI 이동 중지
                EnemyReset();

                // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
                // 단, targetLayers에 해당하는 레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders =
                    Physics.OverlapSphere(transform.position, 16f, whatIsTarget);

                // 모든 콜라이더들을 순회하면서, 살아있는 플레이어를 찾기
                for (int i = 0; i < colliders.Length; i++)
                {
                    // 콜라이더로부터 PlayerST 컴포넌트 가져오기
                    PlayerST livingEntity = colliders[i].GetComponent<PlayerST>();

                    // PlayerST 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                    if (livingEntity != null && !livingEntity.isDie)
                    {
                        // 추적 대상을 해당 LivingEntity로 설정
                        target = livingEntity.transform;

                        // for문 루프 즉시 정지
                        break;
                    }
                }
            }

            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }
    void EnemyReset()
    {
        nav.SetDestination(respawn.position);
        RushEff.Stop();
        isChase = false;
        nav.speed = 5f;
        if (!isDie)
            nav.isStopped = false;
        //curHealth = maxHealth;
        if (Vector3.Distance(respawn.position, transform.position) < 1f)
        {
            if (!isDie)
                nav.isStopped = true;
            anim.SetBool("isRun", false);
        }
    }
    void PatternStart()
    {
        if (!Patterning && !playerST.isDie)
        {
            StartCoroutine(Pattern());
            Patterning = true;
        }
    }

    IEnumerator Pattern() //��������
    {
        yield return new WaitForSeconds(6f);
        if (!isDie)
        {
            int ranAction = Random.Range(7, 9);
            switch (ranAction)
            {
                case 4:
                case 5:
                case 6:
                    photonView.RPC("Rush", RpcTarget.All); //돌진스킬
                    MonsterAttack();
                    break;
                case 7:
                case 8:
                case 9:
                    photonView.RPC("RockThrow", RpcTarget.All); //제자리서서 스턴거는스킬
                    MonsterAttack();
                    break;
            }
        }
    }
    void FreezeVelocity() //�̵�����
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targerting()//Ÿ����
    {
        float targetRadius = 1f;
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));  //����ĳ��Ʈ
        if (rayHits.Length > 0 && !isAttack && !isDie && !isSkill) //����ĳ��Ʈ�� �÷��̾ �����ٸ� && ���� �������� �ƴ϶��
        {
            photonView.RPC("Attack", RpcTarget.All);
            MonsterAttack();
        }

    }
    [PunRPC]
    IEnumerator RockThrow()
    {
        nav.speed = 0f;
        isSkill = true;
        isThrow = true;
        isChase = false;
        mat.material.DOColor(Color.red, 2f);
        rigid.velocity = Vector3.zero;
        if (!isDie)
            nav.isStopped = true;
        StopCoroutine(Attack());
        anim.SetBool("isAttack",false);
        anim.SetBool("isThrow", true);
        yield return new WaitForSeconds(2.8f);
        anim.SetBool("isThrowShot", true);
        Collider[] colliders =
                    Physics.OverlapSphere(transform.position, 8f, LayerMask.GetMask("Player"));

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerST livingEntity = colliders[i].GetComponent<PlayerST>();
            if (livingEntity != null && !livingEntity.isDie && !livingEntity.isDamage)
            {
                Instantiate(ThrowEff, livingEntity.transform.position, livingEntity.transform.rotation);
                //photonView.RPC("StunEffStop", RpcTarget.All);
                livingEntity.GetComponent<PlayerStat>().DamagedHp(100);
                livingEntity.GetComponent<PlayerST>().healthbar.fillAmount = livingEntity.GetComponent<PlayerStat>().playerstat._Hp /
                    livingEntity.GetComponent<PlayerStat>().playerstat._MAXHP;
                Debug.Log("데미지 들어감" + livingEntity.GetComponent<PlayerStat>()._Hp);
                if (livingEntity.GetComponent<PlayerStat>()._Hp <= 0)
                {
                    livingEntity.GetComponent<PlayerST>().PlayerDie();
                }
            }
            if (livingEntity != null && !livingEntity.isDie)
            {
                livingEntity.SendMessage("OnDamageNuck");
            }   
        }
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("isThrowShot", false);
        anim.SetBool("isThrow", false);
        nav.speed = 4f;
        mat.material.DOColor(Color.white, 2f);
        isChase = true;
        if (!isDie)
            nav.isStopped = false;
        isThrow = false;
        isSkill = false;
        yield return new WaitForSeconds(2.5f);
        Patterning = false;
    }
    [PunRPC]
    IEnumerator Rush()
    {
        isSkill = true;
        isChase = false;
        isAttack = true;
        if (!isDie)
            nav.isStopped = true;
        isRush = true;
        StopCoroutine(Attack());
        anim.SetBool("isAttack", false);
        anim.SetBool("isRush", true);
        transform.DOJump(target.position, 1.5f, 1, 1.2f);
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("isRush", false);
        RushEff.transform.position = target.position;
        RushEff.Play();
        isRush = false;
        isChase = true;
        isAttack = false;
        if (!isDie)
            nav.isStopped = false;
        isSkill = false;
        yield return new WaitForSeconds(2.5f);

        Patterning = false;

    }

    [PunRPC]
    IEnumerator Attack() //������ �ϰ� �������ϰ� �ٽ� ������ ����
    {
        attacking.isAttacking = true;


        isChase = false;
        isAttack = true;
        if (!isDie)
            nav.isStopped = true;
        anim.SetBool("isAttack", true);
        anim.SetBool("isRun", false);
        yield return new WaitForSeconds(0.8f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(1f);
        rigid.velocity = Vector3.zero;
        meleeArea.enabled = false;

        if(!isSkill)
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
        if(!isDie)
        nav.isStopped = false;

    }
    void FixedUpdate()
    {

        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other)  //�ǰ�
    {
        if (!isDamage)
        {
            if (other.tag == "Melee")
            {
                Weapons weapon = other.GetComponent<Weapons>();
                curHealth -= weapon.damage;

                StartCoroutine(OnDamage());

            }
            else if (other.tag == "Arrow")
            {
                Arrow arrow = other.GetComponent<Arrow>();
                curHealth -= arrow.damage;

                StartCoroutine(OnDamage());
            }
            else if (other.tag == "ArrowSkill")
            {
                ArrowSkill arrow = other.GetComponent<ArrowSkill>();
                curHealth -= arrow.damage;

                StartCoroutine(OnDamage());
            }
        }
    }

    IEnumerator OnDamage()
    {
        HitSoundManager.hitsoundManager.SlimeHitSound();
        Hiteff.Play();
        Hiteff2.Play();
        isDamage = true;
        yield return new WaitForSeconds(0.1f);
        isDamage = false;
        HitMonster();
        // SetHpBar();
        if (curHealth < 0)
        {
            //Die();
        }


    }

    public override void Die()
    {
        BossDrop();
        MonsterDie();
        if (!isDie)
            nav.isStopped = true;
        isDie = true;
        boxCollider.enabled = false;
        isChase = false; //�׾����� ��������
        anim.SetBool("isDie", true);
        gameObject.SetActive(false);
        Invoke("Diegg", 1.5f);

        if (!questStore.MainSuccess)
        {
            questStore.MainQuestSuccess(3);
        }
    }

    void Diegg()
    {

        respawn.gameObject.SetActive(true);
        --SpawnManager.spawnManager.TurtleSlimeObjs;

        gameObject.SetActive(false);
    }

    
}






