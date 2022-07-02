using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NET_Member : MonoBehaviourPun
{
    Image Hp_bar;
    Image Mp_bar;
    Text Name;
    public int id;

    public int orderNum;

    public Image leader;

    public NET_PartyPlayer partyplayer;

    public NET_PartyPlayer myPartyNet;
    void Start()
    {
       transform.parent = GameObject.Find("NET_Party").transform.GetChild(0).GetChild(1);

        Hp_bar = transform.GetChild(2).GetComponent<Image>();
        Mp_bar = transform.GetChild(3).GetComponent<Image>();

        NET_PartyPlayer[] allmember = FindObjectsOfType<NET_PartyPlayer>();
        foreach (NET_PartyPlayer member in allmember)
        {
            if (member.photonView.ViewID == id)
            {
                partyplayer = member;
            }
            if (member.photonView.IsMine)
            {
                myPartyNet = member;
            }
        }

    }


    private void Update()
    {
        if (partyplayer == null) //��Ƽ���� �������� �ϴ¼���.
        {

                if(id == myPartyNet.partyNum) //��Ƽ���� ����
                {
                    FindObjectOfType<NET_PartyUI>().ReaderExit(id);
                }
                else //�Ϲ� ĳ���Ͱ� ����
                {
                    FindObjectOfType<NET_PartyUI>().MemberExit(id);
                }

        }

    }

    public void NameSet(int ID , string NickName, int _orderNum)
    {

        if (myPartyNet == null)
        {
            NET_PartyPlayer[] allmember = FindObjectsOfType<NET_PartyPlayer>();
            foreach (NET_PartyPlayer member in allmember)
            {
                if (member.photonView.IsMine)
                {
                    myPartyNet = member;
                }
            }
        }


        Name = transform.GetChild(1).GetComponent<Text>();
        orderNum = _orderNum;
        id = ID;
        Name.text = NickName;
        LeaderSet();
    }
    public void BarSet(float hp , float mp) // �Ҽ������� ����Ѵ�.
    {
        if(!Hp_bar)
        {
            Hp_bar = transform.GetChild(2).GetComponent<Image>();
        }
        if (!Mp_bar)
        {
            Mp_bar = transform.GetChild(3).GetComponent<Image>();
        }
        Hp_bar.fillAmount = hp;
        Mp_bar.fillAmount = mp;
    }

    public void LeaderSet()
    {



        leader = transform.GetChild(4).GetComponent<Image>();
        if (myPartyNet.partyNum == id)
        {
            leader.enabled = true;
        }
    }

}
