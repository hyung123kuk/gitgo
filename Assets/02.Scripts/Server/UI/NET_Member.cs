using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NET_Member : MonoBehaviour
{
    Image Hp_bar;
    Image Mp_bar;
    Text Name;
    public int id;


    void Start()
    {
       transform.parent = GameObject.Find("NET_Party").transform.GetChild(0).GetChild(1);

        Hp_bar = transform.GetChild(2).GetComponent<Image>();
        Mp_bar = transform.GetChild(3).GetComponent<Image>();
       

    }

    public void NameSet(int ID , string NickName)
    {
        Name = transform.GetChild(1).GetComponent<Text>();
    
        id = ID;
        Name.text = NickName;
    }
    public void BarSet(float hp , float mp) // 소수점으로 줘야한다.
    {
        if(!Hp_bar)
        {
            Hp_bar = transform.GetChild(2).GetComponent<Image>();
        }
        if (!Mp_bar)
        {
            Mp_bar = transform.GetChild(3).GetComponent<Image>();
        }
        Hp_bar.fillAmount = hp;
        Mp_bar.fillAmount = mp;
    }

}
