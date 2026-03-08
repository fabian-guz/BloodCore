using UnityEngine;

public class AudioHelper
{
    public static void PlayClipAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null)
        {
            return;
        }

        GameObject soundObject = new GameObject("TempAudio");
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;

        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 2f;
        audioSource.maxDistance = 20f;

        audioSource.Play();

        Object.Destroy(soundObject, clip.length);
    }
}