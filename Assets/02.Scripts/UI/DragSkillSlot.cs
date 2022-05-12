using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSkillSlot : MonoBehaviour
{
    static public DragSkillSlot instance;

    public SkillSlot dragSkillSlot;

    [SerializeField]
    private Image imageSkill;

    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    public void DragSetImage(Image _itemImage)
    {
        imageSkill.sprite = _itemImage.sprite;
        SetColor(1);
    }
    public void SetColor(float _alpha)
    {
        Color color = imageSkill.color;
        color.a = _alpha;
        imageSkill.color = color;
    }
}
