using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayTimeTracker : MonoBehaviour
{
    private const string PlayTimeKey = "TotalPlayTimeSeconds";

    [Header("UI Elements")]
    [SerializeField] private TMP_Text playTimeLabel;
    [SerializeField] private string labelPrefix = "Playtime: ";
    [SerializeField] private string labelGameObjectName = "PlayTimeLabel";

    // Total playtime in seconds
    private float _savedSeconds;

    // Seconds accumulated during the current session
    private float _sessionSeconds;

    // Set to false if you don't want to track playtime while in the main menu
    [Header("Settings")]
    [SerializeField] public bool trackInMainMenu = true;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string weaponSelectSceneName = "WeaponSelect";
    [SerializeField] private string skillTreeSceneName = "SkillTree";

    private static PlayTimeTracker _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _savedSeconds = PlayerPrefs.GetFloat(PlayTimeKey, 0f);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {   
        //Search label in the new loaded scene
        GameObject labelObj = GameObject.Find(labelGameObjectName);
        if (labelObj != null) 
        {
            playTimeLabel = labelObj.GetComponent<TMP_Text>();
        }
        
        UpdateDisplay();
    }

    void Start()
    {
        UpdateDisplay();
        #if UNITY_EDITOR
        Debug.Log($"Loaded playtime: {_savedSeconds} seconds");
        #endif
    }

    void Update()
    {
        bool isMainMenu = SceneManager.GetActiveScene().name == mainMenuSceneName;
        bool isWeaponSelect = SceneManager.GetActiveScene().name == weaponSelectSceneName;
        bool isSkillTree = SceneManager.GetActiveScene().name == skillTreeSceneName;

        if ((isMainMenu && !trackInMainMenu) || (isWeaponSelect && !trackInMainMenu) || (isSkillTree && !trackInMainMenu))
        {
            return; // Don't track time in main menu (if disabled) or weapon select screen
        }

        _sessionSeconds += Time.deltaTime;
        UpdateDisplay();
    }

    private void OnDisable() {
        // OnApplicationQuit is not always called (e.g. when exiting play mode in the editor), so also save playtime here
        SavePlayTime();
    }

    private void OnApplicationQuit()
    {
        SavePlayTime();
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            SavePlayTime();
        }
    }

    // Call this from other scripts (e.g. MainMenuManager)
    public void SavePlayTime()
    {
        float total = _savedSeconds + _sessionSeconds;
        PlayerPrefs.SetFloat(PlayTimeKey, total);
        PlayerPrefs.Save();

        // Update saved seconds and reset session seconds
        _savedSeconds = total;
        _sessionSeconds = 0f;
    }

    private void UpdateDisplay()
    {
        if (playTimeLabel == null) return;
        playTimeLabel.text = labelPrefix + FormatTime(_savedSeconds + _sessionSeconds);
    }

    private static string FormatTime(float totalSeconds)
    {
        int totalMinutes = Mathf.FloorToInt(totalSeconds / 60f);
        int hours        = totalMinutes / 60;
        int minutes      = totalMinutes % 60;
        if (hours < 1)
        {
            return $"{minutes}m";
        }

        return $"{hours}h {minutes}m";
    }
}
