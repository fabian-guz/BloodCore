using UnityEngine;
using TMPro;

public class FPSManager : MonoBehaviour
{
    public static FPSManager instance;

    private int[] frameRateOptions = { 60, 75, 90, 120, 144, 240, 360, -1 }; // -1 for unlimited FPS
    public const string FpsKey = "Settings_FPS";

    [SerializeField] public TMP_Dropdown fpsDropdown;
    
    // Store the current selected index to re-apply the FPS setting when the application regains focus
    private int currentSelectedIndex;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Don't destroy would destroy the FPSManager in the GameScene because it overwrites the FPSManager with the Dropdown reference
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Turn of VSync
        QualitySettings.vSyncCount = 0;

        currentSelectedIndex = PlayerPrefs.GetInt(FpsKey, 2); // Load saved FPS setting or default to first option

        if (fpsDropdown != null)
        {
            fpsDropdown.SetValueWithoutNotify(currentSelectedIndex); // Update dropdown to reflect current setting
        }
        
        ApplyFPS(currentSelectedIndex);
    }

    void Start()
    {
        if (fpsDropdown != null)
        {
            fpsDropdown.SetValueWithoutNotify(currentSelectedIndex); // Ensure dropdown reflects the current FPS setting at start
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if(hasFocus)
        {
            // Re-apply the selected FPS when the application regains focus
            ApplyFPS(currentSelectedIndex);
            
            #if UNITY_EDITOR
            Debug.Log("Focus gained: FPS set to: " + frameRateOptions[currentSelectedIndex]);
            #endif
        }
        else
        {
            Application.targetFrameRate = 15; // Reduce FPS when the application loses focus to save resources

            #if UNITY_EDITOR
            Debug.Log("Focus lost: FPS reduced to 15 to save resources");
            #endif
        }
    }

    public void SetFPS(int index)
    {
        if (index >= 0 && index < frameRateOptions.Length)
        {
            currentSelectedIndex = index; // Update the current selected index
            PlayerPrefs.SetInt(FpsKey, index); // Save the selected index to PlayerPrefs
            ApplyFPS(index);

            #if UNITY_EDITOR
            Debug.Log("FPS set to: " + frameRateOptions[index] + " if FPS = (-1) = unlimited");
            #endif
        }
    }

    public void ApplyFPS(int index)
    {
        if (index >= 0 && index < frameRateOptions.Length)
        {
            Application.targetFrameRate = frameRateOptions[index];
        }
    }

}
