using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Photon.Pun;

public class EnemySlime  : Monster
{

    public BoxCollider meleeArea;
    public bool isChase;
    public bool isAttack;
    public Transform respawn;
    public bool isDie;
    public bool isStun;
    public bool isDamage; //현재맞고있나

    public LayerMask whatIsTarget; // 공격 대상 레이어

    public ParticleSystem Hiteff;
    public ParticleSystem Hiteff2;


    public Transform movepoint;
    [SerializeField]
  
    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    [SerializeField]
    NavMeshAgent nav;
    Animator anim;
    public static EnemySlime enemySlime;

    QuestNormal questNormal;

    [SerializeField]  
    Attacking attacking;

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
        attacking = transform.GetChild(2).GetComponent<Attacking>();
        enemySlime = this;
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        questNormal = FindObjectOfType<QuestNormal>();
    }
    private void OnEnable()
    {
        boxCollider.enabled = true;
        isAttack = false;
        nav.isStopped = false;
        isDie = false;
        curHealth = maxHealth;
        mat.color = Color.white;
        isStun = false;
        anim=GetComponent<Animator>();
       
        StartMonster();
        Monstername.text = "슬라임";
        level.text = "1";
        Exp = 10;
        coin = 3;
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작

        
         StartCoroutine(UpdatePath());
    }
    void Update()
    {

        if (isDie)
        {
            StopAllCoroutines();
        }
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        //if (!isStun && !isDie)
        //{
        //    Targerting();
        //    if (Vector3.Distance(target.position, transform.position) <= 20f && nav.enabled)
        //    {
        //        nav.speed = 3.5f;
        //        if (!isAttack)
        //        {
        //            isChase = true;
        //            nav.isStopped = false;
        //            nav.SetDestination(target.position);
        //            anim.SetBool("isWalk", true);
        //            if (playerST.isDie)
        //                EnemyReset();
        //        }
        //    }
        //    else if (Vector3.Distance(target.position, transform.position) > 20f && nav.enabled) //리셋
        //    {
        //        EnemyReset();
        //    }
        //}
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
                nav.SetDestination(target.position);
                nav.speed = 3.5f;
                if (!isAttack)
                {
                    isChase = true;
                    nav.isStopped = false;
                    anim.SetBool("isWalk", true);
                }
                if(Vector3.Distance(target.position, transform.position) > 15f)
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
        if (PhotonNetwork.IsMasterClient)
        {

            nav.SetDestination(respawn.transform.position);
            nav.speed = 20f;
            //curHealth = maxHealth;  서버에서 체력동기화가 잘 안일어나서 우선 주석 했습니다.
            isChase = false;
            if (Vector3.Distance(respawn.position, transform.position) < 1f)
            {
                nav.isStopped = true;
                anim.SetBool("isWalk", false);

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
        if (PhotonNetwork.IsMasterClient)
        {
            float targetRadius = 1f;
            float targetRange = 0.7f;

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position,
                targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));  //����ĳ��Ʈ

            if (rayHits.Length > 0 && !isAttack && !isDie) //����ĳ��Ʈ�� �÷��̾ �����ٸ� && ���� �������� �ƴ϶��
            {
                StartCoroutine(Attack());
                MonsterAttack();
            }
        }
    }

    IEnumerator Attack() //������ �ϰ� �������ϰ� �ٽ� ������ ����
    {
        attacking.isAttacking = true;

        isChase = false;
        isAttack = true;
        nav.isStopped = true;
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.7f);
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
        if (other.tag == "CCAREA")
        {
            StartCoroutine(Stun());
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if(other.name == "SlimeArea")
    //    {
    //        curHealth = maxHealth;
    //        int ranx = Random.Range(43, 134);
    //        int ranz = Random.Range(-65, -110);

    //        transform.position = new Vector3(ranx, 0.5f, ranz);
    //    }
    //}


    IEnumerator Stun()
    {
        isStun = true;
        anim.SetBool("isStun", true);
        yield return new WaitForSeconds(3f);
        isStun = false;
        anim.SetBool("isStun", false);
    }

    IEnumerator OnDamage()
    {
        if (!isDie)
        {
            HitSoundManager.hitsoundManager.SlimeHitSound();
            HitMonster();
        }
        if (!isDie)
        {
            isDamage = true;
            mat.color = Color.red;
            Hiteff.Play();
            Hiteff2.Play();
            yield return new WaitForSeconds(0.1f);
            isDamage = false;
           // SetHpBar();
            if (curHealth > 0)
            {
                
                mat.color = Color.white;
            }
            else
            {
                //Die();
            }
        }
    }

    public override void Die()
    {
        MonsterDie();
        nav.isStopped = true;
        boxCollider.enabled = false;
        mat.color = Color.black;
        isChase = false; //�׾����� ��������
        isDie = true;
        anim.SetBool("isDie", true);
        Invoke("Diegg", 1.5f);
        questNormal.SlimeKillCount();
    }
    

    void Diegg()
    {
        
        respawn.gameObject.SetActive(true);
        --SpawnManager.spawnManager.SlimeObjs;
        gameObject.SetActive(false);
    }
}
