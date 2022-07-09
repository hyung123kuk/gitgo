using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelEffect : MonoBehaviour
{
    [SerializeField]
    List<RectTransform> Page = new List<RectTransform>(); // [0]-> 시작페이지 ,[1] -> 캐릭터만들기페이지, [2] ->캐릭터만들기 2번째페이지
    [SerializeField]
    RectTransform startPointPage;
    [SerializeField]
    GameObject LeftPageParent;
    [SerializeField]
    GameObject RightPageParent;
    [SerializeField]
    RectTransform endPontPage;
    [SerializeField]
    GameObject allblcok;

    int character=-1; //0:워리어 1:아처 2:메이지
    [SerializeField]
    GameObject[] Characters; //0:워리어 1:아처 2:메이지
    [SerializeField]
    GameObject Ch_Effect;
    [SerializeField]
    RectTransform[] Ch_button; //0:워리어 1:아처 2:메이지
    [SerializeField]
    Image[] block;
    [SerializeField]
    GameObject[] Ch_InfoText;
    [SerializeField]
    GameObject[] Ch_Skill;



    public float pageSpeed=0.1f;//페이지 하나와 하나의 간격
    public float pageFlipSpeed = 0.02f; //페이지 넘기는 속도


    public void PageEffect()
    {
       
        for (int i = 0; i < Page.Count; i++)
        {
            allblcok.SetActive(true);
            StartCoroutine(P_Effect_Left(Page.Count- 1 - i));
        }
    }
    IEnumerator P_Effect_Left(int i, bool Start = true)
    {
        

        if (Start)
            yield return new WaitForSeconds((Page.Count - 1 - i)* pageSpeed);
        else
        {
            yield return new WaitForSeconds((1 - i) * pageSpeed);
        }
        

        
        while (Page[i].localEulerAngles.y >= 200)
        {

            yield return new WaitForSeconds(0.001f);
            Page[i].rotation = Quaternion.Slerp(Page[i].rotation, endPontPage.rotation, pageFlipSpeed * Time.deltaTime);
            if(Page[i].localEulerAngles.y <= 270 && Page[i].parent!=RightPageParent.transform)
            {
                Page[i].parent = RightPageParent.transform;
            }

        }

        
        if(Start && i==1)
             allblcok.SetActive(false);
        if (!Start)
            allblcok.SetActive(false);
    }

    IEnumerator P_Effect_Right(int i)
    {
        
        yield return new WaitForSeconds(i * pageSpeed);

        Debug.Log(Page[i].localEulerAngles.y);
        Debug.Log(startPointPage.localEulerAngles.y);
        while (Page[i].localEulerAngles.y <= 340)
        {

            yield return new WaitForSeconds(0.001f);
            Page[i].rotation = Quaternion.Slerp(Page[i].rotation, startPointPage.rotation, pageFlipSpeed * Time.deltaTime);
            if (Page[i].localEulerAngles.y >= 300 && Page[i].parent != LeftPageParent.transform)
            {
                Page[i].parent = LeftPageParent.transform;
            }
           
        }
        allblcok.SetActive(false);
    }

    public void Character1Button()
    {
        Debug.Log("만약에 캐릭터가 없다면");
        {
            allblcok.SetActive(true);
            StartCoroutine(P_Effect_Right(0));
            StartCoroutine(P_Effect_Right(1));
        }
    }
    public void Character2Button()
    {
        Debug.Log("만약에 캐릭터가 없다면");
        {
            allblcok.SetActive(true);
            StartCoroutine(P_Effect_Right(0));
            StartCoroutine(P_Effect_Right(1));
        }
    }

    public void Backbutton()
    {
        Debug.Log("뒤로가기 버튼");
        {
            allblcok.SetActive(true);
            StartCoroutine(P_Effect_Left(1, false));
            StartCoroutine(P_Effect_Left(0,false));
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[i].SetActive(false);
                Ch_button[i].localScale = Vector3.one * 1f;
            }
            character = -1;
            block[0].fillAmount = 1f;
            block[1].fillAmount = 1f;

           
        }
    }


    
    public void WorriorButton()
    {
        if (character == 0)
            return;
        Debug.Log("워리어 생성선택 버튼");
        character = 0;
        
        CharButton();

    }

    public void ArcherButton()
    {
        if (character == 1)
            return;
        Debug.Log("아쳐 생성선택 버튼");
        character = 1;
        CharButton();
    }

    public void MageButton()
    {
        if (character == 2)
            return;
        Debug.Log("마법사 생성선택 버튼");
        character = 2;
        CharButton();
    }

    void CharButton()
    {
        StopAllCoroutines();
        FindObjectOfType<csDestroyEffect>().particleOn();
        for (int i = 0; i < 3; i++)
        {
           
            if(character == i)
            {
                Debug.Log(1);
                StartCoroutine(buttonCloseUp(i));
            }
            else
            {
                
                StartCoroutine(buttonCloseDown(i));
            }
        }

        StartCoroutine(InfoSkill());
        StartCoroutine(Character());

    }

    IEnumerator buttonCloseUp(int i)
    {
        Debug.Log(Ch_button[i].localScale.x);
        while (Ch_button[i].localScale.x <  1.2f)
        {
            yield return new WaitForSeconds(0.02f);
            float Scale = Mathf.Lerp(Ch_button[i].localScale.x, 1.4f, Time.deltaTime * 4f);
            Ch_button[i].localScale = Vector3.one * Scale;

            
        }
    }

    IEnumerator buttonCloseDown(int i)
    {
        while (Ch_button[i].localScale.x > 0.8f)
        {
            yield return new WaitForSeconds(0.02f);
            float Scale = Mathf.Lerp(Ch_button[i].localScale.x, 0.6f, Time.deltaTime * 4f);
            Ch_button[i].localScale = Vector3.one * Scale;


        }
    }

    IEnumerator InfoSkill()
    {
        
        while (block[0].fillAmount <= 0.98f)
        {
            yield return new WaitForSeconds(0.02f);
            block[0].fillAmount = Mathf.Lerp(block[0].fillAmount, 1, Time.deltaTime * 10f);
            block[1].fillAmount = block[0].fillAmount;
        }

        for(int i = 0; i < 3; i++)
        {
            if(character == i)
            {
                Ch_InfoText[i].SetActive(true);
                Ch_Skill[i].SetActive(true);
            }
            else
            {
                Ch_InfoText[i].SetActive(false);
                Ch_Skill[i].SetActive(false);
            }
        }

        while(block[0].fillAmount >= 0.02f)
        {
            yield return new WaitForSeconds(0.02f);
            block[0].fillAmount = Mathf.Lerp(block[0].fillAmount, 0, Time.deltaTime * 10f);
            block[1].fillAmount = block[0].fillAmount;
        }

    }

    IEnumerator Character()
    {
        allblcok.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<csDestroyEffect>().particleOff();
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < 3; i++)
        {
            if(character == i)
            {
                Characters[i].SetActive(true);
            }
            else
            {
                Characters[i].SetActive(false);
            }
        }
        
        yield return new WaitForSeconds(0.2f);
        allblcok.SetActive(false);
    }

}
