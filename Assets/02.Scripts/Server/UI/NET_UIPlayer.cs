using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NET_UIPlayer : MonoBehaviourPun
{
    public AllUI allUI;
    public Camera Camera; // 카메라 지정
    public RaycastHit hit; //힛 한곳 취득해 넣어둘곳


    public GameObject menuPrefabs; // 누르면 나올 메뉴
    private Canvas uiCanvas; // 붙일UICanvas

    public static bool isMenu;

    public static bool TradeOn;

    public bool TradeComplete;
    public bool YourTradeComlete;


    private void Start()
    {
        allUI = FindObjectOfType<AllUI>();
    }




    public void Update()
    {
        if (photonView.IsMine)
        { PlayerClick(); }
    }

    public GameObject menu;
    public void DestroyMenu() {
        if (menu != null)
        {
            Destroy(menu);
            isMenu = false;
            allUI.CheckCursorLock();
        }
    }

    public GameObject Target;
    public int TradeNum;

    private void PlayerClick()
    {
        if (AllUI.ctrlDown) //컨트롤 키를 누른 상태에서 
        {

            if (Input.GetMouseButtonDown(0)) //마우스로 눌렀을때
            {
                DestroyMenu();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, maxDistance: 15f))
                {
                    if (hit.transform.gameObject.tag == "Player") //누른 캐릭터가 플레이어 이고
                    {
                        if (!hit.transform.GetComponent<PhotonView>().IsMine) // 자기의 캐릭터가 아니라면
                        {
                            menuPrefabs = Resources.Load<GameObject>("CharacterMenu");
                            uiCanvas = GameObject.Find("EnemyHp_Bar_UI").GetComponent<Canvas>();
                            menu = Instantiate<GameObject>(menuPrefabs, uiCanvas.transform);
                            var _menu = menu.GetComponent<NET_ChSelMenu>();
                            _menu.targetTr = hit.transform;
                            isMenu = true;
                            Target = hit.transform.gameObject;
                            allUI.CheckCursorLock();

                        }
                    }
                }
            }
        }
    }

    public void Trade() //트레이드 버튼을 클릭했을때
    {
        NET_UIPlayer targetNet = Target.GetComponent<NET_UIPlayer>(); //타겟을 찾는다.
        TradeNum = GetComponent<PhotonView>().ViewID; // 구분을 위해 ViewID를 넘겨 그 플레이어가 받아야 트레이드창이 열리게 한다.
        photonView.RPC("TradeNumberSet", RpcTarget.Others, TradeNum); // 트레이드를 건 플레이어의 트레이드 넘버를 모든 플레이어 에게 세팅한다.
        targetNet.TradeTargetCheck(TradeNum); //타겟된 캐릭터에게 체크하라고 명령한다.

    }

    public void TradeTargetCheck(int _TradeNum)
    {
        photonView.RPC("TradeTarget", RpcTarget.Others, _TradeNum); //본인캐릭터인지 판단한다.
    }
    [PunRPC]
    public void TradeTarget(int _TradeNum)
    {
        if (gameObject.GetComponent<PhotonView>().IsMine == true && !TradeOn) // 만약 본인의 캐릭터 라면 , 트레이드 중이 아니라면
        {
            TradeNum = _TradeNum; //트레이드 넘버를 받는다.
            FindObjectOfType<NET_TradeRecieve>().Net_RecieveOn(); // 받을건지 물어볼 창을 연다.
        }
    }
    [PunRPC]
    public void TradeNumberSet(int _TradeNum)
    {
        TradeNum = _TradeNum;
    }

    public void TradeConnect() //트레이드 수락을 눌렀을때
    {
        NET_UIPlayer[] net_uis = FindObjectsOfType<NET_UIPlayer>();
        foreach (NET_UIPlayer target in net_uis)
        {
            if (target.TradeNum == TradeNum) //타겟의 트레이드 번호가 있는지 찾는다. 있으면 트레이드 온!
            {
                target.TradeSet(); //그 캐릭터의 플레이어를 찾는다.
                photonView.RPC("TradeOnSet", RpcTarget.Others);
                photonView.RPC("TradeNumberSet", RpcTarget.Others, TradeNum); //받은 캐릭터의 트레이드 넘버를 다른 플레이어 에게 세팅한다.
                FindObjectOfType<NET_Trade>().Net_TradeOn();
            }
        }


    }
    public void TradeSet()
    {
        photonView.RPC("TradeRecieveSet", RpcTarget.Others); //타겟 캐릭터의 TradeON한다.       

    }



    [PunRPC]
    public void TradeRecieveSet()
    {

        if (photonView.IsMine) // 자기의 캐릭터가 맞으면 트레이드 연다.
        {
            TradeOn = true;
            photonView.RPC("TradeOnSet", RpcTarget.Others); //자신이 트레이드 중임을 다른 플레이어에게 밝힌다.
            FindObjectOfType<NET_Trade>().Net_TradeOn();
        }

    }

    [PunRPC]
    public void TradeOnSet() // 모든플레이어에게 지금 이캐릭터는 트레이중임을 알린다.
    {
        TradeOn = true;
    }



    [PunRPC]
    public void TradeOffSet() // 모든플레이어에게 지금 이 캐릭터는 트레이드가 종료됐음을 알린다.
    {
        TradeOn = false;
    }


    public void TradeItemConnect(int item, int slotNum, int itemCount)
    {
        NET_UIPlayer[] net_uis = FindObjectsOfType<NET_UIPlayer>();
        foreach (NET_UIPlayer target in net_uis)
        {
            if (target.TradeNum == TradeNum && !target.photonView.IsMine) //타겟의 트레이드 번호가 있는지 찾는다.
            {
                Debug.Log(item);
                target.photonView.RPC("TradeItemTarget", RpcTarget.Others, item, slotNum, itemCount); //그 캐릭터의 플레이어를 찾아 아이템을 넘긴다.


            }
        }
    }

    [PunRPC]
    public void TradeItemTarget(int item, int slotNum, int itemCount)
    {

        if (photonView.IsMine) // 자기의 캐릭터가 맞으면 아이템을 넘긴다.
        {

            FindObjectOfType<NET_Trade>().RecieveItem(item, slotNum, itemCount);
        }
    }


    public void MyCompleteButton()
    {

        NET_UIPlayer[] net_uis = FindObjectsOfType<NET_UIPlayer>();
        foreach (NET_UIPlayer target in net_uis)
        {
            if (target.TradeNum == TradeNum) //타겟의 트레이드 번호가 있는지 찾는다. 있으면 컴플리트!
            {

                target.photonView.RPC("CompleteSet", RpcTarget.Others, TradeComplete);

            }
        }

    }

    [PunRPC]
    public void CompleteSet(bool _tradeComplete)
    {
        if (photonView.IsMine)
        {

            YourTradeComlete = _tradeComplete;
            FindObjectOfType<NET_Trade>().YourCompleteButton(_tradeComplete);
        }

    }


    public void SlotNumCheck(bool Check)
    {

        NET_UIPlayer[] net_uis = FindObjectsOfType<NET_UIPlayer>();
        foreach (NET_UIPlayer target in net_uis)
        {
            if (target.TradeNum == TradeNum) //타겟의 트레이드 번호가 있는지 찾는다. 있으면 체크보내기!
            {
                target.photonView.RPC("CheckSlot", RpcTarget.Others, Check);

            }
        }
    }

    [PunRPC]
    public void CheckSlot(bool _check)
    {
        if (photonView.IsMine)
        {
            if (_check && FindObjectOfType<NET_Trade>().SlotCheck) //상대와 내 것 모두 슬롯 체크가 완료 되면 트레이드 성공
            {
                FindObjectOfType<NET_Trade>().SuccessTrade();
            }
            else   //둘중 하나라도 안되면 실패
            {

                FindObjectOfType<NET_Trade>().FailTrade();

            }


        }
    }

    public void TradeResetCheck()
    {
       

        NET_UIPlayer[] net_uis = FindObjectsOfType<NET_UIPlayer>();
        foreach (NET_UIPlayer target in net_uis)
        {
            if (target.TradeNum == TradeNum) //타겟의 트레이드 번호가 있는지 찾는다. 있으면 체크보내기!
            {
                target.photonView.RPC("TradeReset", RpcTarget.Others);
                TradeNum = 0;
                photonView.RPC("TradeNumberSet", RpcTarget.Others, TradeNum);

            }
        }
    }

    [PunRPC]
    public void TradeReset()
    {
        if (photonView.IsMine && TradeNum!=0)
        {
            FindObjectOfType<NET_Trade>().TradeReset();
            TradeNum = 0;
            photonView.RPC("TradeNumberSet", RpcTarget.Others, TradeNum);
        }
    }

}
