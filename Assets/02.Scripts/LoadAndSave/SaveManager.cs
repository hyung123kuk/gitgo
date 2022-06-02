using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

class Character{
    public int Characternum;
    public int characterType;

    public Character(int characternum, int characterType)
    {
        Characternum = characternum;
        this.characterType = characterType;
    }
}


public class SaveManager : MonoBehaviour
{
    public static SaveManager saveManager;


    void Awake()
    {
        saveManager = this;
    }

    private void Start()
    {
        StartCoroutine(DataLoad());
    }
        


    IEnumerator DataLoad()
    {
        yield return new WaitForSeconds(1.0f);


    }

    void characterSave()
    {
        Character c1 = new Character(1, (int)CharacterSel.characterSel.character1);
        Character c2 = new Character(2, (int)CharacterSel.characterSel.character2);
        string chdata1 = JsonConvert.SerializeObject(c1);
        File.WriteAllText(Application.dataPath + "/character.json", chdata1);
        string chdata2 = JsonConvert.SerializeObject(c2);
    }

    void characterLod()
    {


    }

}
