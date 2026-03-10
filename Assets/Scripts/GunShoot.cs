using UnityEngine;
using System.Collections;

public class GunShoot : MonoBehaviour
{
    [Header("Shooting")]
    public float range = 100f;
    public int damage = 1;
    public float fireRate = 0.2f;
    public GameObject muzzleFlash;
    public MuzzleShotLight muzzleShotLight;

    [Header("Ammo")]
    public int magazineSize = 10;
    public int currentAmmo = 10;
    public int reserveAmmo = 50;
    public int maxReserveAmmo = 100;
    public float reloadTime = 1.5f;

    [Header("Audio")]
    public AudioClip gunShotSound;
    public AudioClip reloadSound;
    public AudioClip emptyMagazineSound;

    [Header("References")]
    public MouseLook mouseLook;
    public GunRecoil gunRecoil;
    public GunEffectsController gunEffectsController;
    public CameraShake cameraShake;

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

    void Shoot()
    {
        cameraShake.Shake(0.08f, 0.05f);
        currentAmmo--;

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateAmmo(currentAmmo, reserveAmmo);
        }

        if (audioSource != null && gunShotSound != null)
        {
            audioSource.PlayOneShot(gunShotSound);
        }

        if (gunRecoil == null)
        {
            Debug.LogError("GunRecoil Referenz fehlt in GunShoot!");
        }
        else
        {
            gunRecoil.Fire();
        }

        if (mouseLook != null)
        {
            mouseLook.AddRecoil(2.0f, 0.35f);
        }

        if (gunEffectsController != null)
        {
            gunEffectsController.PlayShotEffects();
        }

        if (muzzleFlash  != null)
        {
            StartCoroutine(ShowMuzzleFlash());
        }

        if (muzzleShotLight != null)
        {
            muzzleShotLight.Flash();
        }

        Ray ray;

        if (Camera.main != null)
        {
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }
        else
        {
            ray = new Ray(transform.position, transform.forward);
        }

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
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

        if (currentAmmo <= 0 && reserveAmmo > 0)
        {
            StartReload();
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