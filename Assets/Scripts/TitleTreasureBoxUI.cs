using System.Collections.Generic;
using UnityEngine;

public class TitleTreasureBoxUI : MonoBehaviour
{
    public static TitleTreasureBoxUI Instance;

    public GameObject noBoxWarningPanel;
    public int boxCount = 0;

    private PlayerData playerData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        playerData = LocalSaveManager.LoadData();
    }

    public void OpenAllBoxes()
    {
        // 1. 최신 데이터 불러오기
        playerData = PlayerDataManager.Instance.playerData;

        // 2. 상자가 있는지 확인 (데이터상의 상자 개수 체크)
        if (playerData.totalTreasureBoxes <= 0)
        {
            Debug.LogWarning("열 수 있는 상자가 없습니다!");
            if (noBoxWarningPanel != null)
            {
                noBoxWarningPanel.SetActive(true);
            }
            return;
        }

        // 3. 몇 개를 열지 결정 (보유한 상자 전부)
        int openCount = playerData.totalTreasureBoxes;
        Dictionary<int, WeaponData> summary = new Dictionary<int, WeaponData>();

        for (int i = 0; i < openCount; i++)
        {
            int grade = Random.Range(1, 5); // 1~4 등급 랜덤
            WeaponData newWeapon = new WeaponData($"Weapon_{grade}", grade, 1);

            AddWeapon(newWeapon); // 인벤토리에 추가

            // 결과창 합산 로직
            if (summary.ContainsKey(grade)) summary[grade].quantity++;
            else summary[grade] = new WeaponData($"Weapon_{grade}", grade, 1);
        }

        // 4. 상자 개수 차감 및 데이터 저장
        playerData.totalTreasureBoxes = 0; // 전부 열었으므로 0
        PlayerDataManager.Instance.SaveData();
        TitleManager tm = Object.FindFirstObjectByType<TitleManager>();
        if (tm != null)
        {
            tm.RefreshTreasureUI();
        }

        // 5. 결과 UI 표시
        if (GetWeaponUIManager.Instance != null)
        {
            GetWeaponUIManager.Instance.ShowObtainedWeapons(new List<WeaponData>(summary.Values));
        }
        else
        {
            Debug.LogError("GetWeaponUIManager 인스턴스를 찾을 수 없습니다!");
        }

        // 6. 무기 창이 열려있다면 즉시 갱신
        EquipmentUI ui = Object.FindFirstObjectByType<EquipmentUI>();
        if (ui != null) ui.RefreshUI();
    }

    private void AddWeapon(WeaponData newWeapon)
    {
        WeaponData existing = playerData.ownedWeapons.Find(w => w.grade == newWeapon.grade);

        if (existing != null)
        {
            existing.quantity += newWeapon.quantity;

            // 64개 모이면 자동 업그레이드
            if (existing.quantity >= 64 && existing.grade < 9)
            {
                existing.quantity -= 64;
                AddWeapon(new WeaponData($"Weapon_{existing.grade + 1}", existing.grade + 1, 1));
            }
        }
        else
        {
            playerData.ownedWeapons.Add(newWeapon);
        }
    }
    public void CloseWarningPanel()
    {
        if (noBoxWarningPanel != null)
        {
            noBoxWarningPanel.SetActive(false);
        }
    }
}