using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    // Static variables that store data relative to the player who scored the highest
    private static string bestPlayer;
    private static int bestScore;

    public Text bestPlayerInfo; // display the name and score of the record holder
    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        DisplayBestPlayerInfo(); // Display the best player information
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        PlayerDataHandler.Instance.playerScore = m_Points;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        UpdateBestPlayerInfo(); // Update the best player information
        GameOverText.SetActive(true);
    }

    private void UpdateBestPlayerInfo()
    {
        /* Updates the bestPlayerInfo if the current player has set a new record */
        int currentScore = PlayerDataHandler.Instance.playerScore;
        
        if (currentScore > bestScore)
        {
            bestPlayer = PlayerDataHandler.Instance.playerName;
            bestScore = currentScore;
            bestPlayerInfo.text = $"Best score is {bestScore} by {bestPlayer}";
        }
    }

    private void DisplayBestPlayerInfo()
    {
        /* Displays the bestPlayer/bestScore if it exists/is not =0 */
        if (bestPlayer == null && bestScore == 0)
        {
            bestPlayerInfo.text = "";
        }
        else
        {
            bestPlayerInfo.text = $"Best score is {bestScore} by {bestPlayer}";
        }
    }
}
