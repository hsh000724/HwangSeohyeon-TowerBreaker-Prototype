using UnityEngine;
using System.Collections;

public class TreasureBox : MonoBehaviour
{
    public float collectDuration = 0.5f; // 습득 애니메이션 시간

    // 습득 연출
    public void Collect()
    {
        StartCoroutine(CollectRoutine());
    }

    private IEnumerator CollectRoutine()
    {
        // 예시: Scale을 0으로 줄이며 사라지는 연출
        Vector3 originalScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < collectDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, elapsed / collectDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        gameObject.SetActive(false); // 오브젝트 비활성화
    }
}