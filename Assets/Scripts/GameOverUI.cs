using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup panel;           // 검정색 배경 패널
    public CanvasGroup gameOverText;    // GameOver 텍스트 CanvasGroup
    public TMP_Text resultText;         // 점수/상자 텍스트
    public CanvasGroup buttonsGroup;    // 버튼 전체 CanvasGroup

    [Header("Settings")]
    public float fadeDuration = 1f;     // 텍스트/버튼 페이드 시간
    public float typingSpeed = 0.05f;   // 글자 타이핑 속도

    private string[] resultLines;

    // 타이핑 사운드 재생 중 체크
    private bool isTypingPlaying = false;

    private void Start()
    {
        // 초기화: 모두 보이지 않게
        panel.alpha = 0f;
        gameOverText.alpha = 0f;
        resultText.text = "";
        buttonsGroup.alpha = 0f;
        buttonsGroup.interactable = false;
        buttonsGroup.blocksRaycasts = false;
    }

    // 게임오버 시 호출
    public void ShowGameOver(int round, int score, int boxCount)
    {
        gameObject.SetActive(true); // GameOverUI 오브젝트 활성화

        // 한 줄씩 나오는 텍스트
        resultLines = new string[]
        {
            $"ROUND : {round}",
            $"SCORE : {score}",
            $"Acquired Treasure Box : {boxCount}"
        };

        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // 1️⃣ 검정색 패널 페이드 인
        yield return StartCoroutine(FadeCanvasGroup(panel, 0f, 1f, fadeDuration));

        // 2️⃣ GameOver 텍스트 페이드 인
        yield return StartCoroutine(FadeCanvasGroup(gameOverText, 0f, 1f, fadeDuration));

        // 3️⃣ Result 텍스트 한 줄씩 타이핑
        resultText.text = "";

        foreach (string line in resultLines)
        {
            yield return StartCoroutine(TypeTextWithSound(resultText, line, typingSpeed));
            resultText.text += "\n"; // 줄바꿈
            yield return new WaitForSecondsRealtime(0.2f); // 각 줄 사이 딜레이
        }

        // 4️⃣ 타이핑 사운드 종료
        if (isTypingPlaying && SoundManager.Instance != null)
        {
            SoundManager.Instance.sfxSource.Stop();
            isTypingPlaying = false;
        }

        // 5️⃣ 버튼들 페이드 인
        yield return StartCoroutine(FadeCanvasGroup(buttonsGroup, 0f, 1f, fadeDuration));
        buttonsGroup.interactable = true;
        buttonsGroup.blocksRaycasts = true;
    }

    // CanvasGroup 페이드
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.unscaledDeltaTime; // TimeScale 0에서도 동작
            yield return null;
        }
        cg.alpha = end;
    }

    // 텍스트 타이핑 + SoundManager 호출 (한 번만 재생)
    private IEnumerator TypeTextWithSound(TMP_Text textComponent, string message, float speed)
    {
        // 타이핑 시작 시 한 번만 사운드 재생
        if (!isTypingPlaying && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayTyping();
            isTypingPlaying = true;
        }

        foreach (char c in message)
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(speed); // TimeScale 0에서도 동작
        }
    }

    // 버튼 클릭 이벤트용
    public void OnRetryButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void OnTitleButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title"); // 타이틀 씬 이름
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}