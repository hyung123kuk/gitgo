using Photon.Pun; // ����Ƽ�� ���� ������Ʈ��
using Photon.Realtime; // ���� ���� ���� ���̺귯��
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1"; // ���� ����

    public Text connectionInfoText; // ��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ
    public Button joinButton; // �� ���� ��ư
    private SaveManager saveManager;
    private GameObject Character; //���Ŵ��� ù��° �ڽ� ����

    public CharacterSel CharSel;

    // ���� ����� ���ÿ� ������ ���� ���� �õ�
    private void Awake()
    {
        Character = GameObject.Find("SelManager").transform.GetChild(0).gameObject;
        saveManager = FindObjectOfType<SaveManager>();
    }
    private void Start()
    {

        //���ӿ� �ʿ��� ���� (���� ����) ����
        PhotonNetwork.GameVersion = this.gameVersion;
        //������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();


        this.joinButton.interactable = false;
        this.connectionInfoText.text = "������ ������ ������...";
    }

    // ������ ���� ���� ������ �ڵ� ����
    public override void OnConnectedToMaster()
    {
        this.joinButton.interactable = true;
        this.connectionInfoText.text = "�¶��� : ������ ������ ���� ��";
    }

    // ������ ���� ���� ���н� �ڵ� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        this.joinButton.interactable = false;
        this.connectionInfoText.text = "�������� : ������ ������ ������� ����\n ���� ��õ���... ";
        //������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();
    }
    // �� ���� �õ�
    public void Connect()
    {
        // �ߺ� ���� ����
        this.joinButton.interactable = false;

        // ������ ������ ���� ���̶��
        if (PhotonNetwork.IsConnected)
        {

            //�뿡 �����Ѵ�.
            this.connectionInfoText.text = "�뿡 ����....";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            this.connectionInfoText.text = "�������� : ������ ������ ���� ��Ŵ \n �ٽ� ���� �õ��մϴ�.";
            //������ ������ ������ ���� ���� �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // (�� ���� ����)���� �� ������ ������ ��� �ڵ� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        
        this.connectionInfoText.text = "�� �� ����, ���ο�� ����...";
        //�ִ� �ο��� 4������ ���� + ���� ����
        //���̸� , 4�� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 , CleanupCacheOnLeave=false });

    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        this.connectionInfoText.text = "�� ���� ����!";

        //saveManager = FindObjectOfType<SaveManager>();
        //saveManager.CharacterNum = 0;
        //CharacterSel.characterSel.GameStart();


        //��� �� �����ڰ� Main ���� �ε��ϰ� ��
        PhotonNetwork.LoadLevel("Play_SANGIN2 1");
        CharacterSel.characterSel.LobbyUi.SetActive(false);
        Character.SetActive(false);
        

       


    }



}
