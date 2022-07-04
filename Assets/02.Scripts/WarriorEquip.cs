using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorEquip : MonoBehaviour
{
    public static WarriorEquip warriorEquip;

    #region �Ӹ�
    [Header("�Ӹ�")]
    public GameObject LV1_FacialHair; //����
    public GameObject LV1_Hair; //�Ӹ�ī��
    public GameObject LV1_Head; //�Ӹ�
    public GameObject LV3_Helmet; //��� �� ���
    public GameObject LV7_Helmet; 
    public GameObject LV3_AttackHelmet; //���
    public GameObject LV7_AttackHelmet;
    #endregion
    #region ����
    [Header("����")]
    public GameObject LV1_Torso; //����
    public GameObject LV3_Torso;
    public GameObject LV7_Torso;

    public GameObject LV1_Arm_Upper_Right; //�ȶ�
    public GameObject LV3_Arm_Upper_Right;
    public GameObject LV7_Arm_Upper_Right;
    public GameObject LV1_Arm_Upper_Left;
    public GameObject LV3_Arm_Upper_Left;
    public GameObject LV7_Arm_Upper_Left;

    public GameObject LV1_Arm_Lower_Right; //��
    public GameObject LV3_Arm_Lower_Right;
    public GameObject LV7_Arm_Lower_Right;
    public GameObject LV1_Arm_Lower_Left;
    public GameObject LV3_Arm_Lowerr_Left;
    public GameObject LV7_Arm_Lower_Left;
    #endregion
    #region ���
    [Header("���")]
    public GameObject LV3_Right_Shoulder;
    public GameObject LV7_Right_Shoulder;
    public GameObject LV3_Left_Shoulder;
    public GameObject LV7_Left_Shoulder;

    public GameObject LV3_Right_Elbow;  //�Ⱥ�ȣ��
    public GameObject LV7_Right_Elbow;
    public GameObject LV3_Left_Elbow;
    public GameObject LV7_Left_Elbow;
    #endregion
    #region �尩
    [Header("�尩")]
    public GameObject LV1_Right_Hand;
    public GameObject LV3_Right_Hand;
    public GameObject LV7_Right_Hand;
    public GameObject LV1_Left_Hand;
    public GameObject LV3_Left_Hand;
    public GameObject LV7_Left_Hand;
    #endregion
    #region ����
    [Header("����")]
    public GameObject LV3_Right_Knee;  //�ٸ���ȣ��
    public GameObject LV7_Right_Knee;
    public GameObject LV3_Left_Knee;
    public GameObject LV7_Left_Knee;

    public GameObject LV1_Hips; //����
    public GameObject LV3_Hips;
    public GameObject LV7_Hips;
    #endregion
    #region �Ź�
    [Header("�Ź�")]
    public GameObject LV1_Leg_Right;
    public GameObject LV3_Leg_Right;
    public GameObject LV7_Leg_Right;
    public GameObject LV1_Leg_Left;
    public GameObject LV3_Leg_Left;
    public GameObject LV7_Leg_Left;

    #endregion
    #region ����
    [Header("����")]
    public GameObject LV3_Back;
    public GameObject LV7_Back;
    #endregion


    void Awake()
    {
        warriorEquip = this;
    }

}
