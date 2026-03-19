using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    // 인스펙터에서 1~9등급 순서대로 무기 오브젝트들을 넣어주세요.
    public List<WeaponButton> weaponButtons;

    void OnEnable()
    {
        RefreshUI();
    }
    public void RefreshUI()
    {
        PlayerData data = PlayerDataManager.Instance.playerData;
        if (data == null) return;

        foreach (var btn in weaponButtons)
        {
            // 내 인벤토리에서 해당 등급의 무기를 찾음
            WeaponData found = data.ownedWeapons.Find(w => w.grade == btn.weaponGrade);

            // 버튼에 데이터 전달 (없으면 null 전달됨)
            btn.SetWeapon(found, btn.weaponGrade);

            // 장착 상태 새로고침
            btn.UpdateEquippedLabel();
        }
    }
}