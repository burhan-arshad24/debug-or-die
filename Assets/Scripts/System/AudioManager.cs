using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    [Header("SFX")]
    public AudioClip walkClip;
    public AudioClip pickupClip;
    public AudioClip jumpscareClip;
    public AudioClip horrorLaughClip;
    public AudioClip doorKnockClip;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip horrorMusic;

    private AudioSource sfxSource;
    private AudioSource musicSource;

    private void Awake() {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        // Footsteps should loop
        sfxSource.loop = true;
        sfxSource.clip = walkClip;
        sfxSource.volume = 0.5f;
    }

    public void PlayWalkSFX() {
        if (!sfxSource.isPlaying) sfxSource.Play();
    }

    public void StopWalkSFX() {
        if (sfxSource.isPlaying) sfxSource.Stop();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f) {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip, float volume = 1f) {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();
}
