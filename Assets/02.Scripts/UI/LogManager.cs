using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    [SerializeField]
    private Text[] logText;

    void Start()
    {
        logText = GetComponentsInChildren<Text>();    
    }

    


    public void Log()
    {

    }
    
    public void LogError()
    {

    }
}
