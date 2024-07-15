﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource m_MusicAudio;
    AudioSource m_SoundAudio;

    private float SoundVolume
    {
        get { return PlayerPrefs.GetFloat("SoundVolume", 1.0f); }
        set
        {
            m_SoundAudio.volume = value;
            PlayerPrefs.SetFloat("SoundVolume", value);
        }
    }
    private float MusicVolume
    {
        get { return PlayerPrefs.GetFloat("MusicVolume", 1.0f); }
        set
        {
            m_MusicAudio.volume = value;
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }

    void Awake()
    {
        m_MusicAudio = this.gameObject.AddComponent<AudioSource>();
        m_MusicAudio.playOnAwake = false;
        m_MusicAudio.loop = true;

        m_SoundAudio = this.gameObject.AddComponent<AudioSource>();
        m_SoundAudio.loop = false;
    }

    //播放音乐
    public void PlayMusic(string name)
    {
        if (this.MusicVolume < 0.1f)
            return;
        string oldName = "";
        if (m_MusicAudio.clip != null)
            oldName = m_MusicAudio.clip.name;
        //相同的音乐不重复播放
        if (oldName == name)
        {
            m_MusicAudio.Play();
            return;
        }

        Manager.Resource.LoadMusic(name, (UnityEngine.Object obj) =>
        {
            m_MusicAudio.clip = obj as AudioClip;
            m_MusicAudio.Play();
        });
    }

    //暂停音乐
    public void PauseMusic()
    {
        m_MusicAudio.Pause();
    }

    //继续播放
    public void OnUnPauseMusic()
    {
        m_MusicAudio.UnPause();
    }

    //停止音乐
    public void StopMusic()
    {
        m_MusicAudio.Stop();
    }

    //播放音效
    public void PlaySound(string name)
    {
        if (this.SoundVolume < 0.1f)
            return;

        Manager.Resource.LoadSound(name, (UnityEngine.Object obj) =>
        {
            m_SoundAudio.PlayOneShot(obj as AudioClip);
        });
    }

    //设置背景音乐音量
    public void SetMusicVolume(float value)
    {
        this.MusicVolume = value;
    }
    //设置音效音量
    public void SetSoundVolume(float value)
    {
        this.SoundVolume = value;
    }
}
