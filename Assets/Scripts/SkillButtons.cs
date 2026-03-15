using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    public int skillCost = 50;
    public Button buyButton;
    public TextMeshProUGUI statusText;
    public string skillName = "Dash-ability";

    void Start()
    {
        UpdateButtonUI();
    }

    public void OnBuyDashClicked()
    {
        if (ExperienceManager.instance.UnlockDash(skillCost))
        {
            Debug.Log("Dash unlocked!");
            UpdateButtonUI();
            FindFirstObjectByType<SkillTreeMenuController>()?.UpdateExpDisplay();
        }
    }

    void UpdateButtonUI()
    {
        if (ExperienceManager.instance.IsDashUnlocked())
        {
            buyButton.interactable = false;
            statusText.text = "Bought";
        }
        else
        {
            buyButton.interactable = ExperienceManager.instance.GetTotalExperience() >= skillCost;
            statusText.text = skillName + $" ({skillCost} EXP)";
        }
    }
}