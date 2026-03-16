using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager instance;

    [Header("Settings")]
    public string saveKey = "PlayerTotalEXP"; 

    private int _currentTotalExp;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }

        LoadExperience();
    }

    private void LoadExperience()
    {
        _currentTotalExp = PlayerPrefs.GetInt(saveKey, 0);
        Debug.Log("EXP geladen: " + _currentTotalExp);
    }

    public void AddExperience(int amount)
    {
        _currentTotalExp += amount;

        // Direct save
        PlayerPrefs.SetInt(saveKey, _currentTotalExp);
        PlayerPrefs.Save();

        Debug.Log("EXP gained! New total: " + _currentTotalExp);
    }

    public int GetTotalExperience()
    {
        return _currentTotalExp;
    }

    // --- SkillTree section
    public bool IsDashUnlocked()
    {
        return PlayerPrefs.GetInt("Skill_Dash", 0) == 1;
    }

    public bool UnlockDash(int cost)
    {
        int currentExp = GetTotalExperience();
        if (currentExp >= cost && !IsDashUnlocked())
        {
            // Deduct EXP
            _currentTotalExp -= cost;
            PlayerPrefs.SetInt(saveKey, _currentTotalExp);

            // Skill unlocking
            PlayerPrefs.SetInt("Skill_Dash", 1);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }
}