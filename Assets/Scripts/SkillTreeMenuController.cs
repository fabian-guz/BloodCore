using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SkillTreeMenuController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI expText; 

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateExpDisplay();
    }

    void Update()
    {
        // Only for testing: Deletes everything with the Button L
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("All data deleted! Restart the game.");
        }
    }

    public void UpdateExpDisplay()
    {
        if (expText != null && ExperienceManager.instance != null)
        { 
            expText.text = "EXP: " + ExperienceManager.instance.GetTotalExperience().ToString();
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}