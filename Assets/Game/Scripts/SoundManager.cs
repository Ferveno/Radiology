using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;

[System.Serializable]
public class Sound
{
    public string name="";
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume=1f;
    [Range(.1f, 3f)]
    public float pitch=1f;
    public bool loop=false;
}


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    [Space(12)]
    [Header("----- MUSIC -----")]
    public AudioSource asrc_sfx;
    public AudioSource asrc_Music;

    [Space(12)]
    [Header("----- VFX ----")]
    public Sound[] sounds;

    public AudioClip menu_Music;
    public AudioClip gameOver_Music;
    public AudioClip gameWin_Music;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

    }

    void Start()
    {
        asrc_Music.volume = PlayerPrefs.GetFloat("musicVolume", 1f);
        asrc_sfx.volume = PlayerPrefs.GetFloat("sfxVolume", 1f);
    }



    public void Stop(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name.Equals(name))
            {
                asrc_sfx.Stop();
            }
        }
    }

    public void Play(string name)
    {
        bool soundFound = false;
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name.Equals(name))
            {
                asrc_sfx.PlayOneShot(sounds[i].clip);
                soundFound = true;
            }
        }
        if (!soundFound)
        {
            Debug.LogError(name + " SOUND CLIP DOESN't MATCH");
        }
    }



    public void Change_MUSIC_Volume()
    {
        asrc_Music.volume = UIManager.Instance.settingsPanel.GetComponent<SettingsPanel_UI>().music.value;
        PlayerPrefs.SetFloat("musicVolume", asrc_Music.volume);
    }


    public void Change_SFX_Volume()
    {
        asrc_sfx.volume = UIManager.Instance.settingsPanel.GetComponent<SettingsPanel_UI>().sfx.value;
        PlayerPrefs.SetFloat("sfxVolume", asrc_sfx.volume);
    }

    public IEnumerator ChangeBGMusicByFade(AudioClip newBGMusic)
    {

        // Fading Out
        float oldValue = PlayerPrefs.GetFloat("musicVolume", 0.35f);
        float newValue = 0f;
        float duration = 1f;


        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            asrc_Music.volume = Mathf.Lerp(oldValue, newValue, t / duration);
            yield return null;
        }
        asrc_Music.volume = newValue;
        asrc_Music.Stop();
        asrc_Music.clip = newBGMusic;
        asrc_Music.Play();

        yield return new WaitForSeconds(0.2f);

        // Fading In
        oldValue = 0f;
        newValue = PlayerPrefs.GetFloat("musicVolume", 0.35f);
        duration = 1f;


        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            asrc_Music.volume = Mathf.Lerp(oldValue, newValue, t / duration);
            yield return null;
        }
        asrc_Music.volume = newValue;

    }
}