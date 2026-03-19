using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SkillCooldownUI : MonoBehaviour
{
    public string skillKey;

    public Image cooldownMask;
    public TextMeshProUGUI cooldownText;

    public Image shineImage;

    float lastRatio = 1f;

    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float ratio = SkillManager.Instance.GetCooldownRatio(skillKey);

        cooldownMask.fillAmount = ratio;

        float cooldownTime = SkillManager.Instance.GetCooldownTime(skillKey);

        if (cooldownTime > 0)
        {
            cooldownText.text = Mathf.Ceil(cooldownTime).ToString();
            cooldownText.gameObject.SetActive(true);
        }
        else
        {
            cooldownText.gameObject.SetActive(false);
        }

        if (ratio <= 0 && lastRatio > 0)
        {
            StartCoroutine(Shine());
        }

        lastRatio = ratio;
    }

    IEnumerator Shine()
    {
        float t = 0;

        shineImage.gameObject.SetActive(true);

        while (t < 0.4f)
        {
            t += Time.deltaTime;

            float alpha = Mathf.PingPong(t * 6f, 1f);

            shineImage.color = new Color(1, 1, 1, alpha);

            yield return null;
        }

        shineImage.color = new Color(1, 1, 1, 0);
        shineImage.gameObject.SetActive(false);

        StartCoroutine(IconBounce());
    }

    IEnumerator IconBounce()
    {
        transform.localScale = originalScale * 1.15f;

        yield return new WaitForSeconds(0.1f);

        transform.localScale = originalScale;
    }
}