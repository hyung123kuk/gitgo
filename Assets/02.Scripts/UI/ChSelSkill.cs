using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChSelSkill : MonoBehaviour
{
    public static ChSelSkill chSelSkill;


    [Header("���� ����")]
    public Transform Aurapos; //��ų2 ����
    public GameObject SwordAura; //��ų2 ����Ʈ

    [Header("�ü� ����")]
    public GameObject ArcDefaultAttack; //�⺻ȭ��
    public Transform arrowPos; //ȭ�쳪������ġ
    public GameObject Arc1Skilarrow; //��ȭ��
    public GameObject Arc3SkillArrow1; //1ȭ�� �߻�����Ʈ

    [Header("������ ����")]
    public Transform MagicPos; //������������ġ
    public GameObject MageDefaultAttack; //������ �⺻����
    public GameObject Mage1SkillEff; //������1��ų
    public Transform Mage1SkillPos1; //1��ų ������ġ
    public Transform Mage1SkillPos2;
    public Transform Mage1SkillPos3;
    public GameObject Mage2SkillEff; //2��ų ����Ʈ
    public GameObject Mage2SkillReadyEff; //2��ų �غ�������Ʈ

    void Start()
    {
        Aurapos = GameObject.Find("SelManager").transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).transform.
            GetChild(0).transform.GetChild(2).transform;
        arrowPos = GameObject.Find("SelManager").transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).transform.
           GetChild(1).transform.GetChild(2).transform;
        MagicPos = GameObject.Find("SelManager").transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).transform.
           GetChild(2).transform.GetChild(2).transform.GetChild(3).transform;
        Mage1SkillPos1 = GameObject.Find("SelManager").transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).transform.
           GetChild(2).transform.GetChild(2).transform.GetChild(0).transform;
        Mage1SkillPos2 = GameObject.Find("SelManager").transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).transform.
           GetChild(2).transform.GetChild(2).transform.GetChild(1).transform;
        Mage1SkillPos3 = GameObject.Find("SelManager").transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).transform.
           GetChild(2).transform.GetChild(2).transform.GetChild(2).transform;

        SwordAura = Resources.Load<GameObject>("Sel@Skills/Sel@Slash");
        ArcDefaultAttack = Resources.Load<GameObject>("Sel@Skills/Sel@Arrow");
        Arc1Skilarrow = Resources.Load<GameObject>("Sel@Skills/Sel@PoisonArrow");
        Arc3SkillArrow1 = Resources.Load<GameObject>("Sel@Skills/Sel@EnergyArrow1");
        MageDefaultAttack = Resources.Load<GameObject>("Sel@Skills/Sel@MageDefaultAttack");
        Mage1SkillEff = Resources.Load<GameObject>("Sel@Skills/Sel@Mage1Skill");
        Mage2SkillEff = GameObject.Find("SelManager").transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).transform.
           GetChild(2).transform.GetChild(3).gameObject;
        Mage2SkillReadyEff = GameObject.Find("SelManager").transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).transform.
           GetChild(2).transform.GetChild(4).gameObject;

        chSelSkill = this;
    }
    public IEnumerator WarriorSkill0()
    {
        yield return new WaitForSeconds(0.5f);
        CharacterSel.characterSel.wSkill0 = 0;
    } //���� ���ݹ�ư ������
    public IEnumerator WarriorSkill1()
    {
        yield return new WaitForSeconds(1f);
        CharacterSel.characterSel.wSkill1 = 0;
    }//���� ��ų1��ư ������
    public IEnumerator WarriorSkill2()
    {
        yield return new WaitForSeconds(0.7f);
        GameObject swordaura = Instantiate(SwordAura, Aurapos.transform.position, Aurapos.transform.rotation);
        Rigidbody aurarigid = swordaura.GetComponent<Rigidbody>();
        aurarigid.velocity = Aurapos.forward * 20;
        Destroy(swordaura, 1f);
        yield return new WaitForSeconds(0.8f);
        CharacterSel.characterSel.wSkill2 = 0;
    }//���� ��ų2��ư ������
    public IEnumerator ArcherSkill0() //�ü� ���ݹ�ư ������
    {
        yield return new WaitForSeconds(0.2f);
        GameObject intantArrow = Instantiate(ArcDefaultAttack, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward * 150;
        yield return new WaitForSeconds(0.5f);
        CharacterSel.characterSel.aSkill0 = 0;
    } 
    public IEnumerator ArcherSkill1() //�ü� ��ų1��ư ������
    {
        yield return new WaitForSeconds(0.2f);
        GameObject intantArrow = Instantiate(Arc1Skilarrow, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward * 150;
        yield return new WaitForSeconds(0.5f);
        CharacterSel.characterSel.aSkill1 = 0;
    }
    public IEnumerator ArcherSkill2() //�ü� ��ų2��ư ������
    {
        yield return new WaitForSeconds(0.2f);
        GameObject intantArrow = Instantiate(Arc3SkillArrow1, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward * 200;
        Destroy(intantArrow, 1f);
        yield return new WaitForSeconds(1f);
        CharacterSel.characterSel.aSkill2 = 0;
    }
    public IEnumerator MageSkill0() //���� ���ݹ�ư ������
    {
        yield return new WaitForSeconds(0.3f);
        GameObject intantArrow = Instantiate(MageDefaultAttack, MagicPos.position, MagicPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = MagicPos.forward * 20;
        Destroy(intantArrow, 0.7f);
        yield return new WaitForSeconds(0.3f);
        CharacterSel.characterSel.mSkill0 = 0;
    } 
    public IEnumerator MageSkill1() //���� ��ų1��ư ������
    {
        yield return new WaitForSeconds(1.2f);
        GameObject darkball1 = Instantiate(Mage1SkillEff, MagicPos.position, MagicPos.rotation);
        GameObject darkball2 = Instantiate(Mage1SkillEff, MagicPos.position, MagicPos.rotation);
        GameObject darkball3 = Instantiate(Mage1SkillEff, MagicPos.position, MagicPos.rotation);
        darkball1.transform.DOMove(Mage1SkillPos1.position, 1f).SetEase(Ease.Linear);
        darkball2.transform.DOMove(Mage1SkillPos2.position, 1f).SetEase(Ease.Linear);
        darkball3.transform.DOMove(Mage1SkillPos3.position, 1f).SetEase(Ease.Linear);
        Destroy(darkball1, 1.1f);
        Destroy(darkball2, 1.1f);
        Destroy(darkball3, 1.1f);
        yield return new WaitForSeconds(0.5f);
        CharacterSel.characterSel.mSkill1 = 0;
    }
    public IEnumerator MageSkill2() //���� ��ų2��ư ������
    {
        Mage2SkillReadyEff.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        Mage2SkillReadyEff.SetActive(false);
        Mage2SkillEff.SetActive(true);


        yield return new WaitForSeconds(1f);
        Mage2SkillEff.SetActive(false);
        CharacterSel.characterSel.mSkill2 = 0;
    }
}
