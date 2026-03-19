using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float duration = 0.8f;

    private TextMeshProUGUI textMesh;
    private float timer;

    void Awake() => textMesh = GetComponent<TextMeshProUGUI>();

    public void SetText(int damage)
    {
        textMesh.text = damage.ToString();
        timer = duration;
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
    }
}