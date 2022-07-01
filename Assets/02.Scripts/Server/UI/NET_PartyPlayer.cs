using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NET_PartyPlayer : MonoBehaviourPun
{

    public int alreadyPartyNum; //������ ���� ��Ƽ ��ȣ
    public int partyNum;       // ��Ƽ ��ȣ
    public int partyOrderNum=1;  //��Ƽ �� ��ȣ
    public int playerOrderNum=0; //�÷��̾� ���� ��ȣ
    public  bool isParty;   //��Ƽ ���ΰ�?
    public bool SendPartyMessage;

    NET_UIPlayer net_UIPlayer;
    NET_PartyRecieve net_partyRecieve;
    NET_PartyUI net_partyUI;
    PlayerStat playerStat;
    private void Awake()
    {
        net_UIPlayer= GetComponent<NET_UIPlayer>();
        net_partyRecieve = FindObjectOfType<NET_PartyRecieve>();
        net_partyUI = FindObjectOfType<NET_PartyUI>();
        playerStat = FindObjectOfType<PlayerStat>();
        partyNum = 0;
    }

    public void PartyInvite() // ��Ƽ �ʴ� ��ư Ŭ��
    {
        if (isParty && partyNum == 0) // ��Ƽ �ʴ����� �޾������� �ٸ�������� ��Ƽ ������ �Ұ�
            return;
        if (isParty)    //��Ƽ ���϶�
        {
            net_UIPlayer.Target.GetComponent<NET_PartyPlayer>().photonView.RPC("PartyRecieveWindowOn", RpcTarget.Others, net_UIPlayer.Target.GetComponent<PhotonView>().ViewID, partyNum); //Ÿ���Ǿ��̵�� ��Ƽ ��ȣ ����
        }
        else //��Ƽ���� �ƴҶ�
        {
            net_UIPlayer.Target.GetComponent<NET_PartyPlayer>().photonView.RPC("PartyRecieveWindowOn", RpcTarget.Others, net_UIPlayer.Target.GetComponent<PhotonView>().ViewID, photonView.ViewID); //Ÿ���Ǿ��̵�� �ڽ��� ���̵𺸳�
        }
        SendPartyMessage = true;
    }
    
    [PunRPC]
    public void PartyRecieveWindowOn(int viewID , int _partyNum)
    {
       
        if (viewID == photonView.ViewID && !isParty && !SendPartyMessage) // Ÿ���� ���̵� ���� �÷��̾�� ��Ƽ �ʴ����� ����
        {
         
            isParty = true;
            net_partyRecieve.PartyRecieveWindowOn();
            alreadyPartyNum = _partyNum;
        }
    }

    public void partyFail()
    {
        NET_PartyPlayer[] player = FindObjectsOfType<NET_PartyPlayer>(); 
        foreach (NET_PartyPlayer mem in player)
        {
            if (mem.photonView.ViewID == alreadyPartyNum) //��Ƽ�� ���� �÷��̾�� ��ȣ������.
            {
              
                mem.photonView.RPC("PartyMemberRecieve", RpcTarget.Others);
                break;
            }
        }
    }

   


    [PunRPC]
    public void fail()
    {
        if (photonView.IsMine)
        {
            SendPartyMessage = false;
        }
    }


    public void PartyRecieve() //��Ƽ �ʴ����� ������
    {
        partyNum = alreadyPartyNum; // ��Ƽ �ѹ� �ٲ�.
        photonView.RPC("AllPartyMember", RpcTarget.Others, photonView.ViewID , partyNum); //��� ������� �ڽ��� �������� �˸�.(�г��� ���⼭ �ѱ��)
    }

    [PunRPC]
    public void AllPartyMember(int PartyMember, int _partyNum) // ��� ������� �� ����� �������� �˸�
    {
       
        NET_PartyPlayer[] partyplayer =  FindObjectsOfType<NET_PartyPlayer>();
        
        foreach(NET_PartyPlayer partyMem in partyplayer)
        {
            if (partyMem.photonView.IsMine) {

                

                if (partyMem.photonView.ViewID == _partyNum) //��Ƽ�� ���̶��
                {
                    partyMem.isParty = true;
                    partyMem.partyNum = partyMem.photonView.ViewID;
                    

                    NET_PartyPlayer[] player = FindObjectsOfType<NET_PartyPlayer>(); //���ε��� ��Ƽ����� ã������ ���
                    foreach (NET_PartyPlayer mem in player)
                    {
                        if (mem.photonView.ViewID == PartyMember) //���ε��� ��Ƽ������ ��Ƽ ������ȣ �Ѱ��ش�.
                        {
                           
                            mem.photonView.RPC("PartyOrderNum", RpcTarget.Others, partyOrderNum); 
                        }
                    }

                }
                Debug.Log(1);
                if (partyMem.partyNum == _partyNum) //��� ��Ƽ������ ��Ƽ���� ��ȣ�� 1�������Ѵ�.
                {

                    partyOrderNum++;
                    net_partyUI.PartyOn();
                    net_partyUI.IstancePartyMem(PartyMember);
                    Debug.Log(2);
                    NET_PartyPlayer[] player2 = FindObjectsOfType<NET_PartyPlayer>(); //���� ���� ������� �ڽ��� ������ �����ش�.
                    foreach (NET_PartyPlayer mem2 in player2)
                    {
                        Debug.Log(3);
                        if (mem2.photonView.ViewID == PartyMember) 
                        {
                            Debug.Log(4);
                            mem2.photonView.RPC("PartyMemberRecieve", RpcTarget.Others, partyMem.photonView.ViewID);
                        }
                    }
                }
            }
        }

    }

    [PunRPC]
    public void PartyOrderNum(int _partyOrderNum) //��Ƽ�켱������ ���� ����� �ڱⲨ�� ��Ƽ�켱���� ��ȣ �����Ѵ�.
    {
        if (photonView.IsMine)
        {
            playerOrderNum = _partyOrderNum;
            partyOrderNum = _partyOrderNum;
            _partyOrderNum++;
            net_partyUI.PartyOn();
        }
             
    }
    [PunRPC]
    public void PartyMemberRecieve(int memCode)
    {
        if (photonView.IsMine)
        {
            net_partyUI.IstancePartyMem(memCode);
        }
    }


    private float DelayTime = 0.5f;
    private float prevtime=0f;

    private void Update() //��Ƽ�� ������ �����ð����� �ڽ��� ĳ���� ���� ������.
    {

        if (isParty && partyNum !=0)
        {
            prevtime += Time.deltaTime;
            if (prevtime > DelayTime) //�ð� ���ֱ�
            {
                SendInfo();
                prevtime = 0f;
            }
        }

    }

    public void SendInfo()
    {
        if(photonView.IsMine) //�ڽ��� ĳ����
        {
            float HpPer = playerStat._Hp / playerStat._MAXHP;
            float MpPer = playerStat._Mp / playerStat._MAXMP;
            photonView.RPC("infoRecieve", RpcTarget.Others, partyNum, HpPer, MpPer , photonView.ViewID);
        }
    }

    [PunRPC]
    public void infoRecieve(int _partyNum, float _HpPer,float _MpPer , int memberID)
    {
        NET_PartyPlayer[] player = FindObjectsOfType<NET_PartyPlayer>(); 
        foreach (NET_PartyPlayer mem in player)
        {
            if (mem.photonView.IsMine && mem.partyNum == _partyNum) 
            {
                net_partyUI.InfoRecieve(_HpPer , _MpPer, memberID);


            }
        }
    }
}
