using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ch_sel2DAni : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer ani;
    [SerializeField]
    Image character;
    void Start()
    {
        character = GetComponent<Image>();
        ani = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        character.sprite = ani.sprite;
    }
}
