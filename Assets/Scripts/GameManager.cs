using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{

    // Singleton
    public static GameManager Instance { get; private set; }

    // last or current players name
    private string playerName;

    // last or current players record
    private string playerRecord;

    // Playerlist
    private List<PlayerInfo> playerList = new List<PlayerInfo>();

    // Highscore list
    private List<PlayerInfo> highscoreList;

    // Language
    private Locale currentLocale;

    // Awake is called when the script instance is being loaded
    private void Awake() {

        // Make GameManager a singleton
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        // Do not destroy this Object when loading a new Scene
        DontDestroyOnLoad(gameObject);

        Instance = this;

        // Load saved stuff
        Load();

    }

    public void GotoScene(string sceneName) {

        // Load specified scene
        SceneManager.LoadScene(sceneName);
    }

    // Quit game from native Linux build
    public void QuitGame() {

        // Save if exit game
        Save();

         // original code to quit Unity player
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    // Return player name
    public string GetPlayerName() {
        return playerName;
    }

    // Set player name
    public void SetPlayerName(string name) {

        // if name is different from the last player
        if(name.Equals(playerName) == false) {

            // check list of players
            if(playerList.Exists(x => x.playerName.Contains(name)) == true) {
                // if found then load player info for name
                PlayerInfo playerInfo = playerList.Find(x => x.playerName.Contains(name));
                playerRecord = playerInfo.playerRecord;

            } else {
                // not found = new player
                playerRecord = "0";

                // Add new player to list of players
                playerList.Add(new PlayerInfo(name,playerRecord));

            }
            playerName = name;

            // save new player
            Save();
        }
    }

    // Return player score record
    public string GetPlayerRecord() {
        return playerRecord;
    }

    // Set player score record
    public void SetPlayerRecord(string record) {
        playerRecord = record;

        // update record in playerList
        PlayerInfo playerInfo = playerList.Find(x => x.playerName.Contains(playerName));
        playerInfo.playerRecord = record;

        // Save
        Save();
    }

    public List<PlayerInfo> GetHighscoreList() {

        highscoreList = new List<PlayerInfo>();
 
         // Sort the playerList using their record
        playerList.Sort();

        int players = playerList.Count;

        // Add max 5 to the highscore list
        if (players > 10) {
            players = 10;
        }

        // Add players to the highscore list
        for (int i = 0; i < players; i++){
            highscoreList.Add(playerList[i]);
        }

        return highscoreList;
    }

    public string GetRecordName() {

        if (playerList.Count == 0) {
            return "";
        }
        playerList.Sort();

        return playerList[0].playerName;
    }

    public string GetRecordRecord() {

        if (playerList.Count == 0) {

            return "0";
        }
        playerList.Sort();

        return playerList[0].playerRecord;
    }

    public void ResetHighscoreList() {
        playerList =  new List<PlayerInfo>();

        // not found = new player
        playerRecord = "0";

        // Add the player agin with record = 0 to the list of players
        playerList.Add(new PlayerInfo(playerName,playerRecord));


        // Save the stuff
        Save();
    }

    // Get current locale
    public Locale GetCurrentLocale() {

        // If we don't have a Locale then this is the first run of the app
        // Default language is English, but if Danish then use it.
        if (currentLocale == null) {

            if (LocalizationSettings.SelectedLocale.LocaleName.Equals("Danish (da)") == true ){
                currentLocale = Locale.CreateLocale(SystemLanguage.Danish);
            } else {
                currentLocale = Locale.CreateLocale(SystemLanguage.English);
            }
        }

        return currentLocale;
    }

    // Set current locale
    public void SetCurrentLocale(Locale locale) {

        // remember locale
        currentLocale = locale;

        LocalizationSettings.SelectedLocale = currentLocale;

        // Save
        Save();
    }

    // Load and save code between sessions
    [System.Serializable]
    class SaveData {

         // Name of last/current player
        public string playerNameSave;

        // The player record
        public string playerRecordSave;

        // The selcted language
        public string locale; // language saved by settings

        // List of players
        public List<PlayerInfo> playerListSave = new List<PlayerInfo>();

    }

    public void Save() {
        SaveData data = new SaveData();
        data.playerNameSave = playerName;
        data.playerRecordSave = playerRecord;
        data.playerListSave = playerList;
        data.locale = currentLocale.LocaleName;

        string json = JsonUtility.ToJson(data);
  
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load() {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Load last playername
            playerName = data.playerNameSave;

            // Load the last players record
            playerRecord = data.playerRecordSave;

            // Load playerlist
            playerList = data.playerListSave;

            // Load Locale
            string savedLocale = data.locale;
            if (savedLocale != null && savedLocale.Equals("Danish") == true) {
                currentLocale = Locale.CreateLocale(SystemLanguage.Danish);
                LocalizationSettings.SelectedLocale = currentLocale;
            } else { // Default language is english
                currentLocale = Locale.CreateLocale(SystemLanguage.English);
                LocalizationSettings.SelectedLocale = currentLocale;
            }

        }
    }

}
