using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Photon.Pun;
public class EnemyBlueSlime : Monster
{

    //public float maxHealth;
    //public float curHealth;
    public BoxCollider meleeArea;
    public bool isChase;
    public bool isAttack;
    public Transform respawn;
    public bool isDie;
    public bool isStun;
    public bool isDamage; //현재맞고있나
    public float speed = 5;
    public bool check;

    public ParticleSystem Hiteff;
    public ParticleSystem Hiteff2;
    public Transform movingpos;

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
    void Awake()
    {
        attacking = transform.GetChild(2).GetComponent<Attacking>();
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
        StartMonster();
        Monstername.text = "블루 슬라임";
        level.text = "3";
        coin = 10;
    }



    void Update()
    {

        if (isDie)
        {
            StopAllCoroutines();
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (!isStun && !isDie)
        {
            Targerting();
            if (Vector3.Distance(target.position, transform.position) <= 20f && nav.enabled)
            {
                nav.speed = 3.5f;
                if (!isAttack)
                {
                    isChase = true;
                    nav.isStopped = false;
                    nav.SetDestination(target.position);
                    anim.SetBool("isWalk", true);
                    if (playerST.isDie)
                        EnemyReset();
                }
            }
            else if (Vector3.Distance(target.position, transform.position) > 20f && nav.enabled) //리셋
            {
                EnemyReset();
            }
        }
        if (isChase || isAttack) //룩엣
            if (!isDie && !playerST.isJump && !playerST.isFall && !isStun)
                transform.LookAt(target);
    }
    void EnemyReset()
    {
        
        nav.SetDestination(respawn.transform.position);
        nav.speed = 20f;
        //curHealth = maxHealth;
        isChase = false;
        if (Vector3.Distance(respawn.position, transform.position) < 1f)
        {
            nav.isStopped = true;
            anim.SetBool("isWalk", false);

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
        float targetRange = 0.7f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));  

        if (rayHits.Length > 0 && !isAttack && !isDie) 
        {
            StartCoroutine(Attack());
            MonsterAttack();
        }
    }

    IEnumerator Attack() 
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
        if (!isDie)
        {
            HitMonster();
           // SetHpBar();

            HitSoundManager.hitsoundManager.SlimeHitSound();
            isDamage = true;
            mat.color = Color.red;
            Hiteff.Play();
            Hiteff2.Play();
            yield return new WaitForSeconds(0.1f);
            isDamage = false;
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
        isChase = false;
        isDie = true;
        anim.SetBool("isDie", true);
        Invoke("Diegg", 1.5f);
        questNormal.SlimeKillCount();
    }

    void Diegg()
    {


            respawn.gameObject.SetActive(true);
            --SpawnManager.spawnManager.BlueSlimeObjs;

        gameObject.SetActive(false);
    }
}

