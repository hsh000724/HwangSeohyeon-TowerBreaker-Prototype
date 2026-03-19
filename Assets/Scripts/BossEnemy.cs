using System.Collections;
using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss Settings")]
    public bool isFinalBoss = false;
    public float patternInterval = 3f;

    private Transform fireBallSpawnPoint;

    [Header("Boss Stats")]
    public int bossMaxHP = 200;

    [Header("Buff Icons")]
    public GameObject moveSpeedIcon;
    public GameObject defenseIcon;

    private Coroutine patternCoroutine;

    void Start()
    {
        fireBallSpawnPoint = GameObject.FindGameObjectWithTag("FireBallPoint").transform;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        currentHP = bossMaxHP;
        isKnockback = false;
        isStopped = false;

        if (patternCoroutine != null) StopCoroutine(patternCoroutine);
        patternCoroutine = StartCoroutine(PatternLoop());
    }

    IEnumerator PatternLoop()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(patternInterval);

            if (!isKnockback && !isStopped)
            {
                StartCoroutine(ExecutePatternCoroutine());
            }
        }
    }

    IEnumerator ExecutePatternCoroutine()
    {
        isStopped = true; // 패턴 준비 시 잠깐 정지
        yield return new WaitForSeconds(0.5f);

        if (isFinalBoss)
        {
            int r = Random.Range(0, 4);

            switch (r)
            {
                case 0:
                    ShootFireBallWave();
                    break;

                case 1:
                    GroundSmash();
                    break;

                case 2:
                    BuffAlliesRandom();
                    break;

                case 3:
                    BuffSelfRandom();
                    break;
            }
        }
        else
        {
            int r = Random.Range(0, 2);

            switch (r)
            {
                case 0:
                    ShootFireBallWave();
                    break;

                case 1:
                    GroundSmash();
                    break;
            }
        }

        yield return new WaitForSeconds(0.5f);
        isStopped = false; // 패턴 종료 후 이동 재개
    }

    // 랜덤 버프 선택
    Buff.BuffType GetRandomBuff()
    {
        Buff.BuffType[] buffs = (Buff.BuffType[])System.Enum.GetValues(typeof(Buff.BuffType));
        return buffs[Random.Range(0, buffs.Length)];
    }

    // 검기 발사
    void ShootFireBallWave()
    {
        SoundManager.Instance.PlayShootFireBallWave();
        Instantiate(Resources.Load("FireBall") as GameObject,
                    fireBallSpawnPoint.position,
                    Quaternion.identity);
    }

    // 점프 공격
    void GroundSmash()
    {
        SoundManager.Instance.PlayGroundSmash();
        Debug.Log("Ground Smash!");
        StartCoroutine(GroundSmashCoroutine());
    }

    IEnumerator GroundSmashCoroutine()
    {
        rb.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);

        foreach (var col in colliders)
        {
            if (col.attachedRigidbody != null)
            {
                col.attachedRigidbody.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
            }
        }
    }

    // 자기 버프
    void BuffSelfRandom()
    {
        SoundManager.Instance.PlayBuff();
        Debug.Log("Buff Self!");
        Buff.BuffType randomBuff = GetRandomBuff();
        ApplyBuff(this, randomBuff);
    }

    // 모든 적 버프
    void BuffAlliesRandom()
    {
        SoundManager.Instance.PlayBuff();
        Debug.Log("Buff Allies!");
        Buff.BuffType randomBuff = GetRandomBuff();

        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (var enemy in allEnemies)
        {
            ApplyBuff(enemy, randomBuff);
        }
    }

    void ApplyBuff(Enemy enemy, Buff.BuffType type)
    {
        GameObject buffObj = new GameObject("Buff");
        Buff buff = buffObj.AddComponent<Buff>();

        buff.buffType = type;

        if (type == Buff.BuffType.Defense)
            buff.multiplier = 0.2f;
        else
            buff.multiplier = 1.2f;

        buff.duration = 5f;

        buff.Apply(enemy);

        SpawnBuffIcon(enemy, type);
    }

    void SpawnBuffIcon(Enemy enemy, Buff.BuffType type)
    {
        GameObject iconPrefab = null;

        if (type == Buff.BuffType.MoveSpeed)
            iconPrefab = moveSpeedIcon;

        else if (type == Buff.BuffType.Defense)
            iconPrefab = defenseIcon;

        if (iconPrefab == null) return;

        Transform existingIcon = enemy.transform.Find("BuffIcon");

        if (existingIcon != null)
            Destroy(existingIcon.gameObject);

        GameObject icon = Instantiate(iconPrefab, enemy.transform.position + new Vector3(0f, 1f, -1f), Quaternion.identity);

        icon.transform.SetParent(enemy.transform);
        icon.name = "BuffIcon";

        Destroy(icon, 5f);
    }
}