using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
public enum SkillTreeType { Fire, Ice, Earth};

public class Skilltree : MonoBehaviour
{

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
        float xPosition = 5;
        int previousLevel = 0;
        foreach (SkillSO skill in fireSkills)
        {
            Debug.Log(skill);
            GameObject currentSkill = Instantiate(skillTreeButtonPrefab);
            RectTransform currentTransform = currentSkill.GetComponent<RectTransform>();
            currentSkill.transform.SetParent(fireTree.transform.Find(skill.level.ToString()));
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
                lineRend.SetPosition(0, currentSkill.transform.position);
                Debug.Log("Drawing Line from " + currentSkill.transform.position + " " + currentSkill.name);
                lineRend.SetPosition(1, fireTree.transform.Find(skill.skillNeeded.level.ToString()).Find(skill.skillNeeded.name).position);
                Debug.Log("Drawing Line to " + fireTree.transform.Find(skill.skillNeeded.level.ToString()).Find(skill.skillNeeded.name).position + " " + fireTree.transform.Find(skill.skillNeeded.level.ToString()).Find(skill.skillNeeded.name).gameObject.name);
            }
            previousLevel = skill.level;
        }
    }

    
}
