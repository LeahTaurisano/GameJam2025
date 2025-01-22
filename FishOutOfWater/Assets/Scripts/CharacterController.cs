using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    //Speed we set
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject bubbleObject;
    [SerializeField] private float bubbleSpeedMod;
    [SerializeField] private float bubbleMoveSpeedCap;
    [SerializeField] private float bubbleFloatSpeed;
    [SerializeField] private float velocityDecay;

    enum PlayerState
    {
        Grounded,
        Airborne,
        Bubbled,
        Falling,
        Bouncing
    };
    private PlayerState currentState = PlayerState.Grounded;
    private bool canBubble = true;
    private bool tryJump = false;
    private bool tryBubble = false;
    private float moveInputX;

    private float xVel = 0.0f;
    private float yVel = 0.0f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    #region Movement
    public void MoveInput(InputAction.CallbackContext context)
    {
        moveInputX = context.ReadValue<Vector2>().x;
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        tryJump = context.performed;
    }    
    public void BubbleInput(InputAction.CallbackContext context)
    {
        tryBubble = context.performed;
    }
    #endregion
    private void FixedUpdate()
    {
        xVel = rb.linearVelocityX * velocityDecay;
        if (CompareState(PlayerState.Grounded))
        {
            xVel = 0.0f;
        }
        yVel = rb.linearVelocityY;
        Move();
        Jump();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            ChangeState(PlayerState.Grounded);
        }
    }

    private void Move()
    {
        bool isMoveInput = moveInputX != 0.0f;
        if (isMoveInput && !CompareState(PlayerState.Bubbled))
        {
            xVel = moveInputX * moveSpeed;
        }

        if (CompareState(PlayerState.Bubbled))
        {
            if (isMoveInput)
            {
                xVel += moveInputX * moveSpeed * bubbleSpeedMod;
                xVel = Mathf.Clamp(xVel, -bubbleMoveSpeedCap, bubbleMoveSpeedCap);
            }
            yVel = bubbleFloatSpeed;
        }
    }

    private void Jump()
    {
        if (tryJump && CompareState(PlayerState.Grounded))
        {
            yVel = jumpForce;
            ChangeState(PlayerState.Airborne);
            tryJump = false;
        }
    }

    private void Bubble()
    {
        if (tryBubble && canBubble)
        {
            bubbleObject.SetActive(true);
            ChangeState(PlayerState.Bubbled);
        }
    }
}