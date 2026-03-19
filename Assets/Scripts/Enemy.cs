using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Normal,
        Tank,
        Speed,
        BOSS,
        FINAL_BOSS
    }

    [Header("Enemy Type")]
    public EnemyType enemyType;

    [Header("Settings")]
    public float moveSpeed;
    public float knockbackTime = 0.3f;
    public int maxHP;

    [Header("Base Stats")]
    public int baseHP = 10;
    public float baseSpeed = 4f;

    [Header("States")]
    protected bool isKnockback;
    protected bool isStopped;

    [Header("Defense")]
    public float defenseMultiplier = 0f;

    protected Rigidbody2D rb;
    private float knockbackTimer;

    private bool stopCooldownActive = false;
    private float stopCooldownTime = 0.5f;

    protected int currentHP;

    // currentHP 접근용 public 프로퍼티 추가
    public int CurrentHP
    {
        get => currentHP;
        set => currentHP = value;
    }

    public event Action<Enemy> OnEnemyDeath;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // virtual로 변경
    protected virtual void OnEnable()
    {
        ApplyEnemyType();

        currentHP = baseHP;
        moveSpeed = baseSpeed;

        isKnockback = false;
        isStopped = false;
    }

    void Update()
    {
        if (isKnockback)
        {
            HandleKnockback();
            return;
        }

        if (isStopped)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        Move();
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
    }

    void ApplyEnemyType()
    {
        switch (enemyType)
        {
            case EnemyType.Normal:
                baseHP = 10;
                baseSpeed = 4f;
                break;

            case EnemyType.Tank:
                baseHP = 25;
                baseSpeed = 2f;
                break;

            case EnemyType.Speed:
                baseHP = 8;
                baseSpeed = 8f;
                break;

            case EnemyType.BOSS:
                baseHP = 200;
                baseSpeed = 2f;
                break;

            case EnemyType.FINAL_BOSS:
                baseHP = 500;
                baseSpeed = 2f;
                break;
        }
    }

    void HandleKnockback()
    {
        knockbackTimer -= Time.deltaTime;
        //SoundManager.Instance.PlayEnemyKnockback(); 

        if (knockbackTimer <= 0f)
            isKnockback = false;
    }

    public void ApplyKnockback(float force, float dir)
    {
        isKnockback = true;
        isStopped = false;
        knockbackTimer = knockbackTime;

        rb.linearVelocity = Vector2.zero;

        Vector2 forceDir = new Vector2(dir, 0.2f).normalized;
        rb.AddForce(forceDir * force, ForceMode2D.Impulse);
    }

    public void StopMovement()
    {
        if (stopCooldownActive) return;

        isStopped = true;
        rb.linearVelocity = Vector2.zero;

        stopCooldownActive = true;
        Invoke(nameof(ResetStopCooldown), stopCooldownTime);
    }

    void ResetStopCooldown()
    {
        stopCooldownActive = false;
    }

    public void ResumeMovement()
    {
        isStopped = false;
        isKnockback = false;

        rb.linearVelocity = Vector2.zero;
    }

    public void TakeDamage(int damage)
    {
        SoundManager.Instance.PlayEnemyHit();

        int finalDamage = Mathf.RoundToInt(damage * (1 - defenseMultiplier));
        currentHP -= finalDamage;

        DamageTextManager.Instance.ShowDamage(finalDamage, transform.position);

        if (currentHP <= 0)
        {
            SoundManager.Instance.PlayEnemyDie();
            Die();
        }
    }

    public void SetHP(int hp)
    {
        maxHP = hp;
        currentHP = hp;
    }

    public void ApplyHPMultiplier(float multiplier)
    {
        currentHP = Mathf.RoundToInt(baseHP * multiplier);
    }

    private int GetEnemyScore()
    {
        switch (enemyType)
        {
            case EnemyType.Tank: return 25;
            case EnemyType.Speed: return 40;
            case EnemyType.BOSS: return 500;
            case EnemyType.FINAL_BOSS: return 1000;
            default: return 10;
        }
    }

    private void Die()
    {
        if (TreasureBoxManager.Instance != null)
            TreasureBoxManager.Instance.TryDropBox(transform.position);

        gameObject.SetActive(false);
        OnEnemyDeath?.Invoke(this);

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(GetEnemyScore());
    }
}