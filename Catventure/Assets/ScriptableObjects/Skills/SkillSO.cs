using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]

public class SkillSO : ScriptableObject
{
    public SkillTreeType skillTreeType;
    public int level;
    public List<SkillSO> skillsNeeded;
    public int skillPointsNeeded;
    public int attackBonus;
    public int defenseBonus;
    public int magicBonus;
}
