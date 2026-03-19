using UnityEngine;

public class PowerKnockback : MonoBehaviour
{
    public float speed = 15f;
    public float knockbackForce = 20f;
    public float lifeTime = 2f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.right * speed;

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>();

        if (enemy != null)
        {
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

            if (enemyRb != null)
            {
                enemyRb.AddForce(Vector2.right * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}