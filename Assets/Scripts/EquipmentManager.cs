using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    // 인스펙터에서 보였던 public PlayerData playerData; 변수를 삭제하거나 아래처럼 바꿉니다.
    // 이제 이 변수는 PlayerDataManager의 데이터를 가리키는 통로가 됩니다.
    public PlayerData CurrentData
    {
        get { return PlayerDataManager.Instance.playerData; }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EquipWeapon(WeaponData weapon)
    {
        if (weapon == null) return;

        // PlayerDataManager에 있는 진짜 데이터에 등급 저장
        CurrentData.equippedWeaponID = weapon.grade.ToString();

        // 저장 후 파일로 기록
        PlayerDataManager.Instance.SaveData();
        Debug.Log($"[장착 완료] 등급: {weapon.grade}");
    }

    public bool IsEquipped(WeaponData weapon)
    {
        if (weapon == null || CurrentData == null) return false;
        return CurrentData.equippedWeaponID == weapon.grade.ToString();
    }

    public int GetEquippedAttackBonus()
    {
        if (CurrentData == null || string.IsNullOrEmpty(CurrentData.equippedWeaponID)) return 0;

        if (int.TryParse(CurrentData.equippedWeaponID, out int grade))
        {
            // 진짜 데이터의 리스트에서 해당 등급 무기를 찾음
            WeaponData w = CurrentData.ownedWeapons.Find(x => x.grade == grade);
            return w != null ? w.attackBonus : 0;
        }
        return 0;
    }
}