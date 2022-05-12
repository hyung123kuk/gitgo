using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRange : MonoBehaviour
{
    public float maxHealth; //최대 체력
    public float curHealth; //현재 체력
    public bool isChase; //추적중인 상태
    public bool isAttack; //현재 공격중
    public Transform respawn;
    public GameObject bullet;
    public Transform firepos;
    private bool isDie;

    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    SkinnedMeshRenderer[] mat; //피격시 색깔변하게
    NavMeshAgent nav; //추적
    Animator anim;
    

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentsInChildren<SkinnedMeshRenderer>();
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
        if (Vector3.Distance(target.position, transform.position) <= 25f && nav.enabled) //15미터 안에 포착
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
        else if (Vector3.Distance(target.position, transform.position) > 25f && nav.enabled) //15미터 밖
        {
            nav.SetDestination(respawn.position);
            isChase = false;
            nav.speed = 20f;
            curHealth = maxHealth;
            if (Vector3.Distance(respawn.position, transform.position) < 1f)
            {
                nav.isStopped = true;
                anim.SetBool("isWalk", false);
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
        float targetRadius = 0.5f;
        float targetRange = 20f;

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
        foreach (SkinnedMeshRenderer mesh in mat)
            mesh.material.color = Color.gray;

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            
            foreach (SkinnedMeshRenderer mesh in mat)
                mesh.material.color = Color.white;
        }
        else
        {
            nav.isStopped = true;
            boxCollider.enabled = false;
            foreach (SkinnedMeshRenderer mesh in mat)
                mesh.material.color = Color.black;
            isDie = true;
            isChase = false; //죽었으니 추적중지
            anim.SetBool("isDie", true);
            Destroy(gameObject, 1f);
        }
    }
}
