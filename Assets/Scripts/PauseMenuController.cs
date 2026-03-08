using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;

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

        isPaused = false;
        ResumeGameState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
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

        SetPlayerScriptsEnabled(false);

        Time.timeScale = 0f;
        AudioListener.pause = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        ResumeGameState();
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

    private void ResumeGameState()
    {
        SetPlayerScriptsEnabled(true);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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