using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance;

    [Header("Damage Text Settings")]
    public GameObject damageTextPrefab;
    public Transform worldCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDamage(int damage, Vector3 worldPosition)
    {
        if (damageTextPrefab == null || worldCanvas == null) return;

        // »ìÂ¦ ·£´ý À§Ä¡
        Vector3 spawnPos = worldPosition + new Vector3(
            Random.Range(-0.2f, 0.2f),
            Random.Range(0.3f, 0.6f),
            0
        );

        GameObject obj = Instantiate(damageTextPrefab);
        obj.transform.SetParent(worldCanvas, false);
        obj.transform.position = spawnPos;

        DamageText dmg = obj.GetComponent<DamageText>();
        if (dmg != null)
        {
            dmg.SetText(damage);
        }
    }
}