using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    
    public AudioSource[] weaponSounds;
    [SerializeField]
    GameObject setting;
    [SerializeField]
    AudioClip mon;
    [SerializeField]
    AudioSource monDead;

    [SerializeField]
    AudioClip blastClip;

    [SerializeField]
    AudioSource levelup;

    public AudioClip open;                //열기 버튼
    public AudioClip close;               //닫기 버튼

    public AudioSource bgm;               //배경음악
    public AudioSource openSource;        //열기
    public AudioSource closeSource;       //닫기

    float time;                           //소리 중첩 방지를 위한 타이머

    private void Update()
    {
        time += Time.deltaTime;

        if (gameManager == null)
            return;

        if (gameManager.isOver)
        {
            bgm.Pause();                  //배경음악 정지
            StopSound();                  //무기 소리 중단
        }
            
    }

    void LateUpdate()
    {
        if (gameManager == null)
            return;

        if(Time.timeScale == 0)           //일시정지 상태 시 무기 사운드 정지
        {
            PauseSound();
        }
        else if(Time.timeScale == 1)      //일시정지 해제 시 무기 사운드 정지해제
        {
            UnPauseSound();
        }
    }

    public void PauseSound()              //무기 사운드 정지
    {
        weaponSounds[0].Pause();
        weaponSounds[1].Pause();
        weaponSounds[2].Pause();
        weaponSounds[3].Pause();
    }
    public void StopSound()               //무기 사운드 중지
    {
        weaponSounds[0].Stop();
        weaponSounds[1].Stop();
        weaponSounds[2].Stop();
        weaponSounds[3].Stop();
    }

    public void UnPauseSound()            //무기 사운드 정지 해제
    {
        weaponSounds[0].UnPause();
        weaponSounds[1].UnPause();
        weaponSounds[2].UnPause();
        weaponSounds[3].UnPause();
    }

    public void PlayMissileSound()        //미사일 사운드 재생
    {
        weaponSounds[0].Play();
    }
    public void PlaySwordSound()          //마법검 사운드 재생
    {
        weaponSounds[1].Play();
    }
    public void PlayLaserSound()          //레이저 사운드 재생
    {
        weaponSounds[2].Play();
    }
    public void PlayBlastSound()          //블래스트 사운드 재생
    {
        weaponSounds[3].PlayOneShot(blastClip);    //블래스트 오브젝트가 여러 개이므로 사운드 동시 출력 가능하게
    }

    public void PlayMonDeadSound()                 //몬스터 사망 사운드 재생
    {
        if(time > 0.05f)                           //여러마리가 한번에 사망 시 사운드가 증폭되고 깨지는 것을 방지하기 위한 타이머
        {
            time = 0;
            monDead.PlayOneShot(mon);
        }
    }

    public void StopSwordSound()          //마법검 사운드 중지
    {
        weaponSounds[1].Stop();
    }
    public void StopLaserSound()          //레이저 사운드 중지
    {
        weaponSounds[2].Stop();
    }

    public void LevelUpSound()            //레벨업 사운드 재생
    {
        levelup.Play();
    }
    public void UIOpen()                  //열기 버튼 사운드 재생
    {
        if (setting.activeSelf == false)  //게임화면 일시정지 버튼 연속 클릭 사운드 재생 방지
            UIClose();
        else
            openSource.PlayOneShot(open);
    }

    public void UIOpen_()                //열기 버튼 사운드 재생
    {
            openSource.PlayOneShot(open);
    }
    public void UIClose()                //닫기 버튼 사운드 재생
    {
        closeSource.PlayOneShot(close);
    }
}
