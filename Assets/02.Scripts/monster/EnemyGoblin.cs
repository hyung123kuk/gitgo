using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGoblin : MonoBehaviour
{
    public float maxHealth =100; //�ִ� ü��
    public float curHealth =100; //���� ü��
    public BoxCollider meleeArea; //���� ���ݹ���
    public bool isChase; //�������� ����
    public bool isAttack; //���� ������
    public Transform respawn;
    private bool isDie;

    public ParticleSystem Hiteff; //������ ����Ʈ
    public ParticleSystem Hiteff2; //������ ����Ʈ

    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat; //�ǰݽ� �����ϰ�
    NavMeshAgent nav; //����
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
       
    }
    void Update()
    {
        if (isDie)  //�׾����� ����������� �ڷ�ƾ ��������
        {
            StopAllCoroutines();
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Targerting();
        if (Vector3.Distance(target.position, transform.position) <= 15f && nav.enabled) //15���� �ȿ� ����
        {
            if (!isAttack)
            {
                anim.SetBool("isRun",false);
                nav.speed = 4.5f;
                isChase = true;
                nav.isStopped = false;
                nav.destination = target.position;
                anim.SetBool("isWalk", true);
                if (Vector3.Distance(target.position, transform.position) >= 6f && nav.enabled)
                {
                    anim.SetBool("isWalk", false);
                    nav.speed = 10f;
                    anim.SetBool("isRun",true);
                }
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > 15f && nav.enabled) //15���� ��
        {
            nav.SetDestination(respawn.position);
            isChase = false;
            nav.speed = 20f;
            curHealth = maxHealth;
            if (Vector3.Distance(respawn.position, transform.position) < 1f)
            {
                nav.isStopped = true;
                anim.SetBool("isWalk", false);
                anim.SetBool("isRun", false);
            }
        }

        if (isChase || isAttack) //�����̳� �������϶���
            if (!isDie && !PlayerST.isJump && !PlayerST.isFall)
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
        float targetRadius = 1f;
        float targetRange = 0.5f;

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

    void OnTriggerEnter(Collider other)  //�ǰ�
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

    IEnumerator OnDamage()
    {
        Hiteff.Play();
        Hiteff2.Play();
        mat.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            mat.color = Color.white;
        }
        else
        {
            boxCollider.enabled = false;
            mat.color = Color.black;
            nav.isStopped = true;
            isDie = true;
            isChase = false; //�׾����� ��������
            anim.SetBool("isDie",true);
            Destroy(gameObject, 2f);
        }
    }
}
