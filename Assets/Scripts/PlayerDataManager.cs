using UnityEngine;
using System.IO;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    public PlayerData playerData;

    private string dataPath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        dataPath = Application.persistentDataPath + "/playerData.json";
        LoadData();
    }

    // 데이터 저장
    public void SaveData()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(dataPath, json);
        Debug.Log("PlayerData 저장 완료");
    }

    // 데이터 로드
    public void LoadData()
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            playerData = new PlayerData(); // 새로 생성
        }
    }

    // 최고 점수 갱신
    public void TryUpdateHighScore(int score)
    {
        if (score > playerData.totalScore)
        {
            playerData.totalScore = score;
            SaveData();
            Debug.Log("최고 점수 갱신: " + score);
        }
    }

    // 상자 수 누적
    public void AddTreasureBoxes(int count)
    {
        playerData.totalTreasureBoxes += count;
        SaveData();
    }
}