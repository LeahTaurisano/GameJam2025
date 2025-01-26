using NUnit.Framework.Internal;
using UnityEngine;

public class AudioPlayTest : MonoBehaviour
{
    [SerializeField] private AudioClip walk;
    [SerializeField] private AudioClip walkLong;
    [SerializeField] private AudioClip bounce;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {


        ////input keycode is just for testing
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Debug.Log("play sound");
        //    //AudioManager.instance.PlayGlobalSoundEffect(walk, 0f);
        //    AudioManager.instance.PlaySoundEffect(audioSource, new AudioClip[] { walk, bounce}, 0.5f);

        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    Debug.Log("play music");
        //    AudioManager.instance.PlayGlobalMusic(walk, 0.5f, true);
        //}

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Debug.Log("play long effect");
        //    AudioManager.instance.PlayGlobalSoundEffect(walkLong, 0.4f, true);
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Debug.Log("play long effect");
        //    AudioManager.instance.StopAudio(audioSource);
        //}

    
    }
}
