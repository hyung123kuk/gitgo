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

public class CharacterData
{
    public Vector3 Position;
    public Vector3 Rotate;


}


public class SaveManager : MonoBehaviour
{


    private CharacterSelData characterSelData = new CharacterSelData();
    private CharacterSel characterSel;

    private string SAVE_DATA_DIRECTORY;
    private string Sel_SAVE_FILENAME = "/CharacterSelSaveFile.txt";


    private void OnEnable()
    {
        
    }

    private void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }

        CharacterSelLoad();

    }


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


}
