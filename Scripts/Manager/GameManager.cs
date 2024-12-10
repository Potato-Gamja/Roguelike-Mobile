using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class GameManager : MonoBehaviour
{ 
    public SoundManager SoundManager;
    public PlayerScript playerScript;
    public LevelUpScript levelUpScript;
    public MonManager monManager;

    public Sprite[] damageSprite;        //데미지 표시를 위한 스프라이트

    public bool isSpawn = true;
    public bo트
    public int time = 0;

    public GameObject joystick;
    public FloatingJoystick floatJoystick;

    [SerializeField]
    Dropdown languageDown;                //로컬라이제이션 드롭다운
    [SerializeField]
    GameObject settingPanel;              //설정창
    [SerializeField]
    GameObject gameoverPanel;             //게임오버창

    bool isClick = false;                 //정지버튼 눌렀는지 여부 체크

    public Text kill_;
    public Text time_;

    public int killCount = 0;

    void Awake()
    {    
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
        Time.timeScale = 1f;
        LoadingSceneScript.LoadScene("GameScene");
    }
    //로비화면 - 로딩씬
    public void Title()
    {
        LoadingSceneScript.LoadScene("RobyScene");
    }
}
