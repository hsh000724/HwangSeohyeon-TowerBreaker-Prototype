using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
     void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [Header("Move")]
    public float dashSpeed = 50f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRange = 1f;
    public int attackDamage = 10;

    [Header("Defend")]
    public float defendRange = 2f;
    public float knockbackForce = 20f;
    public float defendCooldown = 1f;

    [Header("Player Knockback")]
    public float playerKnockbackForce = 6f;

    [Header("Health")]
    public int playerHP = 3;

    [Header("Effects")]
    public GameObject[] attackEffects;
    public GameObject hitEffect;
    public GameObject guardEffect;

    private Rigidbody2D rb;
    private bool isDashing;
    private bool isTouchingWall;
    private bool isTouchingEnemy;
    private bool isPlayerHit;
    private bool isInvincible;

    private float dashTimer;
    private float dashCooldownTimer;
    private float defendTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isPlayerHit = false;
        isInvincible = false;

        UIManager.Instance.UpdatePlayerHP(playerHP);

        CheckDamageCondition();
    }

    void FixedUpdate()
    {
        CheckDamageCondition();
    }

    void Update()
    {
        if (isPlayerHit)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                ResetHitState();
            }
        }
        if (!GameManager.Instance.isGameRunning) return;

        HandleDash();
        HandleAttack();
        HandleDefend();
    }

    void CheckDamageCondition()
    {
        if (isInvincible || isPlayerHit) return;

        if (isTouchingWall && isTouchingEnemy)
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        if (isInvincible) return;

        SoundManager.Instance.PlayHurt();
        CameraShake.Instance.Shake(0.1f, 0.1f);
        DamageFlash.Instance.Flash();

        playerHP--;
        UIManager.Instance.UpdatePlayerHP(playerHP);
        if (playerHP <= 0)
        {
            GameManager.Instance.GameOver();
        }

        isPlayerHit = true;
        isInvincible = true;

        Debug.Log($"플레이어 피격! 남은 체력: {playerHP}");
        rb.linearVelocity = Vector2.zero;

        // 모든 적 Stop
        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in allEnemies)
        {
            enemy.StopMovement();
        }

        Invoke("DisableInvincibility", 0.15f);
    }

    void ResetHitState()
    {
        isPlayerHit = false;
    }

    void HandleDash()
    {
        if (isPlayerHit) return;

        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && dashCooldownTimer <= 0)
        {
            if (SkillManager.Instance.CanUseDash())
            {
                StartDash(1);
                SkillManager.Instance.UseDash();
            }
        }


        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0) StopDash();
        }
    }

    void StartDash(int direction)
    {
        SoundManager.Instance.PlayDash();
        isDashing = true;
        dashTimer = dashTime;
        dashCooldownTimer = dashCooldown;
        rb.linearVelocity = new Vector2(direction * dashSpeed, 0);
    }

    void StopDash() { isDashing = false; rb.linearVelocity = Vector2.zero; }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0)) Attack();
    }

    void Attack()
    {
        // 1. 기본 데미지 가져오기
        int baseDamage = attackDamage;
        int bonusDamage = 0;

        // 2. 장착된 무기 보너스 가져오기
        if (EquipmentManager.Instance != null)
        {
            bonusDamage = EquipmentManager.Instance.GetEquippedAttackBonus();
        }

        // 3. 최종 데미지 합산
        int finalDamage = baseDamage + bonusDamage;

        SoundManager.Instance.PlayAttack();
        SpawnAttackEffect(this.transform.position);

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        bool hitAnyEnemy = false;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hitAnyEnemy = true;
                Enemy e = hit.GetComponent<Enemy>();
                if (e != null)
                {
                    // 4. 합산된 최종 데미지로 적을 타격!
                    e.TakeDamage(finalDamage);

                    Debug.Log($"[데미지 계산] 기본({baseDamage}) + 무기({bonusDamage}) = 최종({finalDamage})");

                    CameraShake.Instance.Shake(0.08f, 0.08f);
                    SpawnHitEffect(hit.transform.position);

                    float dir = e.transform.position.x > transform.position.x ? 1f : -1f;
                    e.ApplyKnockback(knockbackForce * 0.5f, dir);
                }
            }
        }

        if (hitAnyEnemy)
        {
            StartCoroutine(ResumeAllEnemiesDelayed(Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None), 0.05f));
        }
    }

    // 모든 적 프리즈 상태 해제 코루틴
    IEnumerator ResumeAllEnemiesDelayed(Enemy[] enemies, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var enemy in enemies)
        {
            enemy.ResumeMovement(); // Stop 상태만 해제
        }
    }
    void SpawnAttackEffect(Vector3 pos)
    {
        if (attackEffects.Length == 0) return;

        int rand = Random.Range(0, attackEffects.Length);

        GameObject effect = Instantiate(attackEffects[rand], pos, Quaternion.identity);

        Destroy(effect, 0.5f); // 이펙트 자동 삭제
    }
    void SpawnHitEffect(Vector3 pos)
    {
        if (hitEffect == null) return;

        GameObject effect = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(effect, 0.5f);
    }
    void SpawnGuardEffect(Vector3 pos)
    {
        if (guardEffect == null) return;

        GameObject effect = Instantiate(guardEffect, pos, Quaternion.identity);
        Destroy(effect, 0.5f);
    }
    void HandleDefend()
    {
        if (defendTimer > 0) defendTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && defendTimer <= 0)
        {
            if (SkillManager.Instance.CanUseDefend())
            {
                Defend();
                SkillManager.Instance.UseDefend();
            }
        }
    }

    void Defend()
    {
        // 범위내 적 존재 시 방어 성공
        bool success = false;
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, defendRange);
        foreach (var col in nearbyEnemies)
        {
            if (col.CompareTag("Enemy"))
            {
                success = true;
                break;
            }
        }

        if (success)
        {
            defendTimer = defendCooldown;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.left * playerKnockbackForce, ForceMode2D.Impulse);

            // 방어 성공 시 즉시 무적 적용
            isInvincible = true;
            CancelInvoke("DisableInvincibility"); // 기존 Invoke 제거
            Invoke("DisableInvincibility", 0.5f); // 0.5초 무적

            // Stop + 딜레이 후 넉백
            Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (var enemy in allEnemies)
            {
                enemy.StopMovement();
            }

            // 0.1초 후 모든 적에게 넉백 적용
            SoundManager.Instance.PlayDefend();
            SpawnGuardEffect(transform.position);
            StartCoroutine(ApplyKnockbackAllDelayed(0.1f));
        }
        else
        {
            defendTimer = defendCooldown; // 방어 실패도 쿨 적용
        }
    }

    IEnumerator ApplyKnockbackAllDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in allEnemies)
        {
            float dir = enemy.transform.position.x > transform.position.x ? 1f : -1f;
            enemy.ResumeMovement();
            enemy.ApplyKnockback(knockbackForce, dir);
        }
    }

    void DisableInvincibility() { isInvincible = false; }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) isTouchingWall = true;
        if (collision.gameObject.CompareTag("Enemy")) isTouchingEnemy = true;

        if (isDashing && collision.gameObject.CompareTag("Enemy")) StopDash();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) isTouchingWall = true;
        if (collision.gameObject.CompareTag("Enemy")) isTouchingEnemy = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) isTouchingWall = false;
        if (collision.gameObject.CompareTag("Enemy")) isTouchingEnemy = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null) { Gizmos.color = Color.red; Gizmos.DrawWireSphere(attackPoint.position, attackRange); }
        Gizmos.color = Color.blue; Gizmos.DrawWireSphere(transform.position, defendRange);
    }
}