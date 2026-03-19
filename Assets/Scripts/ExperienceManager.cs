using System.Collections.Generic;
using UnityEngine;
using System;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager instance;
	public static event Action OnSkillTreeUpdated;

    private List<string> unlockedSkillIDs = new List<string>();

    [Header("Settings")]
    public string saveKey = "PlayerTotalEXP";

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip skillUnlockSound;

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
        
        LoadProgress(); // Load progress at game start
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

    public bool TryUnlockSkill(SkillData skill)
    {
        if (GetTotalExperience() >= skill.skillCost && !IsSkillUnlocked(skill.skillID))
        {
            // Deduct EXP
            _currentTotalExp -= skill.skillCost;
            unlockedSkillIDs.Add(skill.skillID);
            
            SaveProgress(); // Direct save after unlocking
            OnSkillTreeUpdated?.Invoke(); // Inform other Buttons about a change

            if (audioSource != null && skillUnlockSound != null)
            {
                audioSource.PlayOneShot(skillUnlockSound);
            }

            return true;
        }
        return false;
    }

    public bool IsSkillUnlocked(string id)
    {
        return unlockedSkillIDs.Contains(id);
    }

    private void SaveProgress()
    {
        // List to string of unlocked skills
        string data = string.Join(",", unlockedSkillIDs);
        PlayerPrefs.SetString("UnlockedSkills", data);
        // Save total EXP
        PlayerPrefs.SetInt(saveKey, _currentTotalExp);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        // Load total EXP
        _currentTotalExp = PlayerPrefs.GetInt(saveKey, 0);
        string data = PlayerPrefs.GetString("UnlockedSkills", "");

        if (!string.IsNullOrEmpty(data))
        {
            // Load unlocked skills from string
            string[] ids = data.Split(",");
            unlockedSkillIDs = new List<string>(ids);
        }
    }
}