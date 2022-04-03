using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // return to menu
    public void ReturnToMenu() {

        if (GameManager.Instance != null) {
            GameManager.Instance.GotoScene("Menu");
        }
    }
}
