using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI_Switch : MonoBehaviour
{
    public CharacterSel characterSel; //���� �ű涧 �����Ǿ��ִ� ������Ʈ�� �̾ŵǴ¹ٶ��� ����ġ�ۼ�

    private void Awake()
    {
        characterSel = GameObject.Find("SelManager").GetComponent<CharacterSel>();
    }

    private void OnEnable()
    {
        characterSel.LobbyUi = GameObject.Find("Canvas_Lobby").transform.GetChild(0).gameObject;
    }
}
