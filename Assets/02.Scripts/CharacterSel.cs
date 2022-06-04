using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSel : MonoBehaviour
{
    public static CharacterSel characterSel;

    

    public GameObject characterScene;
    public GameObject sel;
    public GameObject make;
    public enum Type { None,Warrior, Archer, Mage  };

    //캐릭터 선택창
    public Type character1=Type.None;
    public GameObject[] char1;
    public GameObject maketext;
    public GameObject maketext2;
    public Type character2=Type.None;
    public GameObject[] char2;
    public int charSel;


    //캐릭터 생성창
    
    public Type MakeType=Type.None;
    public GameObject[] makeChar;
    public Text characterText;
    public GameObject[] explanation;
    public Animator aniWor;
    public Animator aniArc;
    public Animator aniMage;

    private SaveManager saveManager;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();

       if(CharacterSel.characterSel == null)
        {
            characterSel = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }



    public void CharButton1()
    {
        charSel = 1;
        if(character1==Type.None)
        {
            sel.SetActive(false);
            make.SetActive(true);
            makeChar[0].SetActive(true);
            makeChar[1].SetActive(false);
            makeChar[2].SetActive(false);
            explanation[0].SetActive(true);
            explanation[1].SetActive(false);
            explanation[2].SetActive(false);
            characterText.text = "WARRIOR";


            MakeType = Type.Warrior;
        }
    
        else
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(LoadCoroutine());
            SceneManager.LoadScene(1);
            
        }

        IEnumerator LoadCoroutine()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1);

            while(!operation.isDone)
            {
                yield return null;
            }

            GameStart();
           
            


        }

    }

    void GameStart()
    {

        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void GameEnd()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void CharButton2()
    {
        charSel = 2;
        if (character2 == Type.None)
        {
            sel.SetActive(false);
            make.SetActive(true);
            makeChar[0].SetActive(true);
            makeChar[1].SetActive(false);
            makeChar[2].SetActive(false);
            explanation[0].SetActive(true);
            explanation[1].SetActive(false);
            explanation[2].SetActive(false);
            characterText.text = "WARRIOR";
            MakeType = Type.Warrior;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(1);
 
        }
    }

    public void WorriorBut()
    {
        makeChar[0].SetActive(true);
        makeChar[1].SetActive(false);
        makeChar[2].SetActive(false);
        explanation[0].SetActive(true);
        explanation[1].SetActive(false);
        explanation[2].SetActive(false);
        MakeType = Type.Warrior;
        characterText.text = "WARRIOR";
    }
    public void ArcherBut()
    {
        makeChar[0].SetActive(false);
        makeChar[1].SetActive(true);
        makeChar[2].SetActive(false);
        explanation[0].SetActive(false);
        explanation[1].SetActive(true);
        explanation[2].SetActive(false);
        MakeType = Type.Archer;
        characterText.text = "ARCHER";
    }
    public void MageBut()
    {
        makeChar[0].SetActive(false);
        makeChar[1].SetActive(false);
        makeChar[2].SetActive(true);
        explanation[0].SetActive(false);
        explanation[1].SetActive(false);
        explanation[2].SetActive(true);
        MakeType = Type.Mage;
        characterText.text = "MAGE";
    }


    public void BackBut()
    {
        make.SetActive(false);
        sel.SetActive(true);

    }
    public void attackBut()
    {
        if (MakeType == Type.Warrior)
        {
            aniWor.SetTrigger("doSwing");
        }
        else if (MakeType == Type.Archer)
        {
            aniArc.SetTrigger("doSwing");
        }
        else if (MakeType == Type.Mage)
        {
            aniMage.SetTrigger("doSwing");
        }
    }
    
    public void MakeBut()
    {
        make.SetActive(false);
        sel.SetActive(true);
        
        if (charSel == 1)
        {
            maketext.SetActive(false);
            character1 = MakeType;
            if (MakeType == Type.Warrior)
            {
                char1[0].SetActive(true);
                char1[1].SetActive(false);
                char1[2].SetActive(false);
            }
            else if (MakeType == Type.Archer)
            {
                char1[0].SetActive(false);
                char1[1].SetActive(true);
                char1[2].SetActive(false);
            }
            else if (MakeType == Type.Mage)
            {
                char1[0].SetActive(false);
                char1[1].SetActive(false);
                char1[2].SetActive(true);
            }
            saveManager.CharacterSelSave1();

        }
        if (charSel == 2)
        {
            maketext2.SetActive(false);
            character2 = MakeType;
            if (MakeType == Type.Warrior)
            {
                char2[0].SetActive(true);
                char2[1].SetActive(false);
                char2[2].SetActive(false);
            }
            else if (MakeType == Type.Archer)
            {
                char2[0].SetActive(false);
                char2[1].SetActive(true);
                char2[2].SetActive(false);
            }
            else if (MakeType == Type.Mage)
            {
                char2[0].SetActive(false);
                char2[1].SetActive(false);
                char2[2].SetActive(true);
            }
            saveManager.CharacterSelSave2();
        }
       
    }


   
}
