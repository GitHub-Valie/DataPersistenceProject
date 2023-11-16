using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    public Text inputPlayerName;

    public void SetPlayerName(Text textInputField)
    {
        Debug.Log($"Text input: {textInputField.text}");
        PlayerDataHandler.Instance.playerName = textInputField.text;
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
        Debug.Log("The application has closed");
    }
}
