using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataHandler : MonoBehaviour
{
    public static PlayerDataHandler Instance;
    public InputField InputPlayerName;
    public Text BestPlayerInfo;
    public string placeholderName;
    public string playerName;
    public string bestPlayer;
    public int bestScore;
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

        LoadLastPlayerName();
        SetPlaceholderName(InputPlayerName);

        LoadBestPlayerScore();
        DisplayBestPlayerInfo(BestPlayerInfo);
    }

    [System.Serializable]
    class SaveData
    {
        public string inputtedPlayerName;
        public string bestPlayer;
        public int bestScore;
    }

    public void SaveSessionData()
    {
        /* Stores the player name to load it as text in placeholder next session */
        SaveData data = new SaveData();
        data.inputtedPlayerName = playerName;
        data.bestPlayer = bestPlayer;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadLastPlayerName()
    {
        /* Loads data from last session */
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            /* Sets the name inputted by the player from last session as the placeholder name */
            placeholderName = data.inputtedPlayerName;
            // Debug.Log($"{placeholderName} played last session");
        }
    }

    public void LoadBestPlayerScore()
    {
        /* Loads data from last session */
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            /* Loads the saved bestPlayer and bestScore back into the singleton */
            bestPlayer = data.bestPlayer;
            bestScore = data.bestScore;
            
            Debug.Log($"Best Score: {bestPlayer} : {bestScore}");
        }
    }

    public void SetPlaceholderName(InputField inputField)
    {
        /* Uses the last player name as a placeholder in the input text field */
        
        // Debug.Log($"Placeholder before if statement: {inputField.placeholder.GetComponent<Text>().text}");
        
        if (Instance.placeholderName != null)
        {
            inputField.placeholder.GetComponent<Text>().text = Instance.placeholderName;
            // Debug.Log($"Last player {Instance.placeholderName} set as placeholder");
        }
        else
        {
            inputField.placeholder.GetComponent<Text>().text = "Enter name";
            // Debug.Log("No player name to set as placeholder");
        }
    }

    public void SetPlayerName(InputField inputField)
    {     
        inputField.text = playerName;       
        // Debug.Log($"Player name is set as {Instance.playerName} for this session");
    }

    public void DisplayBestPlayerInfo(Text TextUI)
    {
        if (bestPlayer == null && bestScore == 0)
        {
            TextUI.text = "";
        }
        else
        {
            TextUI.text = $"Best Score: {bestPlayer} : {bestScore}";
        }
    }
}
