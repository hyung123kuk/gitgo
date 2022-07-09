using UnityEngine;
using System.Collections;

public class csDestroyEffect : MonoBehaviour {

    [SerializeField]
    ParticleSystem[] particles;

    private void Start()
    {
        particleOff();
    }

    public void particleOn()
    {
        for(int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }

    public void particleOff()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop();
        }
    }

}
