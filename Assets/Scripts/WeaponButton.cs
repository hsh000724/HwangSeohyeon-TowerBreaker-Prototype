using TMPro;
using UnityEngine;

public class WeaponButton : MonoBehaviour
{
    public int weaponGrade; // 1~9등급 (인스펙터에서 설정)
    public TMP_Text quantityText; // 수량 텍스트 (현재/64)
    public GameObject equippedLabel;

    private WeaponData weaponData;

    // 데이터가 없을 경우(null)를 대비해 grade를 따로 받습니다.
    public void SetWeapon(WeaponData data, int grade)
    {
        weaponData = data;
        weaponGrade = grade;

        if (data != null)
        {
            quantityText.text = $"{data.quantity} / 64";
        }
        else
        {
            quantityText.text = "0 / 64";
        }

        UpdateEquippedLabel();
    }
    public void OnClick()
    {
        // 데이터가 없는 칸(0/64)은 클릭 무시
        if (weaponData == null || weaponData.quantity <= 0)
        {
            Debug.Log($"{weaponGrade}등급 무기가 없어서 장착할 수 없습니다.");
            return;
        }

        Debug.Log($"{weaponGrade}등급 무기 클릭됨!");

        // 1. 장착 처리
        EquipmentManager.Instance.EquipWeapon(weaponData);

        // 2. UI 즉시 갱신
        EquipmentUI ui = Object.FindFirstObjectByType<EquipmentUI>();
        if (ui != null)
        {
            ui.RefreshUI();
        }
    }

    public void UpdateEquippedLabel()
    {
        if (equippedLabel != null)
        {
            // 현재 이 버튼의 무기가 장착된 무기 ID와 일치하는지 확인
            bool isEquipped = EquipmentManager.Instance.IsEquipped(weaponData);
            equippedLabel.SetActive(isEquipped);
        }
    }
}