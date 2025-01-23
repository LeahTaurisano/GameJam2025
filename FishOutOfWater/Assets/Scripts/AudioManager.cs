using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    #region Variables
    public static AudioManager instance;

    [SerializeField] private AudioSource _SFXSource;
    [SerializeField] private AudioSource _musicSource;

    [SerializeField] private AudioClip _testAudioClip;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #region Play Audio Methods
    //plays just one sound effect
    public void PlayGlobalSoundEffect(AudioClip audioClip, float volume, bool pitchVariance = true)
    {

        _SFXSource.clip = audioClip;
        _SFXSource.volume = volume;

        if (pitchVariance)
        {
            float rand = Random.Range(0.8f, 1.2f);
            _SFXSource.pitch = rand;
        }

        _SFXSource.Play();


    }
    //add a way to modify the sound playing
    public void PlayGlobalMusic(AudioClip audioClip, float volume, bool looping = true)
    {

        _musicSource.clip = audioClip;
        _musicSource.volume = volume;
        _musicSource.loop = looping;

        _musicSource.Play();
    }

    public void PlaySoundEffect(AudioSource source, AudioClip audioClip, float volume, bool pitchVariance = true)
    {
        //change SFX to 
        source.clip = audioClip;
        source.volume = volume;

        if (pitchVariance)
        {
            float randPitch = Random.Range(0.8f, 1.2f);
            source.pitch = randPitch;
        }

        source.Play();
    }
    public void PlaySoundEffect(AudioSource source, AudioClip[] audioClip, float volume, bool pitchVariance = true)
    {
        int randClip = Random.Range(0, audioClip.Length);

        source.clip = audioClip[randClip];
        source.volume = volume;

        if (pitchVariance)
        {
            float randPitch = Random.Range(0.8f, 1.2f);
            source.pitch = randPitch;
        }
        source.Play();
    }
    public void PlayTestSoundEffect(AudioClip audioClip, bool pitchVariance = true)
    {
        _SFXSource.clip = audioClip;
        _SFXSource.volume = 0.5f;

        if (pitchVariance)
        {
            float randPitch = Random.Range(0.8f, 1.2f);
            _SFXSource.pitch = randPitch;
        }
        _SFXSource.Play();
    }
    #endregion

    #region Audio Events
    //like big fall
    /*
     * Needs to dim the music
     * grab all player audio sources
     * enhance them
     * wait an amount of time based on time/distance fallen
     * resume/fade music and sound fx back to normal
     * */
    //
    #endregion


    #region Audio Editing
    public void StopAudio(AudioSource Source)
    {
        Source.Stop();
    }
    public void IncreaseAudio(AudioSource Source, float audioIncease)
    {
        Source.volume += audioIncease;
    }
    public void IncreasePriority(AudioSource Source, float audioIncease)
    {
        Source.volume += audioIncease;
    }
    #endregion
}
