using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

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

    public Dropdown languageDown;
    public GameObject settingPanel;
    public GameObject gameoverPanel;
    
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
        //최대 경험치 설정
        maxExp = playerScript.exp;
        //플레이어 데이터 설정
        playerScript.SetData();
    }

    void Start()
    {    
        //플레이어프리팹에 있는 언어의 값을 가져와 드롭다운 값에 넣기
        int index = PlayerPrefs.GetInt("Language");
        languageDown.value = index;

        //오디오 믹서
        mixer = GameObject.FindWithTag("AudioMixer").GetComponent<AudioMixerScript>();

        //플레이어프리팹에 있는 배경음악과 효과음 값 가져오기
        bgmSlider.value = PlayerPrefs.GetFloat("BGM");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX");

        //타이머 시작
        StartCoroutine(TimerCoroution());
    }

    void Update()
    {
        isSpawn = true; 

        //모바일 뒤로가기 버튼 입력 시 일시정지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingPanel.activeSelf == false && !isOver)
            {
                EnablePause();
            }
        }

        //경험치 게이지가 다 찼을 경우
        if (expBar.fillAmount == 1)
        {
            int level;
            level = playerLevel;

            //레벨에 따른 경험치량 조정
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

        //플레이어의 체력이 0 이하로 떨어지면 게임오버
        if (playerScript.hp <= 0 && !isOver)
        {
            GameOver();
        }
    }

    //게임오버
    void GameOver()
    {   
        //게임오버 시 생존한 시간과 처치한 몬스터의 수의 값 입력
        time_.text = time바
        expBar.fillAmount = curExp / maxExp;
    }
    
    //타이머
    IEnumerator TimerCoroution()
    {    
        var wait = new WaitForSeconds(1f);

        //타이머 1 증가
        time += 1;
        //타이머 텍스트를 자리수를 나누어서 표시
        timerText.text = (time / 60 % 60).ToString("D2") + ":" + (time % 60).ToString("D2");

        //1초 딜레이
        yield return wait;

        StartCoroutine(TimerCoroution());
    }

    //로컬라이제이션 기능
    public void UserLanguage(int index)
    {
        //인덱스 값을 받아 로컬라이제이션의 세팅값을 변경
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        //플레이어프리팹에 설정한 언어 값 저장
        PlayerPrefs.SetInt("Language", index);
    }

    //일시정지
    public void EnablePause()
    {    
        //레벨업해서 무기 능력 선택창이 활성화 되어있을 경우 리턴
        if (levelUpScript.weaponPanel.activeSelf == true)
            return;

        //일시정지
        if (!isClick)
        {
            Time.timeScale = 0f;
            settingPanel.SetActive(true);
            joystick.SetActive(false);
            isClick = true;
        }
        else if(isClick) //일시정지 해제
        {
            Time.timeScale = 1.0f;
            settingPanel.SetActive(false);
            joystick.SetActive(true);
            isClick = false;
        }
    }

    //게임종료
    public void Exit()
    {
        Application.Quit();
    }
    //다시하기 - 로딩씬
    public void ReStart()
    {
        LoadingSceneScript.LoadScene("GameScene");
    }
    //로비화면 - 로딩씬
    public void Title()
    {
        LoadingSceneScript.LoadScene("RobyScene");
    }
}
