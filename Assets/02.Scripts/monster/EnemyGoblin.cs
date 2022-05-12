using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGoblin : MonoBehaviour
{
    public float maxHealth =100; //최대 체력
    public float curHealth =100; //현재 체력
    public BoxCollider meleeArea; //몬스터 공격범위
    public bool isChase; //추적중인 상태
    public bool isAttack; //현재 공격중
    public Transform respawn;
    private bool isDie;

    public ParticleSystem Hiteff; //맞을때 이펙트
    public ParticleSystem Hiteff2; //맞을때 이펙트

    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat; //피격시 색깔변하게
    NavMeshAgent nav; //추적
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
        if (isDie)  //죽었으면 현재실행중인 코로틴 강제종료
        {
            StopAllCoroutines();
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Targerting();
        if (Vector3.Distance(target.position, transform.position) <= 15f && nav.enabled) //15미터 안에 포착
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
        else if (Vector3.Distance(target.position, transform.position) > 15f && nav.enabled) //15미터 밖
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

        if (isChase || isAttack) //추적이나 공격중일때만
            if (!isDie && !PlayerST.isJump && !PlayerST.isFall)
                transform.LookAt(target); //플레이어 바라보기
    }

    void FreezeVelocity() //이동보정
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }
    void Targerting()//타겟팅
    {
        float targetRadius = 1f;
        float targetRange = 0.5f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));  //레이캐스트

        if (rayHits.Length > 0 && !isAttack && !isDie) //레이캐스트에 플레이어가 잡혔다면 && 현재 공격중이 아니라면
        {
            StartCoroutine(Attack());
        }
    }

    
    IEnumerator Attack() //정지를 하고 공격을하고 다시 추적을 개시
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

    void OnTriggerEnter(Collider other)  //피격
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
            isChase = false; //죽었으니 추적중지
            anim.SetBool("isDie",true);
            Destroy(gameObject, 2f);
        }
    }
}
