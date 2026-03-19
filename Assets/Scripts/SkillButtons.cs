using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    public SkillData skillData;

    [Header("UI References")]
    public Button buyButton;
    public TextMeshProUGUI statusText;

    void OnEnable()
    {
        ExperienceManager.OnSkillTreeUpdated += RefreshUI;
    }

    void OnDisable()
    {
        ExperienceManager.OnSkillTreeUpdated -= RefreshUI;
    }

    void Start()
    {
        RefreshUI();
    }

    public void OnBuyClicked()
    {
        if (ExperienceManager.instance.TryUnlockSkill(skillData))
        {
            Debug.Log($"{skillData.skillName} unlocked!");
            RefreshUI();
            FindFirstObjectByType<SkillTreeMenuController>()?.UpdateExpDisplay();
        }
    }

    public void RefreshUI()
    {
        if (skillData == null)
        {
            return;
        }

        bool isBought = ExperienceManager.instance.IsSkillUnlocked(skillData.skillID);
        bool canBuy = ExperienceManager.instance.GetTotalExperience() >= skillData.skillCost;
        bool requirementsMet = skillData.requiredSkill == null || ExperienceManager.instance.IsSkillUnlocked(skillData.requiredSkill.skillID);

        if (isBought)
        {
            SetState(false, "Bought", Color.green);
        }
        else if (!requirementsMet)
        {
            SetState(false, "Locked", Color.gray);
        }
        else
        {
            SetState(canBuy, $"{skillData.skillName}\n ({skillData.skillCost} EXP)", canBuy ? Color.white : Color.red);
        }
    }

    private void SetState(bool interactable, string text, Color color)
    {
        buyButton.interactable = interactable;
        statusText.text = text;
        statusText.color = color;
    }
}