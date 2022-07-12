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
    [SerializeField]
    GameObject MakeButton;
    [SerializeField]
    GameObject nickName;
    [SerializeField]
    GameObject nickName2;


    [SerializeField]
    Image[] abillity_Image;
    [SerializeField]
    Text[] abillity_Text;

    //클릭 했을때 커지고, +  에니메이션
    [SerializeField]
    GameObject[] infoClick;
    [SerializeField]
    RectTransform[] characterInfo;


    float[,] Abillity = new float[3, 3] { { 70, 90, 40 }, { 80, 70, 50 }, { 50, 50, 100 } };




    public float pageSpeed=0.1f;//페이지 하나와 하나의 간격
    public float pageFlipSpeed = 0.02f; //페이지 넘기는 속도

    [SerializeField]
    CharacterSel chSel;

    [SerializeField]
    public Text startText;
    string startString = "GameStart";
    private void Start()
    {
        chSel = GetComponent<CharacterSel>();
        
    }


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


        if (Start && i == 0)
        {
            yield return new WaitForSeconds(0.2f);
            allblcok.SetActive(false);
        }
        if (!Start)
            allblcok.SetActive(false);
    }

    IEnumerator P_Effect_Right(int i)
    {
        
        yield return new WaitForSeconds(i * pageSpeed);

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

        chSel.CharButton1();
        if (chSel.character1 == CharacterSel.Type.None) { 
            allblcok.SetActive(true);
            StartCoroutine(P_Effect_Right(0));
            StartCoroutine(P_Effect_Right(1));
            StartCoroutine(InfoReset());
            infoClick[0].SetActive(true);
            infoClick[1].SetActive(true);
            startText.text = "";
            if (chSel.characterAni2D[1])
                chSel.characterAni2D[1].SetBool("run", false);
            nickName.GetComponent<InputField>().text = "";
        }
        else
        {
           
            infoClick[0].SetActive(false);
            infoClick[1].SetActive(true);
            if (chSel.characterAni2D[0])
                chSel.characterAni2D[0].SetBool("run", true);
            if (chSel.characterAni2D[1])
               chSel.characterAni2D[1].SetBool("run", false);
            StopAllCoroutines();
            StartCoroutine(StartText());
            StartCoroutine(InfoClick());
        }

 

    }
    public void Character2Button()
    {
        chSel.CharButton2();
        if (chSel.character2 == CharacterSel.Type.None)
        {
            infoClick[0].SetActive(true);
            infoClick[1].SetActive(true);
            allblcok.SetActive(true);
            StartCoroutine(P_Effect_Right(0));
            StartCoroutine(P_Effect_Right(1));
            StartCoroutine(InfoReset());
            startText.text = "";
            if (chSel.characterAni2D[0])
                chSel.characterAni2D[0].SetBool("run", false);
            nickName2.GetComponent<InputField>().text = "";
        }
        else
        {
           
            infoClick[0].SetActive(true);
            infoClick[1].SetActive(false);
            if (chSel.characterAni2D[1])
                chSel.characterAni2D[1].SetBool("run",true);
            if (chSel.characterAni2D[0])
                chSel.characterAni2D[0].SetBool("run", false);
            StopAllCoroutines();
            StartCoroutine(StartText());
            StartCoroutine(InfoClick());
        }



    }

    IEnumerator InfoClick()
    {
        while (infoClick[1].activeSelf && characterInfo[0].localScale.x <1.05f)
        {            
            yield return new WaitForSeconds(0.02f);
            float Scale = Mathf.Lerp(characterInfo[0].localScale.x, 1.4f, Time.deltaTime * 4f);
            characterInfo[0].localScale = Vector3.one * Scale;
            float Scale2 = Mathf.Lerp(characterInfo[1].localScale.x, 0.6f, Time.deltaTime * 4f);
            characterInfo[1].localScale = Vector3.one * Scale2;

        }
        while (infoClick[0].activeSelf && characterInfo[1].localScale.x < 1.05f)
        {
            yield return new WaitForSeconds(0.02f);
            float Scale = Mathf.Lerp(characterInfo[1].localScale.x, 1.4f, Time.deltaTime * 4f);
            characterInfo[1].localScale = Vector3.one * Scale;
            float Scale2 = Mathf.Lerp(characterInfo[0].localScale.x, 0.6f, Time.deltaTime * 4f);
            characterInfo[0].localScale = Vector3.one * Scale2;
        }
    }
    IEnumerator InfoReset()
    {
        while (characterInfo[0].localScale.x <= 1.01f || characterInfo[0].localScale.x >= 0.99f && characterInfo[1].localScale.x <= 1.01f || characterInfo[1].localScale.x >= 0.99f)
        {
            yield return new WaitForSeconds(0.02f);
            float Scale = Mathf.Lerp(characterInfo[0].localScale.x, 1f, Time.deltaTime * 4f);
            characterInfo[0].localScale = Vector3.one * Scale;
            float Scale2 = Mathf.Lerp(characterInfo[1].localScale.x, 1f, Time.deltaTime * 4f);
            characterInfo[1].localScale = Vector3.one * Scale2;

        }
    }

    public void Backbutton()
    {
        Debug.Log("뒤로가기 버튼");
        {
            StopAllCoroutines();
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
            MakeButton.SetActive(false);
            nickName.SetActive(false);
            nickName2.SetActive(false);
            StartCoroutine(AbilliytyReset());
           
            
        }
    }


    
    public void WorriorButton()
    {
        if (character == 0)
            return;
        chSel.WorriorBut();
        Debug.Log("워리어 생성선택 버튼");
        character = 0;
        
        CharButton();

    }

    public void ArcherButton()
    {
        if (character == 1)
            return;
        chSel.ArcherBut();
        Debug.Log("아쳐 생성선택 버튼");
        character = 1;
        CharButton();
    }

    public void MageButton()
    {
        if (character == 2)
            return;
        chSel.MageBut();
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
               
                StartCoroutine(buttonCloseUp(i));
            }
            else
            {
                
                StartCoroutine(buttonCloseDown(i));
            }
        }

        StartCoroutine(InfoSkill());
        StartCoroutine(Character());
        StartCoroutine(AbilltyFillAmount());

        if (chSel.charSel == 1)
        {
            nickName.SetActive(true);
        }
        else if (chSel.charSel == 2)
        {
            nickName2.SetActive(true);
        }
        MakeButton.SetActive(true);
    }

    IEnumerator buttonCloseUp(int i)
    {
        
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

    IEnumerator AbilltyFillAmount()
    {
        StartCoroutine(AbilliytyReset());

        while(abillity_Image[0].fillAmount >0.1f || abillity_Image[1].fillAmount > 0.1f || abillity_Image[2].fillAmount > 0.1f)
        {
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);
        float attack=0;
        float shiel=0;
        float mageDamage=0;

        while(attack <= Abillity[character,0] || shiel <= Abillity[character, 1] || mageDamage <= Abillity[character, 2])
        {
            yield return new WaitForSeconds(0.02f);
            if (attack < Abillity[character, 0]) //능력치 점점 채워지게 fillamount와 텍스트 값 올라감
            {
                attack++;
                abillity_Image[0].fillAmount = attack/100;
                abillity_Text[0].text = ((int)attack).ToString();
            }
            if (shiel < Abillity[character, 1])
            {
                shiel++;
                abillity_Image[1].fillAmount = shiel / 100;
                abillity_Text[1].text = ((int)shiel).ToString();
            }
            if (mageDamage < Abillity[character, 2])
            {
                mageDamage++;
                abillity_Image[2].fillAmount = mageDamage / 100;
                abillity_Text[2].text = ((int)mageDamage).ToString();
            }
            

        }
            

    }
    IEnumerator AbilliytyReset()
    {
        float attack = abillity_Image[0].fillAmount*100;
        float shiel = abillity_Image[1].fillAmount*100;
        float mageDamage = abillity_Image[2].fillAmount*100;

        while (attack >= 0 || shiel >= 0 || mageDamage >= 0)
        {
            yield return new WaitForSeconds(0.01f);
            if (attack >= 0.1f) //능력치 0으로 쭉 내려감
            {
                attack-=2;
                Debug.Log(attack);
                if (attack < 0)
                    attack = 0;
                abillity_Image[0].fillAmount = attack / 100;
                abillity_Text[0].text = ((int)attack).ToString();
            }
            if (shiel >= 0.1f)
            {

                shiel-=2;
                if (shiel < 0)
                    shiel = 0;
                abillity_Image[1].fillAmount = shiel / 100;
                abillity_Text[1].text = ((int)shiel).ToString();
            }
            if (mageDamage >= 0.1f)
            {
                mageDamage-=2;
                if (mageDamage < 0)
                    mageDamage = 0;
                abillity_Image[2].fillAmount = mageDamage / 100;
                abillity_Text[2].text = ((int)mageDamage).ToString();
            }


        }
    }

    public void MakeButtonClick()
    {
        chSel.MakeBut();
        Backbutton();
        

    }


    IEnumerator StartText()
    {
        if (startText.text != "")
            yield break;

        for (int i = 0; i < startString.Length; i++)
        {
            yield return new WaitForSeconds(0.02f);
            startText.text += startString[i];
        }
    }

    public void StartButton()
    {
        
    }

}
