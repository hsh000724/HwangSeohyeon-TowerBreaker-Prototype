using System.Collections;
using UnityEngine;
using static Enemy;

public class RoundManager : MonoBehaviour
{
    public EnemyPoolManager poolManager;
    public Transform spawnPoint;
    public Transform bossSpawnPoint;

    public GameObject midBossPrefab;
    public GameObject finalBossPrefab;

    public float roundStartDelay = 2f;
    public float bossSpawnDelay = 3f;

    [System.Serializable]
    public class Round
    {
        public int enemyCount;
        public float hpMultiplier = 1f;
        public float spawnDelay = 0.5f;

        [Header("Spawn Chance (%)")]
        public int normalChance = 100;
        public int tankChance = 0;
        public int speedChance = 0;
    }

    public Round[] rounds;

    private int currentRound = 0;
    private int aliveEnemies = 0;

    void Start()
    {
        StartCoroutine(StartRound(currentRound));
    }

    IEnumerator StartRound(int roundIndex)
    {
        yield return new WaitForSeconds(roundStartDelay);

        Round round = rounds[roundIndex];

        // UI 업데이트
        UIManager.Instance.ShowRound(roundIndex + 1);

        // GameManager에 현재 라운드 전달
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentRound = roundIndex + 1; // 실제 라운드 번호
        }

        aliveEnemies = round.enemyCount;

        // 보스 라운드라면 +1
        if (ShouldSpawnBoss(roundIndex))
        {
            aliveEnemies += 1;
            StartCoroutine(SpawnBossDelayed(roundIndex));
        }

        for (int i = 0; i < round.enemyCount; i++)
        {
            EnemyType type = GetEnemyType(round);
            Enemy enemy = poolManager.GetEnemy(type);

            enemy.transform.position = spawnPoint.position;
            enemy.ApplyHPMultiplier(round.hpMultiplier);
            enemy.gameObject.SetActive(true);
            enemy.OnEnemyDeath += HandleEnemyDeath;

            yield return new WaitForSeconds(round.spawnDelay);
        }
    }

    IEnumerator SpawnBossDelayed(int roundIndex)
    {
        yield return new WaitForSeconds(bossSpawnDelay);
        SpawnBoss(roundIndex);
    }

    EnemyType GetEnemyType(Round round)
    {
        int total = round.normalChance + round.tankChance + round.speedChance;
        int rand = Random.Range(0, total);

        if (rand < round.normalChance) return EnemyType.Normal;
        rand -= round.normalChance;
        if (rand < round.tankChance) return EnemyType.Tank;
        return EnemyType.Speed;
    }

    void HandleEnemyDeath(Enemy enemy)
    {
        enemy.OnEnemyDeath -= HandleEnemyDeath;

        aliveEnemies--;

        CheckRoundEnd();
    }

    void CheckRoundEnd()
    {
        if (aliveEnemies <= 0)
        {
            if (TreasureBoxManager.Instance != null)
                TreasureBoxManager.Instance.CollectAllBoxes();

            currentRound++;
            SoundManager.Instance.PlayRoundClear();

            if (currentRound < rounds.Length)
            {
                StartCoroutine(StartRound(currentRound));
            }
            else
            {
                GameManager.Instance.GameOver();
                Debug.Log("모든 라운드 완료!");
            }
        }
    }

    bool ShouldSpawnBoss(int roundIndex)
    {
        int round = roundIndex + 1;

        if (round % 10 == 0) return true;
        if (round % 5 == 0) return true;

        return false;
    }

    void SpawnBoss(int roundIndex)
    {
        GameObject bossPrefab = ((roundIndex + 1) % 10 == 0) ? finalBossPrefab : midBossPrefab;

        GameObject bossObj = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

        BossEnemy boss = bossObj.GetComponent<BossEnemy>();

        boss.isFinalBoss = ((roundIndex + 1) % 10 == 0);

        boss.bossMaxHP = Mathf.RoundToInt(100 + roundIndex * 50);

        boss.OnEnemyDeath += HandleEnemyDeath;
    }

    public int CurrentRound => currentRound;
}