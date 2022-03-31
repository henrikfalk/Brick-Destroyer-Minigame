using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class GameManager : MonoBehaviour
{

    // Singleton
    public static GameManager Instance;

    // player name
    private string playerName;

    // Input field for new player name
    public Text playerNameText;

    // Popup
    public GameObject popup;

    // Awake is called when the script instance is being loaded
    private void Awake() {

        // Make GameManager a singleton
        if (Instance != null) {
            return;
        }
        Instance = this;

        // Do not destroy this Object when loading a new Scene
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotoScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void GotoGame() {

        // Get the entered name from the inputfield
        string playerNameInput = playerNameText.text;

        // Trim the name
        char[] charsToTrim = { '*', ' ', '\'', '\\'};
        playerNameInput = playerNameInput.Trim(charsToTrim);

        // If empty string then do nothing
        if (playerNameInput == string.Empty) {
            popup.SetActive(true);
            return;
        }

        // Remember player name
        playerName = playerNameInput;

        // Load game
        SceneManager.LoadScene("Main");

    }

    public string GetPlayerName() {
        return playerName;
    }

}
