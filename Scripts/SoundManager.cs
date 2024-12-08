using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using Unity.VisualScripting;
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

    public AudioClip open;
    public AudioClip close;

    public AudioSource bgm;
    public AudioSource openSource;
    public AudioSource closeSource;

    public AudioSource bgmDead;

    float time;

    private void Update()
    {
        time += Time.deltaTime;

        if (gameManager == null)
            return;

        if (gameManager.isOver)
        {
            bgmDead.Pause();
            StopSound();
        }
            
    }

    void LateUpdate()
    {
        if (gameManager == null)
            return;

        if(Time.timeScale == 0)
        {
            PauseSound();
        }
        else if(Time.timeScale == 1)
        {
            UnPauseSound();
        }
    }

    public void PauseSound()
    {
        weaponSounds[0].Pause();
        weaponSounds[1].Pause();
        weaponSounds[2].Pause();
        weaponSounds[3].Pause();
    }
    public void StopSound()
    {
        weaponSounds[0].Stop();
        weaponSounds[1].Stop();
        weaponSounds[2].Stop();
        weaponSounds[3].Stop();
    }

    public void UnPauseSound()
    {
        weaponSounds[0].UnPause();
        weaponSounds[1].UnPause();
        weaponSounds[2].UnPause();
        weaponSounds[3].UnPause();
    }

    public void PlayMissileSound()
    {
        weaponSounds[0].Play();
    }
    public void PlaySwordSound()
    {
        weaponSounds[1].Play();
    }
    public void PlayLaserSound()
    {
        weaponSounds[2].Play();
    }
    public void PlayBlastSound()
    {
        weaponSounds[3].PlayOneShot(blastClip);
    }

    public void PlayMonDeadSound()
    {
        if(time > 0.05f)
        {
            time = 0;
            monDead.PlayOneShot(mon);
        }
    }

    public void StopSwordSound()
    {
        weaponSounds[1].Stop();
    }
    public void StopLaserSound()
    {
        weaponSounds[2].Stop();
    }

    public void LevelUpSound()
    {
        levelup.Play();
    }
    public void UIOpen()
    {
        if (setting.activeSelf == false)
            UIClose();
        else
            openSource.PlayOneShot(open);
    }

    public void UIOpen_()
    {
            openSource.PlayOneShot(open);
    }
    public void UIClose()
    {
        closeSource.PlayOneShot(close);
    }
}