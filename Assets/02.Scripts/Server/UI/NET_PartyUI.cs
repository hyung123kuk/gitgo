using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NET_PartyUI : MonoBehaviour
{
    [SerializeField]
    GameObject NET_partyUI;
    [SerializeField]
    GameObject Member;
    [SerializeField]
    List<GameObject> members = new List<GameObject>();

    private void Start()
    {
        Member = Resources.Load<GameObject>("Member");
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

    public void IstancePartyMem(int memNum,string NickName)
    {
        GameObject member = Instantiate(Member);
        Debug.Log(member);
        member.GetComponent<NET_Member>().NameSet(memNum,NickName);
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

}
