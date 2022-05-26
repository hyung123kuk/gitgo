using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTyping : MonoBehaviour
{
    [SerializeField]
     public GameObject Quest;
    public Text tx;
    public bool Texting=false;
    public bool istyping = false;
    public bool endExplain = false;
    public Item[] main1_item;
    public Image[] itemImage;
    public Text[] itemNum;

    private string ing_text;
    private string main1_text1 = "오~ 처음 보는 친구구만, 어디서 온 친구 인지 모르겠지만 ,\n" +
        "이 마을에 온 목적은 이마에 써져 있구만 그래 ~ 하하하 ";
    private string main1_text2 = "번지수를 잘 찾아 왔구만. 뭐 패기도 좋고 열정도 좋지만 무엇보다 실력이 있어야지 안그래?\n" +
        "아직 애송이 냄새가 많이 나서 말이지 우선 퀵슬롯 사용법부터 알려줘야 겠구만 그래...";
    private string main1_text3 = "우선은 K키를 눌러 스킬창을 열고, 구르기 스킬을 퀵슬롯에 집어넣어,\n" +
        "구르기 스킬을 한번 사용해 보게나!!";
    public int mainnum = 1;
    public int Textnum = 1;
    void Start()
    {
        Quest = transform.GetChild(0).gameObject;
        tx = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        itemImage = transform.GetChild(0).GetChild(1).GetComponentsInChildren<Image>();
        main1_item = Resources.LoadAll<Item>("Quest/Main1");

    }

    public void QuestStartKey()
    {
        tx.text = "";
        Texting = true;
        AllColorReset(0); 
        Quest.SetActive(true);
        
        
    }

    public void AllColorReset(int _alpha)
    {
        for(int i = 0; i < itemImage.Length; i++)
        {
            Color color = itemImage[i].color;
            color.a = _alpha;
            itemImage[i].color = color;
        }
    }
    public void SetColor(int _alpha,int i)
    {
        Color color = itemImage[i].color;
        color.a = _alpha;
        itemImage[i].color = color;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if(endExplain)
            {
                endExplain = false;
                Textnum = 1;

                Quest.SetActive(false);
            }
            if (Texting)
            {
           
                StopAllCoroutines();
                tx.text = ing_text;
                Texting = false;
                istyping = false;

            }
             else if (!Texting)
            {            
                  Texting = true;
                  Textnum++;
            }
        }
        if (Texting&&!istyping)
        {
            if (mainnum == 1)
            {
                Main1();
            }
            if (mainnum == 2)
            {
                
            }
        }
    }


    public void Main1()
    {
        if (Textnum == 1) {
          StartCoroutine(typing(main1_text1));
         }
        else if (Textnum == 2) {
            StartCoroutine(typing(main1_text2));
        }
        else if(Textnum == 3)
        {
            StartCoroutine(typing(main1_text3));
        }
        else if(Textnum==4)
        {
            endExplain = true;
            ItemSet(main1_item);
        }

    }

    void ItemSet(Item[] items)
    {
        int i = 0;
        foreach (Item item in items)
        {            
            itemImage[i].sprite = item.itemImage;
            SetColor(1, i);
            i++;
        }
    }


    IEnumerator typing(string text)
    {
        Texting = true;
        istyping = true;
        ing_text = text;
        yield return new WaitForSeconds(0.1f);
        for(int i =0;i<= ing_text.Length; i++)
        {
            tx.text = ing_text.Substring(0, i);
            yield return new WaitForSeconds(0.02f);
        }
        Texting = false;
        istyping = false;

    }
}
