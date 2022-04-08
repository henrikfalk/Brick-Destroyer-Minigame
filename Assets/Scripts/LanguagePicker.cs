using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguagePicker : MonoBehaviour
{
    public Material[] availableLanguages;
    public Button LanguageButtonPrefab;
    
    public Material selectedMaterial { get; private set; }
    public System.Action<Material> onLanguageChanged;

    List<Button> m_LanguageButtons = new List<Button>();

    // Start is called before the first frame update
    public void Init() {
        foreach (var material in availableLanguages) {
            var newButton = Instantiate(LanguageButtonPrefab, transform);
            newButton.GetComponent<Image>().material = material;

            newButton.onClick.AddListener(() =>
            {
                selectedMaterial = material;
                foreach (var button in m_LanguageButtons)
                {
                    button.interactable = true;
                }

                newButton.interactable = false;
                
                onLanguageChanged.Invoke(selectedMaterial);
            });
            
            m_LanguageButtons.Add(newButton);
        }

    }

    public void SelectMaterial(Material material)
    {
        for (int i = 0; i < availableLanguages.Length; ++i) {
            if (availableLanguages[i] == material) {
                m_LanguageButtons[i].onClick.Invoke();
            }
        }
    }

    public Locale GetSelectedLanguage() {
        
        // If Danish language
        if (selectedMaterial.name.Equals("Flag da") == true) {
            return Locale.CreateLocale(SystemLanguage.Danish);
        }

        // Default is English
        return Locale.CreateLocale(SystemLanguage.English);
    }

    // Select the language in the UI
    public void SetSelectedLanguage(Locale locale) {

        // if danish locale then the find the danish material by name
        if (locale.LocaleName.Equals("Danish") == true) {
            for (int i = 0; i < availableLanguages.Length; i++) {
                if (availableLanguages[i].name.Equals("Flag da") == true) {
                    selectedMaterial = availableLanguages[i];
                    SelectMaterial(availableLanguages[i]);
                    return;
                }
            }
        }

        // Default is english
            for (int i = 0; i < availableLanguages.Length; i++) {
                if (availableLanguages[i].name.Equals("Flag us") == true) {
                    selectedMaterial = availableLanguages[i];
                    SelectMaterial(availableLanguages[i]);
                    return;
                }
            }
    }
}