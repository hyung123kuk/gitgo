using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NET_PartyPlayer : MonoBehaviourPun
{

    public int alreadyPartyNum; //받을지 정할 파티 번호
    public int partyNum;       // 파티 번호
    public int partyOrderNum=1;  //파티 총 번호
    public int playerOrderNum=0; //플레이어 순서 번호
    public  bool isParty;   //파티 중인가?
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

    public void PartyInvite() // 파티 초대 버튼 클릭
    {
        if (isParty && partyNum == 0) // 파티 초대장을 받았을때는 다른사람에게 파티 보내기 불가
            return;
        if (isParty)    //파티 중일때
        {
            net_UIPlayer.Target.GetComponent<NET_PartyPlayer>().photonView.RPC("PartyRecieveWindowOn", RpcTarget.Others, net_UIPlayer.Target.GetComponent<PhotonView>().ViewID, partyNum); //타겟의아이디와 파티 번호 보냄
        }
        else //파티중이 아닐때
        {
            net_UIPlayer.Target.GetComponent<NET_PartyPlayer>().photonView.RPC("PartyRecieveWindowOn", RpcTarget.Others, net_UIPlayer.Target.GetComponent<PhotonView>().ViewID, photonView.ViewID); //타겟의아이디와 자신의 아이디보냄
        }
        SendPartyMessage = true;
    }
    
    [PunRPC]
    public void PartyRecieveWindowOn(int viewID , int _partyNum)
    {
       
        if (viewID == photonView.ViewID && !isParty && !SendPartyMessage) // 타겟의 아이디를 가진 플레이어는 파티 초대장을 받음
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
            if (mem.photonView.ViewID == alreadyPartyNum) //파티를 보낸 플레이어에게 신호보낸다.
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


    public void PartyRecieve() //파티 초대장을 수락함
    {
        partyNum = alreadyPartyNum; // 파티 넘버 바뀜.
        photonView.RPC("AllPartyMember", RpcTarget.Others, photonView.ViewID , partyNum); //모든 멤버에게 자신이 들어왔음을 알림.(닉네임 여기서 넘긴다)
    }

    [PunRPC]
    public void AllPartyMember(int PartyMember, int _partyNum) // 모든 멤버에게 새 멤버가 들어왔음을 알림
    {
       
        NET_PartyPlayer[] partyplayer =  FindObjectsOfType<NET_PartyPlayer>();
        
        foreach(NET_PartyPlayer partyMem in partyplayer)
        {
            if (partyMem.photonView.IsMine) {

                

                if (partyMem.photonView.ViewID == _partyNum) //파티의 장이라면
                {
                    partyMem.isParty = true;
                    partyMem.partyNum = partyMem.photonView.ViewID;
                    

                    NET_PartyPlayer[] player = FindObjectsOfType<NET_PartyPlayer>(); //새로들어온 파티멤버를 찾기위해 사용
                    foreach (NET_PartyPlayer mem in player)
                    {
                        if (mem.photonView.ViewID == PartyMember) //새로들어온 파티원에게 파티 순서번호 넘겨준다.
                        {
                           
                            mem.photonView.RPC("PartyOrderNum", RpcTarget.Others, partyOrderNum); 
                        }
                    }

                }
                Debug.Log(1);
                if (partyMem.partyNum == _partyNum) //모든 파티원들의 파티오더 번호는 1씩증가한다.
                {

                    partyOrderNum++;
                    net_partyUI.PartyOn();
                    net_partyUI.IstancePartyMem(PartyMember);
                    Debug.Log(2);
                    NET_PartyPlayer[] player2 = FindObjectsOfType<NET_PartyPlayer>(); //새로 들어온 멤버에게 자신의 정보를 보내준다.
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
    public void PartyOrderNum(int _partyOrderNum) //파티우선순위를 받은 멤버가 자기꺼면 파티우선순위 번호 세팅한다.
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

    private void Update() //파티가 있을때 일정시간마다 자신의 캐릭터 정보 보낸다.
    {

        if (isParty && partyNum !=0)
        {
            prevtime += Time.deltaTime;
            if (prevtime > DelayTime) //시간 재주기
            {
                SendInfo();
                prevtime = 0f;
            }
        }

    }

    public void SendInfo()
    {
        if(photonView.IsMine) //자신의 캐릭터
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
