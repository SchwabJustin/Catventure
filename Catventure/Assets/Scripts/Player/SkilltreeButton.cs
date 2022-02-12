using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkilltreeButton : MonoBehaviour
{
    public SkillSO skill;
    public PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        GetComponent<Button>().onClick.AddListener(delegate { playerManager.LearnSkill(skill, GetComponent<Button>()); });
        gameObject.name = skill.name;
        gameObject.GetComponent<Image>().sprite = skill.skillImg;
        gameObject.GetComponentInChildren<TMP_Text>().text = skill.description;
        gameObject.transform.Find("DescriptionWindow").gameObject.SetActive(false);
        if (!skill.doubleSkillable)
        {
            transform.Find("MultiSkill").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("MultiSkill").gameObject.name = "MultiSkill" + skill.name;
        }
        gameObject.transform.localScale = Vector3.one;
    }
}
