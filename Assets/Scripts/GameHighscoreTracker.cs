using TMPro;
using UnityEngine;

public class GameHighscoreTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string playerPrefsKey = "Highscore";
    [SerializeField] private bool resetHighscoreOnStart = false;

    private int currentScore;
    private int savedHighscore;

    private void Start()
    {
        if (resetHighscoreOnStart)
        {
            PlayerPrefs.DeleteKey(playerPrefsKey);
            PlayerPrefs.Save();
            Debug.Log("Highscore wurde zurückgesetzt.");
        }

        savedHighscore = PlayerPrefs.GetInt(playerPrefsKey, 0);
        UpdateHighscoreFromText();
    }

    private void Update()
    {
        UpdateHighscoreFromText();
    }

    private void UpdateHighscoreFromText()
    {
        if (scoreText == null)
        {
            return;
        }

        string scoreString = scoreText.text;
        int parsedScore = ExtractNumber(scoreString);

        if (parsedScore != currentScore)
        {
            currentScore = parsedScore;

            if (currentScore > savedHighscore)
            {
                savedHighscore = currentScore;
                PlayerPrefs.SetInt(playerPrefsKey, savedHighscore);
                PlayerPrefs.Save();
            }
        }
    }

    private int ExtractNumber(string text)
    {
        string numberOnly = "";

        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsDigit(text[i]))
            {
                numberOnly += text[i];
            }
        }

        if (string.IsNullOrEmpty(numberOnly))
        {
            return 0;
        }

        int result;
        if (int.TryParse(numberOnly, out result))
        {
            return result;
        }

        return 0;
    }
}