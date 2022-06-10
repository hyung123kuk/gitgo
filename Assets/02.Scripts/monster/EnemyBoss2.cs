using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss2 : MonsterBoss
{

    public BoxCollider meleeArea; //���� ���ݹ���
    public bool isChase; //�������� ����
    public bool isAttack; //���� ������
    public bool isDie;
    public bool isBuff;
    private bool isStun;
    public Transform respawn;
    public SphereCollider nuckarea;

    public PlayerST playerST;


    public GameObject sohwane;
    public GameObject pokju;
    public GameObject stun;
    public GameObject bullet;
    public GameObject skele;
    private Light stunarea;
    public Transform firepos;
    public Transform fokjupos;
    public Transform Shpos1;
    public Transform Shpos2;
    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    SkinnedMeshRenderer[] mat;
    NavMeshAgent nav;
    Animator anim;

    public ParticleSystem Hiteff; //피격이펙
    public ParticleSystem Hiteff2;
    public bool isDamage;

    public bool Patterning; //현재 패턴진행중?
    public bool isSkill; //현재 스킬사용중?

    QuestStore questStore;
    [SerializeField]
    Attacking attacking;
    void Awake()
    {
        attacking = transform.GetChild(2).GetComponent<Attacking>();
        stunarea = GetComponentInChildren<Light>();
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentsInChildren<SkinnedMeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        questStore = FindObjectOfType<QuestStore>();
        StartBossMonster();
        BossItemSet();
        Monstername.text = "골렘";
        level.text = "10";
        coin = 50;

    }

    private void Start()
    {
        playerST = GetComponent<PlayerST>();
    }


    public void BossItemSet()
    {
        item = Resources.LoadAll<GameObject>("DROP/BOSS2");

    }


    void Update()
    {
        if (isDie && Patterning)
        {
            StopAllCoroutines();
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (isBuff)
        {
            nav.speed = 10f;
            EnemyAttack enemyRange = GetComponentInChildren<EnemyAttack>();
            enemyRange.damage *= 2;
        }

        if (!isDie)
        {
            Targerting();
            if (!isStun)
            {
                if (nav.enabled && playerST.DunjeonBossArena) //추적
                {
                    PatternStart();
                    if (!isAttack && !isDie)
                    {

                        if (!isBuff)
                            nav.speed = 4f;

                        isChase = true;
                        nav.isStopped = false;
                        nav.destination = target.position;
                        anim.SetBool("isRun", true);
                        if (playerST.isDie)
                            EnemyReset();
                    }
                }
                else if (!playerST.DunjeonBossArena && nav.enabled) //복귀
                {
                    EnemyReset();
                }
            }
        }

        if (playerST.DunjeonBossArena)
        {
            if (isChase || isAttack)
            {
                if (!isDie && !playerST.isJump && !playerST.isFall)//플레이어가 공중에 뜬 상태가 아닐때만 바라보기
                    transform.LookAt(target);
            }
        }




    }
    void EnemyReset()
    {
        nav.SetDestination(respawn.position);
        isChase = false;
        nav.speed = 20f;
        nav.isStopped = false;
        curHealth = maxHealth;
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

    IEnumerator Pattern() //패턴생각
    {
        yield return new WaitForSeconds(7f);

        if (!isDie)
        {
            int ranAction = Random.Range(0, 9);
            switch (ranAction)
            {
                case 0:
                case 1:
                    //폭주 버프스킬
                    StartCoroutine(Pokju());
                    MonsterAttack();
                    break;
                case 2:
                case 3:
                case 4:
                    //몬스터 소환스킬
                    StartCoroutine(Sohwan());
                    MonsterAttack();
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                    //공굴리기 스킬
                    StartCoroutine(FireBall());
                    MonsterAttack();
                    break;
                case 9:
                    //스턴거는스킬
                    StartCoroutine(Stun());
                    MonsterAttack();
                    break;
            }
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
        float targetRange = 3f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack && !isDie && !isSkill)
        {
            //StopCoroutine(Attack());
            StartCoroutine(Attack());
            MonsterAttack();
        }

    }
    IEnumerator Sohwan()
    {
        isSkill = true;
        isChase = false;
        isAttack = true;
        nav.isStopped = true;
        anim.SetBool("isSh", true);
        sohwane.SetActive(true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("isSh", false);
        GameObject instantSkele1 = Instantiate(skele, Shpos1.position, Shpos1.rotation);
        GameObject instantSkele2 = Instantiate(skele, Shpos2.position, Shpos2.rotation);
        Destroy(instantSkele1, 30f);
        Destroy(instantSkele2, 30f);

        nav.isStopped = false;
        isChase = true;
        isAttack = false;
        yield return new WaitForSeconds(1f);
        sohwane.SetActive(false);
        isSkill = false;

        yield return new WaitForSeconds(1.5f);
        Patterning = false;

        //StartCoroutine(Pattern());

    }
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
        yield return new WaitForSeconds(3f);
        anim.SetBool("isStun", true);
        yield return new WaitForSeconds(1.3f);
        stun.SetActive(true);
        nuckarea.enabled = true;
        stunarea.enabled = false;
        yield return new WaitForSeconds(0.2f);

        nuckarea.enabled = false;
        anim.SetBool("isStun", false);
        isStun = false;
        isChase = true;
        isAttack = false;
        nav.isStopped = false;
        yield return new WaitForSeconds(1f);
        isSkill = false;
        stun.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        Patterning = false;
        //StartCoroutine(Pattern());

    }
    IEnumerator Pokju()
    {

        isSkill = true;
        isChase = false;
        nav.isStopped = true;
        isAttack = true;
        anim.SetBool("isBuff", true);

        yield return new WaitForSeconds(3f);
        isSkill = false;
        pokju.SetActive(true);
        isBuff = true;
        Invoke("BuffTime", 6f);
        anim.SetBool("isBuff", false);
        isChase = true;
        nav.isStopped = false;
        isAttack = false;

        yield return new WaitForSeconds(2.5f);
        Patterning = false;
        //StartCoroutine(Pattern());

    }
    void BuffTime()
    {
        isBuff = false;
        pokju.SetActive(false);
    }

    IEnumerator FireBall()
    {
        isSkill = true;
        isChase = false;
        isAttack = true;
        nav.isStopped = true;

        anim.SetBool("isBall", true);

        yield return new WaitForSeconds(1f);
        GameObject instantBullet = Instantiate(bullet, firepos.position, firepos.rotation);
        Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
        rigidBullet.velocity = transform.forward * 5;

        Destroy(instantBullet, 4f);


        yield return new WaitForSeconds(1f);
        isSkill = false;
        rigid.velocity = Vector3.zero;

        isChase = true;
        isAttack = false;

        anim.SetBool("isBall", false);
        nav.isStopped = false;
        yield return new WaitForSeconds(2.5f);
        Patterning = false;
        //StartCoroutine(Pattern());

    }

    IEnumerator Attack() //������ �ϰ� �������ϰ� �ٽ� ������ ����
    {
        attacking.isAttacking = true;
        

        isChase = false;
        isAttack = true;
        nav.isStopped = true;

        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.8f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.8f);
        // rigid.velocity = Vector3.zero;
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
    }

    IEnumerator OnDamage()
    {
        HitSoundManager.hitsoundManager.GolemHitSound();
        foreach (SkinnedMeshRenderer mesh in mat)
            mesh.material.color = Color.red;
        isDamage = true;
        Hiteff.Play();
        Hiteff2.Play();
        yield return new WaitForSeconds(0.1f);
        isDamage = false;

        if (curHealth > 0)
        {
            foreach (SkinnedMeshRenderer mesh in mat)
                mesh.material.color = Color.gray;
        }
        // foreach (SkinnedMeshRenderer mesh in mat)
        //   mesh.material.color = Color.red;
        HitMonster();
        SetHpBar();

        if (curHealth < 0)
        {
            BossDrop();
            MonsterDie();

            nav.isStopped = true;
            isDie = true;
            boxCollider.enabled = false;

            isChase = false; //�׾����� ��������
            anim.SetBool("isDie", true);
            gameObject.SetActive(false);
            foreach (SkinnedMeshRenderer mesh in mat)
                mesh.material.color = Color.gray;
            Invoke("Diegg", 1.5f);
            if (!questStore.MainSuccess)
            {
                questStore.MainQuestSuccess(5);
            }

        }
        yield return null;

    }
    void Diegg()
    {

        respawn.GetChild(0).gameObject.SetActive(true);
        --SpawnManager.spawnManager.GolemObjs;
        gameObject.SetActive(false);
    }
}


