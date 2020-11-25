using System;
using System.Collections;
using System.Collections.Generic;
using Homebrew;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Foldout("PlayerStats", true)]
    [Tooltip("Current Health of the Player")]
    public int currentPlayerHealth = 3;
    [Tooltip("Maximal Health of the Player")]
    public int maxPlayerHealth = 3;
    [Tooltip("Damage the Player deals with Attacks")]
    public int playerAttackDmg = 1;
    [Tooltip("Damage the Player deals with Magic")]
    public int playerMagicDmg = 1;

    [Tooltip("Time the Player stays invulnerable after taking a hit")]
    public float invulnerableTime = 0.5F;
    [Foldout("SkillStuff", true)]
    [Tooltip("Current Skill Points the player has")]
    public int currentSkillPoints;
    [Tooltip("Current Skills the Player has learned")]
    public List<SkillSO> learnedSkills;

    private bool invulnerable;


    public void GotDamaged(int damage)
    {
        StartCoroutine(DamageDealt(damage));
    }

    public IEnumerator DamageDealt(int damage)
    {
        if (!invulnerable)
        {
            currentPlayerHealth -= damage;
            invulnerable = true;
        }
        yield return new WaitForSeconds(invulnerableTime);
        if (invulnerable)
        {
            invulnerable = false;
        }
    }

    public void LearnSkill(SkillSO skillToLearn, Button skillBtn)
    {
        Debug.Log("Trying to learn Skill " + skillToLearn.name);
        if (learnedSkills.Contains(skillToLearn))
        {
            Debug.Log("You already learned this skill");
        }
        else if (skillToLearn.skillNeeded)
        {
            if (!learnedSkills.Contains(skillToLearn.skillNeeded))
            {
                Debug.Log("You need to learn " + skillToLearn.skillNeeded.name + " to learn this skill.");
            }

            else if (skillToLearn.skillPointsNeeded > currentSkillPoints)
            {
                Debug.Log("You don't have enough skill points.");
            }
            else
            {
                currentSkillPoints -= skillToLearn.skillPointsNeeded;
                learnedSkills.Add(skillToLearn);
                if (skillBtn)
                {
                    skillBtn.interactable = false;
                }
                Debug.Log("Learned Skill " + skillToLearn.name);
            }
        }

        else if (skillToLearn.skillPointsNeeded > currentSkillPoints)
        {
            Debug.Log("You don't have enough skill points.");
        }
        else
        {
            currentSkillPoints -= skillToLearn.skillPointsNeeded;
            learnedSkills.Add(skillToLearn);
            if (skillBtn)
            {
                skillBtn.interactable = false;
            }
            Debug.Log("Learned Skill " + skillToLearn.name);
        }
    }

}
