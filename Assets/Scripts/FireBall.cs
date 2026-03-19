using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 10f;
    public float pushForce = 10f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.left * speed;

        Destroy(gameObject, 0.8f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Rigidbody2D playerRb = col.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.AddForce(Vector2.left * pushForce, ForceMode2D.Impulse);
            }

            Destroy(gameObject); 
        }
    }
}