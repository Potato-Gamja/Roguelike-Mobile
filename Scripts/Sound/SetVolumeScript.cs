using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolumeScript : MonoBehaviour
{
    public AudioMixer mixer;            //오디오 믹서
    public Slider slider;               //슬라이더
    [SerializeField]
    string type;                        //오디오 종류

    void Start()
    {
        if (type == "BGM")
            slider.value = PlayerPrefs.GetFloat("BGM");
        if (type == "SFX")
            slider.value = PlayerPrefs.GetFloat("SFX");
    }

    public void SetBGM(float volume)
    {
        mixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGM", volume);
    }
    public void SetSFX(float volume)
    {
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX", volume);
    }
}
