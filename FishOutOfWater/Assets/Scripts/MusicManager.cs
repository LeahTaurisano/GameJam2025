using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MusicManager : MonoBehaviour
{
    [SerializeField, Range(0,1)] private float volume;
    bool pause = false;
    bool buffer = false;

    private void Awake()
    {
        AudioManager.instance.PlayGlobalMusic(AudioManager.instance.MusicTracks, volume);
    }
    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.P))
        {
            if(pause)
            {
                AudioManager.instance._musicSource.Play();
                pause = false;
            }
            else
            {
                AudioManager.instance._musicSource.Pause();
                pause = true;
            }

        }
        if (Input.GetKeyDown(KeyCode.K) && !buffer)
        {
            buffer = true;
            AudioManager.instance.StopAudio(AudioManager.instance._musicSource, AudioManager.instance._musicSource.clip);
            pause = false;
            StartCoroutine(SkipSong());
        }

        if(!pause && !AudioManager.instance._musicSource.isPlaying)
        {
            AudioManager.instance.PlayGlobalMusic(AudioManager.instance.MusicTracks, volume);
        }
    }
    IEnumerator SkipSong()
    {
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlayGlobalMusic(AudioManager.instance.MusicTracks, volume);
        yield return new WaitForSeconds(0.5f);
        buffer = false;
    }

}
