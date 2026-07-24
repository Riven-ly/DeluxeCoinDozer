using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource BGM;
    public AudioSource SceneSingleMusic;
    public AudioSource SceneLoopMusic;
    public AudioSource btnMusic;
    public AudioSource getGoldMusic;

    public List<AudioClip> audioClips;
    private Dictionary<string, AudioClip> sceneMusics;

    private bool isSound = true;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        sceneMusics = new Dictionary<string, AudioClip>();
        foreach (var audioClip in audioClips)
        {
            sceneMusics.Add(audioClip.name, audioClip);
        }
    }

    public void PlayBGM(string name)
    {
        AudioClip audioClip = GetAudioClip(name);
        if (audioClip == null) return;
        BGM.clip = audioClip;
        BGM.Play();
    }
    public void StopBGM()
    {
        BGM.Stop();
    }

    public void PlayBtnMusic()
    {
        btnMusic.Play();
    }
    public void PlayGetGoldMusic()
    {
        getGoldMusic.Play();
    }

    public void PlaySceneLoopMusic(string name)
    {
        AudioClip audioClip = GetAudioClip(name);
        if (audioClip == null) return;
        SceneLoopMusic.clip = audioClip;
        SceneLoopMusic.Play();
    }
    public void StopSceneLoopMusic()
    {
        SceneLoopMusic.Stop();
    }
    public void PlaySceneSingleMusic(string name)
    {
        AudioClip audioClip = GetAudioClip(name);
        if (audioClip == null) return;
        SceneSingleMusic.clip = audioClip;
        SceneSingleMusic.Play();
    }

    private AudioClip GetAudioClip(string name)
    {
        AudioClip audioClip = null;
        if (sceneMusics.TryGetValue(name, out audioClip))
        {
            audioClip = sceneMusics[name];
        }
        return audioClip;
    }

    public void MusicState(bool isOpen)
    {
        BGM.volume = isOpen ? 1 : 0;
    }

    public void SoundState(bool isOpen)
    {
        isSound = isOpen;
        float volume = isOpen ? 1 : 0;
        SceneSingleMusic.volume = volume;
        SceneLoopMusic.volume = volume;
        btnMusic.volume = volume;
        getGoldMusic.volume = volume;
    }
    
    public void SetAudioSource(AudioSource audioSource, string name)
    {
        if (!isSound) return;
        AudioClip audioClip = GetAudioClip(name);
        if (audioClip == null) return;
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
