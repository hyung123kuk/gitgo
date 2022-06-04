using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CharacterSelData
{
    public int Charater1;
    public int Charater2;

}

public class Data
{
    //매번업데이트
    public Vector3 Position;
    public float Hp;
    public float Mp;
    public int Level;
    public float Exp;
}


public class SaveManager : MonoBehaviour
{
    
    private string SAVE_DATA_DIRECTORY;//파일이름 Start에서 선언한다.
    #region 캐릭터 선택창 관련 변수
    private CharacterSelData characterSelData = new CharacterSelData();
    private CharacterSel characterSel;
    private string Sel_SAVE_FILENAME = "/CharacterSelSaveFile.txt";
    #endregion

    public bool SaveOn;
    public int CharacterNum;

    [SerializeField]
    private PlayerStat playerStat;
    [SerializeField]
    Data[] data = new Data[2];
        

    
    [SerializeField]
    Data ch1;
    [SerializeField]
    Data ch2;
    private string ch1_SAVE_FILENAME = "/Character1SaveFile.txt";

    private string ch2_SAVE_FILENAME = "/Character2SaveFile.txt";

    private void Awake()
    {
        data[0] = new Data();

        data[1] = new Data();
        
    }

    private void Start()
    {
        

        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }

        CharacterSelLoad();

        if (SaveOn)
        {
            StartCoroutine(SaveStart());
        }
    }

    

    private void Update()
    {

            

    }

    IEnumerator SaveStart()
    {
        yield return new WaitForSeconds(1.0f);
        while (true)
        {
            PositionSave();
            string json = JsonUtility.ToJson(data[CharacterNum]);

            if (CharacterNum == 0)
            {
                File.WriteAllText(SAVE_DATA_DIRECTORY + ch1_SAVE_FILENAME, json);
            }
            else if (CharacterNum == 1)
            {
                File.WriteAllText(SAVE_DATA_DIRECTORY + ch2_SAVE_FILENAME, json);
            }
            
            yield return new WaitForSeconds(0.2f); //0.2초마다 저장
        }
    }

    public void PositionSave()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        data[CharacterNum].Hp = playerStat._Hp;
        data[CharacterNum].Mp = playerStat._Mp;
        data[CharacterNum].Position = playerStat.gameObject.transform.position;
        data[CharacterNum].Exp = playerStat.NowExp;
        data[CharacterNum].Level = playerStat.Level;

    }

    
    public void LoadCharacter()
    {
        if (CharacterNum == 0) { 
            if(File.Exists(SAVE_DATA_DIRECTORY+ ch1_SAVE_FILENAME))
            {
                string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + ch1_SAVE_FILENAME);
                LoadCh(loadJson);
                
            }
        }
        else if(CharacterNum == 1)
        {
            if(File.Exists(SAVE_DATA_DIRECTORY + ch2_SAVE_FILENAME))
            {
                string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + ch2_SAVE_FILENAME);
                LoadCh(loadJson);
            }
        }


        
    }

    private void LoadCh(string _loadJson)
    {
        data[CharacterNum] = JsonUtility.FromJson<Data>(_loadJson);
        playerStat = FindObjectOfType<PlayerStat>();
        playerStat.gameObject.transform.position = data[CharacterNum].Position;
        playerStat._Hp= data[CharacterNum].Hp;
        playerStat._Mp = data[CharacterNum].Mp;
        playerStat.Level = data[CharacterNum].Level;
        playerStat.NowExp = data[CharacterNum].Exp;
        playerStat.StartSet();

    }





    #region 캐릭터 선택창 관련 로드/세이브 함수

    public void CharacterSelSave1()
    {
        characterSel = FindObjectOfType<CharacterSel>();
        characterSelData.Charater1 = (int)characterSel.character1;
       
        string json = JsonUtility.ToJson(characterSelData);
        File.WriteAllText(SAVE_DATA_DIRECTORY + Sel_SAVE_FILENAME, json);
    }
    public void CharacterSelSave2()
    {
        characterSel = FindObjectOfType<CharacterSel>();
        characterSelData.Charater2 = (int)characterSel.character2;
        string json = JsonUtility.ToJson(characterSelData);
        File.WriteAllText(SAVE_DATA_DIRECTORY + Sel_SAVE_FILENAME, json);
    }

    
    public void CharacterSelLoad()
    {
        if(File.Exists(SAVE_DATA_DIRECTORY + Sel_SAVE_FILENAME)) //데이터 세이브 파일이 있다면
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + Sel_SAVE_FILENAME);
            characterSelData = JsonUtility.FromJson<CharacterSelData>(loadJson);           
            characterSel = FindObjectOfType<CharacterSel>();

            
            if (characterSelData.Charater1 == 0) {
                characterSel.MakeType = CharacterSel.Type.None;
             }
            else if (characterSelData.Charater1 == 1)
            {
                characterSel.MakeType = CharacterSel.Type.Warrior;
            }
            else if (characterSelData.Charater1 == 2)
            {
                characterSel.MakeType = CharacterSel.Type.Archer;
            }
            else if (characterSelData.Charater1 == 3)
            {
                characterSel.MakeType = CharacterSel.Type.Mage;
            }
            characterSel.charSel = 1;
            
            if (characterSelData.Charater1 != 0)
                characterSel.MakeBut();


            if (characterSelData.Charater2 == 0)
            {
                characterSel.MakeType = CharacterSel.Type.None;
            }
            else if (characterSelData.Charater2 == 1)
            {
                characterSel.MakeType = CharacterSel.Type.Warrior;
            }
            else if (characterSelData.Charater2 == 2)
            {
                characterSel.MakeType = CharacterSel.Type.Archer;
            }
            else if (characterSelData.Charater2 == 3)
            {
                characterSel.MakeType = CharacterSel.Type.Mage;
            }

            characterSel.charSel = 2;
            if (characterSelData.Charater2 != 0)
                characterSel.MakeBut();
            Debug.Log("로드완료");
        }
        else
            Debug.Log("세이브 파일이 없습니다.");

    }

    #endregion

    


}
