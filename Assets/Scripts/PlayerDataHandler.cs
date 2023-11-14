using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
