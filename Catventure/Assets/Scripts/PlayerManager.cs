using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int currentSkillPoints;
    public List<SkillSO> learnedSkills;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
