using UnityEngine;
using System;
using FMODUnity;
using FMOD.Studio;

[System.Serializable]
public class SoundEntry
{
    public string soundName;
    public EventReference eventRef;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Sounds")]
    [SerializeField] SoundEntry[] musicSounds, voiceLines, sfxSounds;

    [Header("Music Volume")]
    [SerializeField] float normalVolume = 1f;
    [SerializeField] float dialogueVolume = 0.35f;

    private EventInstance currentMusic;
    private EventInstance currentVoice;
    private EventInstance currentLoopSFX;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // --- SFX ------------------------------------------------
    public void PlayOneShot(string sound, Vector3 worldPos)
    {
        SoundEntry s = Array.Find(sfxSounds, x => x.soundName == sound);
        if (s == null) return;
        RuntimeManager.PlayOneShot(s.eventRef, worldPos);
    }

    public void PlayOneShot(string sound)
    {
        SoundEntry s = Array.Find(sfxSounds, x => x.soundName == sound);
        if (s == null) return;
        RuntimeManager.PlayOneShot(s.eventRef);
    }

    public void PlayLoopSFX(string sound)
    {
        if (currentLoopSFX.isValid())
        {
            PLAYBACK_STATE state;
            currentLoopSFX.getPlaybackState(out state);

            if (state == PLAYBACK_STATE.PLAYING)
                return;
        }
        SoundEntry s = Array.Find(sfxSounds, x => x.soundName == sound);
        if (s == null) return;

        StopLoopSFX();
        currentLoopSFX = RuntimeManager.CreateInstance(s.eventRef);
        currentLoopSFX.start();
    }

    public void StopLoopSFX()
    {
        if (currentLoopSFX.isValid())
        {
            currentLoopSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentLoopSFX.release();
        }
    }

    // --- MUSIC ------------------------------------------------
    public void PlayMusic(string sound)
    {
        SoundEntry s = Array.Find(musicSounds, x => x.soundName == sound);
        if (s == null) return;
        
        StopMusic();
        currentMusic = RuntimeManager.CreateInstance(s.eventRef);
        currentMusic.start();
    }

    public void StopMusic()
    {
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }
    }

    public void LowerVolume()
    {
        if (currentMusic.isValid())
        {
            currentMusic.setVolume(dialogueVolume);
        }
    }

    public void RestoreVolume()
    {
        if (currentMusic.isValid())
        {
            currentMusic.setVolume(normalVolume);
        }
    }

    // --- VOICE DIALOGUE ----------------------------------------
    public void PlayVoiceLine(string voiceName)
    {
        SoundEntry s = Array.Find(voiceLines, x => x.soundName == voiceName);
        if (s == null) return;

        StopVoiceLine();
        currentVoice = RuntimeManager.CreateInstance(s.eventRef);
        currentVoice.start();
    }

    public void StopVoiceLine()
    {
        if (currentVoice.isValid())
        {
            currentVoice.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentVoice.release();
        }
    }
}
