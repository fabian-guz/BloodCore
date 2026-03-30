using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "SkillSystem/Skill")]
public class SkillData : ScriptableObject
{
    [Header("Skill Settings")]
    public string skillName;
    public int skillCost;
    public string skillID;
    public bool isUnlocked;
    public SkillData requiredSkill;
}
