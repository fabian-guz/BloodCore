using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string weaponSelectSceneName = "WeaponSelect";

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(weaponSelectSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Spiel wird beendet.");
        Application.Quit();
    }
}