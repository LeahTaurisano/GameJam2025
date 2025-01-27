using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private int playerNumber;
    [Space]
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
    private Vector3 offset = new Vector3(0.0f, 2.0f, 0.0f);

    //Player Animation Implement
    [SerializeField] public Animator myAnimator;
    [SerializeField] public float fallingThreshold = -0.5f;
    [SerializeField] public float landingThreshold = -6.0f;

    //Bubble Animation Implement
    [SerializeField] public float bubbleAnimationSpeedMult = 1.0f;
    [Space]

    //Interaction Cooldown
    [SerializeField] private float interactCooldown = 1.0f;

    //Control Screen
    [SerializeField] public GameObject menu;
    [Space]
    //SFX the player can play
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip bubbleForming;
    [SerializeField] private AudioClip bubblePop;
    [SerializeField] private AudioClip bubbleBounce;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip fishDash;
    [SerializeField] private AudioClip bubbleDash;
    [SerializeField] private AudioClip leverFlick;
    public enum PlayerState
    {
        Grounded,
        Airborne,
        Bubbled,
        Falling,
        Bouncing
    };
    public PlayerState currentState = PlayerState.Grounded;
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

    private bool tryInteract = false;
    private bool nearLever = false;
    private GameObject currentLever;

    private float gravityScale;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator bubbleAnimator;
    private float interactTimer = 0;

    private bool playerCanTeleport = false;
    private PlayerController playerController;

    private bool canFlipMenu = true;

    private void Awake()
    {
        GameObject controls = GameObject.FindGameObjectWithTag("Controls");
        PlayerInput pi = GetComponent<PlayerInput>();
        if (controls == null)
        {
            pi.SwitchCurrentControlScheme(Keyboard.current, Mouse.current); //Keyboard if launched outside main menu screen
            return;
        }
        ControlScheme cs = controls.GetComponent<ControlScheme>();
        if (cs.UsingKeyboardControls(playerNumber))
        {
            if (playerNumber == 1)
            {
                pi.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current); //Keyboard if keyboard selected at main screen
            }
            else
            {
                pi.SwitchCurrentControlScheme("ArrowKeys", Keyboard.current);
            }
            return;
        }
        if (Gamepad.all.Count >= cs.NumPlayersUsingGamepad())
        {
            if (cs.NumPlayersUsingGamepad() == 1)
            {
                pi.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]); //Give only controller to only player who picked controller
            }
            else
            {
                pi.SwitchCurrentControlScheme("Gamepad", Gamepad.all[playerNumber - 1]); //Give respective controller to each player
            }
            return;
        }
        else
        {
            if (playerNumber == 1)
            {
                pi.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]); //If both pick controller but only 1 is plugged in, give player 2 keyboard controls
                return;
            }
        }
        pi.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        gravityScale = rb.gravityScale;
        bubbleAnimator = bubbleObject.GetComponent<Animator>();
        interactTimer = interactCooldown;
        playerController = otherPlayer.GetComponent<PlayerController>();
    }

    #region Movement
    public void MoveInput(InputAction.CallbackContext context)
    {
        moveInputX = context.ReadValue<Vector2>().x;

        if (moveInputX < 0)
        {
            if (moveInputX > -0.5f)
            {
                moveInputX = 0.0f;
                return;
            }
            moveInputX = -1;
            sr.flipX = true;
        }
        else if (moveInputX > 0)
        {
            if (moveInputX < 0.5f)
            {
                moveInputX = 0.0f;
                return;
            }
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
    public void InteractInput(InputAction.CallbackContext context)
    {
        tryInteract = context.performed;

    }
    #endregion
    private void FixedUpdate()
    {
        if (rb.linearVelocity.y < fallingThreshold && CompareState(PlayerState.Airborne) && !CompareState(PlayerState.Bubbled))
        {
            myAnimator.SetBool("Fall", true);
            if (rb.linearVelocity.y < landingThreshold)
            {

                myAnimator.SetBool("Land", true);
            }
        }
        else
        {
            if (rb.linearVelocity.y >= 0.001f)
            {
                myAnimator.SetBool("Land", false);
            }
            myAnimator.SetBool("Fall", false);
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
        if (interactTimer > 0)
        {
            interactTimer -= Time.fixedDeltaTime;
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
        Interact();
        if (this.gameObject.transform.position.y < (otherPlayer.gameObject.transform.position.y - 20))
        {
            playerCanTeleport = true;
        }
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
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player") && playerController.currentState == PlayerState.Grounded) && !CompareState(PlayerState.Bubbled))
        {
            ChangeState(PlayerState.Grounded);
            canDash = true;
            canBubble = true;



            if (playerCanTeleport)
            {
                if (this.gameObject.transform.position.y < (otherPlayer.gameObject.transform.position.y - 20))
                {
                    otherPlayer.transform.position = this.gameObject.transform.position + offset;
                    playerCanTeleport = false;
                }
            }
        }
        else if (collision.gameObject.CompareTag("BubbleBounce"))
        {
            ChangeState(PlayerState.Bouncing);
            rb.linearVelocityY = jumpForce;
            AudioManager.instance.PlaySoundEffect(audioSource, bubbleBounce, 0.8f);
        }
        if (collision.gameObject.CompareTag("Lever"))
        {
            nearLever = true;
            currentLever = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        nearLever = false;
        currentLever = null;
        if (collision.gameObject.CompareTag("Ground") && !CompareState(PlayerState.Airborne) && !CompareState(PlayerState.Bubbled))
        {
            ChangeState(PlayerState.Airborne);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CompareState(PlayerState.Bubbled))
        {
            AudioManager.instance.PlaySoundEffect(audioSource, bubbleBounce, 0.6f);
        }
    }

    private void Move()
    {
        bool isMoveInput = moveInputX >= 0.2f || moveInputX <= -0.2f;

        if (isMoveInput)
        {
            moveDirX = moveInputX;
            if (!CompareState(PlayerState.Bubbled))
            {
                myAnimator.SetBool("Run", true);
                xVel = moveInputX * moveSpeed;
            }
        }
        else
        {
            if (myAnimator.GetBool("Run"))
            {
                myAnimator.SetBool("Run", false);
            }
        }
        if (CompareState(PlayerState.Bubbled))
        {
            xVel += moveInputX * bubbleSpeed;

            if (yVel < bubbleMoveSpeedCap)
            {
                yVel += bubbleFloatSpeed * Time.fixedDeltaTime;
            }
        }
    }

    private void Jump()
    {
        if (tryJump && CompareState(PlayerState.Grounded))
        {
            myAnimator.SetTrigger("Jump");
            yVel = jumpForce;
            AudioManager.instance.PlayOneShotSoundEffect(jump, 0.8f, gameObject.transform.position);
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
                AudioManager.instance.PlayOneShotSoundEffect(bubbleDash, 1f, gameObject.transform.position);
            }
            else
            {
                myAnimator.SetTrigger("Dash");
                AudioManager.instance.PlayOneShotSoundEffect(fishDash, 0.8f, gameObject.transform.position);

            }
        }
        tryDash = false;
    }

    private void Bubble()
    {
        if (CompareState(PlayerState.Bubbled))
        {
            bubbleTimer += Time.fixedDeltaTime;
            bubbleAnimator.speed = (bubbleTimer / bubbleAnimationSpeedMult);
            if (bubbleTimer > (bubbleDuration - 0.15))
            {
                bubbleAnimator.speed = 1.0f;
                bubbleAnimator.SetTrigger("Pop");
                AudioManager.instance.PlaySoundEffect(audioSource, bubblePop, 0.5f);
            }
            if (bubbleTimer > bubbleDuration)
            {
                PopBubble();
            }
        }
        else if (tryBubble && canBubble)
        {
            bubbleObject.SetActive(true);
            ChangeState(PlayerState.Bubbled);
            myAnimator.SetBool("Bubble", true);
            canBubble = false;
            AudioManager.instance.PlaySoundEffect(audioSource, bubbleForming, 0.5f);
        }
        tryBubble = false;
    }

    private void Interact()
    {
        if (tryInteract && nearLever && interactTimer <= 0)
        {

            currentLever.GetComponent<Lever>().touched = true;
            interactTimer = interactCooldown;
            tryInteract = false;
            AudioManager.instance.PlayOneShotSoundEffect(leverFlick, 0.8f, gameObject.transform.position);
        }
    }

    public void PopBubble()
    {
        ChangeState(PlayerState.Airborne);
        bubbleObject.SetActive(false);
        myAnimator.SetBool("Bubble", false);
        bubbleTimer = 0.0f;
    }

    public void ShowMenu()
    {
        if (canFlipMenu)
        {
            menu.gameObject.SetActive(!menu.gameObject.activeSelf);
            PlayerController otherPC = otherPlayer.GetComponent<PlayerController>();
            otherPC.FlipCanUseMenu();
            otherPC.Invoke("FlipCanUseMenu", 0.2f);
        }
    }

    public void FlipCanUseMenu()
    {
        canFlipMenu = !canFlipMenu;
    }
}