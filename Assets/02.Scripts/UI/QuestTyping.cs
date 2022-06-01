using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTyping : MonoBehaviour
{
    static public QuestTyping questTyping;
    [SerializeField]
     public GameObject Quest;
    
    public Text tx;
    public bool Texting=false;
    public bool istyping = false;
    public bool endExplain = false;
    
    public Text[] itemNum;
    


    private string ing_text;
    private string main1_text1 = "��~ ó�� ���� ģ������, ��� �� ģ�� ���� �𸣰����� ,\n" +
        "�� ������ �� ������ �̸��� ���� �ֱ��� �׷� ~ ������ ";
    private string main1_text2 = "�������� �� ã�� �Ա���. �� �б⵵ ���� ������ ������ �������� �Ƿ��� �־���� �ȱ׷�?\n" +
        "���� �ּ��� ������ ���� ���� ������ �켱 ������ �������� �˷���� �ڱ��� �׷�...";
    private string main1_text3 = "�켱�� KŰ�� ���� ��ųâ�� ����, ������ ��ų�� �����Կ� ����־�,\n" +
        "������ ��ų�� �ѹ� ����� ���Գ�!!";
    public Item[] main1_item;



    private string main2_text1 = "�̹����� ���� �� ����� �̼��� �ຼ��?\n" +
       "�ƹ��� ��ü�� ���ص�, ��� �� ��븦 �̱�� ���� ���̾�..!";
    private string main2_text2 = "�� ���ڴٸ� ����� ��� �ֿ� �� ���� ������ ���� ������ �ȵǾ� ���̴±�.. \n";
    private string main2_text3 = "���Ǿ� �������� 3���� ���⸦ �����ؼ� ������ ���Գ� ! �ξ� ���� �� ���ϼ�..\n"+
        "��.. ��� ������ �ϴ����� ������� �ϴ°� �ƴ���..?? ������";
    public Item[] main2_item;


    private string main3_text1 = "����.. �ݹ� �������� �˾Ҵµ� ������ �ŷڰ� ����� ����...\n";
    private string main3_text2 = "���� �� ���� �̼��� ��� �ھ� �� �̼��� �����ϸ� �ڳ׸� �ּ��� ������� �ʰڳ� \n" +
       "�̹� �̼��� Ŭ���� �Ѵٸ� ���� �Ƴ��� ���� �ϳ� ����!";
    private string main3_text3 = "���忡 ���� �źϽ������� ��ƺ��Գ�.. �̹����� ���� �������� ���� �ɼ�... ���� \n" +
        "������ ��ٳ�!!";
    public Item[] main3_item;

    private string main4_text1 = "�ڳ׸� �� �����ϰ� ���� �ʿ䰡 �ְڱ���.. �츮 ���� �ʿ��� ���� ��������\n" +
        "���� ���� �Ҿ���, ���� ���ݴ����� ���� ��� �Ҿȿ� ���� ����...";
    private string main4_text2 = "�׷��� ������� �ɰ� �Ǿ��� ���� ������� ������ ��� ���� �ϰ� ������ \n" +
       "�ڳװ� ���� �� �׻� �׷��� ��ó�� �߰��� ������ ���̶� �����ߴµ� ������ ������ ���±���..";
    private string main4_text3 = "������ ���� �� ���� ���̱⿡ �ڳ״� �����ϳ�,\n" +
        "�켱 8������ ��� ������ų�� ���Գ� !!";
    public Item[] main4_item;

    private string main5_text1 = "����� �� ������ ���� ���̴� ����.. ���ݱ��� ������༭ �����ϱ���..\n" +
        "���� ������ �� �ܰ� ���ҳ�...";
    private string main5_text2 = "�ε� ��� ���ƿ��� �ٶ��, ���� ����� �ϴ´ٳ�! \n";
    private string main5_text3 = "���� ���̰� , �� ������ ��ȭ�� ��ã�� �ְԳ�!! \n";
    public Item[] main5_item;


    public int mainnum = 1;
    public int Textnum = 1;
    void Start()
    {
        questTyping = this;
        Quest = transform.GetChild(0).gameObject;
        tx = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        main1_item = Resources.LoadAll<Item>("Quest/Main1");
        main2_item = Resources.LoadAll<Item>("Quest/Main2");
        main3_item = Resources.LoadAll<Item>("Quest/Main3");
        main4_item = Resources.LoadAll<Item>("Quest/Main4");
        main5_item = Resources.LoadAll<Item>("Quest/Main5");

    }

    public void QuestStartKey()
    {
        tx.text = "";
        Texting = true;
        Quest.SetActive(true);
        
        
    }


    void Update()
    {
        if (Quest.activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            
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
            else if (mainnum == 2)
            {
                Main2();
            }
            else if (mainnum == 3)
            {
                Main3();
            }
            else if (mainnum == 4)
            {
                Main4();
            }
            else if (mainnum == 5)
            {
                Main5();
            }
        }
    }

    private void EndExplain()
    {
        if (endExplain)
        {
            endExplain = false;
            Textnum = 1;

            Quest.SetActive(false);
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
            
            QuestExplain.questExplain.Main1(main1_item,false);
            endExplain = true;
            EndExplain();
        }

    }

    public void Main2()
    {
        if (Textnum == 1)
        {
            StartCoroutine(typing(main2_text1));
        }
        else if (Textnum == 2)
        {
            StartCoroutine(typing(main2_text2));
        }
        else if (Textnum == 3)
        {
            StartCoroutine(typing(main2_text3));
        }
        else if (Textnum == 4)
        {

            QuestExplain.questExplain.Main2(main2_item, false);
            endExplain = true;
            EndExplain();
        }

    }

    public void Main3()
    {
        if (Textnum == 1)
        {
            StartCoroutine(typing(main3_text1));
        }
        else if (Textnum == 2)
        {
            StartCoroutine(typing(main3_text2));
        }
        else if (Textnum == 3)
        {
            StartCoroutine(typing(main3_text3));
        }
        else if (Textnum == 4)
        {
           
            QuestExplain.questExplain.Main3(main3_item, false);
            endExplain = true;
            EndExplain();
        }

    }

    public void Main4()
    {
        if (Textnum == 1)
        {
            StartCoroutine(typing(main4_text1));
        }
        else if (Textnum == 2)
        {
            StartCoroutine(typing(main4_text2));
        }
        else if (Textnum == 3)
        {
            StartCoroutine(typing(main4_text3));
        }
        else if (Textnum == 4)
        {
           
            QuestExplain.questExplain.Main4(main4_item, false);
            endExplain = true;
            EndExplain();
        }

    }

    public void Main5()
    {
        if (Textnum == 1)
        {
            StartCoroutine(typing(main5_text1));
        }
        else if (Textnum == 2)
        {
            StartCoroutine(typing(main5_text2));
        }
        else if (Textnum == 3)
        {
            StartCoroutine(typing(main5_text3));
        }
        else if (Textnum == 4)
        {
        
            QuestExplain.questExplain.Main5(main5_item, false);
            endExplain = true;
            EndExplain();
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
