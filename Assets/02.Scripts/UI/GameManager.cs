using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterSel CharSel;
    public GameObject Wor;
    public GameObject Arc;
    public GameObject mage;
    public Vector3 StartPosition = new Vector3(30, 3, 50);
    public Transform startPoint;
    
    void Awake()
    {

        CharSel = GameObject.FindGameObjectWithTag("SelManager").GetComponent<CharacterSel>();
        //CharSel.sel.SetActive(false);
        if(CharSel.charSel == 1)
        {
            if (CharSel.character1 == CharacterSel.Type.Warrior)
            {
                
                GameObject character=Instantiate(Wor, startPoint.position, Quaternion.identity);
                character.transform.GetChild(0).position = StartPosition;
            }
            else if (CharSel.character1 == CharacterSel.Type.Archer)
            {
                GameObject character = Instantiate(Arc, startPoint.position, Quaternion.identity);
                character.transform.GetChild(0).position = StartPosition;
            }
            else if (CharSel.character1 == CharacterSel.Type.Mage)
            {
                GameObject character = Instantiate(mage, startPoint.position, Quaternion.identity);
                character.transform.GetChild(0).position = StartPosition;
            }
        }
        else if (CharSel.charSel == 2)
        {
            if (CharSel.character2 == CharacterSel.Type.Warrior)
            {
                GameObject character = Instantiate(Wor, startPoint.position, Quaternion.identity);
                character.transform.GetChild(0).position = StartPosition;
            }
            else if (CharSel.character2 == CharacterSel.Type.Archer)
            {
                GameObject character = Instantiate(Arc, startPoint.position, Quaternion.identity);
                character.transform.GetChild(0).position = StartPosition;
            }
            else if (CharSel.character2 == CharacterSel.Type.Mage)
            {
                GameObject character = Instantiate(mage, startPoint.position, Quaternion.identity);
                character.transform.GetChild(0).position = StartPosition;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
