using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorEquip : MonoBehaviour
{
    public static WarriorEquip warriorEquip;

    #region ¸Ó¸®
    [Header("¸Ó¸®")]
    public GameObject LV1_FacialHair; //¼ö¿°
    public GameObject LV1_Hair; //¸Ó¸®Ä«¶ô
    public GameObject LV1_Head; //¸Ó¸®
    public GameObject LV3_Helmet; //Çï¸ä µÚ Àå½Ä
    public GameObject LV7_Helmet; 
    public GameObject LV3_AttackHelmet; //Çï¸ä
    public GameObject LV7_AttackHelmet;
    #endregion
    #region »óÀÇ
    [Header("»óÀÇ")]
    public GameObject LV1_Torso; //¸öÅë
    public GameObject LV3_Torso;
    public GameObject LV7_Torso;

    public GameObject LV1_Arm_Upper_Right; //ÆÈ¶Ò
    public GameObject LV3_Arm_Upper_Right;
    public GameObject LV7_Arm_Upper_Right;
    public GameObject LV1_Arm_Upper_Left;
    public GameObject LV3_Arm_Upper_Left;
    public GameObject LV7_Arm_Upper_Left;

    public GameObject LV1_Arm_Lower_Right; //ÆÈ
    public GameObject LV3_Arm_Lower_Right;
    public GameObject LV7_Arm_Lower_Right;
    public GameObject LV1_Arm_Lower_Left;
    public GameObject LV3_Arm_Lowerr_Left;
    public GameObject LV7_Arm_Lower_Left;
    #endregion
    #region ¾î±ú
    [Header("¾î±ú")]
    public GameObject LV3_Right_Shoulder;
    public GameObject LV7_Right_Shoulder;
    public GameObject LV3_Left_Shoulder;
    public GameObject LV7_Left_Shoulder;

    public GameObject LV3_Right_Elbow;  //ÆÈº¸È£±¸
    public GameObject LV7_Right_Elbow;
    public GameObject LV3_Left_Elbow;
    public GameObject LV7_Left_Elbow;
    #endregion
    #region Àå°©
    [Header("Àå°©")]
    public GameObject LV1_Right_Hand;
    public GameObject LV3_Right_Hand;
    public GameObject LV7_Right_Hand;
    public GameObject LV1_Left_Hand;
    public GameObject LV3_Left_Hand;
    public GameObject LV7_Left_Hand;
    #endregion
    #region ÇÏÀÇ
    [Header("ÇÏÀÇ")]
    public GameObject LV3_Right_Knee;  //´Ù¸®º¸È£±¸
    public GameObject LV7_Right_Knee;
    public GameObject LV3_Left_Knee;
    public GameObject LV7_Left_Knee;

    public GameObject LV1_Hips; //¹ÙÁö
    public GameObject LV3_Hips;
    public GameObject LV7_Hips;
    #endregion
    #region ½Å¹ß
    [Header("½Å¹ß")]
    public GameObject LV1_Leg_Right;
    public GameObject LV3_Leg_Right;
    public GameObject LV7_Leg_Right;
    public GameObject LV1_Leg_Left;
    public GameObject LV3_Leg_Left;
    public GameObject LV7_Leg_Left;

    #endregion
    #region ¸ÁÅä
    [Header("¸ÁÅä")]
    public GameObject LV3_Back;
    public GameObject LV7_Back;
    #endregion


    void Awake()
    {
        warriorEquip = this;
    }

}
