using UnityEngine;
using System.Collections;

public class GunShoot : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip gunShotSound;
    public AudioClip reloadSound;
    public AudioClip emptyMagazineSound;

    [Header("References")]
    public MouseLook mouseLook;
    public GunEffectsController gunEffectsController;
    public CameraShake cameraShake;

    [Header("Current Weapon Runtime Data")]
    public float range = 100f;
    public int damage = 1;
    public float fireRate = 0.2f;

    public bool usePellets = false;
    public int pelletCount = 1;
    public float spreadAngle = 0f;

    public int magazineSize = 10;
    public int currentAmmo = 10;
    public int reserveAmmo = 50;
    public int maxReserveAmmo = 100;
    public float reloadTime = 1.5f;

    public float mouseRecoilPitch = 2.0f;
    public float mouseRecoilYaw = 0.35f;

    public GameObject muzzleFlash;
    public MuzzleShotLight muzzleShotLight;
    public GunRecoil gunRecoil;

    private WeaponDefinition activeWeapon;
    private float nextTimeToFire = 0f;
    private AudioSource audioSource;
    private bool isReloading = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateAmmo(currentAmmo, reserveAmmo);
        }
    }

    void Update()
    {
        if (activeWeapon == null)
        {
            return;
        }

        if (isReloading)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
            return;
        }

        if (currentAmmo <= 0)
        {
            if (reserveAmmo > 0)
            {
                StartReload();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                PlayEmptyMagazineSound();
            }

            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
    }

    public void ApplyWeaponDefinition(WeaponDefinition weaponDefinition)
    {
        if (weaponDefinition == null)
        {
            Debug.LogError("ApplyWeaponDefinition hat eine leere WeaponDefinition bekommen!");
            return;
        }

        activeWeapon = weaponDefinition;

        range = activeWeapon.range;
        damage = activeWeapon.damage;
        fireRate = activeWeapon.fireRate;

        usePellets = activeWeapon.usePellets;
        pelletCount = Mathf.Max(1, activeWeapon.pelletCount);
        spreadAngle = activeWeapon.spreadAngle;

        magazineSize = activeWeapon.magazineSize;
        currentAmmo = activeWeapon.startingAmmoInMagazine;
        reserveAmmo = activeWeapon.reserveAmmo;
        maxReserveAmmo = activeWeapon.maxReserveAmmo;
        reloadTime = activeWeapon.reloadTime;

        mouseRecoilPitch = activeWeapon.mouseRecoilPitch;
        mouseRecoilYaw = activeWeapon.mouseRecoilYaw;

        muzzleFlash = activeWeapon.muzzleFlash;
        muzzleShotLight = activeWeapon.muzzleShotLight;
        gunRecoil = activeWeapon.gunRecoil;

        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }

        if (gunRecoil != null)
        {
            gunRecoil.ResetRecoil();
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateAmmo(currentAmmo, reserveAmmo);
        }
    }

    void Shoot()
    {
        if (cameraShake != null)
        {
            cameraShake.Shake(0.08f, 0.05f);
        }

        currentAmmo--;

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateAmmo(currentAmmo, reserveAmmo);
        }

        if (audioSource != null && gunShotSound != null)
        {
            audioSource.PlayOneShot(gunShotSound);
        }

        if (gunRecoil != null)
        {
            gunRecoil.Fire();
        }

        if (mouseLook != null)
        {
            mouseLook.AddRecoil(mouseRecoilPitch, mouseRecoilYaw);
        }

        if (muzzleFlash != null)
        {
            StartCoroutine(ShowMuzzleFlash());
        }

        if (muzzleShotLight != null)
        {
            muzzleShotLight.Flash();
        }

        if (usePellets)
        {
            ShootPellets();
        }
        else
        {
            ShootSingleRay();
        }

        if (currentAmmo <= 0 && reserveAmmo > 0)
        {
            StartReload();
        }
    }

    void ShootSingleRay()
    {
        Ray ray = CreateBaseRay();

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            HandleHit(hit);
        }
    }

    void ShootPellets()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            Ray ray = CreateSpreadRay();
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range))
            {
                HandleHit(hit);
            }
        }
    }

    Ray CreateBaseRay()
    {
        if (Camera.main != null)
        {
            return new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }

        return new Ray(transform.position, transform.forward);
    }

    Ray CreateSpreadRay()
    {
        Transform rayOriginTransform = Camera.main != null ? Camera.main.transform : transform;

        Vector3 forward = rayOriginTransform.forward;
        Vector3 right = rayOriginTransform.right;
        Vector3 up = rayOriginTransform.up;

        float horizontalSpread = Random.Range(-spreadAngle, spreadAngle);
        float verticalSpread = Random.Range(-spreadAngle, spreadAngle);

        Vector3 spreadDirection = forward + right * horizontalSpread + up * verticalSpread;
        spreadDirection.Normalize();

        return new Ray(rayOriginTransform.position, spreadDirection);
    }

    void HandleHit(RaycastHit hit)
    {
        EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();

        if (enemy == null)
        {
            enemy = hit.transform.GetComponentInParent<EnemyHealth>();
        }

        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            if (UIManager.instance != null)
            {
                UIManager.instance.ShowHitMarker();
            }

            if (gunEffectsController != null)
            {
                gunEffectsController.SpawnBloodHit(hit.point, hit.normal);
            }
        }
    }

    void StartReload()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo == magazineSize)
        {
            return;
        }

        if (reserveAmmo <= 0)
        {
            return;
        }

        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        isReloading = true;

        if (UIManager.instance != null)
        {
            UIManager.instance.ShowReloadText();
        }

        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime);

        int neededAmmo = magazineSize - currentAmmo;
        int ammoToLoad = Mathf.Min(neededAmmo, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateAmmo(currentAmmo, reserveAmmo);
            UIManager.instance.HideReloadText();
        }

        isReloading = false;
    }

    void PlayEmptyMagazineSound()
    {
        if (audioSource != null && emptyMagazineSound != null)
        {
            audioSource.PlayOneShot(emptyMagazineSound);
        }
    }

    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;

        if (reserveAmmo > maxReserveAmmo)
        {
            reserveAmmo = maxReserveAmmo;
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateAmmo(currentAmmo, reserveAmmo);
        }
    }
}