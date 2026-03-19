using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int totalScore;
    public int totalTreasureBoxes;
    public List<WeaponData> ownedWeapons = new List<WeaponData>();
    public string equippedWeaponID = ""; // 착용중 무기
}