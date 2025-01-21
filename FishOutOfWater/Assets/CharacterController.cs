using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    //Speed we set
    [SerializeField] private float speed;

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
    private float moveInputX;

    //Actual Speed used by code
    private float movementSpeed;

    #region Movement
    public void Move(InputAction.CallbackContext context)
    {
        moveInputX = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        tryJump = context.performed;
    }
    #endregion
    private void Start()
    {
        movementSpeed = speed;
    }
    private void FixedUpdate()
    {
        float xVel = moveInputX * movementSpeed;
        float yVel = rb.linearVelocityY;
        if (tryJump && CompareState(PlayerState.Grounded))
        {
            yVel = movementSpeed;
            ChangeState(PlayerState.Airborne);
            tryJump = false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            ChangeState(PlayerState.Grounded);
        }
    }
}