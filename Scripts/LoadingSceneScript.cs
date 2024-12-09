using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneScript : MonoBehaviour
{
    public static string nextScene;                    //다음 씬 화면의 문자열을 static으로 선언
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
        AsyncOperation ao = SceneManager.LoadSceneAsync(nextScene);
        ao.allowSceneActivation = false;
        float time = 0.0f;
        while (!ao.isDone)                                                                        //동작이 준비가 안 되어 있으면 반복
        {
            yield return null;
            time += Time.deltaTime;
            if (ao.progress < 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, ao.progress, time);   //선형보간으로 포
                if (loadingBar.fillAmount >= ao.progress)
                {
                    time = 0f;
                }
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, time);
                if (loadingBar.fillAmount == 1.0f)
                {
                    ao.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
