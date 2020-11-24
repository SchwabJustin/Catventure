using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public enum SkillTreeType { Fire, Ice, Earth };

public class Skilltree : MonoBehaviour
{
    private GameObject player;
    public GameObject skillTreeButtonPrefab;
    public List<SkillSO> allSkills;
    public List<SkillSO> fireSkills;
    public List<SkillSO> iceSkills;
    public List<SkillSO> earthSkills;
    GameObject fireTree;
    GameObject iceTree;
    GameObject earthTree;
    Camera cam;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
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
            Debug.Log(skill);
            GameObject currentSkill = Instantiate(skillTreeButtonPrefab);
            currentSkill.GetComponent<Button>().onClick.AddListener(delegate { player.GetComponent<PlayerManager>().LearnSkill(skill, currentSkill.GetComponent<Button>()); });
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
                Debug.Log("Drawing Line from " + currentSkill.transform.position + " " + currentSkill.name);
                lineRendPosition = skilltree.transform.Find(skill.skillNeeded.level.ToString()).Find(skill.skillNeeded.name).position +
                                   new Vector3(0.7F, 0.7F, 0);
                lineRend.SetPosition(1, lineRendPosition);
                Debug.Log("Drawing Line to " + skilltree.transform.Find(skill.skillNeeded.level.ToString()).Find(skill.skillNeeded.name).position + " " + skilltree.transform.Find(skill.skillNeeded.level.ToString()).Find(skill.skillNeeded.name).gameObject.name);
            }
            previousLevel = skill.level;
        }
    }

}
