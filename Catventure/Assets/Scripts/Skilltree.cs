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
    void Start()
    {
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

        foreach (SkillSO skill in fireSkills)
        {
            GameObject currentSkill = Instantiate(skillTreeButtonPrefab);
            currentSkill.transform.SetParent(fireTree.transform.Find(skill.level.ToString()));
            currentSkill.name = skill.name;
            currentSkill.GetComponentInChildren<TMP_Text>().text = skill.name;
            currentSkill.transform.localScale = Vector3.one;
            if(skill.skillNeeded)
            {
                Debug.Log("Drawing Line");
                LineRenderer lineRend = currentSkill.GetComponent<LineRenderer>();
                lineRend.SetPosition(0, currentSkill.transform.position);
                lineRend.SetPosition(1, fireTree.transform.Find(skill.skillNeeded.level.ToString()).Find(skill.skillNeeded.name).position);
            }
        }
    }

    
}
