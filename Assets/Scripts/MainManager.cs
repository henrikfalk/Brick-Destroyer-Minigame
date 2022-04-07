//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody ballRigidbody;

    public Ball ballPrefab;
    private GameObject ball;

    public GameObject paddle;

    public TextMeshProUGUI playerText;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI playerRecordText;

    public TextMeshProUGUI bestPlayerRecordNameText;
    public TextMeshProUGUI bestPlayerRecordScoreText;

    public TextMeshProUGUI pressSpaceToText;

    public GameObject GameOverText;
    public Button quitButton;
    public Button retryButton;
    
    private bool m_Started = false;
    private int playerPoints;
    private string playerName = "Anonymous";

    private bool isNewGame;

    private bool playRecordOnce;
    
    // Made public to be able to test different levels in the editor
    [SerializeField]private int level;

    private GameObject[] bricks;

    public Camera mainCamera;
    public Camera paddleCamera;
    
    public AudioSource playerAudio;
    public AudioClip levelSound;
    public AudioClip recordSound;
    public AudioClip gameOverSound;

    // Start is called before the first frame update
    void Start()
    {

        // Remember AudioSource
        playerAudio = GetComponent<AudioSource>();

        // Set Main camera
        mainCamera.enabled = true;
        paddleCamera.gameObject.SetActive(true); // because I have disabled it in the editor
        paddleCamera.enabled = false;

        // create new ball
        ball = Instantiate(ballPrefab.gameObject, ballPrefab.gameObject.transform.position, Quaternion.identity);
        ball.GetComponent<Ball>().mainManager = this;
        ball.SetActive(true);
        ballRigidbody = ball.GetComponent<Rigidbody>();

        // Put some bricks up to destroy
        InstantiateBricks(level);

        // Reset score text
        AddPoint(0);

        // Set ball velocity and paddle size depending on level
        // Used if editor is running and testing higher levels
        SetBallAndPaddle();

        // only if GameManager exists otherwise we are in the editor and run Main directly
        if (GameManager.Instance != null) {

            // Get playername and show the name
            playerName = GameManager.Instance.GetPlayerName();
            playerText.text = playerName;

            // Set player score if saved
            playerRecordText.text = GameManager.Instance.GetPlayerRecord();

            // Set total record name and score
            bestPlayerRecordNameText.text = GameManager.Instance.GetRecordName();
            bestPlayerRecordScoreText.text = GameManager.Instance.GetRecordRecord();
        }

    }

    private void Update()
    {
        // Get the bricks currently alive
        bricks = GameObject.FindGameObjectsWithTag("Brick");

        // If all bricks is destroyed then spawn new bricks at a higher level, if new game
        if (bricks.Length == 0 && isNewGame == false) {
            
            // Increase level
            level++;

            // Set ball velocity and paddle size depending on level
            SetBallAndPaddle();

            // Put some bricks up to deatroy
            InstantiateBricks(level);

            // Play new level sound
            playerAudio.PlayOneShot(levelSound,1f);

            // Let the ball follow Paddle   
            ball.transform.position = paddle.transform.position + new Vector3(0,0.15f,0);
            m_Started = false;
        }

        // remember player record
        int playerRecord = System.Convert.ToInt32(playerRecordText.text);

        // remember current best player record
        int bestPlayerRecord = System.Convert.ToInt32(GameManager.Instance.GetRecordRecord());

        // If personal record is higher player record or best player/record then update UI
        if (playerPoints > playerRecord) {

            // update UI and remember personal record
            if (GameManager.Instance != null) {
                playerRecordText.text = playerPoints.ToString();
                GameManager.Instance.SetPlayerRecord(playerRecordText.text);


                // If player beaten the best player ever
                if (playerPoints > bestPlayerRecord) {

                    // Update UI - The player automatically be remembered
                    bestPlayerRecordNameText.text = playerText.text;
                    bestPlayerRecordScoreText.text = playerPoints.ToString();
                }
            }

            if (playRecordOnce == false) {
                // Play only one time if record is beaten
                playRecordOnce = true;

                // Play record beaten sound
                playerAudio.PlayOneShot(recordSound,1f);
            }
        }

        // if game running then do the ball start stuff
        if (m_Started == false) {
            if (Input.GetKeyDown(KeyCode.Space)) {

                    // Hide instruction text to start game
                    pressSpaceToText.gameObject.SetActive(false);

                    isNewGame = false;
                    m_Started = true;

                    float randomDirection = Random.Range(-1.0f, 1.0f);
                    Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                    forceDir.Normalize();

                    ballRigidbody.transform.SetParent(null);
                    ballRigidbody.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            } else {
                // Let the ball follow Paddle
                ball.transform.position = paddle.transform.position + new Vector3(0,0.15f,0);
            }
        }
        
        // Check if we need to change camera
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            mainCamera.enabled = !mainCamera.enabled;
            paddleCamera.enabled = !paddleCamera.enabled;
        }
    }

    // Called from Brick object before being destroyed
    void AddPoint(int point)
    {
        playerPoints += point;
        playerScoreText.text = playerPoints.ToString();
    }

    public void GameOver()
    {
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

        // find bricks that is still alive
        bricks = GameObject.FindGameObjectsWithTag("Brick");
        
        // remove them
        for (int i = 0; i < bricks.Length; i++) {
            Destroy(bricks[i]);
        }

        // set flag that we want to try a new game which enables the level up stuff
        isNewGame = true;

        // Play record sound if beaten again
        playRecordOnce = false;

        // start at level 1
        level = 1;

        // reset player points
        playerPoints = 0;

        // Reset score text
        AddPoint(0);

        // create new ball
        ball = Instantiate(ballPrefab.gameObject, ballPrefab.gameObject.transform.position, Quaternion.identity);
        ball.GetComponent<Ball>().mainManager = this;
        ball.SetActive(true);
        ballRigidbody = ball.GetComponent<Rigidbody>();

        m_Started = false;

        // Reset the Paddle position
        paddle.transform.SetPositionAndRotation(new Vector3(0,0.7f,0),Quaternion.identity);

        // Reset paddle size and ball velocity
        SetBallAndPaddle();

        // Show instruction text to start game
        pressSpaceToText.gameObject.SetActive(true);


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

    // SetBallAndPaddle
    private void SetBallAndPaddle() {

            // Use the same ball velocity and paddle size for level 1,2,3 and 4
            ball.GetComponent<Ball>().maxVelocity = 3;
            paddle.GetComponent<Paddle>().Speed = 4;

            // Change paddle with and ball velocity depending on level
            switch (level) {
                case 1:
                    paddle.transform.localScale = new Vector3(0.8f, 0.1f, 0.2f);
                    break;
                case 2:
                    paddle.transform.localScale = new Vector3(0.6f, 0.1f, 0.2f);
                    break;
                case 3:
                    paddle.transform.localScale = new Vector3(0.4f, 0.1f, 0.2f);
                    break;
                case 4:
                    paddle.transform.localScale = new Vector3(0.3f, 0.1f, 0.2f);
                    break;
                case 5:
                    paddle.transform.localScale = new Vector3(0.3f, 0.1f, 0.2f);
                    ball.GetComponent<Ball>().maxVelocity = 4;
                    paddle.GetComponent<Paddle>().Speed = 5;
                    break;
                case 6:
                    paddle.transform.localScale = new Vector3(0.3f, 0.1f, 0.2f);
                    ball.GetComponent<Ball>().maxVelocity = 5;
                    paddle.GetComponent<Paddle>().Speed = 6;
                    break;
                case 7:
                    paddle.transform.localScale = new Vector3(0.3f, 0.1f, 0.2f);
                    ball.GetComponent<Ball>().maxVelocity = 6;
                    paddle.GetComponent<Paddle>().Speed = 7;
                    break;
                case 8:
                    paddle.transform.localScale = new Vector3(0.3f, 0.1f, 0.2f);
                    ball.GetComponent<Ball>().maxVelocity = 7;
                    paddle.GetComponent<Paddle>().Speed = 8;
                    break;
                default:
                    paddle.transform.localScale = new Vector3(0.3f, 0.1f, 0.2f);
                    ball.GetComponent<Ball>().maxVelocity = 8;
                    paddle.GetComponent<Paddle>().Speed = 9;
                    break;
            }

    }

    // return to menu
    public void ReturnToMenu() {
        SceneManager.LoadScene("Menu");
    }

}
