using UnityEngine;

public class WeaponDefinition : MonoBehaviour
{
    [Header("Weapon Info")]
    public string weaponName = "Pistol";

    [Header("Shooting")]
    public float range = 100f;
    public int damage = 1;
    public float fireRate = 0.2f;

    [Header("Shotgun / Pellet Settings")]
    public bool usePellets = false;
    public int pelletCount = 1;
    public float spreadAngle = 0f;

    [Header("Ammo")]
    public int magazineSize = 10;
    public int startingAmmoInMagazine = 10;
    public int reserveAmmo = 50;
    public int maxReserveAmmo = 100;
    public float reloadTime = 1.5f;

    [Header("Camera Recoil")]
    public float mouseRecoilPitch = 2.0f;
    public float mouseRecoilYaw = 0.35f;

    [Header("Visual References")]
    public GameObject muzzleFlash;
    public MuzzleShotLight muzzleShotLight;
    public GunRecoil gunRecoil;

    private void Awake()
    {
        if (gunRecoil == null)
        {
            gunRecoil = GetComponentInChildren<GunRecoil>(true);
        }

        if (muzzleShotLight == null)
        {
            muzzleShotLight = GetComponentInChildren<MuzzleShotLight>(true);
        }
    }
}