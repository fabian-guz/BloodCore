using UnityEngine;
using UnityEngine.UI;

public class SkillTreeLine : MonoBehaviour
{
    [Header("Settings")]
    public SkillData requiredSkill;
    public Image lineImage; // Line object

    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.yellow;

    void OnEnable()
    {
        ExperienceManager.OnSkillTreeUpdated += RefreshLine;
    }

    void OnDisable()
    {
        ExperienceManager.OnSkillTreeUpdated -= RefreshLine;
    }

    void Start()
    {
        if (lineImage == null)
        {
            lineImage = GetComponent<Image>();
        }
        RefreshLine();
    }

    public void RefreshLine()
    {
        if (requiredSkill == null || ExperienceManager.instance == null)
        {
            return;
        }

        if (ExperienceManager.instance.IsSkillUnlocked(requiredSkill.skillID))
        {
            lineImage.color = unlockedColor;
        }
        else
        {
            lineImage.color = lockedColor;
        }
    }
}
