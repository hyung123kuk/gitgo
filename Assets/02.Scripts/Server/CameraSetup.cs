using Cinemachine; //�ó׸ӽ� ���� �ڵ�
using Photon.Pun; //PUN ���� �ڵ�
using UnityEngine;

//�ó׸ӽ� ī�޶� ���� �÷��̾ �����ϵ��� ����
public class CameraSetup : MonoBehaviourPun
{

    void Start()
    {
        //���� ���� �÷��̾��
        //if (this.photonView.IsMine)
        //{
        //    //���� �ִ� �ó׸ӽ� ���� ī�޶� ã�´�.
        //    var followCam = FindObjectOfType<CinemachineVirtualCamera>();
        //    //���� ī�޶��� ���� ����� ���� �����.
        //    followCam.Follow = this.transform;
        //    followCam.LookAt = this.transform;
        //}
    }


    void Update()
    {
        
    }
}
