using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour     //로비화면 스크립트
{
    public Dropdown languageDown;            //로컬라이제이션을 위한 드롭다운

    public GameObject settingPanel;          //설정창 판넬
    public GameObject developerPanel;        //개발자창 판넬
    public GameObject exitPanel;             //게임종료창 판넬

    public Button[] buttons;                 //버튼 그룹

    private void Awake()
    {
        int index = PlayerPrefs.GetInt("Language");        //플레이어프리팹에서 Language 이름의 int 형을 가져와 대입
        languageDown.value = index;                        //드롭다운의 값에 인덱스 값을 대입
        UserLanguage(index);                               //로컬라이제이션 함수 실행
    }

    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Escape))              //뒤로 가기 버튼 입력 시 실행
        {
            if (settingPanel.activeSelf == false && developerPanel.activeSelf == false && exitPanel.activeSelf == false) 
            {                            //아무런 창도 활성화가 안되어있는 경우
                ExitPanelEnable();       //게임종료 창 활성화
            }
            else
                EnablePanelDisenable();  //활성화된 창 비활성화
        }
    }

    public void GotoGameScene()                        //게임시작 버튼 이벤트인 로딩씬
    {
        LoadingSceneScript.LoadScene("GameScene");
    }

    public void EnablePanelDisenable()                 //활성화된 판넬 오브젝트들 비활성화
    {
        if(settingPanel)                               //설정창 활성화 상태 시 비활성화
        {
            settingPanel.SetActive(false);
        }
        if(developerPanel)                             //개발자창 활성화 상태 시 비활성화
        {
            developerPanel.SetActive(false);
        }
        if(exitPanel)                                  //게임종료창 활성화 상태 시 비활성화
        {
            exitPanel.SetActive(false);
        }
        ButtonEnable();                                  //버튼 기능 활성화 함수
    }
    
    public void UserLanguage(int index)                                                              //로컬라이제이션
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];  //로컬라이제이션세팅의 값을 변경
        PlayerPrefs.SetInt("Language", index);                                                       //플레이어프리팹 Language에 index 값 저장
    }
    
    public void SettingPanelEnable()               //설정 버튼 입력 시
    {
        settingPanel.SetActive(true);              //설정창 활성화
        ButtonDisable();                           //버튼 비활성화
    }

    public void DeveloperPanelEnable()             //개발자 버튼 입력 시
    {
        developerPanel.SetActive(true);
        ButtonDisable();
    }

    public void ExitPanelEnable()                  //게임종료 버튼 입력 시
    {
        exitPanel.SetActive(true);
        ButtonDisable();
    }

    public void ButtonDisable()                    //버튼 기능 비활성화
    {
        buttons[0].interactable = false;
        buttons[1].interactable = false;
        buttons[2].interactable = false;
        buttons[3].interactable = false;
    }

    public void ButtonEnable()                     //버튼 기능 활성화
    {
        buttons[0].interactable = true;
        buttons[1].interactable = true;
        buttons[2].interactable = true;
        buttons[3].interactable = true;
    }

    public void Exit()                             //게임종료
    {
        Application.Quit();
    }
}
