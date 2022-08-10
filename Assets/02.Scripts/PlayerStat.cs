using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class PlayerStat : MonoBehaviourPun, IPunObservable //포톤으로 만들려고함.
{
    public int Level;
    public float TotalExp;

    public float NowExp;

    public float MONEY;         
    public float _STR;          
    public float _DEX;          
    public float _INT;        
    public float _DAMAGE;       
    public float _DEFENCE;      
    public float _SKILL_COOLTIME_DEC_PER; 
    public float _SKILL_ADD_DAMAGE_PER;
    public float _MAXHP;             
    public float _Hp;              
    public float _MAXMP;            
    public float _Mp;                
    public float _CRITICAL_PROBABILITY; 
    public float _CRITICAL_ADD_DAMAGE_PER;
    public float _MOVE_SPEED;      

    public float LEV_ADD_STR;
    public float LEV_ADD_DEX;
    public float LEV_ADD_INT;

    [SerializeField]
    private PlayerST playerST; 
    [SerializeField]
    private inventory inven;
    [SerializeField]
    private GameUI gameUI;
    [SerializeField]
    SkillWindow skillWindow;
    [SerializeField]
    StatWindow statWindow;
    public Slot[] equSlots;
    public SaveManager saveManager;


    public PlayerStat playerstat;

    [SerializeField]
    private ParticleSystem LevelUp;

    private void Awake()
    {
        //if (!photonView.IsMine)
        //    this.enabled = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트라면 쓰기 부분이 실행됨
        if (stream.IsWriting)
        {
            stream.SendNext(_Hp);
            stream.SendNext(_Mp);
            
        }
        else
        {
            // 리모트 오브젝트라면 읽기 부분이 실행됨         
            _Hp = (float)stream.ReceiveNext();
            _Mp = (float)stream.ReceiveNext();
        }
    }

    void Start()
    {

        LevelUp = transform.GetChild(1).GetComponent<ParticleSystem>();
        playerstat = this;
        playerST = GetComponent<PlayerST>();
        inven = FindObjectOfType<inventory>();
        gameUI = FindObjectOfType<GameUI>();
        skillWindow = FindObjectOfType<SkillWindow>();
        statWindow = FindObjectOfType<StatWindow>();
        inven.invenOn();
        equSlots = GameObject.FindGameObjectWithTag("EqueSlot").GetComponentsInChildren<Slot>(); //여기서 오류가 뜬다면 인벤토리디자인을 킨 상태로 시작해주세요.
        

        Invoke("StartSet", 0.2f);



    }


    public void StartSet()
    {

        StatAllUpdate();

        _Hp = _MAXHP;
        _Mp = _MAXMP;
        gameUI.LevelSet();
        TotalExp = LevelExp();
        StatAllUpdate();
        gameUI.ExpSet();
        statWindow.SetLevel();
        statWindow.SetStat();
        gameUI.bar_set();

        Load(); //로드 데이터가 있다면 다시 로드합니다.
    }


    void Load()
    {
        saveManager = FindObjectOfType<SaveManager>();

        
        saveManager.LoadCharacter();
    }

    public void startLoad()
    {
        gameUI.LevelSet();
        TotalExp = LevelExp();
        StatAllUpdate();
        gameUI.ExpSet();
        statWindow.SetLevel();
        statWindow.SetStat();
        gameUI.bar_set();

    }



    public void StatAllUpdate()
    {
        if (!photonView.IsMine)
            return;

        _STR = 0;
        _DEX = 0;
        _INT = 0;
        _MAXHP = 200 * Level;
        _MAXMP= 200 * Level;
        _DAMAGE = 0;
        _DEFENCE = 0;
        _SKILL_ADD_DAMAGE_PER = 0;
        _SKILL_COOLTIME_DEC_PER = 0;
        _CRITICAL_ADD_DAMAGE_PER = 0;
        _CRITICAL_PROBABILITY = 0;
        _MOVE_SPEED = 0;


        if (playerST.CharacterType == PlayerST.Type.Warrior)
        {
            _STR += 20f;
            _DEX += 15f;
            _INT += 5f;
        }
        else if (playerST.CharacterType == PlayerST.Type.Archer)
        {
            _STR += 10f;
            _DEX += 25f;
            _INT += 5f;
        }
        else if (playerST.CharacterType == PlayerST.Type.Mage)
        {
            _STR += 10f;
            _DEX += 10f;
            _INT += 20f;
        }

        _STR += LEV_ADD_STR;
        _DEX += LEV_ADD_DEX;
        _INT += LEV_ADD_INT;

        for (int i = 0; i < equSlots.Length; i++)
        {
            if (equSlots[i].item != null)
            {
                _STR += equSlots[i].item._STR;
                _DEX += equSlots[i].item._DEX;
                _INT += equSlots[i].item._INT;
                _DAMAGE += equSlots[i].item._DAMAGE;
                _DEFENCE += equSlots[i].item._DEFENCE;
                _SKILL_COOLTIME_DEC_PER += equSlots[i].item._SKILL_COOLTIME_DEC_PER;
                _SKILL_ADD_DAMAGE_PER += equSlots[i].item._SKILL_ADD_DAMAGE_PER;
                _MAXHP += equSlots[i].item._HP;
                _MAXMP += equSlots[i].item._MP;
                _CRITICAL_PROBABILITY += equSlots[i].item._CRITICAL_PROBABILITY;
                _CRITICAL_ADD_DAMAGE_PER += equSlots[i].item._CRITICAL_ADD_DAMAGE_PER;
                _MOVE_SPEED += equSlots[i].item._MOVE_SPEED;
            }
        }

        Statcalculate();
    }



    public void Statcalculate()
    {

        if (!photonView.IsMine)
            return;

        _DAMAGE += ((_STR * 1f) + (_DEX * 0.5f) + (_INT * 0.25f)) * Level;
        _DEFENCE += ((_STR * 0.2f) + (_DEX * 0.1f)) * Level;
        _SKILL_COOLTIME_DEC_PER += _INT;
        _SKILL_ADD_DAMAGE_PER += ((_INT * 0.5f) + (_DEX * 0.25f)) * Level;
        _MAXHP += ((_STR * 5) + (_DEX * 3) + (_INT * 1)) * Level;
        _MAXMP += ((_INT * 10) + (_DEX * 2) + (_STR * 2)) * Level / 2;
        _CRITICAL_PROBABILITY += (_STR * 0.25f) + (_DEX * 0.25f);
        _CRITICAL_ADD_DAMAGE_PER += (30 + (_STR * 1f) + (_DEX * 0.5f));
        _MOVE_SPEED += _DEX * 0.5f;
        playerST.speed = 5 + (5 * _MOVE_SPEED / 100);
        MaxSet();
        StatWindow.statWindow.SetStat();

    }


    public void AddGold(float _Gold)
    {
        if (photonView.IsMine)
        {
            LogManager.logManager.Log((int)_Gold + "골드 획득 ");
            MONEY += _Gold;
            inven.GoldUpdate();
        }
    }

    public void AddExp(float _Exp)
    {



        if (Level < 10)
        {
            NowExp += _Exp;
            ExpFunc();
            gameUI.ExpSet();
        }
    }

    private void ExpFunc()
    {

        if (!photonView.IsMine)
            return;

        if (NowExp >= TotalExp)
        {
            LevelUp.Play();
            Level++;
            NowExp = 0;
            TotalExp=LevelExp();
            StatAllUpdate();
            _Hp = _MAXHP;
            _Mp = _MAXMP;
            gameUI.bar_set();
            gameUI.LevelSet();
            gameUI.ExpSet();
            inven.AllSlotItemLimitColor();
            skillWindow.AllSkillSlotColor();
            StatWindow.statWindow.SetLevel();
            if(Level == 8)
            {
                QuestStore.qustore.MainQuestSuccess(4);
            }
            
        }
    }

    public float LevelExp()
    {



            switch (Level)
            {
                case 1:
                    return 62f;

                case 2:
                    return 82f;

                case 3:
                    return 262f;

                case 4:
                    return 344f;

                case 5:
                    return 445f;

                case 6:
                    return 1434f;

                case 7:
                    return 1926f;

                case 8:
                    return 2495f;

                case 9:
                    return 3392f;

                case 10:
                    return 1f;
            }
            return 0f;
        
    } 

    public void RecoverHp(float _Hp_per)
    {

        if (!photonView.IsMine)
            return;

        _Hp += _MAXHP * _Hp_per / 100;
        if (_Hp > _MAXHP)
            _Hp = _MAXHP;
    }
    public void RecoverMp(float _Mp_per)
    {

        if (!photonView.IsMine)
            return;

        _Mp += _MAXMP * _Mp_per / 100;
        if (_Mp > _MAXMP)
            _Mp = _MAXMP;
    }
    public void MaxHpSet()
    {

        if (!photonView.IsMine)
            return;

        if (_Hp > _MAXHP)
            _Hp = _MAXHP;
    }
    public void MaxMpSet()
    {

        if (!photonView.IsMine)
            return;

        if (_Mp > _MAXMP)
            _Mp = _MAXMP;
    }
    public void MaxSet()
    {

        if (!photonView.IsMine)
            return;

        MaxHpSet();
        MaxMpSet();
    }

    public void DamagedHp(float _damage)
    {

        if (!photonView.IsMine)
            return;

        _damage = _damage * Random.Range(0.95f, 1.05f);

        if (_DEFENCE >= _damage)
        {
            _Hp--;
        }
        else
        {
            _Hp -= (_damage - _DEFENCE);
        }
        
        if(_Hp <= 0)
        {
            _Hp = 0;
            Debug.Log("체력이 없다");
        }
        gameUI.bar_set();

    }




    public void SkillMp(float UseMp)
    {

        if (!photonView.IsMine)
            return;

        if (_Mp > UseMp)
        {
            _Mp -= UseMp;
        }
        else
        {
            _Mp = 0;
            Debug.Log("마나가 없다");
        }
        gameUI.bar_set();
    }

   
}
