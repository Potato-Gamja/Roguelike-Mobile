using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioMixerScript : MonoBehaviour
{
   
    public Slider bgmSlider;                //배경음악 조절 슬라이더
    public Slider sfxSlider;                //효과음 조절 슬라이더

    void Start()
    {
            
        bgmSlider.value = PlayerPrefs.GetFloat("BGM", 1);                                //슬라이더의 값을 플레이어 프리팹에서 가져오기
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1);

        DontDestroyOnLoad(this.gameObject);                                              //씬 전환 시 파괴되지 않게 하기
    }

    public void SetBGM(float volume)                                                     //오디오 믹서의 배경음악 볼륨 조절, 플레이어 프리팹에 값 저장
    {
        mixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGM", volume);
    }
    public void SetSFX(float volume)                                                     //오디오 믹서의 효과음 볼륨 조절, 플레이어 프리팹에 값 저장
    {
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX", volume);
    }

}
