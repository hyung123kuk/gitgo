using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;
using Photon.Pun;

public class Nickname : MonoBehaviourPun
{

    public InputField nickNameInput1;
    public InputField nickNameInput2;
    private SaveManager saveManager;
    private CharacterSel characterSel;

    public List<string> namelist = new List<string>();

    private void Awake()
    {
       /* nickNameInput1 = transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(10).
            GetComponent<InputField>();
        nickNameInput2 = transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(11).
            GetComponent<InputField>();*/


        saveManager = FindObjectOfType<SaveManager>();
        characterSel = FindObjectOfType<CharacterSel>();
    }

    private void Start()
    {
        nickNameInput1.characterLimit = 8; //�ִ��Է¼� ����
        nickNameInput2.characterLimit = 8; //�ִ��Է¼� ����
    }



    private bool CheckNickname() //�ѱ�,����,���ڸ� �Է°����ϰ�
    {
        return Regex.IsMatch(nickNameInput1.text, "^[0-9a-zA-Z��-�R]*$") || Regex.IsMatch(nickNameInput2.text, "^[0-9a-zA-Z��-�R]*$"); 
        //�ѱۿ�����ڷ� �Է��������� true
    }

    private bool CheckOverlap() //�ߺ��˻�
    {
        return namelist.Count != namelist.Distinct().Count(); //�ߺ������Ͱ� �����ϸ� true
    }

    public void OnClickCreateName() //�г��� ����
    {
        if (CheckNickname() == false)
        {
            Debug.Log("�ѱ�,����,���ڷθ� ������ֽ��ϴ�.");
            return;
        }
        
        if(CheckOverlap() == true)
        {
            Debug.Log("�ߺ����̵��Դϴ�. �ٽ� �Է��ϼ���.");
            return;
        }

        if (characterSel.charSel == 1) //�г��� ����
        {
            namelist.Add(nickNameInput1.text); //�����Ŵ������� �����ؾ���

            if (nickNameInput1.text.Equals(""))
                PhotonNetwork.LocalPlayer.NickName = "unknown";
            else
                PhotonNetwork.LocalPlayer.NickName = nickNameInput1.text;

            characterSel.Char1Name.text = PhotonNetwork.LocalPlayer.NickName;
            saveManager.Setname1 = PhotonNetwork.LocalPlayer.NickName;
        }
        else if (characterSel.charSel == 2)
        {
            namelist.Add(nickNameInput2.text);

            if (nickNameInput2.text.Equals(""))
                PhotonNetwork.LocalPlayer.NickName = "unknown";
            else
                PhotonNetwork.LocalPlayer.NickName = nickNameInput2.text;

            characterSel.Char2Name.text = PhotonNetwork.LocalPlayer.NickName;
            saveManager.Setname2 = PhotonNetwork.LocalPlayer.NickName;
        }
        
    }

}
