using UnityEngine;

public static class LocalSaveManager
{
    private const string key = "PlayerData";

    public static void SaveData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public static PlayerData LoadData()
    {
        if (!PlayerPrefs.HasKey(key))
            return new PlayerData();

        string json = PlayerPrefs.GetString(key);
        return JsonUtility.FromJson<PlayerData>(json);
    }
}