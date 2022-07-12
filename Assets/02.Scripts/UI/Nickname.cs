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
        nickNameInput1.characterLimit = 8; //ÃÖ´ëÀÔ·Â¼ö Á¦ÇÑ
        nickNameInput2.characterLimit = 8; //ÃÖ´ëÀÔ·Â¼ö Á¦ÇÑ
    }



    private bool CheckNickname() //ÇÑ±Û,¿µ¾î,¼ýÀÚ¸¸ ÀÔ·Â°¡´ÉÇÏ°Ô
    {
        return Regex.IsMatch(nickNameInput1.text, "^[0-9a-zA-Z°¡-ÆR]*$") || Regex.IsMatch(nickNameInput2.text, "^[0-9a-zA-Z°¡-ÆR]*$"); 
        //ÇÑ±Û¿µ¾î¼ýÀÚ·Î ÀÔ·ÂÀ»ÇßÀ¸¸é true
    }

    private bool CheckOverlap() //Áßº¹°Ë»ç
    {
        return namelist.Count != namelist.Distinct().Count(); //Áßº¹µ¥ÀÌÅÍ°¡ Á¸ÀçÇÏ¸é true
    }

    public void OnClickCreateName() //´Ð³×ÀÓ »ý¼º
    {
        if (CheckNickname() == false)
        {
            Debug.Log("ÇÑ±Û,¿µ¾î,¼ýÀÚ·Î¸¸ ¸¸µé¼öÀÖ½À´Ï´Ù.");
            return;
        }
        
        if(CheckOverlap() == true)
        {
            Debug.Log("Áßº¹¾ÆÀÌµðÀÔ´Ï´Ù. ´Ù½Ã ÀÔ·ÂÇÏ¼¼¿ä.");
            return;
        }

        if (characterSel.charSel == 1) //´Ð³×ÀÓ ÀúÀå
        {
            namelist.Add(nickNameInput1.text); //¼­¹ö¸Å´ÏÀú¿¡¼­ °ü¸®ÇØ¾ßÇÔ

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
