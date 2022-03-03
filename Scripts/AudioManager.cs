using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    // Start is called before the first frame update
    public void SetMasterVolume(float volume)    // 控制主音量的函数
    {
        audioMixer.SetFloat("MasterVolume", volume);        
    }

    public void SetMusicVolume(float volume)    // 控制背景音乐音量的函数
    {
        audioMixer.SetFloat("BGMVolume", volume);        
    }

    public void SetSoundEffectVolume(float volume)    // 控制音效音量的函数
    {
        audioMixer.SetFloat("SoundEffectVolume", volume);        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.active = false;
        }
    }
}
