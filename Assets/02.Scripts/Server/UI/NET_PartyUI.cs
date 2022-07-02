using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NET_PartyUI : MonoBehaviourPun
{
    [SerializeField]
    GameObject NET_partyUI;
    [SerializeField]
    GameObject Member;
    [SerializeField]
    List<GameObject> members = new List<GameObject>();
    [SerializeField]
    NET_PartyPlayer net_PartyPlayer;


    private void Start()
    {
        Member = Resources.Load<GameObject>("Member");
        NET_PartyPlayer[] net_parplayes = GameObject.FindObjectsOfType<NET_PartyPlayer>();
        foreach (NET_PartyPlayer myparPlaye in net_parplayes)
        {
            if (myparPlaye.GetComponent<PhotonView>().IsMine)
            {
                net_PartyPlayer = myparPlaye;
                break;
            }
        }
    }

    public void PartyOn()
    {
        if (!NET_partyUI.activeSelf)
        {
            NET_partyUI.SetActive(true);
        }
    }

    public void PartyOff() 
    {
        NET_partyUI.SetActive(false);
    }

    public void IstancePartyMem(int memNum,string NickName,int playerOrederNum)
    {
        GameObject member = Instantiate(Member);
        Debug.Log(member);
        member.GetComponent<NET_Member>().NameSet(memNum,NickName, playerOrederNum);
        members.Add(member);

    }

    public void InfoRecieve(float hp , float mp ,int memID)
    {
        foreach(GameObject member in members)
        {
            if(member.GetComponent<NET_Member>().id == memID)
            {
                member.GetComponent<NET_Member>().BarSet(hp, mp);
            }
        }
    }

    public void PartyExit()//파티탈퇴 버튼
    {
        net_PartyPlayer.PartyExit();
        foreach(GameObject member in members)
        {
            Destroy(member);
        }
        members.Clear();
    }

    public void MemberExit(int id)
    {
        foreach(GameObject member in members)
        {
            if (member.GetComponent<NET_Member>().id == id)
            {
                members.Remove(member);
                Destroy(member);
                break;
            }
        }
        if(members.Count == 0)
        {
            net_PartyPlayer.PartyCodeReset();
        }
    }

    public void ReaderExit(int id)
    {
        MemberExit(id);

        if (members.Count == 0)
            return;

        int min = net_PartyPlayer.playerOrderNum;
        int minindex = -1;
        
        for(int i = 0; i < members.Count; i++)
        {
            if(min > members[i].GetComponent<NET_Member>().orderNum)
            {
                min = members[i].GetComponent<NET_Member>().orderNum;
                minindex = i;
            }
        }
        if (minindex == -1)
        {
            net_PartyPlayer.partyNum = net_PartyPlayer.photonView.ViewID;
            return;
        }
        net_PartyPlayer.partyNum = members[minindex].GetComponent<NET_Member>().id;
        members[minindex].GetComponent<NET_Member>().LeaderSet();
        
    }

    public int newPlayerOrderNum() 
    {
        int maxOrder= net_PartyPlayer.playerOrderNum;
        foreach(GameObject member in members)
        {
            if(member.GetComponent<NET_Member>().orderNum >maxOrder)
            {
                maxOrder = member.GetComponent<NET_Member>().orderNum;
            }
        }
        return maxOrder + 1;    //모든파티멤버의 최고 순서값에 1을 더한값을 리턴한다.


    }
}
