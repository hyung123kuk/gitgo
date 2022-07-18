using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using DG.Tweening;
public class EnemyBoss2 : MonsterBoss
{

    public BoxCollider meleeArea; //���� ���ݹ���
    public bool isChase; //�������� ����
    public bool isAttack; //���� ������
    public bool isDie;
    public bool isBuff;
    public bool isBuffPlay;
    public Transform respawn;
    public SphereCollider nuckarea;


    public LayerMask whatIsTarget; // 공격 대상 레이어

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
    CapsuleCollider boxCollider;
    [SerializeField]
    SkinnedMeshRenderer mat;
    NavMeshAgent nav;
    Animator anim;

    public ParticleSystem Hiteff; //피격이펙
    public ParticleSystem Hiteff2;
    public bool isDamage;

    public bool Patterning; //현재 패턴진행중?
    public bool isSkill; //현재 스킬사용중?

    float targetRange = 3f; //몬스터 공격사정거리
    QuestStore questStore;
    [SerializeField]
    Attacking attacking;

    public ParticleSystem SkillStarEff;
    public ParticleSystem[] Skill1Effs = new ParticleSystem[10];
    public Transform[] Sohwanpos = new Transform[5];
    public Transform HealPoint;
    public bool isHeal; //현재 힐 캐스팅중?
    public bool HealStop;
    public ParticleSystem Healeff;
    public ParticleSystem Pokjueff;
    public GameObject Razoreff;
    public ParticleSystem RazorReadyeff;
    public bool isRazor;
    void Awake()
    {
        attacking = transform.GetChild(2).GetComponent<Attacking>();
        stunarea = GetComponentInChildren<Light>();
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<CapsuleCollider>();
        mat = transform.GetChild(4).transform.GetComponent<SkinnedMeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        questStore = FindObjectOfType<QuestStore>();
        for (int i = 0; i < 10; i++)
        {
            Skill1Effs[i] = GameObject.Find("EffectPool").transform.GetChild(1).transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
        }
    }
    private void OnEnable()
    {
        isDamage = false;
        boxCollider.enabled = true;
        isAttack = false;
        nav.isStopped = false;
        isDie = false;
        curHealth = maxHealth;
        anim = GetComponent<Animator>();
        StartBossMonster();
        BossItemSet();

        Monstername.text = "저주받은자 아몬";
        level.text = "10";
        coin = 50;
        BossMonsterHpBarSet();

    }

    public override void BossHpBarSettting()
    {
        bossHpBar = GameObject.Find("BossMonsterHp").transform.GetChild(1).gameObject;
    }

