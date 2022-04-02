using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{

    // Input field for new player name
    public GameObject playerNameInputField;

    // Popup
    public GameObject popup;

    // Quit button only in Linux native app
    public GameObject quitButton;

    // Start is called before the first frame update
    void Start()
    {

        // Set last player of the game in the InputField
        playerNameInputField.GetComponent<TMP_InputField>().text = GameManager.Instance.GetPlayerName();

        // set focus in InputField
        playerNameInputField.GetComponent<TMP_InputField>().ActivateInputField();

        // Add Quit button if we are on Linux native
        if (Application.platform == RuntimePlatform.LinuxPlayer) {
            quitButton.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update() {

    }

    public void GotoGame() {

        // Get the entered name from the inputfield
        string playerNameInput =  playerNameInputField.GetComponent<TMP_InputField>().text;

        // Trim the name
        char[] charsToTrim = { '*', ' ', '\'', '\\'};
        playerNameInput = playerNameInput.Trim(charsToTrim);

        // If empty string then don't start game
        if (playerNameInput.Length == 0) {
            popup.SetActive(true);
            return;
        }

        // Set playername in GameManager
        GameManager.Instance.SetPlayerName(playerNameInput);

        // Load game
        SceneManager.LoadScene("Main");

    }

    // Quit game from native Linux build
    public void QuitGame() {

        // Save if exit game
        GameManager.Instance.Save();

         // original code to quit Unity player
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
