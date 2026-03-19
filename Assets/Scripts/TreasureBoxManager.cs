using System.Collections.Generic;
using UnityEngine;

public class TreasureBoxManager : MonoBehaviour
{
    public static TreasureBoxManager Instance;

    [Header("Settings")]
    public GameObject treasureBoxPrefab;
    public float dropChance = 0.3f; // 드랍 확률

    [Header("Tracking")]
    public List<TreasureBox> droppedBoxes = new List<TreasureBox>();
    public int boxCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 적 처치 시 드랍 시도
    public void TryDropBox(Vector3 position)
    {
        if (Random.value <= dropChance)
        {
            GameObject boxGO = Instantiate(treasureBoxPrefab, position, Quaternion.identity);
            TreasureBox box = boxGO.GetComponent<TreasureBox>();
            droppedBoxes.Add(box);
        }
    }

    // 라운드 클리어 시 모든 보물상자 습득
    public void CollectAllBoxes()
    {
        foreach (TreasureBox box in droppedBoxes)
        {
            if (box != null && box.gameObject.activeSelf)
            {
                box.Collect();
                boxCount++;
            }
        }

        droppedBoxes.Clear();

        // UI 업데이트
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateTreasureBoxUI(boxCount);
    }
}