using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public enum SkillTreeType { Fire, Ice, Earth };

public class Skilltree : MonoBehaviour
{
    private PlayerManager playerManager;
    [Tooltip("Prefab for the SkilltreeButton")]
    public GameObject skillTreeButtonPrefab;
    private List<SkillSO> allSkills;
    private List<SkillSO> fireSkills;
    private List<SkillSO> iceSkills;
    private List<SkillSO> earthSkills;
    private GameObject fireTree;
    private GameObject iceTree;
    private GameObject earthTree;
    private TMP_Text currentSkillPointsUI;
    void Start()
    {

        DontDestroyOnLoad(this.gameObject.transform.root);
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        currentSkillPointsUI = transform.Find("CurrentSkillPointsUI").GetComponent<TMP_Text>();
        currentSkillPointsUI.text = "Skillpoints \n " + playerManager.currentSkillPoints;
        allSkills = Resources.LoadAll<SkillSO>("ScriptableObjects/Skills").ToList();
        fireSkills = allSkills.Where(s => s.skilltreeType == SkillTreeType.Fire).ToList();
        fireSkills = fireSkills.OrderBy(s => s.level).ToList();
        iceSkills = allSkills.Where(s => s.skilltreeType == SkillTreeType.Ice).ToList();
        iceSkills = iceSkills.OrderBy(s => s.level).ToList();
        earthSkills = allSkills.Where(s => s.skilltreeType == SkillTreeType.Earth).ToList();
        earthSkills = earthSkills.OrderBy(s => s.level).ToList();

        fireTree = gameObject.transform.Find("FireTree").gameObject;
        iceTree = gameObject.transform.Find("IceTree").gameObject;
        earthTree = gameObject.transform.Find("EarthTree").gameObject;
        GenerateSkilltree(fireTree, fireSkills);
        GenerateSkilltree(iceTree, iceSkills);
        GenerateSkilltree(earthTree, earthSkills);
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
