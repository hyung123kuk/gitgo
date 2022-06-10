using Cinemachine; //시네머신 관련 코드
using Photon.Pun; //PUN 관련 코드
using UnityEngine;

//시네머신 카메라가 로컬 플레이어를 추적하도록 설정
public class CameraSetup : MonoBehaviourPun
{

    void Start()
    {
        //내가 로컬 플레이어면
        //if (this.photonView.IsMine)
        //{
        //    //씬에 있는 시네머신 가상 카메라를 찾는다.
        //    var followCam = FindObjectOfType<CinemachineVirtualCamera>();
        //    //가상 카메라의 추적 대상을 나로 맞춘다.
        //    followCam.Follow = this.transform;
        //    followCam.LookAt = this.transform;
        //}
    }


    void Update()
    {
        
    }
}
