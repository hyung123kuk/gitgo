using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.SceneManagement;
using Photon.Pun;

[System.Serializable]
public class CharacterSelData
{
    public int Charater1;
    public int Charater2;
    public string Charater1name;
    public string Charater2name;

}

public class Data
{
    //매번업데이트
    public Vector3 Position;
    public float Hp;
    public float Mp;
    public int Level;
    public float Exp;
    public float Gold;

    public List<int> invenArrNum = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemCount = new List<int>();

    public List<int> QuickArrNum = new List<int>();
    public List<string> QuickName = new List<string>();
    public List<int> QuickitemCount = new List<int>();
    public List<bool> QuickisItem = new List<bool>();

    public List<int> EQitemArrNum = new List<int>();
    public List<string> EQitemArrName = new List<string>();

    public List<int> QuestNum = new List<int>();
    public List<int> QuestName = new List<int>();
    public int QuestMainNum; // 메인퀘스트 이름 띄우기 위한변수
    public int QuestOrder;

    public bool MainRecieve;
    public bool MainClear;
    public int MainNum;// 퀘스트 스토어에 이름 띄우기 위한 변수

    public int slimeKill;
    public bool Quest_slime;
    public bool slime_Success;

    public int goblinKill;
    public bool Quest_goblin;
    public bool goblin_Success;

    public int skelletonKill;
    public bool Quest_skelleton;
    public bool skelleton_Success;

    #region 방어구,무기 저장용
    public int Helmet;
    public int Shoulder;
    public int Chest;
    public int Gloves;
    public int Pants;
    public int Boots;
    public int Back;
    public int Weapon;
    #endregion


}


public class SaveManager : MonoBehaviourPunCallbacks
{

    private string SAVE_DATA_DIRECTORY;//파일이름 Start에서 선언한다.
    #region 캐릭터 선택창 관련 변수
    private CharacterSelData characterSelData = new CharacterSelData();
    public string Getname1
    {
        get
        {
            return characterSelData.Charater1name;
        }
    }
    public string Setname1
    {
        set
        {
            characterSelData.Charater1name = value;
        }
    }
    public string Getname2
    {
        get
        {
            return characterSelData.Charater2name;
        }
    }
    public string Setname2
    {
        set
        {
            characterSelData.Charater2name = value;
        }
    }
    private Nickname nickname;
    private CharacterSel characterSel;
    private string Sel_SAVE_FILENAME = "/CharacterSelSaveFile.txt";
    #endregion

    public bool SaveOn;
    public int CharacterNum;


    #region 캐릭터 기본스탯 관련 변수
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
    #endregion

    [SerializeField]
    inventory inven;
    [SerializeField]
    QuickSlots QuikSlots;
    [SerializeField]
    QuestWindow questWindow;
    [SerializeField]
    QuestExplain questExplain;
    [SerializeField]
    QuestStore questStore;
    [SerializeField]
    QuestNormal questNormal;
    [SerializeField]
    WarriorEquipChange warriorEquipChange;
    [SerializeField]
    ArcherEquipChange archerEquipChange;
    [SerializeField]
    MageEquipChange mageEquipChange;
    [SerializeField]
    PlayerST playerst;

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
        if (!SaveOn)// 캐릭터 선택창이라면
        {
            CharacterSelLoad();
        }


