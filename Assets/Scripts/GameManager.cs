using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool isGameRunning = false;
    public bool isGameOver = false;
    public bool isPaused = false;

    [HideInInspector]
    public int currentRound = 0;

    [Header("Score")]
    public int score = 0;

    [Header("UI")]
    public GameOverUI gameOverUI; // GameOver UI 연결
    public GameObject pausePanel;

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

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (isGameRunning)
        {
            HandlePause();
        }
    }

    void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void StartGame()
    {
        score = 0;
        isGameRunning = true;
        isGameOver = false;

        Time.timeScale = 1f;

        Debug.Log("게임 시작");
        SoundManager.Instance.PlayRoundStart();
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameRunning = false;
        isGameOver = true;

        Time.timeScale = 0f;

        Debug.Log("게임 오버");

        int round = currentRound;
        int boxes = TreasureBoxManager.Instance != null ? TreasureBoxManager.Instance.boxCount : 0;

        // 1️⃣ 최고 점수 갱신
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.TryUpdateHighScore(score); // 최고 점수만 갱신
            PlayerDataManager.Instance.AddTreasureBoxes(boxes);  // 상자 수는 누적
        }

        // GameOverUI 표시
        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver(round, score, boxes);
        }

        // 게임오버 효과음 재생
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameOver();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel.SetActive(isPaused);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title"); // 타이틀씬 이름
    }

    public void AddScore(int amount)
    {
        score += amount; // 점수 중앙 관리
        Debug.Log("Score : " + score);

        // UI 업데이트
        if (UIManager.Instance != null)
            UIManager.Instance.AddScore(amount);
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}