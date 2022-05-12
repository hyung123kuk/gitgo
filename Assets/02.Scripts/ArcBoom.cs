using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBoom : MonoBehaviour
{
    public GameObject Boom;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            GameObject eff = Instantiate(Boom, transform.position, transform.rotation);
            Destroy(eff, 1.5f);
        }
    }
}
