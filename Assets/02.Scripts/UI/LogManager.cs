using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    [SerializeField]
    private Text[] logText;
    [SerializeField]
    public static LogManager logManager;

    void Start()
    {
        logManager = this;
        logText = GetComponentsInChildren<Text>();    
    }

    private void LogUp()
    {
        int index = 0;
        for(int i=0; i<logText.Length; i++)
        {
            index++;
            if (logText[i].text == "")
                break;
            
        }
        for(int i = 0; i < index-1; i++)
        {

            logText[i + 1].text = logText[i].text;
            logText[i + 1].color = logText[i].color;
            
        }
        index = 0;

    }
    IEnumerator LogDisappear()
    {
        while(logText[0].text!="")
        {
            yield return new WaitForSeconds(0.05f);

            for(int i =0;i<logText.Length; i++)
            {
                SetColor(0.05f, logText[i]);
            }
                
        }

    }

    public void SetColor(float _alpha,Text logText)
    {
        Color color = logText.color;
        color.a -= _alpha;
        if(color.a <= 0)
        {
            logText.text = "";
        }
        logText.color = color;
    }

    public void Log(string TextLog,bool Error =false)
    {
        LogUp();
        logText[0].text = TextLog;
        if (!Error) { logText[0].color = Color.white; }
       
        else if (Error) { logText[0].color = Color.red; }
        StopCoroutine(LogDisappear());
        StartCoroutine(LogDisappear());
    }
    
  
}
