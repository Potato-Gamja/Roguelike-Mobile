using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{ 
    public SoundManager SoundManager;
    public PlayerScript playerScript;
    public LevelUpScript levelUpScript;
    public MonManager monManager;

    [SerializeField]
    AudioMixerScript mixer;                  //오디오 믹서 스크립트
    [SerializeField]
    Slider bgmSlider;
    [SerializeField]
    Slider sfxSlider;

    public Sprite[] damageSprite;            //데미지 표시를 위한 스프라이트

    public bool isSpawn = true;
    public bool isOver = false;
    public int time = 0;

    [SerializeField]
    Image expBar;
    float curExp = 0;
    float maxExp;
    float exExp = 0;

    public Image hpBar;

    [SerializeField]
    public int wave = 0;
    [SerializeField]
    Text timerText;

    [SerializeField]
    Text levelText;
    int playerLevel = 1;

    public GameObject joystick;               //조이스틱
    public FloatingJoystick floatJoystick;

    [SerializeField]
    Dropdown languageDown;                    //로컬라이제이션 드롭다운
    [SerializeField]
    GameObject settingPanel;                  //설정창
    [SerializeField]
    GameObject gameoverPanel;                 //게임오버창

    bool isClick = false;                     //정지버튼 눌렀는지 여부 체크
    
    public Text kill_;
    public Text time_;

    public int killCount = 0;

    void Awake()
    {
        maxExp = playerScript.exp;
        //최대 경험치 설정
        maxExp = playerScript.exp;
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

    //경험치 증가
    public void AddExp()
    {
        if (expBar.fillAmount == 1)
            exExp++;
        else
            curExp++;

        expBar.fillAmount = curExp / maxExp;
    }

    //경험치 초기화
    void ResetExp(float newExp)
    {
        curExp = 0;                                //임시 경험치
        maxExp = newExp;                           //필요한 경험치 갱신

        expBar.fillAmount = curExp / maxExp;

        playerLevel++;                             //플레이어 레벨 증가
        levelUpScript.playerData.maxHp++;          //플레이어 최대 체력 증가
        levelUpScript.playerScript.hp++;           //플레이어 현재 체력 증가
        levelUpScript.playerData.offense += 0.5f;  //플레이어 공격력 증가

        playerScript.SetData();                    //플레이어 스탯 재설정
        hpBar.fillAmount = playerScript.hp / playerScript.maxHp; //체력바 조정

        levelText.text = "LV. " + playerLevel;
        levelUpScript.WeaponSelectEvent_0();

        if (exExp >= maxExp)
        {
            curExp = maxExp;
            exExp -= maxExp;
        }

        expBar.fillAmount = curExp / maxExp;
    }
    
    //게임오버
    void GameOver()
    {
        isOver = true;
        gameoverPanel.SetActive(true);
        //게임오버 시 생존한 시간과 처치한 몬스터의 수의 값 문자열화
        time_.text = time.ToString();
        kill_.text = killCount.ToString();
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
        Time.timeScale = 1f;
        LoadingSceneScript.LoadScene("GameScene");
    }
    //로비화면 - 로딩씬
    public void Title()
    {
        LoadingSceneScript.LoadScene("RobyScene");
    }
}
