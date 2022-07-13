using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
public class EnemyBoss1 : MonsterBoss
{

    public BoxCollider meleeArea; //���� ���ݹ���
    public BoxCollider nuckArea; //���Ͻ�ų 
    public SphereCollider nuckarea;
    public bool isChase; //�������� ����
    public bool isAttack; //���� ������
    public bool isRush;
    public bool isStun;
    public bool isbansa;
    public bool isDie;
    public Transform respawn;

    public LayerMask whatIsTarget; // 공격 대상 레이어


    private Light stunarea;
    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    SkinnedMeshRenderer[] mat;
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
        stunarea = GetComponentInChildren<Light>();
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentsInChildren<SkinnedMeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        attacking = transform.GetChild(2).GetComponent<Attacking>();
        questStore = FindObjectOfType<QuestStore>();

    }
    private void OnEnable()
    {
        boxCollider.enabled = true;
        isAttack = false;
        nav.isStopped = false;
        isDie = false;
        curHealth = maxHealth;
        isStun = false;
        anim = GetComponent<Animator>();

        StartBossMonster();
        BossItemSet();
        Monstername.text = "거북 슬라임";
        level.text = "5";
        coin = 30;
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작

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
                nav.speed = 5f;
                if (!isAttack)
                {
                    isChase = true;
                    nav.isStopped = false;
                    anim.SetBool("isWalk", true);
                }
                if (Vector3.Distance(target.position, transform.position) > 15f)
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
        isChase = false;
        nav.speed = 20f;
        nav.isStopped = false;
        //curHealth = maxHealth;
        if (Vector3.Distance(respawn.position, transform.position) < 1f)
        {
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
            int ranAction = Random.Range(0, 9);
            switch (ranAction)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    photonView.RPC("Stun", RpcTarget.All);
                    MonsterAttack();
                    break;
                case 4:
                case 5:
                case 6:
                    photonView.RPC("Rush", RpcTarget.All);
                    MonsterAttack();
                    break;
                case 7:
                case 8:
                case 9:
                    photonView.RPC("Reflect", RpcTarget.All);
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
    IEnumerator Stun()
    {
        isSkill = true;
        isStun = true;
        isChase = false;
        isAttack = true;
        nav.isStopped = true;
        stunarea.enabled = true;
        anim.SetBool("isRun", false);
        anim.SetBool("isAttack", false);
        yield return new WaitForSeconds(2f);
        anim.SetBool("isStun", true);
        yield return new WaitForSeconds(0.3f);
        nuckarea.enabled = true;
        stunarea.enabled = false;
        anim.SetBool("isStun", false);
        yield return new WaitForSeconds(0.2f);
        nuckarea.enabled = false;

        isSkill = false;


        isStun = false;
        isChase = true;
        isAttack = false;
        nav.isStopped = false;
        yield return new WaitForSeconds(2.5f);
        Patterning = false;

    }
    [PunRPC]
    IEnumerator Reflect()
    {
        isSkill = true;
        isbansa = true;
        isChase = false;
        nav.isStopped = true;
        anim.SetBool("isDefend", true);
        yield return new WaitForSeconds(5f);
        rigid.velocity = Vector3.zero;
        meleeArea.enabled = false;

        if (!isDie)
            anim.SetBool("isDefend", false);
        isChase = true;
        nav.isStopped = false;
        isbansa = false;
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
        nav.isStopped = true;
        isRush = true;
        //anim.SetBool("isRush", true);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.2f);
        rigid.AddForce(transform.forward * 40 + transform.up * 20, ForceMode.Impulse);

        yield return new WaitForSeconds(1f);
        transform.position = target.position + Vector3.back * 2;
        isRush = false;
        meleeArea.enabled = false;
        rigid.velocity = Vector3.zero;

        isChase = true;
        isAttack = false;

        //anim.SetBool("isRush", false);
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
        nav.isStopped = true;
        anim.SetBool("isAttack", true);
        anim.SetBool("isWalk", false);
        yield return new WaitForSeconds(0.4f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(1f);
        rigid.velocity = Vector3.zero;
        meleeArea.enabled = false;

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
        nav.isStopped = false;

    }
    void FixedUpdate()
    {

        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other)  //�ǰ�
    {
        if (!isbansa && !isDamage)
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
        else if (isbansa)
        {
            if (other.tag == "Melee")
            {
                Weapons weapon = other.GetComponent<Weapons>();
                playerStat._Hp -= weapon.damage;
            }
            else if (other.tag == "Arrow")
            {
                Arrow arrow = other.GetComponent<Arrow>();
                playerStat._Hp -= arrow.damage;
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
        if (curHealth > 0)
            foreach (SkinnedMeshRenderer mesh in mat)
                mesh.material.color = Color.white;
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
        nav.isStopped = true;
        isDie = true;
        boxCollider.enabled = false;
        foreach (SkinnedMeshRenderer mesh in mat)
            mesh.material.color = Color.white;
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






