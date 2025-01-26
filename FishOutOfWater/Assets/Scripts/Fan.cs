using UnityEngine;
public class Fan : MonoBehaviour
{
    enum WindDirection 
    { 
        Up, 
        Down, 
        Left, 
        Right 
    };
    [SerializeField] private float airForce;
    [SerializeField] private WindDirection fanDirection;
    private Rigidbody2D playerRb;
    private Transform fanPosition;
    private void Start()
    {
        fanPosition = transform.parent;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
            if (collision.gameObject.CompareTag("Bubble"))
            {

                playerRb = collision.gameObject.GetComponentInParent<Rigidbody2D>();

                //direction facing the player (i.e direction from which the player is facing the fan)
                Vector2 playerDirection = (collision.transform.position - fanPosition.position).normalized;

                if (fanDirection == WindDirection.Up)
                {
                    playerRb.AddForceY(playerDirection.y * airForce);
                }
                if (fanDirection == WindDirection.Down)
                {
                    playerRb.AddForceY(playerDirection.y * airForce);
                }
                if (fanDirection == WindDirection.Right)
                {
                    playerRb.AddForceX(playerDirection.x * airForce);
                }
                if (fanDirection == WindDirection.Left)
                {
                    playerRb.AddForceX(playerDirection.x * airForce);
                }
            }
        
       
    }
}

