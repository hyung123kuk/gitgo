using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/Skill")]
public class SkillUI : ScriptableObject
{
    public Sprite SkillImage;
    public enum SkillCharacter { Warrior,Archer,Mage,Common}
    public  SkillCharacter skillCharacter;
    public int skillNum;
    public int Level;
    public string skillName;
    public enum SkillType { Attack, Buff, Move}
    public SkillType skillType;

    [TextArea]
    public string skillText;

    public VideoClip[] skillVideo;
    
}
