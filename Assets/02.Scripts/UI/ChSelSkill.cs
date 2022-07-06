using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChSelSkill : MonoBehaviour
{
    public static ChSelSkill chSelSkill;


    [Header("전사 관련")]
    public Transform Aurapos; //스킬2 포스
    public GameObject SwordAura; //스킬2 이펙트

    [Header("궁수 관련")]
    public GameObject ArcDefaultAttack; //기본화살
    public Transform arrowPos; //화살나가는위치
    public GameObject Arc1Skilarrow; //독화살
    public GameObject Arc3SkillArrow1; //1화살 발사이펙트

    [Header("마법사 관련")]
    public Transform MagicPos; //마법나가는위치
    public GameObject MageDefaultAttack; //마법사 기본공격
    public GameObject Mage1SkillEff; //마법사1스킬
    public Transform Mage1SkillPos1; //1스킬 도착위치
    public Transform Mage1SkillPos2;
    public Transform Mage1SkillPos3;
    public GameObject Mage2SkillEff; //2스킬 이펙트
    public GameObject Mage2SkillReadyEff; //2스킬 준비동작이펙트

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
    } //전사 공격버튼 딜레이
    public IEnumerator WarriorSkill1()
    {
        yield return new WaitForSeconds(1f);
        CharacterSel.characterSel.wSkill1 = 0;
    }//전사 스킬1버튼 딜레이
    public IEnumerator WarriorSkill2()
    {
        yield return new WaitForSeconds(0.7f);
        GameObject swordaura = Instantiate(SwordAura, Aurapos.transform.position, Aurapos.transform.rotation);
        Rigidbody aurarigid = swordaura.GetComponent<Rigidbody>();
        aurarigid.velocity = Aurapos.forward * 20;
        Destroy(swordaura, 1f);
        yield return new WaitForSeconds(0.8f);
        CharacterSel.characterSel.wSkill2 = 0;
    }//전사 스킬2버튼 딜레이
    public IEnumerator ArcherSkill0() //궁수 공격버튼 딜레이
    {
        yield return new WaitForSeconds(0.2f);
        GameObject intantArrow = Instantiate(ArcDefaultAttack, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward * 150;
        yield return new WaitForSeconds(0.5f);
        CharacterSel.characterSel.aSkill0 = 0;
    } 
    public IEnumerator ArcherSkill1() //궁수 스킬1버튼 딜레이
    {
        yield return new WaitForSeconds(0.2f);
        GameObject intantArrow = Instantiate(Arc1Skilarrow, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward * 150;
        yield return new WaitForSeconds(0.5f);
        CharacterSel.characterSel.aSkill1 = 0;
    }
    public IEnumerator ArcherSkill2() //궁수 스킬2버튼 딜레이
    {
        yield return new WaitForSeconds(0.2f);
        GameObject intantArrow = Instantiate(Arc3SkillArrow1, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward * 200;
        Destroy(intantArrow, 1f);
        yield return new WaitForSeconds(1f);
        CharacterSel.characterSel.aSkill2 = 0;
    }
    public IEnumerator MageSkill0() //법사 공격버튼 딜레이
    {
        yield return new WaitForSeconds(0.3f);
        GameObject intantArrow = Instantiate(MageDefaultAttack, MagicPos.position, MagicPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = MagicPos.forward * 20;
        Destroy(intantArrow, 0.7f);
        yield return new WaitForSeconds(0.3f);
        CharacterSel.characterSel.mSkill0 = 0;
    } 
    public IEnumerator MageSkill1() //법사 스킬1버튼 딜레이
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
    public IEnumerator MageSkill2() //법사 스킬2버튼 딜레이
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
