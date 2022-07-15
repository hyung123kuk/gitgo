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

    public string[] forbidWord;

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

        

        string msg = "[" + PhotonNetwork.LocalPlayer.NickName + "]" +  input.text;

        for (int i = 0; i < forbidWord.Length; i++)
        {
            string chage = "";
            for (int j = 0; j < forbidWord[i].Length; j++)
            {
                chage += "*";
            }
         
            Debug.Log(msg);
            Debug.Log(forbidWord[i]);
            Debug.Log(chage);

            msg = msg.Replace(forbidWord[i], chage);
        }


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
