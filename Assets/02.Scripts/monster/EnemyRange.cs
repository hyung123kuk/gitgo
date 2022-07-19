using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using DG.Tweening;
public class EnemyRange : Monster
{
    
    public bool isChase; //�������� ����
    public bool isAttack; //���� ������
    public Transform respawn;
    public GameObject bullet;
    public Transform firepos;
    private bool isDie;
    public bool isStun;
    public bool isDamage; //현재맞고있나
    public bool isReset; //리셋이 끝났는가?
    public int Corotineidx; //코루틴 중복방지
    public Transform movepoint;

    public LayerMask whatIsTarget; // 공격 대상 레이어

    public ParticleSystem Hiteff; //������ ����Ʈ
    public ParticleSystem Hiteff2; //������ ����Ʈ
    Transform target;
    Rigidbody rigid;
    CapsuleCollider boxCollider;
    //public SkinnedMeshRenderer mat; //�ǰݽ� �����ϰ�
    NavMeshAgent nav; //����
    Animator anim;
    QuestNormal questNormal;
    float targetRange = 15f; //몬스터 공격사정거리

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 6.5f);
    }
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
        bullet = Resources.Load<GameObject>("Fireball");
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        questNormal = FindObjectOfType<QuestNormal>();

    }
    private void OnEnable()
    {
        anim.SetBool("isDie", false);
        isDamage = false;
        boxCollider.enabled = true;
        isAttack = false;
        nav.isStopped = false;
        isDie = false;
        curHealth = maxHealth;
        isStun = false;
        StartMonster();
        Monstername.text = "리치";
        level.text = "7";
        coin = 35;

        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작

        if (PhotonNetwork.IsMasterClient) //호스트에서만 추적
        {
            StartCoroutine(UpdatePath());
        }
    }
    void Update()
    {
        if (isDie)
        {
            StopAllCoroutines();
        }
        if(isStun && !isDie)
        {
            nav.speed = 0f;
        }
        if (isChase || isAttack) //룩엣
            if (!isDie && !playerST.isJump && !playerST.isFall && !isStun)
                transform.LookAt(target);
    }
    private IEnumerator UpdatePath()
    {

        // 살아있는 동안 무한 루프
        while (!isDie )
        {

            if (hasTarget )
            {
                // 추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
                Targerting();
                nav.speed = 3.5f;
                if (!isAttack && !isStun)
                {
                    
                    nav.SetDestination(target.position);
                    isChase = true;
                    if (!isDie)
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
                    Physics.OverlapSphere(transform.position, 6.5f, whatIsTarget);

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
        if (PhotonNetwork.IsMasterClient && !isReset)
        {
            nav.SetDestination(respawn.transform.position);
            nav.speed = 5f;
            //curHealth = maxHealth;  서버에서 체력동기화가 잘 안일어나서 우선 주석 했습니다.
            isChase = false;
            if (Vector3.Distance(respawn.position, transform.position) < 1f)
            {
                if (!isDie)
                    nav.isStopped = true;
                anim.SetBool("isWalk", false);
                isReset = true;
            }
        }
        else if (PhotonNetwork.IsMasterClient && isReset) //방황
        {
            if (isReset && Corotineidx == 0)
            {
                Corotineidx = 1;
                StartCoroutine(Move());
            }
        }
    }
    IEnumerator Move()
    {
        while (isReset)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 3.0f));
            transform.Rotate(new Vector3(0, 1, 0) * Random.Range(1000, 5000) * Time.smoothDeltaTime);
            if (!isDie)
                nav.isStopped = false;
            anim.SetBool("isWalk", true);
            nav.SetDestination(movepoint.position);
            nav.speed = 0.5f;
            yield return new WaitForSeconds(4f);
            if (!isDie)
                nav.isStopped = true;
            anim.SetBool("isWalk", false);
            yield return new WaitForSeconds(3f);
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


        if (rayHits.Length > 0 && !isAttack && !isDie && !isStun) //����ĳ��Ʈ�� �÷��̾ �����ٸ� && ���� �������� �ƴ϶��
        {
            photonView.RPC("Attack", RpcTarget.All);
            MonsterAttack();
        }
    }
    [PunRPC]
    IEnumerator Attack() //������ �ϰ� �������ϰ� �ٽ� ������ ����
    {
        isChase = false;
        isAttack = true;
        if (!isDie)
            nav.isStopped = true;
        nav.speed = 0f;
        anim.SetBool("isAttack", true);
        anim.SetBool("isWalk", false);
        yield return new WaitForSeconds(1f);
        GameObject instantBullet = Instantiate(bullet, firepos.position, firepos.rotation);
        Attacking attackingbullet = instantBullet.GetComponent<Attacking>();
        attackingbullet.isAttacking = true;
        Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
        rigidBullet.velocity = transform.forward * 15;
        Destroy(instantBullet, 2f);

        yield return new WaitForSeconds(2f);
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
        if(!isDie)
        nav.isStopped = false;
        nav.speed = 3.5f;
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
                MonsterAttack();

            }
            else if (other.tag == "Arrow")
            {
                Arrow arrow = other.GetComponent<Arrow>();
                curHealth -= arrow.damage;

                StartCoroutine(OnDamage());
                MonsterAttack();
            }
            else if (other.tag == "ArrowSkill")
            {
                ArrowSkill arrow = other.GetComponent<ArrowSkill>();
                curHealth -= arrow.damage;

                StartCoroutine(OnDamage());
                MonsterAttack();
            }
        }

        if (other.tag == "CCAREA")
        {
            photonView.RPC("Stun", RpcTarget.All);
            MonsterAttack();
        }
    }
    [PunRPC]
    IEnumerator Stun()
    {
        nav.speed = 0f;
        isStun = true;
        anim.SetBool("isStun", true);
        if (!isDie)
            nav.isStopped = true;
        yield return new WaitForSeconds(3f);
        if (!isDie)
        {
            nav.speed = 3.5f;
            isStun = false;
            nav.isStopped = false;
            anim.SetBool("isStun", false);
        }
    }
    IEnumerator OnDamage()
    {
        HitMonster();
        HitSoundManager.hitsoundManager.GoblinHitSound();
        isDamage = true;
        Hiteff.Play();
        Hiteff2.Play();
        yield return new WaitForSeconds(0.1f);
        isDamage = false;
        //SetHpBar();
    }

    public override void Die()
    {
        target = null;
        hpBarFlame.gameObject.SetActive(false);
        questNormal.GoblinKillCount();
        MonsterDie();
        if (!isDie)
            nav.isStopped = true;
        boxCollider.enabled = false;
        if (isDie)
            return;
        isDie = true;
        isChase = false; //�׾����� ��������
        anim.SetBool("isDie", true);
        Invoke("Diegg", 1.5f);
    }

    void Diegg()
    {
        respawn.gameObject.SetActive(true);
        --SpawnManager.spawnManager.GoblinArObjs;

        gameObject.SetActive(false);
    }
}
