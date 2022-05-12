using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject skillWindow;
    [SerializeField]
    private AllUI allUI;

    public static bool kDown =false;

    private void Start()
    {
        allUI = FindObjectOfType<AllUI>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) //½ºÅ³Ã¢ ÄÑ±â/ ²ô±â
        {
            kDown = !kDown;
            if (!kDown) //²û
            {
                SkillWindowOff();

            }
            else //Å´
            {
                SkillWindowOn();
                allUI.SkillWindowTop();
            }
        }
    }

    public void SkillWindowOn()
    {
        skillWindow.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(true);
        kDown = true;
        
    }

    public void SkillWindowOff()
    {

        skillWindow.SetActive(false);
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(false);
        allUI.MouseCursor.Init_Cursor();
        kDown = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
}
