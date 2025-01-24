using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    public Animator myAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void SetJump()
    {
        myAnimator.SetTrigger("Jump");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
