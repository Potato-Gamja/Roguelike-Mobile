using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour
{
    public Dropdown languageDown;

    public GameObject settingPanel;
    public GameObject developerPanel;
    public GameObject exitPanel;

    public Button[] buttons;

    public int setWidth = 1080;
    public int setHeight = 1920;

    public static Locale language;

    private void Awake()
    {
        int index = PlayerPrefs.GetInt("Language");
        languageDown.value = index;
        UserLanguage(index);
    }

    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingPanel.activeSelf == false && developerPanel.activeSelf == false && exitPanel.activeSelf == false)
            {
                ExitPanelEnable();
            }
            else
                EnablePanelDisenable();
        }
    }

    public void GotoGameScene()
    {
        LoadingSceneScript.LoadScene("GameScene");
    }

    public void EnablePanelDisenable()
    {
        if(settingPanel)
        {
            settingPanel.SetActive(false);
        }
        if(developerPanel)
        {
            developerPanel.SetActive(false);
        }
        if(exitPanel)
        {
            exitPanel.SetActive(false);
        }
        ButtonAble(); 
    }

    public void UserLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        PlayerPrefs.SetInt("Language", index);
    }
    
    public void SettingPanelEnable()
    {
        settingPanel.SetActive(true);
        ButtonDisable();
    }

    public void DeveloperPanelEnable()
    {
        developerPanel.SetActive(true);
        ButtonDisable();
    }

    public void ExitPanelEnable()
    {
        exitPanel.SetActive(true);
        ButtonDisable();
    }

    public void ButtonDisable()
    {
        buttons[0].interactable = false;
        buttons[1].interactable = false;
        buttons[2].interactable = false;
        buttons[3].interactable = false;
    }

    public void ButtonAble()
    {
        buttons[0].interactable = true;
        buttons[1].interactable = true;
        buttons[2].interactable = true;
        buttons[3].interactable = true;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
