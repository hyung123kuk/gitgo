using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioSource audioSource;
    
    public static BGM bgm;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bgm = this;
       
    }
    private void Update()
    {
        if(!audioSource.isPlaying)
        audioSource.Play();
    }



}
