using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerSnapshot fadeInSnapshot;
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume"));
        FadeInMusic();
    }
    
    private void FadeInMusic()
    {
        fadeInSnapshot.TransitionTo(0.7f);
        audioSource.outputAudioMixerGroup = musicMixer;
        audioSource.Play();
    }
}
