using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using System;

public class GameManager : MonoBehaviour
{ 
    public SoundManager SoundManager;
    public PlayerScript playerScript;
    public LevelUpScript levelUpScript;
    [SerializeField]
    MonManager monManager;

    public Sprite[] damageSprite;

    public GameObject[] weapon;
    public int weaponType;

    float monTime = 0;
    int monCount = 0;
    float spawnTime = 0;
    public bool isSpawn = true;
    public bool isOver = false;

    [SerializeField]
    Image expBar;
    float curExp = 0;
    float maxExp;
    float exExp = 0;

    [SerializeField]
    Text levelText;
    int playerLevel = 1;


    public Slider bgmSlider;
    public Slider sfxSlider;

    public Image hpBar;
    bool isDead = false;

    [SerializeField]
    public int wave = 0;

    AudioMixerScript mixer;

    [SerializeField]
    Text timerText;
    public int time = 0;

    public GameObject joystick;
    public FloatingJoystick floatJoystick;

    [SerializeField]
    Dropdown languageDown;
    [SerializeField]
    GameObject settingPanel;
    [SerializeField]
    GameObject gameoverPanel;
    public Text expText;
    public Text hpText;
    public Text defenseText;
    public Text offenseText;
    public Text attackdelayText;
    public Text speedText;
    public Text criticalText;
    bool isClick = false;

    public Text kill_;
    public Text time_;

    public int killCount = 0;

    void Awake()
    {
        SetExp();
        playerScript.SetData();
    }

    void Start()
    {
        int index = PlayerPrefs.GetInt("Language");
        languageDown.value = index;

        mixer = GameObject.FindWithTag("AudioMixer").GetComponent<AudioMixerScript>();
        bgmSlider.value = PlayerPrefs.GetFloat("BGM");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX");
        StartCoroutine(TimerCoroution());
    }

    void Update()
    {
        timerText.text = (time / 60 % 60).ToString("D2") + ":" + (time % 60).ToString("D2");
        isSpawn = true; 
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingPanel.activeSelf == false && !isOver)
            {
                EnablePause();
            }
        }

        if (expBar.fillAmount == 1)
        {
            int level;
            level = playerLevel;

            if (10 < level && level <= 15)
                level = 10;
            else if (15 < level && level <= 20)
                level = 11;
            else if (20 < level && level <= 25)
                level = 12;
            else if (25 < level && level <= 30)
                level = 13;
            else if (30 < level && level <= 35)
                level = 14;
            else if (35 < level)
                level = 15;

            ResetExp(levelUpScript.playerData.exps[level]);
        }

        if (playerScript.hp <= 0 && !isOver)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        time_.text = time + "s";
        kill_.text = killCount.ToString();
        isOver = true;
        
        gameoverPanel.SetActive(true);
    }

    void SetExp()
    {
        maxExp = playerScript.exp;
    }

    //경험치
    public void AddExp()
    {
        if (expBar.fillAmount == 1)
            exExp++;
        else
            curExp++;

        expBar.fillAmount = curExp/maxExp;
        
    }

    void ResetExp(float newExp)
    {
        curExp = 0;
        maxExp = newExp;

        expBar.fillAmount = curExp / maxExp;

        playerLevel++;
        levelUpScript.playerData.maxHp++;
        if (levelUpScript.playerScript.hp + 2 > levelUpScript.playerData.maxHp)
            levelUpScript.playerScript.hp++;
        else if(levelUpScript.playerScript.hp + 2 < levelUpScript.playerData.maxHp)
        {
            levelUpScript.playerScript.hp += 2;
        }
        levelUpScript.playerData.offense += 0.5f;

        playerScript.SetData();
        hpBar.fillAmount = playerScript.hp / playerScript.maxHp;

        levelText.text = "LV. " + playerLevel;
        SoundManager.LevelUpSound();
        levelUpScript.WeaponSelectEvent_0();

        if(exExp >= maxExp)
        {
            curExp = maxExp;
            exExp -= maxExp;
        }

        expBar.fillAmount = curExp / maxExp;
    }
    
    //타이머
    IEnumerator TimerCoroution()
    {
        var wait = new WaitForSeconds(1f);

        time += 1;
        if(time >= 0)
            timerText.text = (time / 60 % 60).ToString("D2") + ":" + (time % 60).ToString("D2");

        yield return wait;

        StartCoroutine(TimerCoroution());
    }

    public void UserLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        PlayerPrefs.SetInt("Language", index);
    }

    public void EnablePause()
    {
        if (levelUpScript.weaponPanel.activeSelf == true)
            return;

        if (!isClick)
        {
            Time.timeScale = 0f;
            settingPanel.SetActive(true);
            joystick.SetActive(false);
            isClick = true;
        }
        else if(isClick)
        {
            Time.timeScale = 1.0f;
            settingPanel.SetActive(false);
            joystick.SetActive(true);
            isClick = false;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ReStart()
    {
        LoadingSceneScript.LoadScene("GameScene");
    }

    public void Title()
    {
        LoadingSceneScript.LoadScene("RobyScene");
    }
}
