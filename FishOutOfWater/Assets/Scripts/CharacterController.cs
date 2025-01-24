using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    //Speed we set
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [Space]
    [SerializeField] private GameObject bubbleObject;
    [SerializeField] private float bubbleSpeed;
    [SerializeField] private float bubbleMoveSpeedCap;
    [SerializeField] private float bubbleFloatSpeed;
    [SerializeField] private float bubbleDuration;
    [SerializeField] private float dashBubblePenalty;
    [Space]
    [SerializeField] private float velocityDecay;

    //Player Teleport Implement
    [SerializeField] private GameObject otherPlayer;
    private Vector3 offset = new Vector3( 0.0f, 2.0f, 0.0f );

    //Player Animation Implement
    [SerializeField] public Animator myAnimator;
    [SerializeField] public float fallingThreshold = -0.5f;
    [SerializeField] public float landingThreshold = -6.0f;


    enum PlayerState
    {
        Grounded,
        Airborne,
        Bubbled,
        Falling,
        Bouncing
    };
    private PlayerState currentState = PlayerState.Grounded;
    private bool tryJump = false;

    private bool tryDash = false;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashTimer = 0.0f;

    private bool tryBubble = false;
    private bool canBubble = true;
    private float bubbleTimer = 0.0f;

    private float moveInputX = 0.0f;
    private float moveDirX = 1.0f;

    private float xVel = 0.0f;
    private float yVel = 0.0f;

    private float gravityScale;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        gravityScale = rb.gravityScale;
    }

    #region Movement
    public void MoveInput(InputAction.CallbackContext context)
    {
        moveInputX = context.ReadValue<Vector2>().x;
        if (moveInputX < 0)
        {
            moveInputX = -1;
            sr.flipX = true;
        }
        else if (moveInputX > 0)
        {
            moveInputX = 1;
            sr.flipX = false;
        }
   
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        tryJump = context.performed;
    }    
    public void DashInput(InputAction.CallbackContext context)
    {
        tryDash = context.performed;
    }    
    public void BubbleInput(InputAction.CallbackContext context)
    {
        tryBubble = context.performed;
    }
    #endregion
    private void FixedUpdate()
    {
        if (rb.linearVelocity.y < fallingThreshold)
        {
            myAnimator.SetTrigger("Fall");
        }

        xVel = rb.linearVelocityX * velocityDecay;

        if (isDashing)
        {
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer < dashDuration)
            {
                rb.gravityScale = 0;
                return;
            }
            isDashing = false;
            rb.gravityScale = gravityScale;
            xVel /= 4.0f;

            if (CompareState(PlayerState.Grounded))
            {
                canDash = true;
            }
        }

        if (CompareState(PlayerState.Grounded))
        {
            xVel = 0.0f;
        }
        yVel = rb.linearVelocityY;
        Move();
        Jump();
        Dash();
        Bubble();
        rb.linearVelocity = new Vector2(xVel, yVel);
    }

    private void ChangeState(PlayerState state)
    {
        currentState = state;
    }

    private bool CompareState(PlayerState state)
    {
        return currentState == state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player")) && !CompareState(PlayerState.Bubbled))
        {           
            ChangeState(PlayerState.Grounded);
            canDash = true;
            canBubble = true;

            if (collision.gameObject.CompareTag("Ground"))
            {
                if (rb.linearVelocity.y < landingThreshold)
                {
                    myAnimator.SetTrigger("Land");
                }
                else
                {
                    myAnimator.SetTrigger("SoftLand");
                }
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                myAnimator.SetTrigger("SoftLand");
            }

            if (PlayerGlobals.canTeleport && PlayerGlobals.PlayerOneZone != PlayerGlobals.PlayerTwoZone)
            {
                if (this.gameObject.transform.position.y < otherPlayer.gameObject.transform.position.y)
                {
                    otherPlayer.transform.position = this.gameObject.transform.position + offset;
                    PlayerGlobals.canTeleport = false;
                }                
            }            
        }
        else if (collision.gameObject.CompareTag("BubbleBounce"))
        {
            ChangeState(PlayerState.Bouncing);
            rb.linearVelocityY = jumpForce;
        }
    }

    private void Move()
    {
        bool isMoveInput = moveInputX != 0.0f;

        if (isMoveInput)
        {
            moveDirX = moveInputX;
            if (!CompareState(PlayerState.Bubbled))
            {
                myAnimator.SetBool("Run", true);
                xVel = moveInputX * moveSpeed;
            }
            else if (CompareState(PlayerState.Bubbled))
            {
                xVel += moveInputX * bubbleSpeed;

                if (yVel < bubbleMoveSpeedCap)
                {
                    yVel += bubbleFloatSpeed * Time.fixedDeltaTime;
                }
            }
        }
        else
        {
            myAnimator.SetBool("Run", false);
        }

    }

    private void Jump()
    {
        if (tryJump && CompareState(PlayerState.Grounded))
        {
            myAnimator.SetTrigger("Jump");
            yVel = jumpForce;
            ChangeState(PlayerState.Airborne);
        }
        tryJump = false;
    }    
    private void Dash()
    {
        if (tryDash && canDash)
        {
            xVel = moveDirX * moveSpeed * dashSpeed;
            canDash = false;
            isDashing = true;
            dashTimer = 0.0f;

            if (CompareState(PlayerState.Bubbled))
            {
                bubbleTimer += dashBubblePenalty;
            }
            else
            {
                myAnimator.SetTrigger("Dash");
            }
        }
        tryDash = false;
    }

    private void Bubble()
    {
        if (CompareState(PlayerState.Bubbled))
        {
            bubbleTimer += Time.fixedDeltaTime;
            if (bubbleTimer > bubbleDuration)
            {
                bubbleObject.SetActive(false);
                ChangeState(PlayerState.Airborne);
                myAnimator.SetBool("Bubble", false);
                bubbleTimer = 0.0f;
            }
        }
        else if (tryBubble && canBubble)
        {
            bubbleObject.SetActive(true);
            ChangeState(PlayerState.Bubbled);
            myAnimator.SetBool("Bubble", true);
            canBubble = false;
        }
        tryBubble = false;
    }
}