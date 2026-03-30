using UnityEngine;
using TMPro;

public class FPSManager : MonoBehaviour
{

    private int[] frameRateOptions = { 60, 75, 90, 120, 144, 240, 360, -1 }; // -1 for unlimited FPS
    public const string FpsKey = "Settings_FPS";

    [SerializeField] public TMP_Dropdown fpsDropdown;

    void Awake()
    {
        // Turn of VSync
        QualitySettings.vSyncCount = 0;

        if (fpsDropdown != null)
        {
            SetFPS(PlayerPrefs.GetInt(FpsKey, 2)); // Load saved FPS setting or default to first option
            fpsDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt(FpsKey, 0)); // Update dropdown to reflect current setting
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

            PlayerPrefs.SetInt(FpsKey, index);
            Debug.Log("FPS set to: " + targetFPS + " if FPS = (-1) = unlimited");
        }
    }

}
