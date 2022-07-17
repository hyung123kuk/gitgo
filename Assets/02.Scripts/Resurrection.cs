using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrection : MonoBehaviour
{
    public Resurrection resurrection;

    public Transform Town; //������ ����
    public Transform Dunjeon;  //������ ����

    public int WherePos; //0 : �����չ��� 1:�����չ���

    [SerializeField]
    private PlayerST playerst;
    [SerializeField]
    private Bar bar;
    [SerializeField]
    private GameUI gameUI;

    void Start()
    {
        gameUI = FindObjectOfType<GameUI>();
        bar = FindObjectOfType<Bar>();
        playerst = GetComponent<PlayerST>();
        resurrection = this;
        Town = GameObject.Find("Mausoleum-Town").transform.GetChild(0).transform;
        Dunjeon = GameObject.Find("Mausoleum-Dunjeon").transform.GetChild(0).transform;
    }


    void Update()
    {
        float Towndist = Vector3.Distance(transform.position, Town.position);
        float Dunjeondist = Vector3.Distance(transform.position, Dunjeon.position);


        if (Towndist > Dunjeondist)
        {
            WherePos = 0;
        }
        else if (Towndist < Dunjeondist)
        {
            WherePos = 1;
        }

    }

   public IEnumerator TownResurrection() //���� ��Ȱ
    {
        transform.position = Town.position;
        yield return new WaitForSeconds(0.1f);
        playerst.PlayerResurrection();
        playerst.DiePRC(false,true);
        playerst.SelectPlayer.enabled = true;
        playerst.rigid.useGravity = true;
        playerst.anim.SetBool("isDie", false);
        yield return new WaitForSeconds(0.1f);
        gameUI.bar_set();
    }

    public IEnumerator DunjeonResurrection() //���� ��Ȱ
    {
        transform.position = Dunjeon.position;
        yield return new WaitForSeconds(0.1f);
        playerst.PlayerResurrection();
        playerst.DiePRC(false, true);
        playerst.SelectPlayer.enabled = true;
        playerst.rigid.useGravity = true;
        playerst.anim.SetBool("isDie", false);
        yield return new WaitForSeconds(0.1f);
        gameUI.bar_set();
    }
}
