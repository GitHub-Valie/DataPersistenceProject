using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataHandler : MonoBehaviour
{
    public static PlayerDataHandler Instance;
    public InputField InputPlayerName;
    public string placeholderName;
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

        LoadLastPlayerName();
        SetPlaceholderName(InputPlayerName);
    }

    [System.Serializable]
    class SaveData
    {
        public string inputtedPlayerName;
    }

    public void SaveSessionData()
    {
        /* Stores the player name to load it as text in placeholder next session */
        SaveData data = new SaveData();
        data.inputtedPlayerName = playerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadLastPlayerName()
    {
        /* Loads session data */
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            /* Sets the name inputted by the player from last session as placeholder */
            placeholderName = data.inputtedPlayerName;
            // Debug.Log($"{placeholderName} played last session");
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
}
