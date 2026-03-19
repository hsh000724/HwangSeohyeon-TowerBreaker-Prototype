using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button startGameButton;
    public Button openTreasureButton;
    public Button equipmentButton;
    public Button scoreButton;
    public Button quitButton;

    [Header("Panels")]
    public GameObject treasurePanel;    // 보물상자 UI 패널
    public GameObject equipmentPanel;   // 장비 관리 UI 패널
    public GameObject scorePanel;       // 점수 확인 UI 패널

    void Start()
    {
        // 버튼 이벤트 연결
        startGameButton.onClick.RemoveAllListeners();
        startGameButton.onClick.AddListener(StartGame);

        openTreasureButton.onClick.RemoveAllListeners();
        openTreasureButton.onClick.AddListener(OpenTreasurePanel);

        equipmentButton.onClick.RemoveAllListeners();
        equipmentButton.onClick.AddListener(OpenEquipmentPanel);

        scoreButton.onClick.RemoveAllListeners();
        scoreButton.onClick.AddListener(OpenScorePanel);

        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(QuitGame);

        // 패널 초기화
        CloseAllPanels();
    }

    public void CloseAllPanels()
    {
        if (treasurePanel != null) treasurePanel.SetActive(false);
        if (equipmentPanel != null) equipmentPanel.SetActive(false);
        if (scorePanel != null) scorePanel.SetActive(false);
    }

    // -----------------------
    // 버튼 이벤트 함수
    // -----------------------

    void StartGame()
    {
        // 예: GameScene 이름
        SceneManager.LoadScene("GameScene");
    }

    public void OpenTreasurePanel()
    {
        CloseAllPanels();

        if (treasurePanel != null) treasurePanel.SetActive(true);

        // 보유한 상자 개수 표시
        if (PlayerDataManager.Instance != null)
        {
            PlayerData data = PlayerDataManager.Instance.playerData;

            // treasurePanel 내 TextMeshPro 컴포넌트 가져오기
            TMP_Text boxText = treasurePanel.GetComponentInChildren<TMP_Text>();
            if (boxText != null)
            {
                boxText.text = $"TREASURE BOXES : {data.totalTreasureBoxes}";
            }
        }
    }

    void OpenEquipmentPanel()
    {
        CloseAllPanels();
        if (equipmentPanel != null) equipmentPanel.SetActive(true);
    }

    public void OpenScorePanel()
    {
        CloseAllPanels();
        if (scorePanel != null) scorePanel.SetActive(true);

        // 최고 점수 표시
        if (PlayerDataManager.Instance != null)
        {
            PlayerData data = PlayerDataManager.Instance.playerData;
            TMP_Text scoreText = scorePanel.GetComponentInChildren<TMP_Text>();
            if (scoreText != null)
            {
                scoreText.text = $"YOUR SCORE IS : {data.totalScore}";
            }
        }
    }
    public void RefreshTreasureUI()
    {
        if (PlayerDataManager.Instance != null && treasurePanel != null)
        {
            PlayerData data = PlayerDataManager.Instance.playerData;
            TMP_Text boxText = treasurePanel.GetComponentInChildren<TMP_Text>();
            if (boxText != null)
            {
                boxText.text = $"TREASURE BOXES : {data.totalTreasureBoxes}";
            }
        }
    }

    void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}