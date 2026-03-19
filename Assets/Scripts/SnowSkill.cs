using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnowSkill : MonoBehaviour
{
    public float slowAmount = 0.5f;
    public float duration = 5f;

    List<Enemy> affectedEnemies = new List<Enemy>();

    void Start()
    {
        StartCoroutine(SnowEffect());
    }

    IEnumerator SnowEffect()
    {
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemies)
        {
            enemy.moveSpeed *= slowAmount;
            affectedEnemies.Add(enemy);
        }

        yield return new WaitForSeconds(duration);

        foreach (Enemy enemy in affectedEnemies)
        {
            if (enemy != null)
            {
                enemy.moveSpeed /= slowAmount;
            }
        }

        Destroy(gameObject);
    }
}