using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class UiSound : MonoBehaviour
{
    public static UiSound uiSound;

    public AudioSource audioSource;
    SoundBar soundbar;
    private void Awake()
    {
        if (uiSound == null)
        {
            uiSound = this;
        }
    }

    private void Start()
    {

        soundbar = FindObjectOfType<SoundBar>();
        audioSource = GetComponent<AudioSource>();


    }
    //=====================유아이 사운드===============================
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "ChSel_sangin")  //캐릭터선택창 씬에서는 적용X
            audioSource.volume = soundbar.UIVolume;
    }

    public void InventoryOpenSound() //인벤 여는소리
    {

        audioSource.PlayOneShot(Sounds.sounds.InventoryOpenSound);
    }
    public void InventoryCloseSound() //인벤 닫는소리
    {
        audioSource.PlayOneShot(Sounds.sounds.InventoryCloseSound);
    }
    public void UiOptionSound() //유아이 여는소리
    {
        audioSource.PlayOneShot(Sounds.sounds.UiSound);
    }
    public void BuySound() //구매소리
    {
        audioSource.PlayOneShot(Sounds.sounds.BuySound);
    }
    public void BuyfailSound() //구매 실패소리
    {
        audioSource.PlayOneShot(Sounds.sounds.BuyfailSound);
    }
    public void GetCoinSound() //코인줍는소리 or 물건판매소리
    {
        audioSource.PlayOneShot(Sounds.sounds.GetCoinSound);
    }
    public void EquipSound() //장비장착소리
    {
        audioSource.PlayOneShot(Sounds.sounds.EquipSound);
    }
    public void Quest1() //퀘스트 수락소리
    {
        audioSource.PlayOneShot(Sounds.sounds.Quest1);
    }
    public void Quest2() //퀘스트 완료소리
    {
        audioSource.PlayOneShot(Sounds.sounds.Quest2);
    }
}
