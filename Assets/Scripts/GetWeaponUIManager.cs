using System.Collections.Generic;
using UnityEngine;
using TMPro; // TMP 사용을 위해 추가

public class GetWeaponUIManager : MonoBehaviour
{
    public static GetWeaponUIManager Instance;

    public GameObject obtainedWeaponPanel; // 결과창 패널
    public TMP_Text resultText;            // 스크린샷의 "Weapon1 * 1..."이 나올 텍스트

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        obtainedWeaponPanel.SetActive(false);
    }

    public void ShowObtainedWeapons(List<WeaponData> weapons)
    {
        obtainedWeaponPanel.SetActive(true);

        // 결과 문자열 만들기
        string finalResult = "";

        foreach (WeaponData w in weapons)
        {
            // 스크린샷 형식: Weapon등급 * 수량
            finalResult += $"Weapon{w.grade} * {w.quantity}\n";
        }

        // 텍스트 컴포넌트에 할당
        resultText.text = finalResult;
    }

    // OK 버튼 누르면 닫기용 함수
    public void ClosePanel()
    {
        obtainedWeaponPanel.SetActive(false);
    }
}