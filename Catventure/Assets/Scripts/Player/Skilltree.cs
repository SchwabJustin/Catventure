using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public enum SkillTreeType { Sword, Magic, Bow };

public class Skilltree : MonoBehaviour
{
    private PlayerManager playerManager;
    [Tooltip("Prefab for the SkilltreeButton")]
    public GameObject skillTreeButtonPrefab;
    private List<SkillSO> allSkills;
    private List<SkillSO> swordSkills;
    private List<SkillSO> magicSkills;
    private List<SkillSO> bowSkills;
    private GameObject swordTree;
    private GameObject magicTree;
    private GameObject bowTree;
    private TMP_Text currentSkillPointsUI;
    void Start()
    {

        DontDestroyOnLoad(this.gameObject.transform.root);
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        currentSkillPointsUI = transform.Find("CurrentSkillPointsUI").GetComponent<TMP_Text>();
        currentSkillPointsUI.text = "Skillpoints \n " + playerManager.currentSkillPoints;
        allSkills = Resources.LoadAll<SkillSO>("ScriptableObjects/Skills").ToList();
        swordSkills = allSkills.Where(s => s.skilltreeType == SkillTreeType.Sword).ToList();
        swordSkills = swordSkills.OrderBy(s => s.level).ToList();
        magicSkills = allSkills.Where(s => s.skilltreeType == SkillTreeType.Magic).ToList();
        magicSkills = magicSkills.OrderBy(s => s.level).ToList();
        bowSkills = allSkills.Where(s => s.skilltreeType == SkillTreeType.Bow).ToList();
        bowSkills = bowSkills.OrderBy(s => s.level).ToList();

        swordTree = gameObject.transform.Find("SwordTree").gameObject;
        magicTree = gameObject.transform.Find("MagicTree").gameObject;
        bowTree = gameObject.transform.Find("BowTree").gameObject;
        GenerateSkilltree(swordTree, swordSkills);
        GenerateSkilltree(magicTree, magicSkills);
        GenerateSkilltree(bowTree, bowSkills);
    }

    void GenerateSkilltree(GameObject skilltree, List<SkillSO> skills)
    {
        float xPosition = 5;
        int previousLevel = 0;
        foreach (SkillSO skill in skills)
        {
            GameObject currentSkill = Instantiate(skillTreeButtonPrefab);
            currentSkill.GetComponent<Button>().onClick.AddListener(delegate { playerManager.LearnSkill(skill, currentSkill.GetComponent<Button>()); });
            RectTransform currentTransform = currentSkill.GetComponent<RectTransform>();
            currentSkill.transform.SetParent(skilltree.transform.Find(skill.level.ToString()));
            currentSkill.name = skill.name;
            currentSkill.GetComponentInChildren<TMP_Text>().text = skill.name;
            currentSkill.transform.localScale = Vector3.one;
            currentSkill.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            if (previousLevel == skill.level)
            {
                xPosition += 155;
            }
            else
            {
                xPosition = 5;
            }
            currentSkill.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition, currentTransform.anchoredPosition.y);
            if (skill.skillNeeded)
            {
                LineRenderer lineRend = currentSkill.GetComponent<LineRenderer>();
                Vector3 lineRendPosition = currentSkill.transform.position + new Vector3(0.7F, -0.7F, 0);
                lineRend.SetPosition(0, lineRendPosition);
                lineRendPosition = skilltree.transform.Find(skill.skillNeeded.level.ToString()).Find(skill.skillNeeded.name).position +
                                   new Vector3(0.7F, 0.7F, 0);
                lineRend.SetPosition(1, lineRendPosition);
            }
            previousLevel = skill.level;
        }
    }

    void Update()
    {
        currentSkillPointsUI.text = "Skillpoints \n " + playerManager.currentSkillPoints;
    }
}
