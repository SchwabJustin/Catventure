using System;
using System.Collections;
using System.Collections.Generic;
using Homebrew;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerManager : MonoBehaviour
{
    [Foldout("Fight", true)]
    [Tooltip("Current Health of the Player")]
    public int currentPlayerHealth = 3;
    [Tooltip("Maximal Health of the Player")]
    public int maxPlayerHealth = 3;
    [Tooltip("Damage the Player deals with Attacks")]
    public int playerAttackDmg = 1;
    [Tooltip("Time the Player stays invulnerable after taking a hit")]
    public float invulnerableTime = 0.5F;
    private bool invulnerable;

    [Foldout("SkillTree", true)]
    [Tooltip("Current Skill Points the player has")]
    public int currentSkillPoints;
    [Tooltip("Current Skills the Player has learned")]
    public List<SkillSO> learnedSkills = new List<SkillSO>();
    List<SkillSO> multiSkillableSkills = new List<SkillSO>();

    [Foldout("Shopping", true)]

    public TMP_Text cookieCounter;

    private GameObject headsParent;
    public string currentHeadName = "StandardHead";
    public List<String> unlockedHeads;
    public List<GameObject> heads;
    [Tooltip("Current number of Cookies the Player has collected")]
    public int currentCookies;

    private GameObject shopContent;

    void Awake()
    {
        shopContent = GameObject.Find("ShopContent");
        cookieCounter = GameObject.Find("CookieCounter").GetComponent<TMP_Text>();
        cookieCounter.text = "Cookies: " + currentCookies;

        if (!unlockedHeads.Contains("StandardHead"))
        {
            unlockedHeads.Add("StandardHeads");
        }
        headsParent = transform.Find("Heads").gameObject;
        for (int i = 0; headsParent.transform.childCount > i; i++)
        {
            heads.Add(headsParent.transform.GetChild(i).gameObject);
            heads[i].SetActive(false);
        }

        heads.Find(go => go.name == currentHeadName).SetActive(true);
    }

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

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Cookie"))
        {
            currentCookies += col.gameObject.GetComponent<Cookie>().cookieAmount;
            Destroy(col.gameObject);
            cookieCounter.text = "Cookies: " + currentCookies;
        }
    }

    public void LearnSkill(SkillSO skillToLearn, Button skillBtn)
    {
        Debug.Log("Trying to learn Skill " + skillToLearn.name);
        if (learnedSkills.Contains(skillToLearn))
        {
            Debug.Log("You already learned this skill");
        }

        else if (!learnedSkills.Contains(skillToLearn.skillNeeded) && skillToLearn.skillNeeded)
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
            if (!skillToLearn.doubleSkillable)
            {
                Debug.Log("Learned Skill");
                learnedSkills.Add(skillToLearn);
                if (skillBtn)
                {
                    skillBtn.interactable = false;
                }
            }
            else if (multiSkillableSkills.Contains(skillToLearn))
            {
                Debug.Log("Learned Skill");
                learnedSkills.Add(skillToLearn);
                if (skillBtn)
                {
                    skillBtn.interactable = false;
                }
            }
            else
            {
                multiSkillableSkills.Add(skillToLearn);
                Debug.Log("Added Skill to multilearnSKills");
            }
            Debug.Log("Learned Skill " + skillToLearn.name);
            if (skillToLearn.name == "Präziser Schuss")
            {
                Debug.Log("Learned Präziser Schuss");
                GetComponentInChildren<Bow>().learnedSkill = true;
            }
        }

    }


    public void buyHead(EquipmentSO equipmentToBuy)
    {
        Debug.Log("Trying to buy " + equipmentToBuy);
        if (unlockedHeads.Contains(equipmentToBuy.name))
        {
            heads.Find(go => go.name == currentHeadName).SetActive(false);
            shopContent.transform.Find(currentHeadName).GetComponent<Image>().color = Color.white;
            currentHeadName = equipmentToBuy.name;
            heads.Find(go => go.name == currentHeadName).SetActive(true);
            shopContent.transform.Find(currentHeadName).GetComponent<Image>().color = Color.green;
        }


        else if (equipmentToBuy.cookieCost > currentCookies)
        {
            Debug.Log("You don't have enough Cookies.");
        }
        else
        {
            currentCookies -= equipmentToBuy.cookieCost;
            unlockedHeads.Add(equipmentToBuy.name);
            GameObject currentEquipmentUI = shopContent.transform.Find(equipmentToBuy.name).gameObject;
            currentEquipmentUI.transform.Find("CookiePrice").gameObject.SetActive(false);
            Debug.Log("Bought Head" + equipmentToBuy.name);
            heads.Find(go => go.name == currentHeadName).SetActive(false);
            shopContent.transform.Find(currentHeadName).GetComponent<Image>().color = Color.white;
            currentHeadName = equipmentToBuy.name;
            heads.Find(go => go.name == currentHeadName).SetActive(true);
            shopContent.transform.Find(currentHeadName).GetComponent<Image>().color = Color.green;
            cookieCounter.text = "Cookies: " + currentCookies;
        }
    }
}
