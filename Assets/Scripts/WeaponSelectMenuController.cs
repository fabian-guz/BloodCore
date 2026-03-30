using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WeaponSelectMenuController : MonoBehaviour
{
    [System.Serializable]
    public class WeaponSelectEntry
    {
        public string weaponName;
        public Sprite weaponIcon;
        public int unlockHighscoreRequirement;
    }

    [Header("UI References")]
    [SerializeField] private Image weaponImage;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Image lockImage;

    [Header("Weapon Select Data")]
    [SerializeField] public WeaponSelectEntry[] weapons;

    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private int currentWeaponIndex = 0;
    private const string SelectedWeaponIndexKey = "SelectedWeaponIndex";
    private const string playerHighscoreKey = "Highscore";

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentWeaponIndex = PlayerPrefs.GetInt(SelectedWeaponIndexKey, 0);

        if (weapons == null || weapons.Length == 0)
        {
            Debug.LogError("No weapons assigned in the WeaponSelectMenuController.");
            return;
        }

        if (currentWeaponIndex < 0 || currentWeaponIndex >= weapons.Length)
        {
            currentWeaponIndex = 0;
        }

        UpdateWeaponUI();
    }

    public void NextWeapon()
    {
        if (weapons == null || weapons.Length == 0)
        {
            return;
        }

        currentWeaponIndex++;

        if (currentWeaponIndex >= weapons.Length)
        {
            currentWeaponIndex = 0;
        }

        UpdateWeaponUI();
    }

    public void PreviousWeapon()
    {
        if (weapons == null || weapons.Length == 0)
        {
            return;
        }

        currentWeaponIndex--;

        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = weapons.Length - 1;
        }

        UpdateWeaponUI();
    }

    public void ConfirmWeapon()
    {
        PlayerPrefs.SetInt(SelectedWeaponIndexKey, currentWeaponIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void UpdateWeaponUI()
    {
        WeaponSelectEntry currentWeapon = weapons[currentWeaponIndex];

        if (weaponNameText != null)
        {
            weaponNameText.text = currentWeapon.weaponName;
        }

        if (weaponImage != null)
        {
            weaponImage.sprite = currentWeapon.weaponIcon;
            weaponImage.enabled = currentWeapon.weaponIcon != null;
        }

        if (currentWeapon.unlockHighscoreRequirement > PlayerPrefs.GetInt(playerHighscoreKey, 0))
        {
            if (confirmButton != null)
            {
                confirmButton.interactable = false;
            }

            if (weaponNameText != null)
            {
                weaponNameText.text += " (LOCKED)\nScore required: " + currentWeapon.unlockHighscoreRequirement.ToString();
            }

            if (lockImage != null)
            {
                lockImage.enabled = true;
            }
        }
        else
        {
            if (confirmButton != null)
            {
                confirmButton.interactable = true;
            }

            if (weaponNameText != null)
            {
                weaponNameText.text = currentWeapon.weaponName;
            }
            if (lockImage != null)
            {
                lockImage.enabled = false;
            }
        }
    }
}