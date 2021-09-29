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
    public int currentExp;
    public int currentLvl = 1;
    public int armor = 0;
    public int playerAttackDmg = 7;
    public int doubleShotDmg = 8;
    public int poisonDmg = 7;
    public int burnDmg = 15;
    public int paralyzeDmg = 30;
    public int poisonDuration = 5;
    public int burnDuration = 5;
    public int paralyzeDuration = 5;
    [Tooltip("Time the Player stays invulnerable after taking a hit")]
    public float invulnerableTime = 0.5F;
    private bool invulnerable;

    public Vector3 lastCheckpointPosition;

    [Foldout("SkillTree", true)]
    [Tooltip("Current Skill Points the player has")]
    public int currentSkillPoints;
    [Tooltip("Current Skills the Player has learned")]
    public List<SkillSO> learnedSkills = new List<SkillSO>();
    public List<SkillSO> multiSkillableSkills = new List<SkillSO>();

    [Foldout("Shopping", true)]

    public TMP_Text cookieCounter;

    private SpriteRenderer headSpriteRenderer;
    private SpriteRenderer bodySpriteRenderer;
    private SpriteRenderer weaponSpriteRenderer;
    private List<SpriteRenderer> armsSpriteRenderer = new List<SpriteRenderer>();
    private List<SpriteRenderer> legsSpriteRenderer = new List<SpriteRenderer>();
    public string currentHeadName = "StandardHead";
    public string currentBodyName = "StandardBody";
    public string currentWeaponName = "StandardWeapon";
    public string currentArmsName = "StandardArms";
    public string currentLegsName = "StandardLegs";
    public List<String> unlockedHeads = new List<string>();
    public List<String> unlockedBodies = new List<string>();
    public List<String> unlockedWeapons = new List<string>();
    public List<String> unlockedArms = new List<string>();
    public List<String> unlockedLegs = new List<string>();
    [Tooltip("Current number of Cookies the Player has collected")]
    public int currentCookies;
    public GameObject notEnoughCookiesBanner;
    private GameObject shopContent;

    [Foldout("Other", true)]

    public bool level1Finished;
    public bool level2Finished;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        notEnoughCookiesBanner = GameObject.Find("NotEnoughCookiesBanner");
        notEnoughCookiesBanner.SetActive(false);
        lastCheckpointPosition = transform.position;
        shopContent = GameObject.Find("ShopContent");
        cookieCounter = GameObject.Find("CookieCounter").GetComponent<TMP_Text>();
        cookieCounter.text = "Cookies: " + currentCookies;

        headSpriteRenderer = GameObject.Find("Head").GetComponent<SpriteRenderer>();
        bodySpriteRenderer = GetComponent<SpriteRenderer>();
        weaponSpriteRenderer = GameObject.Find("BowSprite").GetComponent<SpriteRenderer>();
        armsSpriteRenderer.Add(GameObject.Find("LeftArm").GetComponent<SpriteRenderer>());
        armsSpriteRenderer.Add(GameObject.Find("RightArm").GetComponent<SpriteRenderer>());
        armsSpriteRenderer.Add(GameObject.Find("LeftFoot").GetComponent<SpriteRenderer>());
        armsSpriteRenderer.Add(GameObject.Find("RightFoot").GetComponent<SpriteRenderer>());
    }

    public void GetExp(int expAmount)
    {
        currentExp += expAmount;
        if (currentExp >= (currentLvl * 100))
        {
            currentExp -= (currentLvl * 100);
            currentLvl += 1;
            currentSkillPoints += 1;
        }
    }

    public void GotDamaged(int damage)
    {
        StartCoroutine(DamageDealt(damage));
        if (currentPlayerHealth <= 0)
        {
            transform.position = lastCheckpointPosition;
            currentPlayerHealth = maxPlayerHealth;
        }
    }

    public IEnumerator DamageDealt(int damage)
    {
        if (!invulnerable)
        {
            damage -= armor;
            if (damage > 0)
            {
                currentPlayerHealth -= damage;
                invulnerable = true;
            }
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

        if (col.gameObject.CompareTag("Checkpoint"))
        {
            lastCheckpointPosition = transform.position;
        }

        if (col.gameObject.CompareTag("Deathzone"))
        {
            transform.position = lastCheckpointPosition;
            currentPlayerHealth = maxPlayerHealth;
        }
    }

    public void LearnSkill(SkillSO skillToLearn, Button skillBtn)
    {
        Debug.Log("Trying to learn Skill " + skillToLearn.name);
        if (learnedSkills.Contains(skillToLearn))
        {
            Debug.Log("You already learned this skill");
        }

        else if (skillToLearn.skillsNeeded.Count > 0 && !ListValueInList(skillToLearn.skillsNeeded, learnedSkills))
        {
            Debug.Log("You need to learn " + skillToLearn.skillsNeeded + " to learn this skill.");
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
                Debug.Log("Added Skill to multilearnSkills");
            }
            Debug.Log("Learned Skill " + skillToLearn.name);

            switch (skillToLearn.name)
            {

                case "Präziser Schuss":
                    if (learnedSkills.Contains(skillToLearn))
                    {
                        playerAttackDmg += 3;
                    };
                    break;

                case "Überspannen":
                    playerAttackDmg += 2;
                    break;

                case "Doppelter Treffer":
                    if (learnedSkills.Contains(skillToLearn))
                    {
                        doubleShotDmg += 3;
                    };
                    break;

                case "Adlerauge":
                    doubleShotDmg += 3;
                    break;

                case "Vergiften":
                    if (learnedSkills.Contains(skillToLearn))
                    {
                        poisonDmg += 3;
                    }
                    break;

                case "Verbrennen":
                    if (learnedSkills.Contains(skillToLearn))
                    {
                        burnDmg += 3;
                    }
                    break;

                case "Alchemist":
                    burnDmg += 3;
                    poisonDmg += 3;
                    break;

                default:
                    break;
            }

        }

    }

    private bool ListValueInList(List<SkillSO> ListA, List<SkillSO> ListB)
    {
        for (int i = 0; i < ListA.Count; i++)
        {
            if (ListA.Contains(ListB[i]))
                return true;
        }
        return false;
    }
    public void buyHead(EquipmentSO equipmentToBuy)
    {
        notEnoughCookiesBanner.SetActive(false);
        Debug.Log("Trying to buy " + equipmentToBuy);
        if (unlockedHeads.Contains(equipmentToBuy.name))
        {
            headSpriteRenderer.sprite = equipmentToBuy.equipmentSprite;
            shopContent.transform.Find(currentHeadName).GetComponent<Image>().color = Color.white;
            currentHeadName = equipmentToBuy.name;
            shopContent.transform.Find(currentHeadName).GetComponent<Image>().color = Color.green;
        }


        else if (equipmentToBuy.cookieCost > currentCookies)
        {
            Debug.Log("You don't have enough Cookies.");
            notEnoughCookiesBanner.SetActive(true);

        }
        else
        {
            currentCookies -= equipmentToBuy.cookieCost;
            unlockedHeads.Add(equipmentToBuy.name);
            GameObject currentEquipmentUI = shopContent.transform.Find(equipmentToBuy.name).gameObject;
            currentEquipmentUI.transform.Find("CookiePrice").gameObject.SetActive(false);
            Debug.Log("Bought Head" + equipmentToBuy.name);
            shopContent.transform.Find(currentHeadName).GetComponent<Image>().color = Color.white;
            currentHeadName = equipmentToBuy.name;
            headSpriteRenderer.sprite = equipmentToBuy.equipmentSprite;
            shopContent.transform.Find(currentHeadName).GetComponent<Image>().color = Color.green;
            cookieCounter.text = "Cookies: " + currentCookies;
        }
    }
    public void buyBody(EquipmentSO equipmentToBuy)
    {
        notEnoughCookiesBanner.SetActive(false);
        Debug.Log("Trying to buy " + equipmentToBuy);
        if (unlockedBodies.Contains(equipmentToBuy.name))
        {
            bodySpriteRenderer.sprite = equipmentToBuy.equipmentSprite;
            shopContent.transform.Find(currentBodyName).GetComponent<Image>().color = Color.white;
            currentBodyName = equipmentToBuy.name;
            shopContent.transform.Find(currentBodyName).GetComponent<Image>().color = Color.green;
        }


        else if (equipmentToBuy.cookieCost > currentCookies)
        {
            Debug.Log("You don't have enough Cookies.");
            notEnoughCookiesBanner.SetActive(true);
        }
        else
        {
            currentCookies -= equipmentToBuy.cookieCost;
            unlockedBodies.Add(equipmentToBuy.name);
            GameObject currentEquipmentUI = shopContent.transform.Find(equipmentToBuy.name).gameObject;
            currentEquipmentUI.transform.Find("CookiePrice").gameObject.SetActive(false);
            Debug.Log("Bought Body" + equipmentToBuy.name);
            shopContent.transform.Find(currentBodyName).GetComponent<Image>().color = Color.white;
            currentBodyName = equipmentToBuy.name;
            bodySpriteRenderer.sprite = equipmentToBuy.equipmentSprite;
            shopContent.transform.Find(currentBodyName).GetComponent<Image>().color = Color.green;
            cookieCounter.text = "Cookies: " + currentCookies;
        }
    }
    public void buyWeapon(EquipmentSO equipmentToBuy)
    {
        notEnoughCookiesBanner.SetActive(false);
        Debug.Log("Trying to buy " + equipmentToBuy);
        if (unlockedWeapons.Contains(equipmentToBuy.name))
        {
            weaponSpriteRenderer.sprite = equipmentToBuy.equipmentSprite;
            shopContent.transform.Find(currentWeaponName).GetComponent<Image>().color = Color.white;
            currentWeaponName = equipmentToBuy.name;
            shopContent.transform.Find(currentWeaponName).GetComponent<Image>().color = Color.green;
        }


        else if (equipmentToBuy.cookieCost > currentCookies)
        {
            Debug.Log("You don't have enough Cookies.");
            notEnoughCookiesBanner.SetActive(true);
        }
        else
        {
            currentCookies -= equipmentToBuy.cookieCost;
            unlockedWeapons.Add(equipmentToBuy.name);
            GameObject currentEquipmentUI = shopContent.transform.Find(equipmentToBuy.name).gameObject;
            currentEquipmentUI.transform.Find("CookiePrice").gameObject.SetActive(false);
            Debug.Log("Bought Weapon" + equipmentToBuy.name);
            shopContent.transform.Find(currentWeaponName).GetComponent<Image>().color = Color.white;
            currentWeaponName = equipmentToBuy.name;
            weaponSpriteRenderer.sprite = equipmentToBuy.equipmentSprite;
            shopContent.transform.Find(currentWeaponName).GetComponent<Image>().color = Color.green;
            cookieCounter.text = "Cookies: " + currentCookies;
        }
    }

    public void buyArms(EquipmentSO equipmentToBuy)
    {
        notEnoughCookiesBanner.SetActive(false);
        Debug.Log("Trying to buy " + equipmentToBuy);
        if (unlockedArms.Contains(equipmentToBuy.name))
        {
            armsSpriteRenderer[0].sprite = equipmentToBuy.equipmentSprite;
            armsSpriteRenderer[1].sprite = equipmentToBuy.equipmentSpriteRightArmOrLeg;
            shopContent.transform.Find(currentArmsName).GetComponent<Image>().color = Color.white;
            currentArmsName = equipmentToBuy.name;
            shopContent.transform.Find(currentArmsName).GetComponent<Image>().color = Color.green;
        }


        else if (equipmentToBuy.cookieCost > currentCookies)
        {
            Debug.Log("You don't have enough Cookies.");
            notEnoughCookiesBanner.SetActive(true);
        }
        else
        {
            currentCookies -= equipmentToBuy.cookieCost;
            unlockedArms.Add(equipmentToBuy.name);
            GameObject currentEquipmentUI = shopContent.transform.Find(equipmentToBuy.name).gameObject;
            currentEquipmentUI.transform.Find("CookiePrice").gameObject.SetActive(false);
            Debug.Log("Bought Arms " + equipmentToBuy.name);
            shopContent.transform.Find(currentArmsName).GetComponent<Image>().color = Color.white;
            currentArmsName = equipmentToBuy.name;
            armsSpriteRenderer[0].sprite = equipmentToBuy.equipmentSprite;
            armsSpriteRenderer[1].sprite = equipmentToBuy.equipmentSpriteRightArmOrLeg;
            shopContent.transform.Find(currentArmsName).GetComponent<Image>().color = Color.green;
            cookieCounter.text = "Cookies: " + currentCookies;
        }
    }
    public void buyLegs(EquipmentSO equipmentToBuy)
    {
        notEnoughCookiesBanner.SetActive(false);
        Debug.Log("Trying to buy " + equipmentToBuy);
        if (unlockedLegs.Contains(equipmentToBuy.name))
        {
            legsSpriteRenderer[0].sprite = equipmentToBuy.equipmentSprite;
            legsSpriteRenderer[1].sprite = equipmentToBuy.equipmentSpriteRightArmOrLeg;
            shopContent.transform.Find(currentLegsName).GetComponent<Image>().color = Color.white;
            currentLegsName = equipmentToBuy.name;
            shopContent.transform.Find(currentLegsName).GetComponent<Image>().color = Color.green;
        }


        else if (equipmentToBuy.cookieCost > currentCookies)
        {
            Debug.Log("You don't have enough Cookies.");
            notEnoughCookiesBanner.SetActive(true);
        }
        else
        {
            currentCookies -= equipmentToBuy.cookieCost;
            unlockedLegs.Add(equipmentToBuy.name);
            GameObject currentEquipmentUI = shopContent.transform.Find(equipmentToBuy.name).gameObject;
            currentEquipmentUI.transform.Find("CookiePrice").gameObject.SetActive(false);
            Debug.Log("Bought Legs " + equipmentToBuy.name);
            shopContent.transform.Find(currentLegsName).GetComponent<Image>().color = Color.white;
            currentLegsName = equipmentToBuy.name;
            legsSpriteRenderer[0].sprite = equipmentToBuy.equipmentSprite;
            legsSpriteRenderer[1].sprite = equipmentToBuy.equipmentSpriteRightArmOrLeg;
            shopContent.transform.Find(currentLegsName).GetComponent<Image>().color = Color.green;
            cookieCounter.text = "Cookies: " + currentCookies;
        }
    }
}
