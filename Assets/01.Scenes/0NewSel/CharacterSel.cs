using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CharacterSel : MonoBehaviourPunCallbacks
{
    public static CharacterSel characterSel;




    public enum Type { None,Warrior, Archer, Mage  };

    //캐릭터 선택창
    public Type character1=Type.None;

    public Type character2=Type.None;

    public int charSel;
    public string Char1Stringname; //세이브매니저에서 저장용
    public string Char2Stringname;
    public Text Char1Name;
    public Text Char2Name;
    public Nickname nickname;
    public CharacterSelEffect ch_selEffect;

    //캐릭터 생성창

    public Type MakeType=Type.None;

    public Animator aniWor;
    public Animator aniArc;
    public Animator aniMage;

    private SaveManager saveManager;

    public AudioListener audioListener;

    [SerializeField]
    GameObject[] characterImage1;
    [SerializeField]
    GameObject[] characterImage2;
    [SerializeField]
    public Animator[] characterAni2D = new Animator[2];

    int widx = 0; //전사 대사소리
    int aidx = 0; //궁수 대사소리
    int midx = 0; //마법사 대사소리
    [Header("애니메이션 버튼 연타방지 & 애니메이션 끝나기전에 다른 애니메이션 버튼 누르는거 방지")]
    public int wSkill0 = 0; //전사 공격 스위치 
    public int wSkill1 = 0; //전사 스킬1 스위치 
    public int wSkill2 = 0; //전사 스킬2 스위치 
    public int aSkill0 = 0; //궁수 공격 스위치 
    public int aSkill1 = 0; //궁수 스킬1 스위치 
    public int aSkill2 = 0; //궁수 스킬2 스위치 
    public int mSkill0 = 0; //법사 공격 스위치 
    public int mSkill1 = 0; //법사 스킬1 스위치 
    public int mSkill2 = 0; //법사 스킬2 스위치 

    private void Awake()
    {
        nickname = GetComponent<Nickname>();
        audioListener = GetComponent<AudioListener>();
        saveManager = FindObjectOfType<SaveManager>();
        //LobbyUi = GameObject.Find("Canvas_Lobby").transform.GetChild(0).gameObject;
        ch_selEffect = GetComponent<CharacterSelEffect>();
        if (CharacterSel.characterSel == null)
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
        UiSound.uiSound.UiOptionSound();
        if (character1==Type.None)
        {
            //nickname.nickNameInput1.gameObject.SetActive(true);
            //nickname.nickNameInput2.gameObject.SetActive(false);
            
 
        }
    
        else
        {
            DontDestroyOnLoad(gameObject);
            audioListener.enabled = true;

            if (PhotonNetwork.LocalPlayer.NickName != null)
            {
                PhotonNetwork.LocalPlayer.NickName = saveManager.Getname1;
            }
        }

        IEnumerator LoadCoroutine()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1);

            while(!operation.isDone)
            {
                yield return null;
            }


            saveManager = FindObjectOfType<SaveManager>();
            saveManager.CharacterNum = 0;
            GameStart();                    
        }

    }

    public void GameStart()
    {       
        saveManager.LoadCharacter();
        saveManager.SaveOn = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void GameEnd()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }

    public void CharButton2()
    {
        charSel = 2;
        UiSound.uiSound.UiOptionSound();
        if (character2 == Type.None)
        {
            //nickname.nickNameInput2.gameObject.SetActive(true);
            //nickname.nickNameInput1.gameObject.SetActive(false);
            

        }
        else
        {
            DontDestroyOnLoad(gameObject);
            //transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            audioListener.enabled = true;

            if(PhotonNetwork.LocalPlayer.NickName != null)
            PhotonNetwork.LocalPlayer.NickName = saveManager.Getname2;
        }
    }

    IEnumerator LoadCoroutine2()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        while (!operation.isDone)
        {
            yield return null;
        }


        saveManager = FindObjectOfType<SaveManager>();
        saveManager.CharacterNum = 1;
        GameStart();
    }

    public void WorriorBut()
    {
        UiSound.uiSound.UiOptionSound();
        if (widx == 0)
        {
            StopSoundManager.stopSoundManager.audioSource.Stop();
            StopSoundManager.stopSoundManager.WarriorTalk1();
            widx = 1;
        }
        else
        {
            StopSoundManager.stopSoundManager.audioSource.Stop();
            StopSoundManager.stopSoundManager.WarriorTalk2();
            widx = 0;
        }


        MakeType = Type.Warrior;

    }
    public void ArcherBut()
    {
        UiSound.uiSound.UiOptionSound();
        if (aidx == 0)
        {
            StopSoundManager.stopSoundManager.audioSource.Stop();
            StopSoundManager.stopSoundManager.ArcherTalk1();
            aidx = 1;
        }
        else
        {
            StopSoundManager.stopSoundManager.audioSource.Stop();
            StopSoundManager.stopSoundManager.ArcherTalk2();
            aidx = 0;
        }


        MakeType = Type.Archer;

    }
    public void MageBut()
    {
        UiSound.uiSound.UiOptionSound();
        if (midx == 0)
        {
            StopSoundManager.stopSoundManager.audioSource.Stop();
            StopSoundManager.stopSoundManager.MageTalk1();
            midx = 1;
        }
        else
        {
            StopSoundManager.stopSoundManager.audioSource.Stop();
            StopSoundManager.stopSoundManager.MageTalk2();
            midx = 0;
        }


        MakeType = Type.Mage;

    }


    public void BackBut()
    {
        StopSoundManager.stopSoundManager.audioSource.Stop();
        UiSound.uiSound.UiOptionSound();

    }
    public void attackBut()
    {
        Debug.Log(1);
        StopSoundManager.stopSoundManager.audioSource.Stop();
        UiSound.uiSound.UiOptionSound();
        
        if (MakeType == Type.Warrior && wSkill0==0 && wSkill1==0 && wSkill2 == 0 )
        {
            wSkill0 = 1;
            aniWor.SetTrigger("doSwing");
            StopSoundManager.stopSoundManager.SelWarriorAttackSound();
            Debug.Log(ChSelSkill.chSelSkill);
            ChSelSkill.chSelSkill.StartCoroutine("WarriorSkill0");
        }
        else if (MakeType == Type.Archer && aSkill0 == 0 && aSkill1 == 0 && aSkill2 == 0)
        {
            aSkill0 = 1;
            aniArc.SetTrigger("doSwing");
            StopSoundManager.stopSoundManager.SelArcherAttackSound();
            ChSelSkill.chSelSkill.StartCoroutine("ArcherSkill0");
        }
        else if (MakeType == Type.Mage && mSkill0 == 0 && mSkill1 == 0 && mSkill2 == 0)
        {
            mSkill0 = 1;
            aniMage.SetTrigger("doSwing");
            StopSoundManager.stopSoundManager.SelMageAttackSound();
            ChSelSkill.chSelSkill.StartCoroutine("MageSkill0");
        }
    }
    public void skill1But()
    {
        StopSoundManager.stopSoundManager.audioSource.Stop();
        UiSound.uiSound.UiOptionSound();

        if (MakeType == Type.Warrior && wSkill0 == 0 && wSkill1 == 0 && wSkill2 == 0)
        {
            wSkill1 = 1;
            aniWor.SetTrigger("doSkill1");
            StopSoundManager.stopSoundManager.SelWarriorAttackSound();
            ChSelSkill.chSelSkill.StartCoroutine("WarriorSkill1");
        }
        else if (MakeType == Type.Archer && aSkill0 == 0 && aSkill1 == 0 && aSkill2 == 0)
        {
            aSkill1 = 1;
            aniArc.SetTrigger("doSwing");
            StopSoundManager.stopSoundManager.SelArcherAttackSound();
            ChSelSkill.chSelSkill.StartCoroutine("ArcherSkill1");
        }
        else if (MakeType == Type.Mage && mSkill0 == 0 && mSkill1 == 0 && mSkill2 == 0)
        {
            mSkill1 = 1;
            aniMage.SetTrigger("doSkill1");
            StopSoundManager.stopSoundManager.SelMageAttackSound();
            ChSelSkill.chSelSkill.StartCoroutine("MageSkill1");
        }
    }
    public void skill2But()
    {
        StopSoundManager.stopSoundManager.audioSource.Stop();
        UiSound.uiSound.UiOptionSound();

        if (MakeType == Type.Warrior && wSkill0 == 0 && wSkill1 == 0 && wSkill2 == 0)
        {
            wSkill2 = 1;
            aniWor.SetTrigger("doSkill2");
            StopSoundManager.stopSoundManager.SelWarriorAttackSound();
            ChSelSkill.chSelSkill.StartCoroutine("WarriorSkill2");
        }
        else if (MakeType == Type.Archer && aSkill0 == 0 && aSkill1 == 0 && aSkill2 == 0)
        {
            aSkill2 = 1;
            aniArc.SetTrigger("doSwing");
            StopSoundManager.stopSoundManager.SelArcherAttackSound();
            ChSelSkill.chSelSkill.StartCoroutine("ArcherSkill2");
        }
        else if (MakeType == Type.Mage && mSkill0 == 0 && mSkill1 == 0 && mSkill2 == 0)
        {
            mSkill2 = 1;
            aniMage.SetTrigger("doSkill2");
            StopSoundManager.stopSoundManager.SelMageAttackSound();
            ChSelSkill.chSelSkill.StartCoroutine("MageSkill2");
        }
    }

    public void MakeBut()
    {

        
        if (charSel == 1)
        {

            character1 = MakeType;
            if (MakeType == Type.Warrior)
            {
                characterImage1[0].SetActive(true);
                characterImage1[1].SetActive(false);
                characterImage1[2].SetActive(false);
                
                
            }
            else if (MakeType == Type.Archer)
            {
                characterImage1[0].SetActive(false);
                characterImage1[1].SetActive(true);
                characterImage1[2].SetActive(false);
            }
            else if (MakeType == Type.Mage)
            {
                characterImage1[0].SetActive(false);
                characterImage1[1].SetActive(false);
                characterImage1[2].SetActive(true);
            }
            int index = -1;
            for (int i = 0; i < 3; i++)
            {
                
                if (characterImage1[i].activeSelf)
                {
                    index = i;
                    break;
                }
                
            }
            if(index != -1)
            {
                characterAni2D[0]= characterImage1[index].transform.GetChild(0).GetComponent<Animator>();
            }


            saveManager.CharacterSelSave1();

        }
        if (charSel == 2)
        {

            character2 = MakeType;
            if (MakeType == Type.Warrior)
            {
                characterImage2[0].SetActive(true);
                characterImage2[1].SetActive(false);
                characterImage2[2].SetActive(false);
            }
            else if (MakeType == Type.Archer)
            {
                characterImage2[0].SetActive(false);
                characterImage2[1].SetActive(true);
                characterImage2[2].SetActive(false);
            }
            else if (MakeType == Type.Mage)
            {
                characterImage2[0].SetActive(false);
                characterImage2[1].SetActive(false);
                characterImage2[2].SetActive(true);
            }

            int index = -1;
            for (int i = 0; i < 3; i++)
            {

                if (characterImage2[i].activeSelf)
                {
                    index = i;
                    break;
                }

            }
            if (index != -1)
            {
                characterAni2D[1] = characterImage2[index].transform.GetChild(0).GetComponent<Animator>();
            }

            saveManager.CharacterSelSave2();
        }
       
    }



}
