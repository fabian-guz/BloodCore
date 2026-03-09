using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Text volumeValueText;
    [SerializeField] private TMP_Text sensitivityValueText;

    [Header("References")]
    [SerializeField] private MouseLook mouseLook;

    [Header("Defaults")]
    [SerializeField] private float defaultVolume = 1f;
    [SerializeField] private float defaultSensitivity = 100f;
    [SerializeField] private bool defaultFullscreen = true;

    private const string VolumeKey = "Settings_Volume";
    private const string SensitivityKey = "Settings_Sensitivity";
    private const string FullscreenKey = "Settings_Fullscreen";

    private bool isInitialized = false;
    private bool currentFullscreenState;

    private void Start()
    {
        InitializeSettings();
    }

    public void InitializeSettings()
    {
        if (isInitialized)
        {
            return;
        }

        LoadSettings();
        SetupListeners();
        ApplyAllSettings();
        RefreshUI();

        isInitialized = true;
    }

    private void SetupListeners()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.RemoveAllListeners();
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.RemoveAllListeners();
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        }
    }

    private void LoadSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, defaultVolume);
        float savedSensitivity = PlayerPrefs.GetFloat(SensitivityKey, defaultSensitivity);
        currentFullscreenState = PlayerPrefs.GetInt(FullscreenKey, defaultFullscreen ? 1 : 0) == 1;

        if (volumeSlider != null)
        {
            volumeSlider.SetValueWithoutNotify(savedVolume);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.SetValueWithoutNotify(savedSensitivity);
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.SetIsOnWithoutNotify(currentFullscreenState);
        }
    }

    private void ApplyAllSettings()
    {
        if (volumeSlider != null)
        {
            ApplyVolume(volumeSlider.value);
        }

        if (sensitivitySlider != null)
        {
            ApplySensitivity(sensitivitySlider.value);
        }

        ApplyFullscreen(currentFullscreenState);
    }

    public void OnVolumeChanged(float value)
    {
        ApplyVolume(value);
        PlayerPrefs.SetFloat(VolumeKey, value);
        PlayerPrefs.Save();
        UpdateValueTexts();
    }

    public void OnSensitivityChanged(float value)
    {
        ApplySensitivity(value);
        PlayerPrefs.SetFloat(SensitivityKey, value);
        PlayerPrefs.Save();
        UpdateValueTexts();
    }

    public void OnFullscreenChanged(bool isFullscreen)
    {
        SetFullscreenState(isFullscreen);
    }

    public void ToggleFullscreen()
    {
        SetFullscreenState(!currentFullscreenState);
    }

    private void SetFullscreenState(bool isFullscreen)
    {
        currentFullscreenState = isFullscreen;

        ApplyFullscreen(currentFullscreenState);

        PlayerPrefs.SetInt(FullscreenKey, currentFullscreenState ? 1 : 0);
        PlayerPrefs.Save();

        RefreshUI();
    }

    private void ApplyVolume(float value)
    {
        AudioListener.volume = Mathf.Clamp01(value);
    }

    private void ApplySensitivity(float value)
    {
        if (mouseLook != null)
        {
            mouseLook.SetSensitivity(value);
        }
    }

    private void ApplyFullscreen(bool isFullscreen)
    {
        if (isFullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void ResetSettings()
    {
        if (volumeSlider != null)
        {
            volumeSlider.SetValueWithoutNotify(defaultVolume);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.SetValueWithoutNotify(defaultSensitivity);
        }

        currentFullscreenState = defaultFullscreen;

        ApplyVolume(defaultVolume);
        ApplySensitivity(defaultSensitivity);
        ApplyFullscreen(currentFullscreenState);

        PlayerPrefs.SetFloat(VolumeKey, defaultVolume);
        PlayerPrefs.SetFloat(SensitivityKey, defaultSensitivity);
        PlayerPrefs.SetInt(FullscreenKey, currentFullscreenState ? 1 : 0);
        PlayerPrefs.Save();

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (fullscreenToggle != null)
        {
            fullscreenToggle.SetIsOnWithoutNotify(currentFullscreenState);
        }

        UpdateValueTexts();
    }

    private void UpdateValueTexts()
    {
        if (volumeValueText != null && volumeSlider != null)
        {
            volumeValueText.text = Mathf.RoundToInt(volumeSlider.value * 100f) + "%";
        }

        if (sensitivityValueText != null && sensitivitySlider != null)
        {
            sensitivityValueText.text = Mathf.RoundToInt(sensitivitySlider.value).ToString();
        }
    }
}