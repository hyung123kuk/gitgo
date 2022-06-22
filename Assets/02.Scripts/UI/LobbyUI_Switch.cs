using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI_Switch : MonoBehaviour
{
    public CharacterSel characterSel; //씬을 옮길때 참조되어있던 오브젝트가 미씽되는바람에 스위치작성

    private void Awake()
    {
        characterSel = GameObject.Find("SelManager").GetComponent<CharacterSel>();
    }

    private void OnEnable()
    {
        characterSel.LobbyUi = GameObject.Find("Canvas_Lobby").transform.GetChild(0).gameObject;
    }
}
