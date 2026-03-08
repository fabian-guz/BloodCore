using TMPro;
using UnityEngine;

public class MainMenuHighscoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private string playerPrefsKey = "Highscore";
    [SerializeField] private string prefix = "HIGHSCORE: ";

    private void Start()
    {
        UpdateHighscoreText();
    }

    private void OnEnable()
    {
        UpdateHighscoreText();
    }

    private void UpdateHighscoreText()
    {
        if (highscoreText == null)
        {
            return;
        }

        int highscore = PlayerPrefs.GetInt(playerPrefsKey, 0);
        highscoreText.text = prefix + highscore.ToString();
    }
}