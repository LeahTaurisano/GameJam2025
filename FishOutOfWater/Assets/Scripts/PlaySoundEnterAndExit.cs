using UnityEngine;

public class PlaySoundEnterAndExit : StateMachineBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField, Range(0, 1)] private float volume;
    [SerializeField, Range(0, 1)] private float maxVolume;
    [SerializeField, Range(0, 5)] private float scaleSpeed;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.instance.PlayScalingSoundEffect(audioClip, volume, maxVolume, scaleSpeed);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.instance._SFXSource.Stop();
    }

}
