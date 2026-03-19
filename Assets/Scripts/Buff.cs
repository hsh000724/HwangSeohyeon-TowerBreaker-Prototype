using UnityEngine;

public class Buff : MonoBehaviour
{
    public enum BuffType
    {
        MoveSpeed,
        Defense
    }

    public BuffType buffType;
    public float multiplier = 1.2f;
    public float duration = 5f;

    private Enemy target;

    public void Apply(Enemy enemy)
    {
        target = enemy;

        switch (buffType)
        {
            case BuffType.MoveSpeed:
                target.moveSpeed *= multiplier;
                break;

            case BuffType.Defense:
                target.defenseMultiplier += multiplier;
                break;
        }

        Invoke(nameof(Remove), duration);
    }

    void Remove()
    {
        if (target == null) return;

        switch (buffType)
        {
            case BuffType.MoveSpeed:
                target.moveSpeed /= multiplier;
                break;

            case BuffType.Defense:
                target.defenseMultiplier -= multiplier;
                break;
        }

        Destroy(gameObject);
    }
}