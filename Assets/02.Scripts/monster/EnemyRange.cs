using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public ParticleSystem Hiteff; //������ ����Ʈ
    public ParticleSystem Hiteff2; //������ ����Ʈ
    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    public SkinnedMeshRenderer mat; //�ǰݽ� �����ϰ�
    NavMeshAgent nav; //����
    Animator anim;
    

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        StartMonster();

    }
    private void OnEnable()
    {
        boxCollider.enabled = true;
        isAttack = false;
        nav.isStopped = false;
        isDie = false;
        curHealth = maxHealth;
        mat.material.color = Color.white;
        isStun = false;
    }
    void Update()
    {
        if (isDie)  //�׾����� ����������� �ڷ�ƾ ��������
        {
            StopAllCoroutines();
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (!isStun)
        {
            Targerting();
            if (Vector3.Distance(target.position, transform.position) <= 25f && nav.enabled) //15���� �ȿ� ����
            {
                if (!isAttack)
                {
                    anim.SetBool("isWalk", true);
                    nav.speed = 3.5f;
                    isChase = true;
                    nav.isStopped = false;
                    nav.destination = target.position;

                }
            }
            else if (Vector3.Distance(target.position, transform.position) > 25f && nav.enabled) //15���� ��
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
        }

        if (isChase || isAttack) //�����̳� �������϶���
            if (!isDie && !PlayerST.isJump && !PlayerST.isFall && !isStun)
                transform.LookAt(target); //�÷��̾� �ٶ󺸱�
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
        float targetRadius = 0.5f;
        float targetRange = 20f;

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
        GameObject instantBullet = Instantiate(bullet, firepos.position, firepos.rotation);
        Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
        rigidBullet.velocity = transform.forward * 20;
        Destroy(instantBullet, 2f);

        yield return new WaitForSeconds(0.5f);
        rigid.velocity = Vector3.zero;

        yield return new WaitForSeconds(2f);
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
        ShakeOn();
        isDamage = true;
        Hiteff.Play();
        Hiteff2.Play();
        mat.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        isDamage = false;
        SetHpBar();
        if (curHealth > 0)
        {
            
            mat.material.color = Color.white;
            
        }
        else
        {
            MonsterDie();
            nav.isStopped = true;
            boxCollider.enabled = false;
            mat.material.color = Color.black;
            isDie = true;
            isChase = false; //�׾����� ��������
            anim.SetBool("isDie", true);
            Invoke("Diegg", 1.5f);
            
        }
    }
    void Diegg()
    {
        respawn.GetChild(0).gameObject.SetActive(true);
        --SpawnManager.spawnManager.GoblinArObjs;
        gameObject.SetActive(false);
    }
}
