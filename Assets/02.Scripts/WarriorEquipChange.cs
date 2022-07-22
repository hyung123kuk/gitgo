using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WarriorEquipChange : MonoBehaviourPun
{
    public bool UICheck; //캐릭터선택창 체크용
    [SerializeField]
    PlayerST playerst;

    #region 장비변경
    public WarriorEquip warriorEquip;

    public GameObject[] Helmet1;
    public GameObject[] Helmet3;
    public GameObject[] Helmet7;


    public GameObject[] Chest1;
    public GameObject[] Chest3;
    public GameObject[] Chest7;


    public GameObject[] Shoulder1;
    public GameObject[] Shoulder3;
    public GameObject[] Shoulder7;


    public GameObject[] Glove1;
    public GameObject[] Glove3;
    public GameObject[] Glove7;


    public GameObject[] Pants1;
    public GameObject[] Pants3;
    public GameObject[] Pants7;


    public GameObject[] Boots1;
    public GameObject[] Boots3;
    public GameObject[] Boots7;


    public GameObject[] Back1;
    public GameObject[] Back3;
    public GameObject[] Back7;

    #endregion
    #region 워리어 방어구 현재입은거 확인용
    public int EquipHelmet;
    public int EquipShoulder;
    public int EquipChest;
    public int EquipGloves;
    public int EquipPants;
    public int EquipBoots;
    public int EquipBack;
    #endregion
    void Awake()
    {
        if (!photonView.IsMine)
        {
            this.enabled = false;
        }
        playerst = GetComponent<PlayerST>();
        warriorEquip = GetComponent<WarriorEquip>();
        #region 워리어 장비초기화

        Helmet1 = new GameObject[3];
        Helmet3 = new GameObject[2];
        Helmet7 = new GameObject[2];
        Shoulder3 = new GameObject[4];
        Shoulder7 = new GameObject[4];
        Chest1 = new GameObject[5];
        Chest3 = new GameObject[5];
        Chest7 = new GameObject[5];
        Glove1 = new GameObject[2];
        Glove3 = new GameObject[2];
        Glove7 = new GameObject[2];
        Pants1 = new GameObject[1];
        Pants3 = new GameObject[3];
        Pants7 = new GameObject[3];
        Boots1 = new GameObject[2];
        Boots3 = new GameObject[2];
        Boots7 = new GameObject[2];
        Back3 = new GameObject[1];
        Back7 = new GameObject[1];

        #endregion
    }

    [PunRPC]
    void EquipSetting()
    {
        //↓머리
        Helmet1[0] = warriorEquip.LV1_FacialHair; Helmet1[1] = warriorEquip.LV1_Hair; Helmet1[2] = warriorEquip.LV1_Head;
        Helmet3[0] = warriorEquip.LV3_Helmet; Helmet3[1] = warriorEquip.LV3_AttackHelmet;
        Helmet7[0] = warriorEquip.LV7_Helmet; Helmet7[1] = warriorEquip.LV7_AttackHelmet;

        //↓어깨
        Shoulder3[0] = warriorEquip.LV3_Right_Shoulder; Shoulder3[1] = warriorEquip.LV3_Left_Shoulder;
        Shoulder3[2] = warriorEquip.LV3_Right_Elbow; Shoulder3[3] = warriorEquip.LV3_Left_Elbow;
        Shoulder7[0] = warriorEquip.LV7_Right_Shoulder; Shoulder7[1] = warriorEquip.LV7_Left_Shoulder;
        Shoulder7[2] = warriorEquip.LV7_Right_Elbow; Shoulder7[3] = warriorEquip.LV7_Left_Elbow;

        //↓상의
        Chest1[0] = warriorEquip.LV1_Torso; Chest1[1] = warriorEquip.LV1_Arm_Upper_Right; Chest1[2] = warriorEquip.LV1_Arm_Upper_Left;
        Chest1[3] = warriorEquip.LV1_Arm_Lower_Right; Chest1[4] = warriorEquip.LV1_Arm_Lower_Left;
        Chest3[0] = warriorEquip.LV3_Torso; Chest3[1] = warriorEquip.LV3_Arm_Upper_Right; Chest3[2] = warriorEquip.LV3_Arm_Upper_Left;
        Chest3[3] = warriorEquip.LV3_Arm_Lower_Right; Chest3[4] = warriorEquip.LV3_Arm_Lowerr_Left;
        Chest7[0] = warriorEquip.LV7_Torso; Chest7[1] = warriorEquip.LV7_Arm_Upper_Right; Chest7[2] = warriorEquip.LV7_Arm_Upper_Left;
        Chest7[3] = warriorEquip.LV7_Arm_Lower_Right; Chest7[4] = warriorEquip.LV7_Arm_Lower_Left;

        //↓장갑
        Glove1[0] = warriorEquip.LV1_Right_Hand; Glove1[1] = warriorEquip.LV1_Left_Hand;
        Glove3[0] = warriorEquip.LV3_Right_Hand; Glove3[1] = warriorEquip.LV3_Left_Hand;
        Glove7[0] = warriorEquip.LV7_Right_Hand; Glove7[1] = warriorEquip.LV7_Left_Hand;

        //↓하의
        Pants1[0] = warriorEquip.LV1_Hips;
        Pants3[0] = warriorEquip.LV3_Right_Knee; Pants3[1] = warriorEquip.LV3_Left_Knee; Pants3[2] = warriorEquip.LV3_Hips;
        Pants7[0] = warriorEquip.LV7_Right_Knee; Pants7[1] = warriorEquip.LV7_Left_Knee; Pants7[2] = warriorEquip.LV7_Hips;

        //↓신발
        Boots1[0] = warriorEquip.LV1_Leg_Right; Boots1[1] = warriorEquip.LV1_Leg_Left;
        Boots3[0] = warriorEquip.LV3_Leg_Right; Boots3[1] = warriorEquip.LV3_Leg_Left;
        Boots7[0] = warriorEquip.LV7_Leg_Right; Boots7[1] = warriorEquip.LV7_Leg_Left;

        //↓망토
        Back3[0] = warriorEquip.LV3_Back;
        Back7[0] = warriorEquip.LV7_Back;
    }

    private void Start()
    {
        if(playerst.CharacterType == PlayerST.Type.Warrior && enabled)
        photonView.RPC("EquipSetting", RpcTarget.AllBuffered);
        #region 워리어 장비 초기화
        ////↓머리
        //Helmet1[0] = warriorEquip.LV1_FacialHair; Helmet1[1] = warriorEquip.LV1_Hair; Helmet1[2] = warriorEquip.LV1_Head;
        //Helmet3[0] = warriorEquip.LV3_Helmet; Helmet3[1] = warriorEquip.LV3_AttackHelmet;
        //Helmet7[0] = warriorEquip.LV7_Helmet; Helmet7[1] = warriorEquip.LV7_AttackHelmet;

        ////↓어깨
        //Shoulder3[0] = warriorEquip.LV3_Right_Shoulder; Shoulder3[1] = warriorEquip.LV3_Left_Shoulder;
        //Shoulder3[2] = warriorEquip.LV3_Right_Elbow; Shoulder3[3] = warriorEquip.LV3_Left_Elbow;
        //Shoulder7[0] = warriorEquip.LV7_Right_Shoulder; Shoulder7[1] = warriorEquip.LV7_Left_Shoulder;
        //Shoulder7[2] = warriorEquip.LV7_Right_Elbow; Shoulder7[3] = warriorEquip.LV7_Left_Elbow;

        ////↓상의
        //Chest1[0] = warriorEquip.LV1_Torso; Chest1[1] = warriorEquip.LV1_Arm_Upper_Right; Chest1[2] = warriorEquip.LV1_Arm_Upper_Left;
        //Chest1[3] = warriorEquip.LV1_Arm_Lower_Right; Chest1[4] = warriorEquip.LV1_Arm_Lower_Left;
        //Chest3[0] = warriorEquip.LV3_Torso; Chest3[1] = warriorEquip.LV3_Arm_Upper_Right; Chest3[2] = warriorEquip.LV3_Arm_Upper_Left;
        //Chest3[3] = warriorEquip.LV3_Arm_Lower_Right; Chest3[4] = warriorEquip.LV3_Arm_Lowerr_Left;
        //Chest7[0] = warriorEquip.LV7_Torso; Chest7[1] = warriorEquip.LV7_Arm_Upper_Right; Chest7[2] = warriorEquip.LV7_Arm_Upper_Left;
        //Chest7[3] = warriorEquip.LV7_Arm_Lower_Right; Chest7[4] = warriorEquip.LV7_Arm_Lower_Left;

        ////↓장갑
        //Glove1[0] = warriorEquip.LV1_Right_Hand; Glove1[1] = warriorEquip.LV1_Left_Hand;
        //Glove3[0] = warriorEquip.LV3_Right_Hand; Glove3[1] = warriorEquip.LV3_Left_Hand;
        //Glove7[0] = warriorEquip.LV7_Right_Hand; Glove7[1] = warriorEquip.LV7_Left_Hand;

        ////↓하의
        //Pants1[0] = warriorEquip.LV1_Hips;
        //Pants3[0] = warriorEquip.LV3_Right_Knee; Pants3[1] = warriorEquip.LV3_Left_Knee; Pants3[2] = warriorEquip.LV3_Hips;
        //Pants7[0] = warriorEquip.LV7_Right_Knee; Pants7[1] = warriorEquip.LV7_Left_Knee; Pants7[2] = warriorEquip.LV7_Hips;

        ////↓신발
        //Boots1[0] = warriorEquip.LV1_Leg_Right; Boots1[1] = warriorEquip.LV1_Leg_Left;
        //Boots3[0] = warriorEquip.LV3_Leg_Right; Boots3[1] = warriorEquip.LV3_Leg_Left;
        //Boots7[0] = warriorEquip.LV7_Leg_Right; Boots7[1] = warriorEquip.LV7_Leg_Left;

        ////↓망토
        //Back3[0] = warriorEquip.LV3_Back;
        //Back7[0] = warriorEquip.LV7_Back;
        #endregion
    }

    #region 전사 방어구 변경
    [PunRPC]
    public void WarriorHelmetChange(Item.HelmetNames HelmetNum) //전사 머리변경
    {
        if ((int)HelmetNum == 2)
        {
            Helmet1[2].SetActive(false);
            for (int i = 0; i < Helmet3.Length; i++)
            {
                Helmet1[i].SetActive(false);
                Helmet3[i].SetActive(false);
                Helmet7[i].SetActive(true);
                EquipHelmet = (int)HelmetNum;
            }
        }
        else if ((int)HelmetNum == 1)
        {
            Helmet1[2].SetActive(false);
            for (int i = 0; i < Helmet3.Length; i++)
            {
                Helmet1[i].SetActive(false);
                Helmet3[i].SetActive(true);
                Helmet7[i].SetActive(false);
                EquipHelmet = (int)HelmetNum;
            }
        }
        else if ((int)HelmetNum == 0)
        {
            Helmet1[2].SetActive(true);
            for (int i = 0; i < Helmet3.Length; i++)
            {
                Helmet1[i].SetActive(true);
                Helmet3[i].SetActive(false);
                Helmet7[i].SetActive(false);
                EquipHelmet = (int)HelmetNum;
            }
        }
    }
    [PunRPC]
    public void WarriorShoulderChange(Item.ShoulderNames ShoulderNum) //전사 어깨변경
    {
        if ((int)ShoulderNum == 2)
        {
            for (int i = 0; i < Shoulder3.Length; i++)
            {
                Shoulder3[i].SetActive(false);
                Shoulder7[i].SetActive(true);
                EquipShoulder = (int)ShoulderNum;
            }
        }
        else if ((int)ShoulderNum == 1)
        {
            for (int i = 0; i < Shoulder3.Length; i++)
            {
                Shoulder3[i].SetActive(true);
                Shoulder7[i].SetActive(false);
                EquipShoulder = (int)ShoulderNum;
            }
        }
        else if ((int)ShoulderNum == 0)
        {
            for (int i = 0; i < Shoulder3.Length; i++)
            {
                Shoulder3[i].SetActive(false);
                Shoulder7[i].SetActive(false);
                EquipShoulder = (int)ShoulderNum;
            }
        }
    }
    [PunRPC]
    public void WarriorChestChange(Item.ChestNames ChestNum) //전사 상의변경
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
    }
    [PunRPC]
    public void WarriorGlovesChange(Item.GlovesNames GlovesNum) //전사 장갑변경
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
    }
    [PunRPC]
    public void WarriorPantsChange(Item.PantsNames PantsNum) //전사 하의변경
    {
        if ((int)PantsNum == 2)
        {
            Pants1[0].SetActive(false);
            for (int i = 0; i < Pants3.Length; i++)
            {
                Pants3[i].SetActive(false);
                Pants7[i].SetActive(true);
                EquipPants = (int)PantsNum;
            }
        }
        else if ((int)PantsNum == 1)
        {
            Pants1[0].SetActive(false);
            for (int i = 0; i < Pants3.Length; i++)
            {
                Pants3[i].SetActive(true);
                Pants7[i].SetActive(false);
                EquipPants = (int)PantsNum;
            }
        }
        else if ((int)PantsNum == 0)
        {
            Pants1[0].SetActive(true);
            for (int i = 0; i < Pants3.Length; i++)
            {
                Pants3[i].SetActive(false);
                Pants7[i].SetActive(false);
                EquipPants = (int)PantsNum;
            }
        }
    }
    [PunRPC]
    public void WarriorBootsChange(Item.BootsNames BootsNum) //전사 신발변경
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
    }
    [PunRPC]
    public void WarriorBackChange(Item.BackNames BackNum) //전사 망토변경
    {
        if ((int)BackNum == 2)
        {
            for (int i = 0; i < Back3.Length; i++)
            {
                Back3[i].SetActive(false);
                Back7[i].SetActive(true);
                EquipBack = (int)BackNum;
            }
        }
        else if ((int)BackNum == 1)
        {
            for (int i = 0; i < Back3.Length; i++)
            {
                Back3[i].SetActive(true);
                Back7[i].SetActive(false);
                EquipBack = (int)BackNum;
            }
        }
        else if ((int)BackNum == 0)
        {
            for (int i = 0; i < Back3.Length; i++)
            {
                Back3[i].SetActive(false);
                Back7[i].SetActive(false);
                EquipBack = (int)BackNum;
            }
        }
    }
    #endregion

}
