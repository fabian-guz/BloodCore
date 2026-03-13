using UnityEngine;
using TMPro;

public class FPSManager : MonoBehaviour
{

    private int[] frameRateOptions = { 60, 75, 90, 120, 144, 240, 360, -1 };

    [SerializeField] private TMP_Dropdown fpsDropdown;

    void Awake()
    {
        // Turn of VSync
        QualitySettings.vSyncCount = 0;

        if (fpsDropdown != null)
        {
            SetFPS(fpsDropdown.value);
        }
        else
        {
            // Fallback to 60 FPS
            Application.targetFrameRate = 60;
        }
    }

    public void SetFPS(int index)
    {
        if (index >= 0 && index < frameRateOptions.Length)
        {
            int targetFPS = frameRateOptions[index];
            Application.targetFrameRate = targetFPS;
            Debug.Log("FPS set to: " + targetFPS + " if FPS = (-1) = unlimited");
        }
    }

}
