using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EnemySlime  : Monster
{

    public BoxCollider meleeArea;
    public bool isChase;
    public bool isAttack;
    public Transform respawn;
    public bool isDie;
    public bool isStun;
    public bool isDamage; //현재맞고있나


    public ParticleSystem Hiteff;
    public ParticleSystem Hiteff2;
    public GameObject Geteff;

    public Transform movepoint;
    [SerializeField]
  
    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;
    public static EnemySlime enemySlime;



    void Awake()
    {
        enemySlime = this;
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
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
                }
            }
            else if (Vector3.Distance(target.position, transform.position) > 20f && nav.enabled) //리셋
            {
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
            else
            {
 
                anim.SetBool("isWalk", false);
            }
        }
        if (isChase || isAttack) //룩엣
            if (!isDie && !PlayerST.isJump && !PlayerST.isFall && !isStun)
                transform.LookAt(target);
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
        float targetRange = 0.7f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));  //����ĳ��Ʈ

        if (rayHits.Length > 0 && !isAttack && !isDie) //����ĳ��Ʈ�� �÷��̾ �����ٸ� && ���� �������� �ƴ϶��
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack() //������ �ϰ� �������ϰ� �ٽ� ������ ����
    {
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
            ShakeOn();
        }
        if (!isDie)
        {
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
                
                Instantiate(Geteff, transform.position, Quaternion.identity);
                MonsterDie();
                nav.isStopped = true;
                boxCollider.enabled = false;
                mat.color = Color.black;
                isChase = false; //�׾����� ��������
                isDie = true;
                anim.SetBool("isDie", true);
                Invoke("Diegg", 1.5f);
            }
        }
    }
    void Diegg()
    {

        respawn.GetChild(0).gameObject.SetActive(true);
        --SpawnManager.spawnManager.SlimeObjs;
        gameObject.SetActive(false);
    }
}