    public void BossItemSet()
    {
        item = Resources.LoadAll<GameObject>("DROP/BOSS2");

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 30);
    }
    void Update()
    {
        if (isDie && Patterning)
        {
            StopAllCoroutines();
        }

        Collider[] colliders =
                    Physics.OverlapSphere(transform.position, 30f, whatIsTarget);


        for (int i = 0; i < colliders.Length; i++)
        {
      
            PlayerST livingEntity = colliders[i].GetComponent<PlayerST>();


            if (livingEntity != null && !livingEntity.isDie)
            { 
                target = livingEntity.transform;
                break;
            }
        }

        if (isBuff)
        {
            isBuff = false;
            nav.speed = 10f;
            EnemyAttack enemyRange = GetComponentInChildren<EnemyAttack>();
            enemyRange.damage *= 2f;
        }

        if (!isDie)
        {
            Targerting();
            if (target != null)
            {
                if (nav.enabled && target.GetComponent<PlayerST>().DunjeonBossArena) //추적
                {
                    PatternStart();
                    if (!isAttack && !isDie)
                    {

                        if (!isBuffPlay && !isSkill)
                            nav.speed = 6f;

                        isChase = true;
                        nav.isStopped = false;
                        nav.destination = target.position;
                        anim.SetBool("isRun", true);
                        if (playerST.isDie)
                            EnemyReset();
                    }
                }
                else if (!target.GetComponent<PlayerST>().DunjeonBossArena && nav.enabled) //복귀
                {
                    EnemyReset();
                }
            }

        }

        if (target != null)
        {
            if (target.GetComponent<PlayerST>().DunjeonBossArena)
            {
                if (isChase || isAttack)
                {
                    if (!isDie && !playerST.isJump && !playerST.isFall && !isRazor)//플레이어가 공중에 뜬 상태가 아닐때만 바라보기
                        transform.LookAt(target);
                }
            }
        }




    }
    void EnemyReset()
    {
        nav.SetDestination(respawn.transform.position);
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
                    photonView.RPC("Pokju", RpcTarget.All);
                    MonsterAttack();
                    break;
                case 2:
                case 3:
                case 4:
                    //몬스터 소환스킬
                    photonView.RPC("Sohwan", RpcTarget.All);
                    MonsterAttack();
                    break;
                case 5:
                case 6:
                    //레이저스킬
                    photonView.RPC("Razor", RpcTarget.All);
                    MonsterAttack();
                    break;
                case 7:
                case 8:
                    //어둠장판 만드는스킬
                    photonView.RPC("Skill1", RpcTarget.All);
                    MonsterAttack();
                    break;
                case 9:
                    //피 회복스킬
                    photonView.RPC("Heal", RpcTarget.All);
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

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack && !isDie && !isSkill)
        {
            photonView.RPC("Attack", RpcTarget.All);
            MonsterAttack();
        }

    }
    [PunRPC]
    IEnumerator Razor()
    {
        StopCoroutine(Attack());
        RazorReadyeff.Play();
        isSkill = true;
        isChase = false;
        isAttack = true;
        nav.isStopped = true;
        nav.speed = 0f;
        isRazor = true;
        anim.SetBool("Razor", true);
        yield return new WaitForSeconds(3f);
        Razoreff.SetActive(true);
        transform.DORotate(new Vector3(0, 360, 0), 10f, RotateMode.FastBeyond360)
                     .SetEase(Ease.Linear);
        yield return new WaitForSeconds(10f);
        RazorReadyeff.Stop();
        isRazor = true;
        Razoreff.SetActive(false);
        anim.SetBool("Razor", false);
        nav.speed = 6f;
        nav.isStopped = false;
        isSkill = false;
        isChase = true;
        isAttack = false;
        yield return new WaitForSeconds(2.5f);
        Patterning = false;
    }
    [PunRPC]
    IEnumerator Sohwan()
    {
        StopCoroutine(Attack());
        isSkill = true;
        isChase = false;
        isAttack = true;
        nav.isStopped = true;
        nav.speed = 0f;
        anim.SetBool("Skill1", true);
        for (int i = 0; i < 5; i++)
        {
            Sohwanpos[i].gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < 5; i++)
        {
            Sohwanpos[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < 5; i++)
        {
            Instantiate(skele, Sohwanpos[i].position, Sohwanpos[i].rotation);
        }
        anim.SetBool("Skill1", false);
        nav.isStopped = false;
        isChase = true;
        isAttack = false;
        isSkill = false;
        nav.speed = 4f;
        yield return new WaitForSeconds(1.5f);
        Patterning = false;
    }
    [PunRPC]
    IEnumerator Heal()
    {
        StopCoroutine(Attack());
        isHeal = true;
        transform.DOMove(HealPoint.position, 0.5f).SetEase(Ease.Linear);
        isSkill = true;
        isChase = false;
        isAttack = true;
        yield return new WaitForSeconds(0.5f);
        Healeff.Play();
        anim.SetBool("Heal", true);
        nav.isStopped = true;
        nav.speed = 0f;

        curHealth += 500;
        if (curHealth > maxHealth)
            curHealth = maxHealth;
        yield return new WaitForSeconds(1f);
        if (!HealStop)
        {
            curHealth += 500;
            if (curHealth > maxHealth)
                curHealth = maxHealth;
            Debug.Log(curHealth);
        }
        if(HealStop)
            nav.speed = 6f;
        yield return new WaitForSeconds(1f);
        if (!HealStop)
        {
            curHealth += 500;
            if (curHealth > maxHealth)
                curHealth = maxHealth;
            Debug.Log(curHealth);
        }
        if (HealStop)
            nav.speed = 6f;
        yield return new WaitForSeconds(1f);
        if (!HealStop)
        {
            curHealth += 500;
            if (curHealth > maxHealth)
                curHealth = maxHealth;
            Debug.Log(curHealth);
        }
        if (HealStop)
            nav.speed = 6f;
        yield return new WaitForSeconds(1f);
        if (!HealStop)
        {
            curHealth += 500;
            if (curHealth > maxHealth)
                curHealth = maxHealth;
            Debug.Log(curHealth);
        }
        isHeal = false;
        Healeff.Stop();
        anim.SetBool("Heal", false);
        nav.speed = 6f;
        isChase = true;
        isAttack = false;
        if(!isDie)
        nav.isStopped = false;
        isSkill = false;
        HealStop = false;
        yield return new WaitForSeconds(2.5f);
        if(Patterning)
        Patterning = false;
    }
    [PunRPC]
    IEnumerator Pokju()
    {
        StopCoroutine(Attack());
        isSkill = true;
        isChase = false;
        nav.isStopped = true;
        isAttack = true;
        anim.SetBool("Buff", true);
        nav.speed = 0f;
        mat.material.DOColor(Color.red, 3f);

        yield return new WaitForSeconds(3f);
        Pokjueff.Play();
        nav.speed = 6f;
        isSkill = false;
        isBuff = true;
        isBuffPlay = true;
        Invoke("BuffTime", 6f);
        anim.SetBool("Buff", false);
        isChase = true;
        nav.isStopped = false;
        isAttack = false;

        yield return new WaitForSeconds(3.5f);
        Patterning = false;
       
    }
    void BuffTime()
    {
        Pokjueff.Stop();
        isBuffPlay = false;
        mat.material.DOColor(Color.white, 1f);
    }

    [PunRPC]
    IEnumerator Skill1()
    {
        StopCoroutine(Attack());
        anim.SetBool("Skill1", true);
        SkillStarEff.Play();
        isSkill = true;
        isChase = false;
        isAttack = true;
        nav.isStopped = true;
        nav.speed = 0f;
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 10; i++)
        {
            float XPOS = Random.Range(300, 358);
            float ZPOS = Random.Range(453, 528);
            Skill1Effs[i].transform.position = new Vector3(XPOS, 11.5f, ZPOS);
        }
        isSkill = false;
        isChase = true;
        isAttack = false;
        anim.SetBool("Skill1", false);
        nav.isStopped = false;
        nav.speed = 4f;
        SkillStarEff.Stop();
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
        anim.SetBool("isRun", false);
        yield return new WaitForSeconds(0.6f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.8f);
        // rigid.velocity = Vector3.zero;
        meleeArea.enabled = false;


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

        if (other.tag == "CCAREA")
        {
            if (isHeal)
            {
                HealStop = true;
                Healeff.Stop();
                anim.SetBool("Heal", false);
                isSkill = false;
            }
        }
    }

    IEnumerator OnDamage()
    {
        HitSoundManager.hitsoundManager.GolemHitSound();
        isDamage = true;
        Hiteff.Play();
        Hiteff2.Play();
        yield return new WaitForSeconds(0.1f);
        isDamage = false;
        // foreach (SkinnedMeshRenderer mesh in mat)
        //   mesh.material.color = Color.red;
        HitMonster();
        // SetHpBar();

        if (curHealth < 0)
        {
            //Die();

        }
        yield return null;

    }

    public override void Die()
    {
        BossDrop();
        MonsterDie();

        nav.isStopped = true;
        isDie = true;
        boxCollider.enabled = false;

        isChase = false; //�׾����� ��������
        anim.SetBool("isDie", true);
        gameObject.SetActive(false);
        Invoke("Diegg", 1.5f);
        if (!questStore.MainSuccess)
        {
            questStore.MainQuestSuccess(5);
        }
    }

    void Diegg()
    {

        respawn.GetChild(0).gameObject.SetActive(true);
        --SpawnManager.spawnManager.GolemObjs;

        gameObject.SetActive(false);
    }
}


