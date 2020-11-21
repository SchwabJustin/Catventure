using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]

public class SkillSO : ScriptableObject
{
    [Tooltip("Which skilltree contains this skill.")]
    public SkillTreeType skilltreeType;
    [Tooltip("Level that is needed to learn this skill.")]
    public int level;
    [Tooltip("Skills the player has to learn before he can learn this skill.")]
    public List<SkillSO> skillsNeeded;
    [Tooltip("Number of skillpoints needed to learn this skill.")]
    public int skillPointsNeeded;
    [Tooltip("How much the physical attack of the player gets buffed. This value adds up on other skills.")]
    public int attackBonus;
    [Tooltip("How much the defense of the player gets buffed. This value adds up on other skills.")]
    public int defenseBonus;
    [Tooltip("How much the magic attack of the player gets buffed. This value adds up on other skills.")]
    public int magicBonus;
}
