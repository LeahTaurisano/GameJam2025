using UnityEngine;

public class PlaySoundOnEnter : StateMachineBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField, Range(0, 1)] private float volume;

    //% of audio that is globaly heard, 0 = Fully global, 1 = Only heard when near sound    (bad var name, cause its reversed)
    [SerializeField, Range(0, 1)] private float globalPercent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.instance.PlaySemiGlobalSoundEffect(animator.gameObject.GetComponent<AudioSource>(), audioClip, volume, globalPercent);
    }

}
