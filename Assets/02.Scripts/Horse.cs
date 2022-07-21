using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Horse : MonoBehaviourPun
{
    public float speed = 30f;
    Vector3 moveVec;

    private float v = 0f;
    private Rigidbody rigid;
    public Animator anim;
    public bool opning;
    public bool NoMove;

    Camera _camera;
    CapsuleCollider _controller;
    public PlayerST playerST;
    public float smoothness = 10f;

    void Awake()
    {


        //HorseSpawnAnim();
    }

    private void OnEnable()
    {


    }

    private void Start()
    {
        PlayerST[] playerSt = GameObject.FindObjectsOfType<PlayerST>();
        foreach (PlayerST myplayerst in playerSt)
        {
            if (myplayerst.GetComponent<PhotonView>().IsMine)
            {
                playerST = myplayerst;
                break;
            }
        }
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        rigid = GetComponent<Rigidbody>();
        _camera = Camera.main;
        _controller = this.GetComponent<CapsuleCollider>();

    }
    IEnumerator HorseActive()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    void HorseSpawnAnim()
    {
        opning = true;
        //transform.LookAt(playerST.horsepos2.position);
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
        if (!photonView.IsMine)
        {
            return;
        }
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

            }
            if (Input.GetKeyDown(KeyCode.V) && playerST.HorseMode) //탑승해제
                HorseModeDis();


            //rigid.angularVelocity = Vector3.zero;

        }
    }
    public void HorseModeDis()
    {
        anim.SetBool("isRun", false);
        playerST.rigid.useGravity = true;
        playerST.HorseMode = false;
        playerST.anim.SetBool("isHorse", false);
        playerST.transform.parent = null;
        playerST.rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
            | RigidbodyConstraints.FreezeRotationZ;
    }
    void LateUpdate()  //플레이어가 카메라를 바라봄
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (inventory.iDown || SkillWindow.kDown || StatWindow.tDown)
            return;

        if (playerST.HorseMode)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "WALL") //충돌되면 이동불가
            {
                NoMove = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "WALL")
            {
                NoMove = false;
            }
        }
    }


}
