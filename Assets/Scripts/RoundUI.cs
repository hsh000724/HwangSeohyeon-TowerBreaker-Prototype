using UnityEngine;
using TMPro;
using System.Collections;

public class RoundUI : MonoBehaviour
{
    public TextMeshProUGUI roundText;

    public float startScale = 2f;
    public float endScale = 1f;
    public float animationTime = 0.6f;

    Coroutine currentAnim;

    public void ShowRound(int round)
    {
        roundText.text = "ROUND " + round;

        if (currentAnim != null)
            StopCoroutine(currentAnim);

        currentAnim = StartCoroutine(RoundAnimation());
    }

    IEnumerator RoundAnimation()
    {
        float t = 0;

        roundText.transform.localScale = Vector3.one * startScale;

        while (t < animationTime)
        {
            t += Time.deltaTime;

            float scale = Mathf.Lerp(startScale, endScale, t / animationTime);

            roundText.transform.localScale = Vector3.one * scale;

            yield return null;
        }

        roundText.transform.localScale = Vector3.one * endScale;
    }
}