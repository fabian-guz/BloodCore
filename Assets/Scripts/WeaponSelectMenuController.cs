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
    }

    [Header("UI References")]
    [SerializeField] private Image weaponImage;
    [SerializeField] private TMP_Text weaponNameText;

    [Header("Weapon Select Data")]
    [SerializeField] private WeaponSelectEntry[] weapons;

    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private int currentWeaponIndex = 0;
    private const string SelectedWeaponIndexKey = "SelectedWeaponIndex";

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentWeaponIndex = PlayerPrefs.GetInt(SelectedWeaponIndexKey, 0);

        if (weapons == null || weapons.Length == 0)
        {
            Debug.LogError("Keine Waffen im WeaponSelectMenuController eingetragen!");
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
    }
}