        StartCoroutine(SaveStart());

    }


    IEnumerator SaveStart()
    {

        yield return new WaitForSeconds(1.0f);
        if (!SaveOn) { yield break; }
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
        PlayerST[] playerst2 = FindObjectsOfType<PlayerST>();
        foreach (PlayerST playerst3 in playerst2)
        {
            if (playerst3.GetComponent<PhotonView>().IsMine)
                playerst = playerst3;
        }
        PlayerStat[] playerstats = FindObjectsOfType<PlayerStat>();
        foreach (PlayerStat playerstat in playerstats)
        {
            if (playerstat.GetComponent<PhotonView>().IsMine)
                playerStat = playerstat;
        }
        data[CharacterNum].Hp = playerStat._Hp;
        data[CharacterNum].Mp = playerStat._Mp;
        data[CharacterNum].Position = playerStat.gameObject.transform.position;
        data[CharacterNum].Exp = playerStat.NowExp;
        data[CharacterNum].Level = playerStat.Level;
        data[CharacterNum].Gold = playerStat.MONEY;
        InvenSave();
        QuickSave();
        EquSlotSave();
        QuestSave();

        if (playerst.CharacterType == PlayerST.Type.Warrior)
            WarriorEquip(); //워리어 장비
        else if (playerst.CharacterType == PlayerST.Type.Archer)
            ArcherEquip(); //궁수장비
        else if (playerst.CharacterType == PlayerST.Type.Mage)
            MageEquip(); //마법사 장비
    }

    public void InvenSave()
    {
        inven = FindObjectOfType<inventory>();
        Slot[] slots = inven.GetSlots();
        data[CharacterNum].invenArrNum.Clear();
        data[CharacterNum].invenItemCount.Clear();
        data[CharacterNum].invenItemName.Clear();
        for (int i = 0; i < slots.Length; i++)
        {

            if (slots[i].item != null)
            {
                data[CharacterNum].invenArrNum.Add(i);
                data[CharacterNum].invenItemCount.Add(slots[i].itemCount);
                data[CharacterNum].invenItemName.Add(slots[i].item.itemName);

            }
            else
            {
                data[CharacterNum].invenArrNum.Add(i);
                data[CharacterNum].invenItemCount.Add(0);
                data[CharacterNum].invenItemName.Add(" ");
            }
        }
    }

    public void QuickSave()
    {
        QuikSlots = FindObjectOfType<QuickSlots>();
        QuikSlot[] quikslots = QuikSlots.GetSlots();

        data[CharacterNum].QuickisItem.Clear();
        data[CharacterNum].QuickArrNum.Clear();
        data[CharacterNum].QuickitemCount.Clear();
        data[CharacterNum].QuickName.Clear();

        for (int i = 0; i < quikslots.Length; i++)
        {
            if (quikslots[i].slot.item != null)
            {
                data[CharacterNum].QuickisItem.Add(true);
                data[CharacterNum].QuickArrNum.Add(i);
                data[CharacterNum].QuickitemCount.Add(quikslots[i].slot.itemCount);
                data[CharacterNum].QuickName.Add(quikslots[i].slot.item.itemName);

            }
            else if (quikslots[i].skill.skill != null)
            {
                data[CharacterNum].QuickisItem.Add(false);
                data[CharacterNum].QuickArrNum.Add(i);
                data[CharacterNum].QuickitemCount.Add(0);
                data[CharacterNum].QuickName.Add(quikslots[i].skill.skill.skillName);
            }
        }

    }

    public void EquSlotSave()
    {
        inven = FindObjectOfType<inventory>();
        Slot[] slots = inven.GetEqSlots();
        data[CharacterNum].EQitemArrNum.Clear();
        data[CharacterNum].EQitemArrName.Clear();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                data[CharacterNum].EQitemArrNum.Add(i);
                data[CharacterNum].EQitemArrName.Add(slots[i].item.itemName);
            }
        }

    }

    public void WarriorEquip()
    {
        WarriorEquipChange[] warEquipChange = FindObjectsOfType<WarriorEquipChange>();
        foreach (WarriorEquipChange warequipChange in warEquipChange)
        {
            if (warequipChange.GetComponent<PhotonView>().IsMine)
                warriorEquipChange = warequipChange;
        }

        data[CharacterNum].Helmet = warriorEquipChange.EquipHelmet;
        data[CharacterNum].Chest = warriorEquipChange.EquipChest;
        data[CharacterNum].Shoulder = warriorEquipChange.EquipShoulder;
        data[CharacterNum].Gloves = warriorEquipChange.EquipGloves;
        data[CharacterNum].Pants = warriorEquipChange.EquipPants;
        data[CharacterNum].Boots = warriorEquipChange.EquipBoots;
        data[CharacterNum].Back = warriorEquipChange.EquipBack;
        data[CharacterNum].Weapon = playerst.NowWeapon;
    }
    public void ArcherEquip()
    {
        ArcherEquipChange[] arcEquipChange = FindObjectsOfType<ArcherEquipChange>();
        foreach (ArcherEquipChange archEquipChange in arcEquipChange)
        {
            if (archEquipChange.GetComponent<PhotonView>().IsMine)
                archerEquipChange = archEquipChange;
        }

        data[CharacterNum].Helmet = archerEquipChange.EquipHelmet;
        data[CharacterNum].Chest = archerEquipChange.EquipChest;
        data[CharacterNum].Gloves = archerEquipChange.EquipGloves;
        data[CharacterNum].Pants = archerEquipChange.EquipPants;
        data[CharacterNum].Boots = archerEquipChange.EquipBoots;
        data[CharacterNum].Back = archerEquipChange.EquipBack;
        data[CharacterNum].Weapon = playerst.NowWeapon;
    }
    public void MageEquip()
    {
        MageEquipChange[] magEquipChange = FindObjectsOfType<MageEquipChange>();
        foreach (MageEquipChange mageeEquipChange in magEquipChange)
        {
            if (mageeEquipChange.GetComponent<PhotonView>().IsMine)
                mageEquipChange = mageeEquipChange;
        }

        data[CharacterNum].Helmet = mageEquipChange.EquipHelmet;
        data[CharacterNum].Chest = mageEquipChange.EquipChest;
        data[CharacterNum].Gloves = mageEquipChange.EquipGloves;
        data[CharacterNum].Pants = mageEquipChange.EquipPants;
        data[CharacterNum].Boots = mageEquipChange.EquipBoots;
        data[CharacterNum].Back = mageEquipChange.EquipBack;
        data[CharacterNum].Weapon = playerst.NowWeapon;
    }

    public void QuestSave()
    {
        questWindow = FindObjectOfType<QuestWindow>();
        QuestSlot[] slots = questWindow.ReturnQuestSlots();
        data[CharacterNum].QuestName.Clear();
        data[CharacterNum].QuestNum.Clear();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].questName != QuestSlot.QuestName.None)
            {
                data[CharacterNum].QuestNum.Add(i);
                data[CharacterNum].QuestName.Add((int)slots[i].questName);
            }

        }

        data[CharacterNum].QuestOrder = questWindow.questOrder;

        questExplain = FindObjectOfType<QuestExplain>();
        data[CharacterNum].QuestMainNum = questExplain.QuestNum;
        if (data[CharacterNum].QuestMainNum > 4)
        {
            questExplain.DungeonOpen();
        }

        questStore = FindObjectOfType<QuestStore>();
        data[CharacterNum].MainNum = questStore.mainNum;
        data[CharacterNum].MainRecieve = questStore.isMainRecive;
        data[CharacterNum].MainClear = questStore.MainSuccess;
        questStore.QuestStoreLoad();

        questNormal = FindObjectOfType<QuestNormal>();
        data[CharacterNum].slimeKill = questNormal.slimeKill;
        data[CharacterNum].slime_Success = questNormal.slime_Success;
        data[CharacterNum].Quest_slime = questNormal.Quest_slime;

        data[CharacterNum].goblinKill = questNormal.goblinKill;
        data[CharacterNum].goblin_Success = questNormal.goblin_Success;
        data[CharacterNum].Quest_goblin = questNormal.Quest_goblin;

        data[CharacterNum].skelletonKill = questNormal.skelletonKill;
        data[CharacterNum].skelleton_Success = questNormal.skelleton_Success;
        data[CharacterNum].Quest_skelleton = questNormal.Quest_skelleton;
        questNormal.QuestNormalLoad();





    }



    public void LoadCharacter()
    {
        if (CharacterNum == 0)
        {
            if (File.Exists(SAVE_DATA_DIRECTORY + ch1_SAVE_FILENAME))
            {
                string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + ch1_SAVE_FILENAME);

                LoadCh(loadJson);

            }
        }
        else if (CharacterNum == 1)
        {
            if (File.Exists(SAVE_DATA_DIRECTORY + ch2_SAVE_FILENAME))
            {
                string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + ch2_SAVE_FILENAME);

                LoadCh(loadJson);
            }
        }



    }

    private void LoadCh(string _loadJson)
    {
        data[CharacterNum] = JsonUtility.FromJson<Data>(_loadJson);

        #region 스크립트 추적
        PlayerStat[] playerstats = FindObjectsOfType<PlayerStat>();
        foreach (PlayerStat playerstat in playerstats)
        {
            if (playerstat.GetComponent<PhotonView>().IsMine)
                playerStat = playerstat;
        }
        WarriorEquipChange[] warEquipChange = FindObjectsOfType<WarriorEquipChange>();
        foreach (WarriorEquipChange warequipChange in warEquipChange)
        {
            if (warequipChange.GetComponent<PhotonView>().IsMine)
                warriorEquipChange = warequipChange;
        }
        ArcherEquipChange[] arcEquipChange = FindObjectsOfType<ArcherEquipChange>();
        foreach (ArcherEquipChange archEquipChange in arcEquipChange)
        {
            if (archEquipChange.GetComponent<PhotonView>().IsMine)
                archerEquipChange = archEquipChange;
        }
        MageEquipChange[] magEquipChange = FindObjectsOfType<MageEquipChange>();
        foreach (MageEquipChange mageeEquipChange in magEquipChange)
        {
            if (mageeEquipChange.GetComponent<PhotonView>().IsMine)
                mageEquipChange = mageeEquipChange;
        }
        PlayerST[] playerst2 = FindObjectsOfType<PlayerST>();
        foreach (PlayerST playerst3 in playerst2)
        {
            if (playerst3.GetComponent<PhotonView>().IsMine)
                playerst = playerst3;
        }
        #endregion


        inven = FindObjectOfType<inventory>();
        QuikSlots = FindObjectOfType<QuickSlots>();
        questWindow = FindObjectOfType<QuestWindow>();
        questStore = FindObjectOfType<QuestStore>();
        questExplain = FindObjectOfType<QuestExplain>();
        questNormal = FindObjectOfType<QuestNormal>();

        #region 전사 방어구,무기 불러오기
        if (playerst.CharacterType == PlayerST.Type.Warrior)
        {
            Debug.Log("워리어 로드.....");
            warriorEquipChange.photonView.RPC("WarriorHelmetChange", RpcTarget.AllBuffered, (Item.HelmetNames)data[CharacterNum].Helmet);
            warriorEquipChange.photonView.RPC("WarriorChestChange", RpcTarget.AllBuffered, (Item.ChestNames)data[CharacterNum].Chest);
            warriorEquipChange.photonView.RPC("WarriorShoulderChange", RpcTarget.AllBuffered, (Item.ShoulderNames)data[CharacterNum].Shoulder);
            warriorEquipChange.photonView.RPC("WarriorGlovesChange", RpcTarget.AllBuffered, (Item.GlovesNames)data[CharacterNum].Gloves);
            warriorEquipChange.photonView.RPC("WarriorPantsChange", RpcTarget.AllBuffered, (Item.PantsNames)data[CharacterNum].Pants);
            warriorEquipChange.photonView.RPC("WarriorBootsChange", RpcTarget.AllBuffered, (Item.BootsNames)data[CharacterNum].Boots);
            warriorEquipChange.photonView.RPC("WarriorBackChange", RpcTarget.AllBuffered, (Item.BackNames)data[CharacterNum].Back);
            playerst.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, (Item.SwordNames)data[CharacterNum].Weapon);
        }
        #endregion
        #region 궁수 방어구,무기 불러오기
        else if (playerst.CharacterType == PlayerST.Type.Archer)
        {
            Debug.Log("궁수 로드.....");
            archerEquipChange.photonView.RPC("ArcherHelmetChange", RpcTarget.AllBuffered, (Item.HelmetNames)data[CharacterNum].Helmet);
            archerEquipChange.photonView.RPC("ArcherChestChange", RpcTarget.AllBuffered, (Item.ChestNames)data[CharacterNum].Chest);
            archerEquipChange.photonView.RPC("ArcherGlovesChange", RpcTarget.AllBuffered, (Item.GlovesNames)data[CharacterNum].Gloves);
            archerEquipChange.photonView.RPC("ArcherPantsChange", RpcTarget.AllBuffered, (Item.PantsNames)data[CharacterNum].Pants);
            archerEquipChange.photonView.RPC("ArcherBootsChange", RpcTarget.AllBuffered, (Item.BootsNames)data[CharacterNum].Boots);
            archerEquipChange.photonView.RPC("ArcherBackChange", RpcTarget.AllBuffered, (Item.BackNames)data[CharacterNum].Back);
            playerst.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, (Item.SwordNames)data[CharacterNum].Weapon);
        }
        #endregion
        #region 마법사 방어구,무기 불러오기
        else if (playerst.CharacterType == PlayerST.Type.Mage)
        {
            Debug.Log("마법사 로드.....");
            mageEquipChange.photonView.RPC("MageHelmetChange", RpcTarget.AllBuffered, (Item.HelmetNames)data[CharacterNum].Helmet);
            mageEquipChange.photonView.RPC("MageChestChange", RpcTarget.AllBuffered, (Item.ChestNames)data[CharacterNum].Chest);
            mageEquipChange.photonView.RPC("MageGlovesChange", RpcTarget.AllBuffered, (Item.GlovesNames)data[CharacterNum].Gloves);
            mageEquipChange.photonView.RPC("MagePantsChange", RpcTarget.AllBuffered, (Item.PantsNames)data[CharacterNum].Pants);
            mageEquipChange.photonView.RPC("MageBootsChange", RpcTarget.AllBuffered, (Item.BootsNames)data[CharacterNum].Boots);
            mageEquipChange.photonView.RPC("MageBackChange", RpcTarget.AllBuffered, (Item.BackNames)data[CharacterNum].Back);
            playerst.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, (Item.SwordNames)data[CharacterNum].Weapon);
        }
        #endregion

        playerStat.gameObject.transform.position = data[CharacterNum].Position;
        playerStat._Hp = data[CharacterNum].Hp;

        playerStat._Mp = data[CharacterNum].Mp;
        playerStat.Level = data[CharacterNum].Level;
        playerStat.NowExp = data[CharacterNum].Exp;
        playerStat.MONEY = data[CharacterNum].Gold;
        playerStat.startLoad();

        for (int i = 0; i < data[CharacterNum].invenItemName.Count; i++)
        {
            inven.LoadToInven(data[CharacterNum].invenArrNum[i], data[CharacterNum].invenItemName[i], data[CharacterNum].invenItemCount[i]);
        }

        for (int i = 0; i < data[CharacterNum].QuickName.Count; i++)
        {
            QuikSlots.LoadToQuick(data[CharacterNum].QuickArrNum[i], data[CharacterNum].QuickName[i], data[CharacterNum].QuickitemCount[i], data[CharacterNum].QuickisItem[i]);
        }

        for (int i = 0; i < data[CharacterNum].EQitemArrName.Count; i++)
        {
            inven.LoadToEq(data[CharacterNum].EQitemArrNum[i], data[CharacterNum].EQitemArrName[i]);
        }

        questExplain.QuestNum = data[CharacterNum].QuestMainNum;
        questStore.isMainRecive = data[CharacterNum].MainRecieve;
        questStore.mainNum = data[CharacterNum].MainNum;

        questStore.MainSuccess = data[CharacterNum].MainClear;
        questNormal.slimeKill = data[CharacterNum].slimeKill;
        questNormal.slime_Success = data[CharacterNum].slime_Success;
        questNormal.Quest_slime = data[CharacterNum].Quest_slime;

        questNormal.goblinKill = data[CharacterNum].goblinKill;
        questNormal.goblin_Success = data[CharacterNum].goblin_Success;
        questNormal.Quest_goblin = data[CharacterNum].Quest_goblin;

        questNormal.skelletonKill = data[CharacterNum].skelletonKill;
        questNormal.skelleton_Success = data[CharacterNum].skelleton_Success;
        questNormal.Quest_skelleton = data[CharacterNum].Quest_skelleton;



        questWindow.questOrder = data[CharacterNum].QuestOrder;
        for (int i = 0; i < data[CharacterNum].QuestName.Count; i++)
        {
            questWindow.LoadToQuest(data[CharacterNum].QuestName[i], data[CharacterNum].QuestNum[i]);
        }

    }





    #region 캐릭터 선택창 관련 로드/세이브 함수
    public void CharacterSelSave1()
    {

        characterSel = FindObjectOfType<CharacterSel>();
        nickname = FindObjectOfType<Nickname>();
        characterSelData.Charater1 = (int)characterSel.character1;
        characterSelData.Charater1name = characterSel.Char1Name.text;


        string json = JsonUtility.ToJson(characterSelData);
        File.WriteAllText(SAVE_DATA_DIRECTORY + Sel_SAVE_FILENAME, json);
    }
    public void CharacterSelSave2()
    {
        characterSel = FindObjectOfType<CharacterSel>();
        nickname = FindObjectOfType<Nickname>();
        characterSelData.Charater2 = (int)characterSel.character2;
        characterSelData.Charater2name = characterSel.Char2Name.text;

        string json = JsonUtility.ToJson(characterSelData);
        File.WriteAllText(SAVE_DATA_DIRECTORY + Sel_SAVE_FILENAME, json);
    }


    public void CharacterSelLoad()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + Sel_SAVE_FILENAME)) //데이터 세이브 파일이 있다면
        {

            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + Sel_SAVE_FILENAME);
            characterSelData = JsonUtility.FromJson<CharacterSelData>(loadJson);
            if (CharacterNum == 0)
                PhotonNetwork.LocalPlayer.NickName = characterSelData.Charater1name;
            else if (CharacterNum == 1)
                PhotonNetwork.LocalPlayer.NickName = characterSelData.Charater2name;
            characterSel = FindObjectOfType<CharacterSel>();
            characterSel.Char1Name.text = characterSelData.Charater1name;
            characterSel.Char2Name.text = characterSelData.Charater2name;


            if (characterSelData.Charater1 == 0)
            {
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
            {
                characterSel.MakeBut();
            }


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
    public override void OnLeftRoom() //방을 나갈때 자동저장 (캐릭터선택창)
    {
        Debug.Log("1자동저장....");
        CharacterSelSave1();
        CharacterSelSave2();
        if (CharacterNum == 0)
            PhotonNetwork.LocalPlayer.NickName = characterSelData.Charater1name;
        else if (CharacterNum == 1)
            PhotonNetwork.LocalPlayer.NickName = characterSelData.Charater2name;
        

        SceneManager.LoadScene(0);
        characterSel.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnApplicationQuit() //강제종료될때 자동저장
    {
        Debug.Log("2자동저장....");
        CharacterSelSave1();
        CharacterSelSave2();
        if (CharacterNum == 0)
            PhotonNetwork.LocalPlayer.NickName = characterSelData.Charater1name;
        else if (CharacterNum == 1)
            PhotonNetwork.LocalPlayer.NickName = characterSelData.Charater2name;
    }



}
