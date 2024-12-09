using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneScript : MonoBehaviour
{
    public static string nextScene;                    //다음씬 화면의 문자열을 static으로 선언
    [SerializeField]
    Image loadingBar;                                 //로딩 게이지바

    private void Start()
    {
        StartCoroutine(LoadScene());                   //코루틴으로 로딩씬 함수 실행
    }

    public static void LoadScene(string sceneName)
    {    
        nextScene = sceneName;                         //다음씬 문자열에 전달 받은 씬이름 값을 대입
        SceneManager.LoadScene("LoadScene");           //로드씬 실행
    }

    IEnumerator LoadScene()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(nextScene);                             //로드할 씬을 대입
        ao.allowSceneActivation = false;                                                        //씬이 준비 되면 활성화를 할 것인지에 대한 여부
        float time = 0.0f;                                                                      //타임을 0으로 선언 및 초기화
        while (!ao.isDone)                                                                      //동작이 준비가 안 되어 있으면 반복
        {
            yield return null;
            time += Time.deltaTime;
            if (ao.progress < 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, ao.progress, time);   //선형보간으로 로딩바의 게이지 값과 로딩 작업의 진행정도를 시간에 흐름에 따라 자연스럽게 한다
                if (loadingBar.fillAmount >= ao.progress)                                       //게이지의 값이 진행도보다 클 경우 타임을 0으로 초기화
                {
                    time = 0f;                                                                   
                }
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, time);
                if (loadingBar.fillAmount == 1.0f)
                {
                    ao.allowSceneActivation = true;                                             //로딩이 완료 시 씬을 즉시 활성화한다
                    yield break;
                }
            }
        }
    }
}
