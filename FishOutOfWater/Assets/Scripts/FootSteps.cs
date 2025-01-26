using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField] AudioClip walk;

    [SerializeField, Range(0, 1)] float volume = 0.005f;

    [SerializeField] AudioSource audioSource;

    public void PlayWalk()
    {
        AudioManager.instance.PlaySoundEffect(audioSource, walk, volume);
    }
}
