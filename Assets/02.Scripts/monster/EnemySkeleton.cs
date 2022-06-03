using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySkeleton : Monster
{
    public BoxCollider meleeArea; 
    public bool isChase; 
    public bool isAttack; 
    public Transform respawn;
    private bool isDie;
    public bool isStun;
    public bool isDamage; //현재맞고있나

    public ParticleSystem Hiteff; 
    public ParticleSystem Hiteff2;

    public bool NoMove; //벽있으면 추적X

    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat; 
    [SerializeField]
    NavMeshAgent nav;
    Animator anim;
    QuestNormal questNormal;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        StartMonster();
        Monstername.text = "스켈레톤";
        level.text = "8";
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
    }
    void Update()
    {
        if (isDie)  
        {
            StopAllCoroutines();
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (!isStun)
        {
            Targerting();
            if (Vector3.Distance(target.position, transform.position) <= 10f && nav.enabled) 
            {
                if (!isAttack)
                {
                    nav.speed = 10f;
                    isChase = true;
                    nav.isStopped = false;
                    nav.destination = target.position;
                    anim.SetBool("isWalk", true);

                }
            }
            else if (Vector3.Distance(target.position, transform.position) > 10f && nav.enabled) 
            {
                anim.SetBool("isWalk", true);
                nav.SetDestination(respawn.transform.position);
                nav.speed = 20f;
                curHealth = maxHealth;
                isChase = false;
                if (Vector3.Distance(respawn.position, transform.position) < 1f)
                {
                    nav.isStopped = true;
                    anim.SetBool("isWalk", false);
                }
            }
        }

        if (isChase || isAttack) 
            if (!isDie && !PlayerST.isJump && !PlayerST.isFall && !isStun)
                transform.LookAt(target);
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
        float targetRange = 0.5f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player")); 

        if (rayHits.Length > 0 && !isAttack && !isDie) 
        {
            StartCoroutine(Attack());
        }
    }


    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        nav.isStopped = true;
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.2f);
        rigid.velocity = Vector3.zero;
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.5f);
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
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
            StartCoroutine(Stun());
        }


    }

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
        HitMonster();
        HitSoundManager.hitsoundManager.SkeletonHitSound();
        isDamage = true;
        mat.color = Color.red;
        Hiteff.Play();
        Hiteff2.Play();
        yield return new WaitForSeconds(0.1f);
        isDamage = false;
        SetHpBar();
        if (curHealth > 0)
        {

            mat.color = Color.white;
        }
        else
        {
            MonsterDie();
            questNormal.SkelletonKillCount();
            boxCollider.enabled = false;
            mat.color = Color.black;
            nav.isStopped = true;
            isDie = true;
            isChase = false; 
            anim.SetBool("isDie", true);
            Invoke("Diegg", 1.5f);
        }
    }
    void Diegg()
    {
        respawn.GetChild(0).gameObject.SetActive(true);
        --SpawnManager.spawnManager.SkeletonObjs;
        gameObject.SetActive(false);
    }
}
