using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrection : MonoBehaviour
{
    public Resurrection resurrection;

    public Transform Town; //마을앞 무덤
    public Transform Dunjeon;  //던전앞 무덤

    public int WherePos; //0 : 던전앞무덤 1:마을앞무덤

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

   public IEnumerator TownResurrection() //마을 부활
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

    public IEnumerator DunjeonResurrection() //던전 부활
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
