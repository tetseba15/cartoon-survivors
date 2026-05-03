using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    // Dictionary for audio culling
    private Dictionary<AudioClip, float> lastTimePlayed = new Dictionary<AudioClip, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Plays a standar SFX like UI, arrow shoots, lvl up.
    /// </summary>
    public void PlaySound(AudioClip clip, float volume = 1f, bool randomizePitch = true)
    {
        if (clip == null) return;

        // Tone variations
        sfxSource.pitch = randomizePitch ? Random.Range(0.85f, 1.15f) : 1f;
        sfxSource.PlayOneShot(clip, volume);
    }

    /// <summary>
    /// Plays sounds that happends many times in a second like hordes or xp gems
    /// Timer to avoid too much sounds
    /// </summary>
    public void PlaySpammedSound(AudioClip clip, float volume = 1f, float cooldown = 0.05f)
    {
        if (clip == null) return;

        if (lastTimePlayed.TryGetValue(clip, out float lastTime))
        {
            if (Time.time < lastTime + cooldown)
            {
                return; 
            }
        }

        
        lastTimePlayed[clip] = Time.time;
        PlaySound(clip, volume, true);
    }
}