using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [Header("Skill Prefabs")]
    public GameObject powerKnockbackPrefab;
    public GameObject snowPrefab;
    public GameObject explosionPrefab;

    public Transform firePoint;

    [Header("Skill Cooldown")]
    public float powerKnockbackCooldown = 30f;
    public float snowCooldown = 45f;
    public float explosionCooldown = 15f;

    float powerKnockbackTimer;
    float snowTimer;
    float explosionTimer;

    float dashTimer;
    float defendTimer;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        powerKnockbackTimer -= Time.deltaTime;
        snowTimer -= Time.deltaTime;
        explosionTimer -= Time.deltaTime;

        dashTimer -= Time.deltaTime;
        defendTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
            UsePowerKnockback();

        if (Input.GetKeyDown(KeyCode.W))
            UseExplosion();

        if (Input.GetKeyDown(KeyCode.E))
            UseSnow();


    }

    void UsePowerKnockback()
    {
       
        if (powerKnockbackTimer > 0) return;
        SoundManager.Instance.PlayPowerKnockback();
        Instantiate(powerKnockbackPrefab, firePoint.position, Quaternion.identity);

        powerKnockbackTimer = powerKnockbackCooldown;
    }

    void UseSnow()
    {
        
        if (snowTimer > 0) return;
        SoundManager.Instance.PlaySnow();
        Instantiate(snowPrefab, Vector3.zero, Quaternion.identity);

        snowTimer = snowCooldown;
    }

    void UseExplosion()
    {
        if (explosionTimer > 0) return;
        SoundManager.Instance.PlayExplosion();
        Vector2 pos = transform.position + Vector3.right * 2f;

        Instantiate(explosionPrefab, pos, Quaternion.identity);

        explosionTimer = explosionCooldown;
    }

    // Dash
    public bool CanUseDash()
    {
        return dashTimer <= 0;
    }

    public void UseDash()
    {
        dashTimer = PlayerController.Instance.dashCooldown;
    }

    // Defend
    public bool CanUseDefend()
    {
        return defendTimer <= 0;
    }

    public void UseDefend()
    {
        defendTimer = PlayerController.Instance.defendCooldown;
    }

    public float GetCooldownRatio(string skill)
    {
        switch (skill)
        {
            case "Q": return powerKnockbackTimer / powerKnockbackCooldown;
            case "W": return explosionTimer / explosionCooldown;
            case "E": return snowTimer / snowCooldown;

            case "Dash":
                return dashTimer / PlayerController.Instance.dashCooldown;

            case "Defend":
                return defendTimer / PlayerController.Instance.defendCooldown;
        }

        return 0;
    }

    public float GetCooldownTime(string skill)
    {
        switch (skill)
        {
            case "Q": return powerKnockbackTimer;
            case "W": return explosionTimer;
            case "E": return snowTimer;

            case "Dash": return dashTimer;
            case "Defend": return defendTimer;
        }

        return 0;
    }
}