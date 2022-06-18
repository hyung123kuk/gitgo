using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
public class GetEff : MonoBehaviourPun
{
    [SerializeField]
    Transform target;
    NavMeshAgent navi;
    [SerializeField]
    PlayerStat playerStat;
    public float exp;
    void Start()
    {
        navi = GetComponent<NavMeshAgent>();
 

    }

    [PunRPC]
    public void SetExp(float _exp)
    {
        photonView.RPC("SetExp", RpcTarget.Others, _exp);
        exp = _exp;
    }

    public void Target(GameObject tr)
    {
        target = tr.transform;
        playerStat = tr.GetComponent<PlayerStat>();
    }

    void Update()
    {
        if (target == null)
            return;

         navi.SetDestination(target.position);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (playerStat != null)
        {
            if (other.gameObject.tag == "Player" && other.gameObject == target.gameObject)
            {
                playerStat.AddExp(exp);
                StartCoroutine(DestroyEff());

            }
        }
    }

    IEnumerator DestroyEff()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy();
    }

    [PunRPC]
    public void Destroy()
    {
        photonView.RPC("Destroy", RpcTarget.Others);

        Destroy(gameObject);
    }
}
