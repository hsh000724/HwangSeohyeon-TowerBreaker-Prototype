using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    public static DamageFlash Instance;

    public Image damageImage;
    public float flashSpeed = 5f;
    public float maxAlpha = 0.5f;

    void Awake()
    {
        Instance = this;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        Color color = damageImage.color;
        color.a = maxAlpha;
        damageImage.color = color;

        while (damageImage.color.a > 0)
        {
            color.a -= flashSpeed * Time.deltaTime;
            damageImage.color = color;
            yield return null;
        }
    }
}