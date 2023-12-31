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

    /* UI Text elements */
    public Text ScoreText;
    public Text BestPlayerInfo; // displays the name and score of the record holder
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    // Static variables that store data relative to the player who scored the highest
    [SerializeField] static string bestPlayer;
    [SerializeField] static int bestScore;
    
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

        Debug.Log("MainManager has started");
        PlayerDataHandler.Instance.SaveSessionData();
        DisplayBestPlayerInfo(BestPlayerInfo); // Displays the best player information
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
        /* Upon GameOver:
            - Update the BestPlayerInfo text so it displays the correct info next game
            - Save the session data that must persist 
        */

        m_GameOver = true;
        UpdateBestPlayerInfo();
        PlayerDataHandler.Instance.SaveSessionData();
        GameOverText.SetActive(true);
    }

    private void UpdateBestPlayerInfo()
    {
        /* Perform a check on the value of the score of the current player by 
        comparing it to the value of the bestScore stored in the singleton */
        int currentScore = PlayerDataHandler.Instance.playerScore;
        Debug.Log($"Before updating | Current score: {currentScore}. Best score (static variable): {bestScore}. Best score (in singleton): {PlayerDataHandler.Instance.bestScore}");
        
        if (currentScore > PlayerDataHandler.Instance.bestScore)
        {
            /* Fill the static bestPlayer variable with the name of the player stored in th singleton */
            bestPlayer = PlayerDataHandler.Instance.playerName;
            PlayerDataHandler.Instance.bestPlayer = bestPlayer; // Store this new bestPlayer in the singleton
            
            /* Fill the static bestScore variable with the current score of the player in the singleton */
            bestScore = currentScore;
            PlayerDataHandler.Instance.bestScore = bestScore; // Store this new bestScore in the singleton
        }
        Debug.Log($"After updating | Current score: {currentScore}. Best score (static variable): {bestScore}");
    }

    public void DisplayBestPlayerInfo(Text TextUI)
    {
        /* Displays the TextUI bestPlayer if not null and bestScore if not = 0 */
        // Debug.Log("Display data on best player");
        
        if (PlayerDataHandler.Instance.bestPlayer == null && PlayerDataHandler.Instance.bestScore == 0)
        {
            TextUI.text = "";
            // Debug.Log("A score is yet to be set");
        }
        else
        {
            TextUI.text = $"Best score : {PlayerDataHandler.Instance.bestScore} : {PlayerDataHandler.Instance.bestPlayer}";
            // Debug.Log($"Best Score is {PlayerDataHandler.Instance.bestScore}. Best Player is {PlayerDataHandler.Instance.bestPlayer}");
        }
    }
}
