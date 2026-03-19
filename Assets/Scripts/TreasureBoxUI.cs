using UnityEngine;
using TMPro;

public class TreasureBoxUI : MonoBehaviour
{
    public TMP_Text boxCountText;

    // UI 업데이트
    public void UpdateBoxCount(int count)
    {
        if (boxCountText != null)
            boxCountText.text = $"{count}";
    }
}