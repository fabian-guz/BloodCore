using System.Collections;
using UnityEngine;

public class EnemyHitFlash : MonoBehaviour
{
    public float flashTime = 0.15f;
    public Color flashColor = Color.yellow;

    private Renderer[] renderers;
    private Color[][] originalColors;
    private Coroutine flashCoroutine;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials;
            originalColors[i] = new Color[materials.Length];

            for (int j = 0; j < materials.Length; j++)
            {
                if (materials[j].HasProperty("_BaseColor"))
                {
                    originalColors[i][j] = materials[j].GetColor("_BaseColor");
                }
                else if (materials[j].HasProperty("_Color"))
                {
                    originalColors[i][j] = materials[j].color;
                }
            }
        }

        Debug.Log("EnemyHitFlash Renderer gefunden auf " + gameObject.name + ": " + renderers.Length);
    }

    public void Flash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        SetFlashColor(flashColor);

        yield return new WaitForSeconds(flashTime);

        RestoreOriginalColors();

        flashCoroutine = null;
    }

    private void SetFlashColor(Color colorToSet)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials;

            for (int j = 0; j < materials.Length; j++)
            {
                if (materials[j].HasProperty("_BaseColor"))
                {
                    materials[j].SetColor("_BaseColor", colorToSet);
                }
                else if (materials[j].HasProperty("_Color"))
                {
                    materials[j].color = colorToSet;
                }
            }
        }
    }

    private void RestoreOriginalColors()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials;

            for (int j = 0; j < materials.Length; j++)
            {
                if (materials[j].HasProperty("_BaseColor"))
                {
                    materials[j].SetColor("_BaseColor", originalColors[i][j]);
                }
                else if (materials[j].HasProperty("_Color"))
                {
                    materials[j].color = originalColors[i][j];
                }
            }
        }
    }
}