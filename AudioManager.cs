using UnityEngine;


public class AudioManager : MonoBehaviour
{
    // Static Fields
    public static AudioManager Instance { get; private set; }

    // Editor Fields

    [Header("Audio Sources")]
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Effects Settings")]
    [Range(0, 1)][SerializeField] private float effectsVolume = 0.2f;
    [Range(0, 1)][SerializeField] private float musicVolume = 0.2f;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip laserClip;
    [SerializeField] private AudioClip thrusterClip;

    public float MusicVolume
    {
        get => musicVolume;
        set => musicVolume =  SetMusicVolume(value);
    }

    public float EffectsVolume
    {
        get => effectsVolume;
        set => effectsVolume = SetEffectsVolume(value);
    }

    // Unity Events
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        effectsSource.volume = effectsVolume;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        MusicVolume = musicVolume;
    }
    
    private float SetEffectsVolume(float volume)
    {
        effectsSource.volume = volume > 1 ? 1 : volume < 0 ? 0 : volume;
        return effectsSource.volume;
    }

    private float SetMusicVolume(float volume)
    {
        musicSource.volume = volume > 1 ? 1 : volume < 0 ? 0 : volume;
        return musicSource.volume;
    }
    
    // External Functions

    public void PlayExplosionSfx()
    {
        effectsSource.PlayOneShot(explosionClip);
    }

    public void PlayLaserSfx()
    {
        effectsSource.PlayOneShot(laserClip);
    }

    public void PlayThrusterSfx()
    {
        effectsSource.PlayOneShot(thrusterClip);
    }

}
