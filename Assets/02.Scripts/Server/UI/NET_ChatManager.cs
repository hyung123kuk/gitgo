using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class NET_ChatManager : MonoBehaviourPunCallbacks
{
    public List<string> chatList = new List<string>();
    public Button sendBtn;
    public Text chatLog;

    public InputField input;
    public ScrollRect scroll_rect = null;
    string chatters;


    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {            
            SendButtonOnClicked();
            
            input.Select();
            
            AllUI.allUI.CheckCursorLock();
        }
        if(input.text != "")
        {
            AllUI.isUI = true;
        }
    }


    public void SendButtonOnClicked()
    {
        if (input.text.Equals("")) { Debug.Log("Empty"); return; }
        string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, input.text);
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        ReceiveMsg(msg);
        input.ActivateInputField();
        input.text = "";
    }

    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        chatLog.text += "\n" + msg;
        scroll_rect.verticalNormalizedPosition = 0.0f;
        
    }


}
