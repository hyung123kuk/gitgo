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
        nickNameInput1.characterLimit = 8; //최대입력수 제한
        nickNameInput2.characterLimit = 8; //최대입력수 제한
    }



    private bool CheckNickname() //한글,영어,숫자만 입력가능하게
    {
        return Regex.IsMatch(nickNameInput1.text, "^[0-9a-zA-Z가-힣]*$") || Regex.IsMatch(nickNameInput2.text, "^[0-9a-zA-Z가-힣]*$"); 
        //한글영어숫자로 입력을했으면 true
    }

    private bool CheckOverlap() //중복검사
    {
        return namelist.Count != namelist.Distinct().Count(); //중복데이터가 존재하면 true
    }

    public void OnClickCreateName() //닉네임 생성
    {
        if (CheckNickname() == false)
        {
            Debug.Log("한글,영어,숫자로만 만들수있습니다.");
            return;
        }
        
        if(CheckOverlap() == true)
        {
            Debug.Log("중복아이디입니다. 다시 입력하세요.");
            return;
        }

        if (characterSel.charSel == 1) //닉네임 저장
        {
            namelist.Add(nickNameInput1.text); //서버매니저에서 관리해야함

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
