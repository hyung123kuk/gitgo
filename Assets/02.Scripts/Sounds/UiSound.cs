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
    //=====================������ ����===============================
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "ChSel_sangin")  //ĳ���ͼ���â �������� ����X
            audioSource.volume = soundbar.UIVolume;
    }

    public void InventoryOpenSound() //�κ� ���¼Ҹ�
    {

        audioSource.PlayOneShot(Sounds.sounds.InventoryOpenSound);
    }
    public void InventoryCloseSound() //�κ� �ݴ¼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.InventoryCloseSound);
    }
    public void UiOptionSound() //������ ���¼Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.UiSound);
    }
    public void BuySound() //���żҸ�
    {
        audioSource.PlayOneShot(Sounds.sounds.BuySound);
    }
    public void BuyfailSound() //���� ���мҸ�
    {
        audioSource.PlayOneShot(Sounds.sounds.BuyfailSound);
    }
    public void GetCoinSound() //�����ݴ¼Ҹ� or �����ǸżҸ�
    {
        audioSource.PlayOneShot(Sounds.sounds.GetCoinSound);
    }
    public void EquipSound() //��������Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.EquipSound);
    }
    public void Quest1() //����Ʈ �����Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.Quest1);
    }
    public void Quest2() //����Ʈ �Ϸ�Ҹ�
    {
        audioSource.PlayOneShot(Sounds.sounds.Quest2);
    }
}
