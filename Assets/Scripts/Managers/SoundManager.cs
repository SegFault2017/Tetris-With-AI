using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public bool m_musicEnabled = true;
    public bool m_fxEnabled = true;
    [Range(0, 1)]
    public float m_musicVolume = 1.0f;
    [Range(0, 1)]
    public float m_fxVolume = 1.0f;

    public AudioClip m_clearRowSound;
    public AudioClip m_moveSound;
    public AudioClip m_dropSound;
    public AudioClip m_gameOverSound;

    public AudioClip m_errorSound;

    // public AudioClip m_backgroundMusic;
    public AudioSource m_musicSource;

    //Volcal sound
    public AudioClip[] m_vocalClips;

    public AudioClip[] m_musicClips;

    public AudioClip m_gameOverVocalClip;
    public AudioClip m_lvlUpVocalClip;


    //Icon
    public IconToggle m_musicIconToggle;
    public IconToggle m_fxIconToggle;


    void UpdateMusic()
    {
        if (m_musicSource.isPlaying != m_musicEnabled)
        {
            if (m_musicEnabled)
            {
                PlaybackgroundMusic(GetRandomClip(m_musicClips));
            }
            else
            {
                m_musicSource.Stop();
            }
        }
    }


    // Get a random background music
    public AudioClip GetRandomClip(AudioClip[] clips)
    {
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        return randomClip;
    }

    public void ToggleMusic()
    {
        m_musicEnabled = !m_musicEnabled;
        UpdateMusic();
        if (m_musicIconToggle)
        {
            m_musicIconToggle.ToggleIcon(m_musicEnabled);
        }
    }

    public void ToggleFX()
    {
        m_fxEnabled = !m_fxEnabled;
        if (m_fxIconToggle)
        {
            m_fxIconToggle.ToggleIcon(m_fxEnabled);
        }
    }

    public void PlaybackgroundMusic(AudioClip musicClip)
    {
        //return if music is diabled or if musicSource is null or is musicClip is null
        if (!m_musicEnabled || !musicClip)
        {
            return;
        }

        m_musicSource.Stop();
        m_musicSource.clip = musicClip;
        m_musicSource.volume = m_musicVolume;
        m_musicSource.loop = true;
        m_musicSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaybackgroundMusic(GetRandomClip(m_musicClips));
    }
    // Update is called once per frame
    void Update()
    {
        if (!m_musicEnabled)
        {
            return;
        }
        m_musicSource.volume = m_musicVolume;
    }


}
