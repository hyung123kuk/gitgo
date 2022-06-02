using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioSource audioSource;
    SoundBar soundbar;
    public static BGM bgm;
    void Start()
    {
        soundbar = FindObjectOfType<SoundBar>();
        audioSource = GetComponent<AudioSource>();
        bgm = this;
       
    }

    private void Update()
    {
        if(!audioSource.isPlaying)
        audioSource.Play();
        audioSource.volume = soundbar.backVolume;
    }



}
