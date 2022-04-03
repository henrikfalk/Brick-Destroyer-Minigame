using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighscoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        // Return if we are running inside the editor
        if (GameManager.Instance == null) {
            return;
        }

        // Get the highscorelist
        List<PlayerInfo> highscoreList = GameManager.Instance.GetHighscoreList();

        // Hide player positions if less than 10
        for (int i = highscoreList.Count; i < 10; i++) {

            // hide player position label
            string tmp = "PlayerNumberText" + (i + 1);
            GameObject.Find(tmp).SetActive(false);

        }

        // Show name and palyer record
        for (int i = 0; i < highscoreList.Count; i++) {
            // Show name
            string tmp = "PlayerText" + (i + 1);
            GameObject.Find(tmp).GetComponent<TextMeshProUGUI>().text = highscoreList[i].playerName;

            // Show record
            tmp = "ScoreText" + (i + 1);
            GameObject.Find(tmp).GetComponent<TextMeshProUGUI>().text = highscoreList[i].playerRecord;            
        }

    }

    // return to menu
    public void ReturnToMenu() {
        if (GameManager.Instance != null) {
            GameManager.Instance.GotoScene("Menu");
        }
    }

}
