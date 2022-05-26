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
    private string main1_text1 = "��~ ó�� ���� ģ������, ��� �� ģ�� ���� �𸣰����� ,\n" +
        "�� ������ �� ������ �̸��� ���� �ֱ��� �׷� ~ ������ ";
    private string main1_text2 = "�������� �� ã�� �Ա���. �� �б⵵ ���� ������ ������ �������� �Ƿ��� �־���� �ȱ׷�?\n" +
        "���� �ּ��� ������ ���� ���� ������ �켱 ������ �������� �˷���� �ڱ��� �׷�...";
    private string main1_text3 = "�켱�� KŰ�� ���� ��ųâ�� ����, ������ ��ų�� �����Կ� ����־�,\n" +
        "������ ��ų�� �ѹ� ����� ���Գ�!!";
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
