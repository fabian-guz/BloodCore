using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("References")]
    [SerializeField] private SettingsMenuController settingsMenuController;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Player References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private GunShoot gunShoot;
    [SerializeField] private WeaponBobbing weaponBobbing;
    [SerializeField] private GunRecoil gunRecoil;

    private bool isPaused;

    private void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        isPaused = false;

        if (settingsMenuController != null)
        {
            settingsMenuController.InitializeSettings();
        }

        ResumeGameState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            ToggleFullscreen();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if (settingsPanel != null && settingsPanel.activeSelf)
                {
                    CloseSettings();
                }
                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        SetPlayerScriptsEnabled(false);

        Time.timeScale = 0f;
        AudioListener.pause = true;

        if (mouseLook != null)
        {
            mouseLook.SetCursorLocked(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (settingsMenuController != null)
        {
            settingsMenuController.RefreshUI();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        ResumeGameState();
    }

    public void OpenSettings()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
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

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        if (settingsMenuController != null)
        {
            settingsMenuController.RefreshUI();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ToggleFullscreen()
    {
        if (settingsMenuController != null)
        {
            settingsMenuController.ToggleFullscreen();
        }
    }

    private void ResumeGameState()
    {
        SetPlayerScriptsEnabled(true);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (mouseLook != null)
        {
            mouseLook.SetCursorLocked(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (settingsMenuController != null)
        {
            settingsMenuController.RefreshUI();
        }
    }

    private void SetPlayerScriptsEnabled(bool enabledState)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = enabledState;
        }

        if (mouseLook != null)
        {
            mouseLook.enabled = enabledState;
        }

        if (gunShoot != null)
        {
            gunShoot.enabled = enabledState;
        }

        if (weaponBobbing != null)
        {
            weaponBobbing.enabled = enabledState;
        }

        if (gunRecoil != null)
        {
            gunRecoil.enabled = enabledState;
        }
    }
}