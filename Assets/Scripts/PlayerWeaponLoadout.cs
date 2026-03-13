using UnityEngine;

public class PlayerWeaponLoadout : MonoBehaviour
{
    [Header("Weapon References")]
    [SerializeField] private WeaponDefinition[] weapons;

    [Header("References")]
    [SerializeField] private GunShoot gunShoot;

    private const string SelectedWeaponIndexKey = "SelectedWeaponIndex";

    private void Start()
    {
        ApplySelectedWeapon();
    }

    public void ApplySelectedWeapon()
    {
        if (weapons == null || weapons.Length == 0)
        {
            Debug.LogError("Keine Waffen im PlayerWeaponLoadout eingetragen!");
            return;
        }

        int selectedIndex = PlayerPrefs.GetInt(SelectedWeaponIndexKey, 0);

        if (selectedIndex < 0 || selectedIndex >= weapons.Length)
        {
            selectedIndex = 0;
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].gameObject.SetActive(i == selectedIndex);
            }
        }

        if (gunShoot == null)
        {
            Debug.LogError("GunShoot Referenz fehlt im PlayerWeaponLoadout!");
            return;
        }

        WeaponDefinition selectedWeapon = weapons[selectedIndex];

        if (selectedWeapon == null)
        {
            Debug.LogError("Die ausgewählte WeaponDefinition ist leer!");
            return;
        }

        gunShoot.ApplyWeaponDefinition(selectedWeapon);
    }
}