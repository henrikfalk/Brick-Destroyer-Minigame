using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody ballRigidbody;

    public GameObject ball;
    public GameObject paddle;

    public Text scoreText;
    public Text playerText;
    public GameObject GameOverText;
    public Button quitButton;
    public Button retryButton;
    
    private bool m_Started = false;
    private int playerPoints;
    private string playerName = "Anonymous";
    
//    private bool m_GameOver = false;

    // Made public to be able to test different levels in the editor
    [SerializeField]private int level;

    private GameObject[] bricks;

    // Start is called before the first frame update
    void Start()
    {

        // create new ball
        GameObject obj = Instantiate(ball, ball.transform.position, Quaternion.identity);
        obj.SetActive(true);
        ballRigidbody = obj.GetComponent<Rigidbody>();

        // Put some bricks up to destroy
        InstantiateBricks(level);

        // Reset score text
        AddPoint(0);

        // Get playername and show the name - only if GameManager exists otherwise we are in the editor and run Main directly
        if (GameManager.Instance != null) {
            playerName = GameManager.Instance.GetPlayerName();
            playerText.text = playerName;
        }
    }

    private void Update()
    {
        // Get the bricks currently alive
        bricks = GameObject.FindGameObjectsWithTag("Brick");

        // If all bricks is destroyed then spawn new bricks at a higher level
        if (bricks.Length == 0) {
            
            // Increase level
            level++;

            // Put some bricks up to deatroy
            InstantiateBricks(level);
        }

        if (!m_Started) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                ballRigidbody.transform.SetParent(null);
                ballRigidbody.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
/*        
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
*/
    }

    // Called from Brick object before being destroyed
    void AddPoint(int point)
    {
        playerPoints += point;
        scoreText.text = $"Score: {playerPoints}";
    }

    public void GameOver()
    {
        // Reset playerpoints
        playerPoints = 0;

        // find bricks that is still alive
        bricks = GameObject.FindGameObjectsWithTag("Brick");
        
        // remove them
        for (int i = 0; i < bricks.Length; i++) {
            Destroy(bricks[i]);
        }

        // Show game over info
        GameOverText.SetActive(true);
        retryButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    public void RetryGame() {

        // Hide game over info
        GameOverText.SetActive(false);
        retryButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        // reset player points
        playerPoints = 0;

        // Reset score text
        AddPoint(0);

        // start at level 1
        level = 1;

        // create new ball
        GameObject obj = Instantiate(ball, ball.transform.position, Quaternion.identity);
        obj.SetActive(true);
        ballRigidbody = obj.GetComponent<Rigidbody>();
        m_Started = false;

        // Reset the Paddle position
        paddle.transform.SetPositionAndRotation(new Vector3(0,0.7f,0),Quaternion.identity);

        // Put some new bricks up to destroy
        InstantiateBricks(level);        

    }

    public void InstantiateBricks(int level) {

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        
        int[] pointCountArray;

        switch (level) {
            case 1:
                pointCountArray = new [] {1,1,2,2,3,3};
                break;
            case 2:
                pointCountArray = new [] {4,4,5,5,6,6};
                break;
            case 3:
                pointCountArray = new [] {7,7,8,8,9,9};
                break;
            default:
            // if level is 6 and higher
                pointCountArray = new [] {Random.Range(1,10),Random.Range(1,10),Random.Range(1,10),Random.Range(1,10),Random.Range(1,10),Random.Range(1,10)};
                break;
        }

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);

                switch (level) {
                    case 4:
                        pointCountArray = new [] {Random.Range(1,10),Random.Range(1,10),8,8,9,9};
                    break;
                    case 5:
                        pointCountArray = new [] {Random.Range(1,10),Random.Range(1,10),Random.Range(1,10),Random.Range(1,10),9,9};
                    break;
                }

                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

    }

    // return to menu
    public void ReturnToMenu() {
        SceneManager.LoadScene("Menu");
    }

}
