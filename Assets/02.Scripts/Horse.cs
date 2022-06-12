using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
    public float speed = 30f;
    Vector3 moveVec;

    public Transform playertr;
    private float h = 0f, v = 0f;
    private Rigidbody rigid;
    public Animator anim;
    public bool opning;
    public bool NoMove;

    Camera _camera;
    CapsuleCollider _controller;
    PlayerST playerST;
    Weapons weapons;
    public float smoothness = 10f;

    private void OnEnable()
    {
        HorseSpawnAnim();
    }
    void Awake()
    {
        playertr = GameObject.FindWithTag("Player").transform;
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        _camera = Camera.main;
        _controller = this.GetComponent<CapsuleCollider>();
        
    }
    private void Start()
    {
        playerST = FindObjectOfType<PlayerST>();
        weapons = FindObjectOfType<Weapons>();
    }

    void HorseSpawnAnim()
    {
        opning = true;
        transform.LookAt(playerST.horsepos2.position);
        anim.SetBool("isRun", true);
        Invoke("HorseSpawnAnimOut", 1.5f);
    }
    void HorseSpawnAnimOut()
    {
        opning = false;
        anim.SetBool("isRun", false);
    }

    void Update()
    {
        if (!opning && playerST.HorseMode)
        {
            #region 무브
            if (playerST.HorseMode && !NoMove)
            {
                //h = Input.GetAxisRaw("Horizontal");    //X좌표 입력받기
                v = Input.GetAxisRaw("Vertical"); //Z좌표 입력받기
                                                  //moveVec = (Vector3.forward * v) + (Vector3.right * h);
                                                  //transform.Translate(moveVec.normalized * Time.deltaTime * speed, Space.Self);
                transform.Translate(Vector3.forward * speed * v * Time.deltaTime);
            }
            #endregion

            if (v >= 0.1f) //앞
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.HorseRunSound();

                anim.SetBool("isRun", true);
            }
            else if (v <= -0.1f) //뒤
            {
                if (!FootSound.footSound.audioSource.isPlaying)
                    FootSound.footSound.HorseRunSound();
                anim.SetBool("isRun", true);
            }
            else
            {
                FootSound.footSound.audioSource.Stop();
                anim.SetBool("isRun", false);
                rigid.velocity = Vector3.zero;
            }

            if (Input.GetKeyDown(KeyCode.V) && playerST.HorseMode) //탑승해제
            {
                anim.SetBool("isRun", false);
                playerST.rigid.useGravity = true;
                playerST.HorseMode = false;
                playerST.anim.SetBool("isHorse", false);
                playerST.rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                    | RigidbodyConstraints.FreezeRotationZ;
            }

            transform.gameObject.layer = LayerMask.NameToLayer(name);
            rigid.angularVelocity = Vector3.zero;

        }



    }

    void LateUpdate()  //플레이어가 카메라를 바라봄
    {
        if (inventory.iDown || weapons.isMeteo || SkillWindow.kDown || StatWindow.tDown)
            return;

        if (playerST.HorseMode)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "WALL") //충돌되면 이동불가
        {
            NoMove = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "WALL")
        {
            NoMove = false;
        }
    }
}
