using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource mainAudioSource;
    public AudioSource gunAudioSource;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip inGameMusic;
    public AudioClip bulletShot;
    public AudioClip bulletBounce;
    public AudioClip boxHit;
    public AudioClip menuSelection;

    public void PlaySound(AudioSource audioSource, AudioClip clip)
    {
        if(clip == null)
            audioSource.Play();
        else
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void PlayOneShotSound(AudioSource audioSource, AudioClip clip)
    {   
        if (clip == null)
            audioSource.PlayOneShot(audioSource.clip);
        else
        {
            audioSource.clip = clip;
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    public void ChangeSong(string song)
    {
        AudioClip clip = null;

        if (song == "menu")
            clip = menuMusic;
        else if (song == "game")
            clip = inGameMusic;

        mainAudioSource.clip = clip;
        mainAudioSource.loop = true;
        mainAudioSource.Play();
    }
}
