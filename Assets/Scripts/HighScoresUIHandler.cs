using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoresUIHandler : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene(0); // Menu scene #0
    }
}
