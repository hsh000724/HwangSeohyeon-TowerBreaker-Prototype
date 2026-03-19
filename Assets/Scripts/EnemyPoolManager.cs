using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class EnemyPoolManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject normalEnemyPrefab;
    public GameObject tankEnemyPrefab;
    public GameObject speedEnemyPrefab;

    public int poolSize = 10;

    private List<Enemy> normalPool = new List<Enemy>();
    private List<Enemy> tankPool = new List<Enemy>();
    private List<Enemy> speedPool = new List<Enemy>();

    void Awake()
    {
        CreatePool(normalEnemyPrefab, normalPool);
        CreatePool(tankEnemyPrefab, tankPool);
        CreatePool(speedEnemyPrefab, speedPool);
    }

    void CreatePool(GameObject prefab, List<Enemy> pool)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj.GetComponent<Enemy>());
        }
    }

    public Enemy GetEnemy(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Normal:
                return GetFromPool(normalPool, normalEnemyPrefab);

            case EnemyType.Tank:
                return GetFromPool(tankPool, tankEnemyPrefab);

            case EnemyType.Speed:
                return GetFromPool(speedPool, speedEnemyPrefab);
        }

        return null;
    }

    Enemy GetFromPool(List<Enemy> pool, GameObject prefab)
    {
        foreach (var enemy in pool)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                enemy.gameObject.SetActive(true);
                return enemy;
            }
        }

        // 钱 何练 矫 货 积己
        GameObject obj = Instantiate(prefab);
        Enemy newEnemy = obj.GetComponent<Enemy>();
        pool.Add(newEnemy);

        return newEnemy;
    }

    public void ResetAllEnemies()
    {
        ResetPool(normalPool);
        ResetPool(tankPool);
        ResetPool(speedPool);
    }

    void ResetPool(List<Enemy> pool)
    {
        foreach (var enemy in pool)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}