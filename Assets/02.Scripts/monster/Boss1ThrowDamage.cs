using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1ThrowDamage : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 3f);
    }
}
