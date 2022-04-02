using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;


#if UNITY_EDITOR
    using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{

    // Singleton
    public static GameManager Instance { get; private set; }

    // last or current players name
    public string playerName;

    // last or current players record
    public string playerRecord;

    // Highscorelist
    public List<PlayerInfo> playerList = new List<PlayerInfo>();

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

    // Load and save code between sessions
    [System.Serializable]
    class SaveData {

        public string playerNameSave;
        public string playerRecordSave;

        public List<PlayerInfo> playerListSave = new List<PlayerInfo>();

    }

    public void Save() {
        SaveData data = new SaveData();
        data.playerNameSave = playerName;
        data.playerRecordSave = playerRecord;
        data.playerListSave = playerList;

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

        }
    }

}
