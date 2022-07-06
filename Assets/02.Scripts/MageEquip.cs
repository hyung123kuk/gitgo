using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEquip : MonoBehaviour
{
    public static MageEquip mageEquip;

    #region 머리
    [Header("머리")]
    public GameObject LV3_Helmet; //헬멧 뒤 장식
    public GameObject LV7_Helmet;
    #endregion
    #region 상의
    [Header("상의")]
    public GameObject LV1_Torso; //몸통
    public GameObject LV3_Torso;
    public GameObject LV7_Torso;

    public GameObject LV1_Arm_Upper_Right; //팔뚝
    public GameObject LV3_Arm_Upper_Right;
    public GameObject LV7_Arm_Upper_Right;
    public GameObject LV1_Arm_Upper_Left;
    public GameObject LV3_Arm_Upper_Left;
    public GameObject LV7_Arm_Upper_Left;

    public GameObject LV1_Arm_Lower_Right; //팔
    public GameObject LV3_Arm_Lower_Right;
    public GameObject LV7_Arm_Lower_Right;
    public GameObject LV1_Arm_Lower_Left;
    public GameObject LV3_Arm_Lowerr_Left;
    public GameObject LV7_Arm_Lower_Left;
    #endregion
    #region 장갑
    [Header("장갑")]
    public GameObject LV1_Right_Hand;
    public GameObject LV3_Right_Hand;
    public GameObject LV7_Right_Hand;
    public GameObject LV1_Left_Hand;
    public GameObject LV3_Left_Hand;
    public GameObject LV7_Left_Hand;
    #endregion
    #region 하의
    [Header("하의")]
    public GameObject LV1_Hips; //바지
    public GameObject LV3_Hips;
    public GameObject LV7_Hips;
    #endregion
    #region 신발
    [Header("신발")]
    public GameObject LV1_Leg_Right;
    public GameObject LV3_Leg_Right;
    public GameObject LV7_Leg_Right;
    public GameObject LV1_Leg_Left;
    public GameObject LV3_Leg_Left;
    public GameObject LV7_Leg_Left;

    #endregion
    #region 망토
    [Header("망토")]
    public GameObject LV3_Back;
    public GameObject LV7_Back;
    #endregion


    void Awake()
    {
        mageEquip = this;
    }
}
