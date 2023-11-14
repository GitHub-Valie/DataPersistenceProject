using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    public InputField inputPlayerName;

    public void SetPlayerName()
    {
        PlayerDataHandler.Instance.playerName = inputPlayerName.text;
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1); // Main scene #1
    }

    public void ToHighScores()
    {
        SceneManager.LoadScene(2); // High score scene #2
    }

    public void Exit()
    {
        /* Exit using conditional compiling */
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit(); // original code to quit Unity player
        #endif
    }
}
