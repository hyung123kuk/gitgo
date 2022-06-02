using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBar : MonoBehaviour
{
    public float backVolume = 0;
    public float CharacterVolume = 0;
    public float UIVolume = 0;


    [SerializeField]
    Scrollbar backSound;
    [SerializeField]
    Text backText;
    [SerializeField]
    Scrollbar characterSound;
    [SerializeField]
    Text characterText;
    [SerializeField]
    Scrollbar uisound;
    [SerializeField]
    Text UiText;








    private void Start()
    {
        backSound = transform.GetChild(0).GetChild(4).GetChild(2).GetChild(0).GetComponent<Scrollbar>();
        characterSound= transform.GetChild(0).GetChild(4).GetChild(3).GetChild(0).GetComponent<Scrollbar>();
        uisound = transform.GetChild(0).GetChild(4).GetChild(4).GetChild(0).GetComponent<Scrollbar>();
        backText= transform.GetChild(0).GetChild(4).GetChild(2).GetChild(0).GetChild(1).GetComponent<Text>();
        characterText = transform.GetChild(0).GetChild(4).GetChild(3).GetChild(0).GetChild(1).GetComponent<Text>();
        UiText = transform.GetChild(0).GetChild(4).GetChild(4).GetChild(0).GetChild(1).GetComponent<Text>();

        BackGroundSoundSet();
        CharacterSoundSet();
        UISoundSet();
        
    }

    

    public void BackGroundSoundSet()
    {
        backVolume = backSound.value;
        backText.text = ((int)(backVolume * 100)).ToString();

    }
    public void CharacterSoundSet()
    {
        CharacterVolume = characterSound.value;
        characterText.text = ((int)(CharacterVolume * 100)).ToString();


    }
    public void UISoundSet()
    {
        UIVolume = uisound.value;
        UiText.text = ((int)(UIVolume * 100)).ToString();

    }
}
