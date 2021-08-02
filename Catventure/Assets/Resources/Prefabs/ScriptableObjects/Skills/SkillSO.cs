using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]

public class SkillSO : ScriptableObject
{
    [Tooltip("Which skilltree contains this skill.")]
    public SkillTreeType skilltreeType;
    [Tooltip("Image that represents this skill.")]
    public Sprite skillImg;
    [Tooltip("Level that is needed to learn this skill."), Range(1, 5)]
    public int level;
    [Tooltip("Skills the player has to learn before he can learn this skill.")]
    public List<SkillSO> skillsNeeded = new List<SkillSO>();
    [Tooltip("Number of skillpoints needed to learn this skill.")]
    public int skillPointsNeeded;
    [Tooltip("How much the physical attack of the player gets buffed. This value adds up on other skills.")]
    public bool doubleSkillable;
    [Tooltip("You can level up this skill 2 times.")]
    public int attackBonus;
    [Tooltip("How much the defense of the player gets buffed. This value adds up on other skills.")]
    public int defenseBonus;
    [Tooltip("How much the magic attack of the player gets buffed. This value adds up on other skills.")]
    public int magicBonus;
    [Tooltip("Description of the skill."), TextAreaAttribute(5, 20)]
    public string description;
}
