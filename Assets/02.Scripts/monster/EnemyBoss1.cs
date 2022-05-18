using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss1 : HpBar
{
   
    public BoxCollider meleeArea; //���� ���ݹ���
    public BoxCollider nuckArea; //���Ͻ�ų 
    public SphereCollider nuckarea;
    public bool isChase; //�������� ����
    public bool isAttack; //���� ������
    public bool isRush;
    public bool isStun;
    public bool isbansa;
    public bool isDie;
    public Transform respawn;



    private Light stunarea;
    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
   // SkinnedMeshRenderer[] mat; //�ǰݽ� �����ϰ�
    NavMeshAgent nav; //����
    Animator anim;

    void Awake()
    {
        stunarea = GetComponentInChildren<Light>();
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
     //   mat = GetComponentsInChildren<SkinnedMeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        StartCoroutine(Pattern());
        StartHpBar();
    }
    void Update()
    {
        if (isDie)  //�׾����� ����������� �ڷ�ƾ ��������
        {
            StopAllCoroutines();
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if (!isbansa && !isRush && !isStun && !isDie)
        {
            Targerting();
            if (Vector3.Distance(target.position, transform.position) <= 27f && nav.enabled) //15���� �ȿ� ����
            {
                if (!isAttack&& !isDie)
                {
                    nav.speed = 5f;
                    isChase = true;
                    nav.isStopped = false;
                    nav.destination = target.position;
                    anim.SetBool("isRun", true);
                }
            }
            else if (Vector3.Distance(target.position, transform.position) > 27f && nav.enabled) //15���� ��
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
            else
            {
                nav.isStopped = true;
                anim.SetBool("isRun", false);
            }
        }

        if (isChase || isAttack)
            if(!isDie && !PlayerST.isJump && !PlayerST.isFall)
            transform.LookAt(target); //�÷��̾� �ٶ󺸱�
        
    }
    

    IEnumerator Pattern() //��������
    {
        
            yield return new WaitForSeconds(6f);
        if (!isDie)
        {
            int ranAction = Random.Range(0, 3);
            switch (ranAction)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    //����
                    StartCoroutine(Stun());
                    break;
                case 4:
                case 5:
                case 6:
                    //����
                    StartCoroutine(Rush());
                    break;
                case 7:
                case 8:
                case 9:
                    //��������(�ݻ絩)
                    StartCoroutine(Reflect());
                    break;
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
            float targetRadius = 1f;
            float targetRange = 3f;
       

        if (isRush)
            {
                targetRange = 20f;
            }
            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position,
                targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));  //����ĳ��Ʈ

            if (rayHits.Length > 0 && !isAttack && !isRush && !isDie) //����ĳ��Ʈ�� �÷��̾ �����ٸ� && ���� �������� �ƴ϶��
            {
                //StopCoroutine(Attack());
                StartCoroutine(Attack());
            }

    }
    IEnumerator Stun()
    {
        isStun = true;
        isChase = false;
        isAttack = true;
        nav.isStopped = true;
        stunarea.enabled = true;
        anim.SetBool("isRun", false);
        anim.SetBool("isAttack", false);
        yield return new WaitForSeconds(2f);
        anim.SetBool("isStun", true);
        yield return new WaitForSeconds(0.3f);
        nuckarea.enabled = true;
        stunarea.enabled = false;
        yield return new WaitForSeconds(0.2f);
        nuckarea.enabled = false;
        anim.SetBool("isStun", false);

        
        isStun = false;
        isChase = true;
        isAttack = false;
        nav.isStopped = false;
        yield return new WaitForSeconds(2.5f);

         StartCoroutine(Pattern());

    }
    IEnumerator Reflect()
    {

            isbansa = true;
            isChase = false;
            nav.isStopped = true;
            anim.SetBool("isDefend",true);
            yield return new WaitForSeconds(5f);
            rigid.velocity = Vector3.zero;
            meleeArea.enabled = false;

            if (!isDie)
            anim.SetBool("isDefend", false);
            isChase = true;
            nav.isStopped = false;
            isbansa = false;
            yield return new WaitForSeconds(2.5f);
            StartCoroutine(Pattern());

    }
    IEnumerator Rush()
    {

            isChase = false;
            isAttack = true;
            nav.isStopped = true;
            isRush = true;
            anim.SetBool("isRush",true);
            meleeArea.enabled = true;
            rigid.AddForce(transform.forward * 30, ForceMode.Impulse);

            yield return new WaitForSeconds(1f);
            isRush = false;
            meleeArea.enabled = false;
            rigid.velocity = Vector3.zero;

            isChase = true;
            isAttack = false;

            anim.SetBool("isRush", false);
            nav.isStopped = false;
            yield return new WaitForSeconds(2.5f);

            StartCoroutine(Pattern());

    }
    IEnumerator Attack() //������ �ϰ� �������ϰ� �ٽ� ������ ����
    {
        
            isChase = false;
            isAttack = true;
            nav.isStopped = true;
            anim.SetBool("isAttack", true);
            yield return new WaitForSeconds(0.4f);
            meleeArea.enabled = true;
            

        yield return new WaitForSeconds(1f);
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
        if (!isbansa)
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
        }
        else if (isbansa)
        {
            if (other.tag == "Melee")
            {
                Weapons weapon = other.GetComponent<Weapons>();
                PlayerStat.playerstat._Hp -= weapon.damage;
            }
            else if (other.tag == "Arrow")
            {
                Arrow arrow = other.GetComponent<Arrow>();
                PlayerStat.playerstat._Hp -= arrow.damage;
            }
        }
    }

    IEnumerator OnDamage()
    {
        SetHpBar();
        if (curHealth < 0)
        {
            
            nav.isStopped = true;
            isDie = true;
            boxCollider.enabled = false;
            //foreach (SkinnedMeshRenderer mesh in mat)
            // mesh.material.color = Color.white;
            isChase = false; //�׾����� ��������
            anim.SetBool("isDie", true);
     
            Destroy(gameObject, 200f);
        }
        

        yield return null;
        
            
        
    }
}
