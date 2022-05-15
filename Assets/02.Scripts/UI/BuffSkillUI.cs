using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffSkillUI : MonoBehaviour
{
    [SerializeField]
    private Image BuffSkillImage;
    [SerializeField]
    private Image BuffSkillCoolImage;
    [SerializeField]
    private GameObject buffSkillUI;
    [SerializeField]
    private AttackDamage attckDamage;
  
    public enum BuffSkills { WarriorBuff1, ArcherBuff1 }
    
    void Start()
    {
        BuffSkillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        BuffSkillCoolImage = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        attckDamage = FindObjectOfType<AttackDamage>();
        buffSkillUI = transform.GetChild(0).gameObject;
    }
    public void OnSkillBuffImage()
    {
        buffSkillUI.SetActive(true);
    }
    public void OffSkillBuffImage()
    {

        buffSkillUI.SetActive(false);
    }

    public void BuffOn( BuffSkills buffname, Sprite skillImage)
    {
        BuffSkillImage.sprite = skillImage;
        BuffSkillCoolImage.sprite = skillImage;
        if (buffname == BuffSkills.WarriorBuff1)
        {
            StartCoroutine(WarriorBuff1());
            Invoke("OffSkillBuffImage", attckDamage.Skill_Buff_duration);
        }
        else if (buffname == BuffSkills.ArcherBuff1)
        {
            StartCoroutine(ArcherBuff1());
            Invoke("OffSkillBuffImage", attckDamage.Skill_Buff_duration);
        }

    }


    IEnumerator WarriorBuff1()
    {
        StopCoroutine(WarriorBuff1());
        OnSkillBuffImage();   
        float Buff1 = 1 - attckDamage.SkillBuff_passedDurationgTime / attckDamage.Skill_Buff_duration;
        while (Buff1 >= 0f)
        {
            Buff1 = 1 - attckDamage.SkillBuff_passedDurationgTime / attckDamage.Skill_Buff_duration;
            if (Buff1 != 1)
                BuffSkillCoolImage.fillAmount = Buff1;

            yield return new WaitForSeconds(0.02f);

        }
       
        yield return null;
    }

    IEnumerator ArcherBuff1()
    {
        StopCoroutine(ArcherBuff1());
        OnSkillBuffImage();
        float Buff2 = 1 - attckDamage.SkillBuff_passedDurationgTime / attckDamage.Skill_Buff_duration;
        while (Buff2 >= 0f)
        {
            Buff2 = 1 - attckDamage.SkillBuff_passedDurationgTime / attckDamage.Skill_Buff_duration;
            if (Buff2 != 1)
                BuffSkillCoolImage.fillAmount = Buff2;

            yield return new WaitForSeconds(0.02f);

        }

       
        yield return null;
    }
}
