using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioMixerScript : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider bgmSlider;
    public Slider sfxSlider;

    [SerializeField]
    GameObject settingPanel;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            bgmSlider = GameObject.FindWithTag("BGMSlider").GetComponent<Slider>();
            bgmSlider = GameObject.FindWithTag("SFXSlider").GetComponent<Slider>();
        }
            
        bgmSlider.value = PlayerPrefs.GetFloat("BGM", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1);

        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
            settingPanel = null;
        }

        DontDestroyOnLoad(this.gameObject);
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
