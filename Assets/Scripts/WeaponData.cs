using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string weaponID;   // 무기 고유 ID
    public int grade;         // 1~9 등급
    public int quantity;      // 보유 수
    public int attackBonus;   // 공격력

    public WeaponData(string id, int grade, int qty)
    {
        this.weaponID = id;
        this.grade = grade;
        this.quantity = qty;
        this.attackBonus = (int)Mathf.Pow(2, grade - 1); // grade 1 -> 1, grade 2 -> 2, grade 3 -> 4 ...
    }
}