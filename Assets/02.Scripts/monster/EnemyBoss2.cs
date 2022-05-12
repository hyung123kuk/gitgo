using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss2 : MonoBehaviour
{
    public float maxHealth = 100; //최대 체력
    public float curHealth = 100; //현재 체력
    public BoxCollider meleeArea; //몬스터 공격범위
    public bool isChase; //추적중인 상태
    public bool isAttack; //현재 공격중
    public bool isDie;
    public bool isBuff;
    private bool isStun;
    public Transform respawn;
    public SphereCollider nuckarea;



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
  //  SkinnedMeshRenderer[] mat; //피격시 색깔변하게
    NavMeshAgent nav; //추적
    Animator anim;



    void Awake()
    {
       
        stunarea = GetComponentInChildren<Light>();
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
   //     mat = GetComponentsInChildren<SkinnedMeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        StartCoroutine(Pattern());
    }
    private void Start()
    {
       
    }

    void Update()
    {
        if(isDie)  //죽었으면 현재실행중인 코로틴 강제종료
        {
            StopAllCoroutines(); 
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if(isBuff)
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
                if (Vector3.Distance(target.position, transform.position) <= 27f && nav.enabled) //15미터 안에 포착
                {
                    if (!isAttack && !isDie)
                    {
                        if (!isBuff)
                            nav.speed = 4f;

                        isChase = true;
                        nav.isStopped = false;
                        nav.destination = target.position;
                        anim.SetBool("isRun", true);
                    }
                }
                else if (Vector3.Distance(target.position, transform.position) > 27f && nav.enabled) //15미터 밖
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
        }

        if (isChase || isAttack) //추적이나 공격중일때만
            if (!isDie && !PlayerST.isJump && !PlayerST.isFall)
                transform.LookAt(target); //플레이어 바라보기




    }
    IEnumerator Pattern() //보스패턴
    {
            yield return new WaitForSeconds(7f);
        if (!isDie)
        {
            int ranAction = Random.Range(0, 9);
            switch (ranAction)
            {
                case 0:
                case 1:
                    //폭주 : 이동속도,공격력증가
                    StartCoroutine(Pokju());
                    break;
                case 2:
                case 3:
                case 4:
                    //스켈레톤 소환
                    StartCoroutine(Sohwan());
                    break;
                case 5:
                case 6:    
                case 7:    
                case 8:
                    //돌굴리기
                    StartCoroutine(FireBall());
                    break;
                case 9:
                    //넓은 범위 스턴
                    StartCoroutine(Stun());
                    break;
            }
        }
        
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
        float targetRange = 3f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));  //레이캐스트

        if (rayHits.Length > 0 && !isAttack && !isDie) //레이캐스트에 플레이어가 잡혔다면 && 현재 공격중이 아니라면
        {
            //StopCoroutine(Attack());
            StartCoroutine(Attack());
        }

    }
    IEnumerator Sohwan() //해골소환
    {
        isChase = false;
        isAttack = true;
        nav.isStopped = true;
        anim.SetBool("isSh", true);
        sohwane.SetActive(true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("isSh", false);
        GameObject instantSkele1 = Instantiate(skele, Shpos1.position, Shpos1.rotation);
        GameObject instantSkele2 =Instantiate(skele, Shpos2.position, Shpos2.rotation);
        Destroy(instantSkele1, 30f);
        Destroy(instantSkele2, 30f);

        nav.isStopped = false;
        isChase = true;
        isAttack = false;
        yield return new WaitForSeconds(1f);
        sohwane.SetActive(false);


        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Pattern());

    }
    IEnumerator Stun() //스턴
    {
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
        stun.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        
        StartCoroutine(Pattern());

    }
    IEnumerator Pokju() //폭주 버프스킬
    {


        isChase = false;
        nav.isStopped = true;
        isAttack = true;
        anim.SetBool("isBuff", true);
        
        yield return new WaitForSeconds(3f);
        pokju.SetActive(true);
        isBuff = true;
        Invoke("BuffTime", 6f);
        anim.SetBool("isBuff", false);
        isChase = true;
        nav.isStopped = false;
        isAttack = false;

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(Pattern());

    }
    void BuffTime()
    {
        isBuff = false;
        pokju.SetActive(false);
    }

    IEnumerator FireBall() //공굴리기
    {

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

        rigid.velocity = Vector3.zero;

        isChase = true;
        isAttack = false;

        anim.SetBool("isBall", false);
        nav.isStopped = false;
        yield return new WaitForSeconds(2.5f);

        StartCoroutine(Pattern());

    }
    
    IEnumerator Attack() //정지를 하고 공격을하고 다시 추적을 개시
    {
        
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
       // foreach (SkinnedMeshRenderer mesh in mat)
         //   mesh.material.color = Color.red;

        

        if (curHealth < 0)
        {
            nav.isStopped = true;
            isDie = true;
            boxCollider.enabled = false;
            //foreach (SkinnedMeshRenderer mesh in mat)
               // mesh.material.color = Color.white;
            isChase = false; //죽었으니 추적중지
            anim.SetBool("isDie", true);
            Destroy(gameObject, 200f);
            //foreach (SkinnedMeshRenderer mesh in mat)
              //  mesh.material.color = Color.white;
        }
        yield return null;
    }
}
