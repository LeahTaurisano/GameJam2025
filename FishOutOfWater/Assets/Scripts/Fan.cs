using UnityEngine;
    public class Fan : MonoBehaviour
    {
    [SerializeField] private float airForce;
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


            playerRb.AddForce(playerDirection * airForce);
        }
    }
}

