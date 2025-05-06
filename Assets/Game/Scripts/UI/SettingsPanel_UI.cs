using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel_UI : MonoBehaviour
{
    public Slider sfx;
    public Slider music;

    // Start is called before the first frame update
    void Start()
    {
        music.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        sfx.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
    }

}
