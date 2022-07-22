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
        if (tr != null)
        {
            target = tr.transform;
            playerStat = tr.GetComponent<PlayerStat>();
        }
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
                NET_PartyUI partyMember = FindObjectOfType<NET_PartyUI>();
                int memberNum = partyMember.members.Count + 1 ; //파티원수 +1(자기자신)

                switch (memberNum)
                {
                    case 1:
                        break;
                    case 2:
                        exp = exp * 0.7f;
                        break;
                    case 3:
                        exp = exp * 0.5f;
                        break;
                    case 4:
                        exp = exp * 0.4f;
                        break;
                }

                if (memberNum > 1)
                {
                    playerStat.GetComponent<NET_PartyPlayer>().partyExp(exp);
                }

                playerStat.AddExp(exp);
                
                StartCoroutine(DestroyEff());

            }
        }
    }

    IEnumerator DestroyEff()
    {
        yield return new WaitForSeconds(0.3f);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    public void Destroy()
    {
        photonView.RPC("Destroy", RpcTarget.Others);

        Destroy(gameObject);
    }
}
