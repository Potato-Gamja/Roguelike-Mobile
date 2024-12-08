using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolumeScript : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    [SerializeField]
    string type;

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
