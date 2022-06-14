using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioSource audioSource;
    SoundBar soundbar;
    void Start()
    {
        soundbar = FindObjectOfType<SoundBar>();
        audioSource = GetComponent<AudioSource>();  
    }

    private void Update()
    {
        if(!audioSource.isPlaying)
        audioSource.Play();
        audioSource.volume = soundbar.backVolume;
    }



}
