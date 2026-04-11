using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string weaponSelectSceneName = "WeaponSelect";
    [SerializeField] private string skillTreeSceneName = "SkillTree";

    [Header("References")]
    [SerializeField] private SettingsMenuController settingsMenuController;

    [Header("UI")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainMenuCanvas;

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (FPSManager.instance != null)
        {
            FPSManager.instance.ApplyFPS(PlayerPrefs.GetInt(FPSManager.FpsKey, 2)); // Ensure FPS setting is applied when returning to main menu
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(true);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(weaponSelectSceneName);
    }

    public void OpenSkillTree()
    {
        SceneManager.LoadScene(skillTreeSceneName);
    }

    public void OpenSettings()
    {
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(false);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }

        if (settingsMenuController != null)
        {
            settingsMenuController.RefreshUI();
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(true);
        }

        if (settingsMenuController != null)
        {
            settingsMenuController.RefreshUI();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Stopped game.");
        Application.Quit();
    }
}