using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MageEquipChange : MonoBehaviourPun
{
    public bool UICheck; //캐릭터선택창 체크용
    public PlayerST playerst;
    #region 장비변경
    public MageEquip mageEquip;

    public GameObject Helmet1;
    public GameObject Helmet3;
    public GameObject Helmet7;

    public GameObject[] Chest1;
    public GameObject[] Chest3;
    public GameObject[] Chest7;

    public GameObject[] Glove1;
    public GameObject[] Glove3;
    public GameObject[] Glove7;

    public GameObject Pants1;
    public GameObject Pants3;
    public GameObject Pants7;

    public GameObject[] Boots1;
    public GameObject[] Boots3;
    public GameObject[] Boots7;

    public GameObject Back1;
    public GameObject Back3;
    public GameObject Back7;

    #endregion
    #region 법사 방어구 착용확인용
    public int EquipHelmet;
    public int EquipChest;
    public int EquipGloves;
    public int EquipPants;
    public int EquipBoots;
    public int EquipBack;
    #endregion

    void Awake()
    {
        if (!photonView.IsMine && !UICheck)
        {
            this.enabled = false;
        }
        mageEquip = GetComponent<MageEquip>();
        playerst = GetComponent<PlayerST>();
        #region 장비초기화
        Chest1 = new GameObject[5];
        Chest3 = new GameObject[5];
        Chest7 = new GameObject[5];
        Glove1 = new GameObject[2];
        Glove3 = new GameObject[2];
        Glove7 = new GameObject[2];
        Boots1 = new GameObject[2];
        Boots3 = new GameObject[2];
        Boots7 = new GameObject[2];

        #endregion
    }

    [PunRPC]
    void EquipSetting()
    {
        //↓머리
        Helmet3 = mageEquip.LV3_Helmet;
        Helmet7 = mageEquip.LV7_Helmet;

        //↓상의
        Chest1[0] = mageEquip.LV1_Torso; Chest1[1] = mageEquip.LV1_Arm_Upper_Right; Chest1[2] = mageEquip.LV1_Arm_Upper_Left;
        Chest1[3] = mageEquip.LV1_Arm_Lower_Right; Chest1[4] = mageEquip.LV1_Arm_Lower_Left;
        Chest3[0] = mageEquip.LV3_Torso; Chest3[1] = mageEquip.LV3_Arm_Upper_Right; Chest3[2] = mageEquip.LV3_Arm_Upper_Left;
        Chest3[3] = mageEquip.LV3_Arm_Lower_Right; Chest3[4] = mageEquip.LV3_Arm_Lowerr_Left;
        Chest7[0] = mageEquip.LV7_Torso; Chest7[1] = mageEquip.LV7_Arm_Upper_Right; Chest7[2] = mageEquip.LV7_Arm_Upper_Left;
        Chest7[3] = mageEquip.LV7_Arm_Lower_Right; Chest7[4] = mageEquip.LV7_Arm_Lower_Left;

        //↓장갑
        Glove1[0] = mageEquip.LV1_Right_Hand; Glove1[1] = mageEquip.LV1_Left_Hand;
        Glove3[0] = mageEquip.LV3_Right_Hand; Glove3[1] = mageEquip.LV3_Left_Hand;
        Glove7[0] = mageEquip.LV7_Right_Hand; Glove7[1] = mageEquip.LV7_Left_Hand;

        //↓하의
        Pants1 = mageEquip.LV1_Hips;
        Pants3 = mageEquip.LV3_Hips;
        Pants7 = mageEquip.LV7_Hips;

        //↓신발
        Boots1[0] = mageEquip.LV1_Leg_Right; Boots1[1] = mageEquip.LV1_Leg_Left;
        Boots3[0] = mageEquip.LV3_Leg_Right; Boots3[1] = mageEquip.LV3_Leg_Left;
        Boots7[0] = mageEquip.LV7_Leg_Right; Boots7[1] = mageEquip.LV7_Leg_Left;

        //↓망토
        Back3 = mageEquip.LV3_Back;
        Back7 = mageEquip.LV7_Back;
    }

    private void Start()
    {
        photonView.RPC("EquipSetting", RpcTarget.AllBuffered);
    }

    #region 법사 방어구 변경
    [PunRPC]
    public void MageHelmetChange(Item.HelmetNames HelmetNum) //법사 머리변경
    {
        if ((int)HelmetNum == 2)
        {
            Helmet3.SetActive(false);
            Helmet7.SetActive(true);
            EquipHelmet = (int)HelmetNum;
        }
        else if ((int)HelmetNum == 1)
        {
            Helmet3.SetActive(true);
            Helmet7.SetActive(false);
            EquipHelmet = (int)HelmetNum;
        }
        else if ((int)HelmetNum == 0)
        {
            Helmet3.SetActive(false);
            Helmet7.SetActive(false);
            EquipHelmet = (int)HelmetNum;
        }
        playerst.smesh = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    [PunRPC]
    public void MageChestChange(Item.ChestNames ChestNum) //법사 상의변경
    {
        if ((int)ChestNum == 2)
        {
            for (int i = 0; i < Chest3.Length; i++)
            {
                Chest1[i].SetActive(false);
                Chest3[i].SetActive(false);
                Chest7[i].SetActive(true);
                EquipChest = (int)ChestNum;
            }
        }
        else if ((int)ChestNum == 1)
        {
            for (int i = 0; i < Chest3.Length; i++)
            {
                Chest1[i].SetActive(false);
                Chest3[i].SetActive(true);
                Chest7[i].SetActive(false);
                EquipChest = (int)ChestNum;
            }
        }
        else if ((int)ChestNum == 0)
        {
            for (int i = 0; i < Chest3.Length; i++)
            {
                Chest1[i].SetActive(true);
                Chest3[i].SetActive(false);
                Chest7[i].SetActive(false);
                EquipChest = (int)ChestNum;
            }
        }
        playerst.smesh = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    [PunRPC]
    public void MageGlovesChange(Item.GlovesNames GlovesNum) //법사 장갑변경
    {
        if ((int)GlovesNum == 2)
        {
            for (int i = 0; i < Glove1.Length; i++)
            {
                Glove1[i].SetActive(false);
                Glove3[i].SetActive(false);
                Glove7[i].SetActive(true);
                EquipGloves = (int)GlovesNum;
            }
        }
        else if ((int)GlovesNum == 1)
        {
            for (int i = 0; i < Glove1.Length; i++)
            {
                Glove1[i].SetActive(false);
                Glove3[i].SetActive(true);
                Glove7[i].SetActive(false);
                EquipGloves = (int)GlovesNum;
            }
        }
        else if ((int)GlovesNum == 0)
        {
            for (int i = 0; i < Glove1.Length; i++)
            {
                Glove1[i].SetActive(true);
                Glove3[i].SetActive(false);
                Glove7[i].SetActive(false);
                EquipGloves = (int)GlovesNum;
            }
        }
        playerst.smesh = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    [PunRPC]
    public void MagePantsChange(Item.PantsNames PantsNum) //법사 하의변경
    {
        if ((int)PantsNum == 2)
        {
            Pants1.SetActive(false);
            Pants3.SetActive(false);
            Pants7.SetActive(true);
            EquipPants = (int)PantsNum;
        }
        else if ((int)PantsNum == 1)
        {
            Pants1.SetActive(false);
            Pants3.SetActive(true);
            Pants7.SetActive(false);
            EquipPants = (int)PantsNum;
        }
        else if ((int)PantsNum == 0)
        {
            Pants1.SetActive(true);
            Pants3.SetActive(false);
            Pants7.SetActive(false);
            EquipPants = (int)PantsNum;
        }
        playerst.smesh = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    [PunRPC]
    public void MageBootsChange(Item.BootsNames BootsNum) //법사 신발변경
    {
        if ((int)BootsNum == 2)
        {
            for (int i = 0; i < Boots1.Length; i++)
            {
                Boots1[i].SetActive(false);
                Boots3[i].SetActive(false);
                Boots7[i].SetActive(true);
                EquipBoots = (int)BootsNum;
            }
        }
        else if ((int)BootsNum == 1)
        {
            for (int i = 0; i < Boots1.Length; i++)
            {
                Boots1[i].SetActive(false);
                Boots3[i].SetActive(true);
                Boots7[i].SetActive(false);
                EquipBoots = (int)BootsNum;
            }
        }
        else if ((int)BootsNum == 0)
        {
            for (int i = 0; i < Boots1.Length; i++)
            {
                Boots1[i].SetActive(true);
                Boots3[i].SetActive(false);
                Boots7[i].SetActive(false);
                EquipBoots = (int)BootsNum;
            }
        }
        playerst.smesh = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    [PunRPC]
    public void MageBackChange(Item.BackNames BackNum) //법사 망토변경
    {
        if ((int)BackNum == 2)
        {
            Back3.SetActive(false);
            Back7.SetActive(true);
            EquipBack = (int)BackNum;
        }
        else if ((int)BackNum == 1)
        {
            Back3.SetActive(true);
            Back7.SetActive(false);
            EquipBack = (int)BackNum;
        }
        else if ((int)BackNum == 0)
        {
            Back3.SetActive(false);
            Back7.SetActive(false);
            EquipBack = (int)BackNum;
        }
        playerst.smesh = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    #endregion
}
