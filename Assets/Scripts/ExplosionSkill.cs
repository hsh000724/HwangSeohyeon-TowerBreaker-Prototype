using UnityEngine;

public class ExplosionSkill : MonoBehaviour
{
    public float damage = 50f;
    public float range = 3f;
    public float lifeTime = 0.5f;

    void Start()
    {
        Explode();
        Destroy(gameObject, lifeTime);
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D col in hits)
        {
            Enemy enemy = col.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage((int)damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}