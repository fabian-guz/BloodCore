using UnityEngine;

public class GunEffectsController : MonoBehaviour
{
    public MuzzleShotLight muzzleShotLight;

    public GameObject bloodHitPrefab;

    public GameObject[] bloodDecals;

    public float hitDecalMinScale = 0.5f;
    public float hitDecalMaxScale = 0.8f;

    public float deathDecalMinScale = 1.0f;
    public float deathDecalMaxScale = 1.5f;

    public void PlayShotEffects()
    {
        if (muzzleShotLight != null)
        {
            muzzleShotLight.Flash();
        }
    }

    public void SpawnBloodHit(Vector3 hitPoint, Vector3 hitNormal)
    {
        if (bloodHitPrefab == null)
        {
            return;
        }

        Quaternion rotation = Quaternion.LookRotation(hitNormal);
        Instantiate(bloodHitPrefab, hitPoint + hitNormal * 0.03f, rotation);
    }

    public void SpawnGroundBloodPuddle(Vector3 enemyPosition)
    {
        if (bloodDecals == null || bloodDecals.Length == 0)
        {
            Debug.LogWarning("Keine Blood Decals gesetzt!");
            return;
        }

        Vector3 rayStart = enemyPosition + Vector3.up * 5f;

        RaycastHit[] hits = Physics.RaycastAll(rayStart, Vector3.down, 20f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Player"))
            {
                continue;
            }

            GameObject randomDecal = bloodDecals[Random.Range(0, bloodDecals.Length)];

            GameObject decal = Instantiate(randomDecal);

            decal.transform.position = hit.point + Vector3.up * 0.02f;

            decal.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            float scale = Random.Range(deathDecalMinScale, deathDecalMaxScale);

            decal.transform.localScale = new Vector3(scale, scale, scale);

            Destroy(decal, 30f);

            Debug.Log("Blood Decal auf Boden gespawnt");
            return;
        }

        Debug.LogWarning("Kein Boden unter Enemy gefunden!");
    }
}