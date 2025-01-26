using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    [SerializeField] private GameObject BubbleTransition;
    [SerializeField] private AudioClip BubbleClip;
    [SerializeField] private float DelayedLoadTime;
    [SerializeField] private float volume;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            BubbleTransition.gameObject.SetActive(true);
            AudioManager.instance.PlayGlobalSoundEffect(BubbleClip, volume, false);
            StartCoroutine(DelayedLoad(DelayedLoadTime));
        }
    }
    IEnumerator DelayedLoad(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(2);
    }
}
