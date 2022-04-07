using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SettingsManager : MonoBehaviour
{

    public GameObject highscorePopupDialog;

    public LanguagePicker languagePicker;

    // Start is called before the first frame update
    void Start()
    {

        // Initialize language picker widget
        languagePicker.Init();

        //this will call the NewLanguageSelected function when the language picker have a language button clicked.
        languagePicker.onLanguageChanged += NewLanguageSelected;

        // Select the button with the current locale
        if (GameManager.Instance != null) {
            Locale locale = GameManager.Instance.GetCurrentLocale();

            languagePicker.SetSelectedLanguage(locale);
        }
    }

    public void NewLanguageSelected(Material material)
    {
        if (GameManager.Instance != null) {
            GameManager.Instance.SetCurrentLocale(languagePicker.GetSelectedLanguage());
        }
    }

    public void Update() {



    }


    // return to menu
    public void ReturnToMenu() {

        if (GameManager.Instance != null) {
            GameManager.Instance.GotoScene("Menu");
        }
    }

    // reset highscorelist
    public void ResetHighscoreList() {
        // Reset highscorelist
        GameManager.Instance.ResetHighscoreList();
    }
}
