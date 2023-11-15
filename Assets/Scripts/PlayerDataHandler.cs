using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataHandler : MonoBehaviour
{
    public static PlayerDataHandler Instance;
    public string playerName;
    public int playerScore;
    private void Awake()
    {
        /* Singleton pattern: Only one instance of 
        PlayerDataHandler can exist at any time */
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadInputtedPlayerName();
    }

    [System.Serializable]
    class SaveData
    {
        public string inputtedPlayerName;
    }

    public void SaveInputtedPlayerName()
    {
        // Saves the text inputted by the player in the input field
        SaveData data = new SaveData();
        data.inputtedPlayerName = playerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadInputtedPlayerName()
    {
        // Loads session data
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Sets the placeholder text in the input field equal to the saved player name
            Debug.Log(data.inputtedPlayerName);
        }
    }

}
