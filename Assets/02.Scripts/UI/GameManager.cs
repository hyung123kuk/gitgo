using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterSel CharSel;
    public GameObject Wor;
    public GameObject Arc;
    public GameObject mage;

    public Transform startPoint;
    
    void Start()
    {

        CharSel = GameObject.FindGameObjectWithTag("SelManager").GetComponent<CharacterSel>();
        CharSel.sel.SetActive(false);
        if(CharSel.charSel == 1)
        {
            if (CharSel.character1 == CharacterSel.Type.Warrior)
            {
                
                Instantiate(Wor, startPoint.position, Quaternion.identity);
            }
            else if (CharSel.character1 == CharacterSel.Type.Archer)
            {
                Instantiate(Arc, startPoint.position, Quaternion.identity);
            }
            else if (CharSel.character1 == CharacterSel.Type.Mage)
            {
                Instantiate(mage, startPoint.position, Quaternion.identity);
            }
        }
        else if (CharSel.charSel == 2)
        {
            if (CharSel.character2 == CharacterSel.Type.Warrior)
            {
                Instantiate(Wor, startPoint.position, Quaternion.identity);
            }
            else if (CharSel.character2 == CharacterSel.Type.Archer)
            {
                Instantiate(Arc, startPoint.position, Quaternion.identity);
            }
            else if (CharSel.character2 == CharacterSel.Type.Mage)
            {
                Instantiate(mage, startPoint.position, Quaternion.identity);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
