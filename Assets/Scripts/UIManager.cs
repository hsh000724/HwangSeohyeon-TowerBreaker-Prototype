using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public PlayerHPUI playerHPUI;
    public RoundUI roundUI;
    public ScoreUI scoreUI;
    public TreasureBoxUI treasureBoxUI; // ✅ 보물상자 UI 추가

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ------------------------
    // 기존 UI 업데이트
    // ------------------------
    public void UpdatePlayerHP(int hp)
    {
        if (playerHPUI != null)
            playerHPUI.UpdateHP(hp);
    }

    public void ShowRound(int round)
    {
        if (roundUI != null)
            roundUI.ShowRound(round);
    }

    public void AddScore(int score)
    {
        if (scoreUI != null)
            scoreUI.AddScore(score);
    }

    // ------------------------
    // 보물상자 UI 업데이트
    // ------------------------
    public void UpdateTreasureBoxUI(int count)
    {
        if (treasureBoxUI != null)
            treasureBoxUI.UpdateBoxCount(count);
    }
}