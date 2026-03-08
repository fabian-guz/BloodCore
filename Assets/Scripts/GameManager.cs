using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;

    public AudioClip music;
    public AudioClip gameOverSound;

    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (music != null)
        {
            audioSource.clip = music;
            audioSource.loop = true;
            audioSource.Play();
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateScore(score);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateScore(score);
        }
    }

    public void PlayGameOverSound()
    {
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}