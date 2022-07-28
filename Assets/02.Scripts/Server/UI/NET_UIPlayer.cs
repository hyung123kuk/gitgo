using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NET_UIPlayer : MonoBehaviourPun
{
    public AllUI allUI;
    public Camera Camera; // ī�޶� ����
    public RaycastHit hit; //�� �Ѱ� ����� �־�Ѱ�


    public GameObject menuPrefabs; // ������ ���� �޴�
    private Canvas uiCanvas; // ����UICanvas

    public static bool isMenu;

    public static bool TradeOn;

    public bool TradeComplete;
    public bool YourTradeComlete;
    PlayerST playerST;
    PlayerStat playerStat;


    private void Start()
    {
        allUI = FindObjectOfType<AllUI>();
        playerST = GetComponent<PlayerST>();
        playerStat = GetComponent<PlayerStat>();
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
        if (AllUI.ctrlDown) //��Ʈ�� Ű�� ���� ���¿��� 
        {

            if (Input.GetMouseButtonDown(0)) //���콺�� ��������
            {
                DestroyMenu();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, maxDistance: 15f))
                {
                    if (hit.transform.gameObject.tag == "Player") //���� ĳ���Ͱ� �÷��̾� �̰�
                    {
                        if (!hit.transform.GetComponent<PhotonView>().IsMine) // �ڱ��� ĳ���Ͱ� �ƴ϶��
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

    #region �ŷ� ���� �ڵ�

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////  /////////////////////////////////////////////////  ///////  ///////////////////////
    ////////////////////////////////////////////////////  /////////////////////////////////////////////////  ///////  ///////////////////////
    //////////////////                         /////////  ///////////////////                  ////////////  ///////  ///////////////////////
    ///////////////////////////////////////   //////////  ///////////////////////////////////  ////////////  ///////  ///////////////////////
    ///////////////////////////////////////  ///////////  ///////////////////////////////////  ////////////  ///////  ///////////////////////
    //////////////////////////////////////  ////////////  ////////////////////                 ////////////  ///////  ///////////////////////
    /////////////////////////////////////  ///////        ///////////////////  ////////////////////////////           ///////////////////////
    ////////////////////////////////////  //////////////  ///////////////////  ////////////////////////////  ///////  ///////////////////////
    ///////////////////////////////////  ///////////////  ///////////////////                  ////////////  ///////  ///////////////////////
    //////////////////////////////////  ////////////////  /////////////////////////////////////////////////  ///////  ///////////////////////
    /////////////////////////////////  /////////////////  /////////////////////////////////////////////////  ///////  ///////////////////////
    ////////////////////////////////  //////////////////  /////////////////////////////////////////////////  ///////  ///////////////////////
    ///////////////////////////////  ///////////////////  /////////////////////////////////////////////////  ///////  ///////////////////////
    ////////////////////////////////////////////////////  /////////////////////////////////////////////////  ///////  ///////////////////////
    ////////////////////////////////////////////////////  /////////////////////////////////////////////////  ///////  ///////////////////////
    ////////////////////////////////////////////////////  /////////////////////////////////////////////////  ///////  ///////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    public void Trade() //Ʈ���̵� ��ư�� Ŭ��������
    {
        NET_UIPlayer targetNet = Target.GetComponent<NET_UIPlayer>(); //Ÿ���� ã�´�.
        TradeNum = GetComponent<PhotonView>().ViewID; // ������ ���� ViewID�� �Ѱ� �� �÷��̾ �޾ƾ� Ʈ���̵�â�� ������ �Ѵ�.
        photonView.RPC("TradeNumberSet", RpcTarget.Others, TradeNum); // Ʈ���̵带 �� �÷��̾��� Ʈ���̵� �ѹ��� ��� �÷��̾� ���� �����Ѵ�.
        targetNet.TradeTargetCheck(TradeNum); //Ÿ�ٵ� ĳ���Ϳ��� üũ�϶�� ����Ѵ�.

    }


    public void TradeTargetCheck(int _TradeNum)
    {
        photonView.RPC("TradeTarget", RpcTarget.Others, _TradeNum); //����ĳ�������� �Ǵ��Ѵ�.
    }
    [PunRPC]
    public void TradeTarget(int _TradeNum)
    {
        if (gameObject.GetComponent<PhotonView>().IsMine == true && !TradeOn) // ���� ������ ĳ���� ��� , Ʈ���̵� ���� �ƴ϶��
        {
            TradeNum = _TradeNum; //Ʈ���̵� �ѹ��� �޴´�.
            FindObjectOfType<NET_TradeRecieve>().Net_RecieveOn(); // �������� ��� â�� ����.
        }
    }
    [PunRPC]
    public void TradeNumberSet(int _TradeNum)
    {
        TradeNum = _TradeNum;
    }

    public void TradeConnect() //Ʈ���̵� ������ ��������
    {
        NET_UIPlayer[] net_uis = FindObjectsOfType<NET_UIPlayer>();
        foreach (NET_UIPlayer target in net_uis)
        {
            if (target.TradeNum == TradeNum) //Ÿ���� Ʈ���̵� ��ȣ�� �ִ��� ã�´�. ������ Ʈ���̵� ��!
            {
                target.TradeSet(); //�� ĳ������ �÷��̾ ã�´�.
                photonView.RPC("TradeOnSet", RpcTarget.Others);
                photonView.RPC("TradeNumberSet", RpcTarget.Others, TradeNum); //���� ĳ������ Ʈ���̵� �ѹ��� �ٸ� �÷��̾� ���� �����Ѵ�.
                FindObjectOfType<NET_Trade>().Net_TradeOn();
            }
        }


    }
    public void TradeSet()
    {
        photonView.RPC("TradeRecieveSet", RpcTarget.Others); //Ÿ�� ĳ������ TradeON�Ѵ�.       

    }



    [PunRPC]
    public void TradeRecieveSet()
    {

        if (photonView.IsMine) // �ڱ��� ĳ���Ͱ� ������ Ʈ���̵� ����.
        {
            TradeOn = true;
            photonView.RPC("TradeOnSet", RpcTarget.Others); //�ڽ��� Ʈ���̵� ������ �ٸ� �÷��̾�� ������.
            FindObjectOfType<NET_Trade>().Net_TradeOn();
        }

    }

    [PunRPC]
    public void TradeOnSet() // ����÷��̾�� ���� ��ĳ���ʹ� Ʈ���������� �˸���.
    {
        TradeOn = true;
    }



    [PunRPC]
    public void TradeOffSet() // ����÷��̾�� ���� �� ĳ���ʹ� Ʈ���̵尡 ��������� �˸���.
    {
        TradeOn = false;
    }


    public void TradeItemConnect(int item, int slotNum, int itemCount)
    {
        NET_UIPlayer[] net_uis = FindObjectsOfType<NET_UIPlayer>();
        foreach (NET_UIPlayer target in net_uis)
        {
            if (target.TradeNum == TradeNum && !target.photonView.IsMine) //Ÿ���� Ʈ���̵� ��ȣ�� �ִ��� ã�´�.
            {
                Debug.Log(item);
                target.photonView.RPC("TradeItemTarget", RpcTarget.Others, item, slotNum, itemCount); //�� ĳ������ �÷��̾ ã�� �������� �ѱ��.


            }
        }
    }

    [PunRPC]
    public void TradeItemTarget(int item, int slotNum, int itemCount)
    {

        if (photonView.IsMine) // �ڱ��� ĳ���Ͱ� ������ �������� �ѱ��.
        {

            FindObjectOfType<NET_Trade>().RecieveItem(item, slotNum, itemCount);
        }
    }


    public void MyCompleteButton()
    {

        NET_UIPlayer[] net_uis = FindObjectsOfType<NET_UIPlayer>();
        foreach (NET_UIPlayer target in net_uis)
        {
            if (target.TradeNum == TradeNum && !target.photonView.IsMine) //Ÿ���� Ʈ���̵� ��ȣ�� �ִ��� ã�´�. ������ ���ø�Ʈ!
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
            if (target.TradeNum == TradeNum && !target.photonView.IsMine) //Ÿ���� Ʈ���̵� ��ȣ�� �ִ��� ã�´�. ������ üũ������!
            {
                target.photonView.RPC("CheckSlot", RpcTarget.Others, Check);

            }
        }
    }

    [PunRPC]
    public IEnumerator CheckSlot(bool _check)
    {
        if (photonView.IsMine)
        {
            while (!FindObjectOfType<NET_Trade>().SlotCheckSuccess)
            {
                yield return null;
            }
            

            

            Debug.Log(_check);
            Debug.Log(FindObjectOfType<NET_Trade>().SlotCheck);

           

            if (_check && FindObjectOfType<NET_Trade>().SlotCheck) //���� �� �� ��� ���� üũ�� �Ϸ� �Ǹ� Ʈ���̵� ����
            {
                FindObjectOfType<NET_Trade>().SuccessTrade();
            }
            else   //���� �ϳ��� �ȵǸ� ����
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
            if (target.TradeNum == TradeNum && !target.photonView.IsMine) //Ÿ���� Ʈ���̵� ��ȣ�� �ִ��� ã�´�. ������ üũ������!
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
            FindObjectOfType<NET_Trade>().FailTrade();
            TradeNum = 0;
            photonView.RPC("TradeNumberSet", RpcTarget.Others, TradeNum);
        }
    }

    public void GoldSetting(int gold)
    {
       
        NET_UIPlayer[] net_uis = FindObjectsOfType<NET_UIPlayer>();
        foreach (NET_UIPlayer target in net_uis)
        {
            if (target.TradeNum == TradeNum && !target.photonView.IsMine) //Ÿ���� Ʈ���̵� ��ȣ�� �ִ��� ã�´�. ������ ��庸����!
            {

                target.photonView.RPC("GoldSet", RpcTarget.Others, gold);

            }
        }


    }

    [PunRPC]
    public void GoldSet(int _gold)
    {


        if (photonView.IsMine)
        {

            FindObjectOfType<NET_Trade>().YourGoldSet(_gold);

        }
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////  ///////////////////////////////////////////////////////////////////////////////////////////////
    //////////                   ///////////  //////////////////////////////  ////////////  /////////////////////////////////////////////////
    /////////////////     //////////////////  //////////////////////////////  ////////////  /////////////////////////////////////////////////
    ////////////////  //  //////////////////  //////////////////////////////                /////////////////////////////////////////////////
    ///////////////  ///  ///////             //////////////////////////////  ////////////  /////////////////////////////////////////////////
    //////////////  ////  //////////////////  //////////////////////////////                /////////////////////////////////////////////////
    /////////////  /////  //////////////////  ///////////////////////////////////////////////////////////////////////////////////////////////
    ////////////  //////  //////////////////  /////////////////////////////////////  ////////////////////////////////////////////////////////
    ////////////////////////////////////////  /////////////////////////////////////  ////////////////////////////////////////////////////////
    ////////////////////////////////////////  /////////////////////////////////////  ////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////  ////////////////////////////////////////////////////////
    ////////////////////////////////    ////////////////////////////////////                 ////////////////////////////////////////////////
    //////////////////////////////  ///  ////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////  ////  ////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////  ////  ////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////     //////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





    public void Info()// ���� ��ư�� Ŭ�� ������
    {
        NET_UIPlayer tagetNet = Target.GetComponent<NET_UIPlayer>();
       
        tagetNet.photonView.RPC("TargetInfo", RpcTarget.Others, GetComponent<PhotonView>().ViewID);
    }
    [PunRPC]
    public void TargetInfo(int ID)
    {
      
        if (GetComponent<PhotonView>().IsMine)
        {
            


            NET_UIPlayer[] players = FindObjectsOfType<NET_UIPlayer>();
            foreach(NET_UIPlayer player in players)
            {
                if (player.GetComponent<PhotonView>().ViewID == ID)
                {
                    Debug.Log(ID);
                    int Ch_type = (int)playerST.CharacterType;
                    player.photonView.RPC("RecieveInfo", RpcTarget.Others, Ch_type, playerStat.Level, playerStat._STR, playerStat._DEX, playerStat._INT, playerStat._DAMAGE, 
                                                                playerStat._DEFENCE, playerStat._MOVE_SPEED, playerStat._SKILL_COOLTIME_DEC_PER, playerStat._SKILL_ADD_DAMAGE_PER, playerStat._CRITICAL_PROBABILITY, playerStat._CRITICAL_ADD_DAMAGE_PER);
                }
            }

        }
    }

    [PunRPC]
    public void RecieveInfo( int  CharacterType, int  Level,float  _STR,float  _DEX,float _INT,float _DAMAGE,
                            float _DEFENCE, float _MOVE_SPEED, float _SKILL_COOLTIME_DEC_PER, float _SKILL_ADD_DAMAGE_PER, float _CRITICAL_PROBABILITY, float _CRITICAL_ADD_DAMAGE_PER)
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            FindObjectOfType<NET_STAT>().Net_StatWindowOn();
            FindObjectOfType<NET_STAT>().SetStat(CharacterType, Level, _STR, _DEX, _INT, _DAMAGE, _DEFENCE, _MOVE_SPEED, _SKILL_COOLTIME_DEC_PER, _SKILL_ADD_DAMAGE_PER, _CRITICAL_PROBABILITY, _CRITICAL_ADD_DAMAGE_PER);
        }
    }

}
