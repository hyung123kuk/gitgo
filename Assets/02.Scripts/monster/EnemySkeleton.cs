using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using DG.Tweening;

public class EnemySkeleton : Monster
{
    public BoxCollider meleeArea;
    public bool isChase;
    public bool isAttack;
    public Transform respawn;
    private bool isDie;
    public bool isStun;
    public bool isDamage; //����°��ֳ�
    public bool isReset; //������ �����°�?
    public int Corotineidx; //�ڷ�ƾ �ߺ�����
    public Transform movepoint;

    public ParticleSystem Hiteff;
    public ParticleSystem Hiteff2;

    public LayerMask whatIsTarget; // ���� ��� ���̾�

    public bool NoMove; //�������� ����X

    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    [SerializeField]
    NavMeshAgent nav;
    Animator anim;
    QuestNormal questNormal;
    [SerializeField]
    Attacking attacking;
    float targetRange = 2f; //���� ���ݻ����Ÿ�

    private bool hasTarget
    {
        get
        {
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (target != null && !target.GetComponent<PlayerST>().isDie)
            {
                return true;
            }

            // �׷��� �ʴٸ� false
            return false;
        }
    }
    void Awake()
    {
        attacking = transform.GetChild(2).GetComponent<Attacking>();
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();


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
        mat.color = Color.white;
        isStun = false;
        anim = GetComponent<Animator>();
        StartMonster();
        Monstername.text = "���̷���";
        level.text = "8";
        questNormal = FindObjectOfType<QuestNormal>();
        coin = 50;
        // ���� ������Ʈ Ȱ��ȭ�� ���ÿ� AI�� ���� ��ƾ ����

        if (PhotonNetwork.IsMasterClient) //ȣ��Ʈ������ ����
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
        if (isStun && !isDie)
        {
            nav.speed = 0f;
        }
        if (isChase || isAttack) //�迧
            if (!isDie && !playerST.isJump && !playerST.isFall && !isStun)
                transform.LookAt(target);
    }
    private IEnumerator UpdatePath()
    {

        // ����ִ� ���� ���� ����
        while (!isDie)
        {

            if (hasTarget )
            {
                Corotineidx = 0;
                isReset = false;
                // ���� ��� ���� : ��θ� �����ϰ� AI �̵��� ��� ����
                Targerting();
                nav.SetDestination(target.position);
                nav.speed = 4f;
                if (!isAttack && !isStun)
                {
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
                // ���� ��� ���� : AI �̵� ����
                EnemyReset();

                // 20 ������ �������� ���� ������ ���� �׷�����, ���� ��ġ�� ��� �ݶ��̴��� ������
                // ��, targetLayers�� �ش��ϴ� ���̾ ���� �ݶ��̴��� ���������� ���͸�
                Collider[] colliders =
                    Physics.OverlapSphere(transform.position, 16f, whatIsTarget);

                // ��� �ݶ��̴����� ��ȸ�ϸ鼭, ����ִ� �÷��̾ ã��
                for (int i = 0; i < colliders.Length; i++)
                {
                    // �ݶ��̴��κ��� PlayerST ������Ʈ ��������
                    PlayerST livingEntity = colliders[i].GetComponent<PlayerST>();

                    // PlayerST ������Ʈ�� �����ϸ�, �ش� LivingEntity�� ����ִٸ�,
                    if (livingEntity != null && !livingEntity.isDie)
                    {
                        // ���� ����� �ش� LivingEntity�� ����
                        target = livingEntity.transform;

                        // for�� ���� ��� ����
                        break;
                    }
                }
            }

            // 0.25�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }
    }
    void EnemyReset()
    {
        if (PhotonNetwork.IsMasterClient && !isReset)
        {
            nav.SetDestination(respawn.transform.position);
            nav.speed = 5f;
            //curHealth = maxHealth;  �������� ü�µ���ȭ�� �� ���Ͼ�� �켱 �ּ� �߽��ϴ�.
            isChase = false;
            if (Vector3.Distance(respawn.position, transform.position) < 1f)
            {
                if (!isDie)
                    nav.isStopped = true;
                anim.SetBool("isWalk", false);
                isReset = true;
            }
        }
        else if (PhotonNetwork.IsMasterClient && isReset) //��Ȳ
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

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }
    void Targerting()
    {
        float targetRadius = 1f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack && !isDie && !isStun)
        {
            photonView.RPC("Attack", RpcTarget.All);
            MonsterAttack();
        }
    }

    [PunRPC]
    IEnumerator Attack()
    {
        attacking.isAttacking = true;


        isChase = false;
        isAttack = true;
        if (!isDie)
            nav.isStopped = true;
        anim.SetBool("isAttack", true);
        anim.SetBool("isWalk", false);
        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.2f);
        rigid.velocity = Vector3.zero;
        meleeArea.enabled = false;


        yield return new WaitForSeconds(0.5f);
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
        if (!isDie)
            nav.isStopped = false;
    }
    void FixedUpdate()
    {
        FreezeVelocity();
    }


    void OnTriggerEnter(Collider other)
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
            photonView.RPC("Stun", RpcTarget.All);
        }


    }
    [PunRPC]
    IEnumerator Stun()
    {
        isStun = true;
        anim.SetBool("isStun", true);
        if (!isDie)
            nav.isStopped = true;
        yield return new WaitForSeconds(3f);
        if (!isDie)
        {
            isStun = false;
            nav.isStopped = false;
            anim.SetBool("isStun", false);
        }
    }

    IEnumerator OnDamage()
    {
        HitMonster();
        HitSoundManager.hitsoundManager.SkeletonHitSound();
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

    public override void Die()
    {
        MonsterDie();
        questNormal.SkelletonKillCount();
        boxCollider.enabled = false;
        mat.color = Color.black;
        if (!isDie)
            nav.isStopped = true;
        if (isDie)
            return;
        isDie = true;
        isChase = false;
        anim.SetBool("isDie", true);
        Invoke("Diegg", 1.5f);
    }

    void Diegg()
    {
        respawn.gameObject.SetActive(true);
        --SpawnManager.spawnManager.SkeletonObjs;
        gameObject.SetActive(false);
    }
}
