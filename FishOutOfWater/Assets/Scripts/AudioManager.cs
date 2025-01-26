using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{

    #region Variables
    public static AudioManager instance;

    public AudioSource _SFXSource;
    public AudioSource _musicSource;
    #endregion

    #region SoundBank
    public AudioClip[] MusicTracks;
    #endregion
    bool firstSong = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
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
    public void PlayScalingSoundEffect(AudioClip audioClip, float volume, float maxAudioScale, float scaleSpeedMult = 1f, bool pitchVariance = true)
    {
        _SFXSource.clip = audioClip;
        _SFXSource.volume = volume;

        if (pitchVariance)
        {
            float randPitch = Random.Range(0.8f, 1.2f);
            _SFXSource.pitch = randPitch;
        }

        _SFXSource.Play();

        StartCoroutine(ScalingAudio(_SFXSource, audioClip, maxAudioScale, scaleSpeedMult));

    }
    //add a way to modify the sound playing
    public void PlayGlobalMusic(AudioClip[] audioClip, float volume)
    {
        int randClip;
        do
        {
            randClip = Random.Range(0, audioClip.Length);
        }
        while (_musicSource.clip == audioClip[randClip]);


        _musicSource.clip = audioClip[randClip];
        _musicSource.volume = volume;


        if (firstSong)
        {
            _musicSource.volume = 0.002f;
            StartCoroutine(ScalingAudio(_musicSource, _musicSource.clip, volume));
            firstSong = false;
        }
        _musicSource.Play();
    }
    public void PlayGlobalMusic(AudioClip audioClip, float volume)
    {
     
        _musicSource.clip = audioClip;
        _musicSource.volume = volume;


        if (firstSong)
        {
            _musicSource.volume = 0.002f;
            StartCoroutine(ScalingAudio(_musicSource, _musicSource.clip, volume));
            firstSong = false;
        }
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
    public void PlayOneShotSoundEffect(AudioClip audioClip, float volume, Vector3 position, bool pitchVariance = true)
    {
        new Vector3(0, 0, 0);
        //change SFX to 
        AudioSource source = Instantiate(_SFXSource, position, Quaternion.identity);

        source.spatialBlend = 1;
        source.clip = audioClip;
        source.volume = volume;

        if (pitchVariance)
        {
            float randPitch = Random.Range(0.8f, 1.2f);
            source.pitch = randPitch;
        }

        source.Play();

        Destroy(source.gameObject, source.clip.length);
    }
    public void PlaySemiGlobalSoundEffect(AudioSource source, AudioClip audioClip, float volume, float globalPercent, bool pitchVariance = true)
    {
        //change SFX to 
        source.clip = audioClip;
        source.volume = volume;
        source.spatialBlend = globalPercent;
        if (pitchVariance)
        {
            float randPitch = Random.Range(0.8f, 1.2f);
            source.pitch = randPitch;
        }

        source.Play();
        StartCoroutine(ResetSpatialBlend(source));
    }

    #endregion

    #region Audio Events

    public void BigFallEvent(float fallTime)
    {

        StartCoroutine(BigFall(fallTime));
    }

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
    public void StopAudio(AudioSource source, AudioClip audioClip)
    {
        StartCoroutine(FadeAudio(source, audioClip));
    }
    public void IncreaseAudio(AudioSource Source, float audioIncease)
    {
        Source.volume += audioIncease;
    }
    public void IncreasePriority(AudioSource Source, float audioIncease)
    {
        Source.volume += audioIncease;
    }

    IEnumerator BigFall(float fallTime)
    {
        float OriginalVolume = _musicSource.volume;
        _musicSource.volume /= fallTime;
        _musicSource.priority = 250;

        yield return new WaitForSeconds(fallTime);

        StartCoroutine(ScalingAudio(_musicSource, _musicSource.clip, OriginalVolume, 0.5f));

        _musicSource.priority = 128;
        yield return null;
    }

    IEnumerator ScalingAudio(AudioSource source, AudioClip audioClip, float maxAudio, float speedMult = 1f)
    {
        while (source.volume < maxAudio)
        {
            yield return new WaitForSeconds(0.25f);
            if (source.isPlaying && source.clip == audioClip)
            {
                source.volume += (source.volume / 5) * speedMult;
            }
            else
            {
                yield break;
            }
        }
        yield return null;
    }
    IEnumerator FadeAudio(AudioSource source, AudioClip audioClip)
    {

        while (source.volume > 0.002)
        {
            yield return new WaitForSeconds(0.2f);
            if (source.isPlaying && source.clip == audioClip)
            {
                source.volume -= source.volume / 4;
            }
            else if (source.clip != audioClip)
            {
                yield break;
            }
        }
        source.Stop();
        yield return null;
    }
    IEnumerator ResetSpatialBlend(AudioSource source)
    {
        yield return new WaitForSeconds(1f);
        source.spatialBlend = 1f;
        yield return null;
    }
    #endregion
}