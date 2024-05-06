using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Transform Transform;
    [SerializeField] AudioSource _SourceBG;
    AudioClip playedClip;
    [SerializeField] AudioClip mainBg;

    [SerializeField] AudioSource _SourceEffectPrefabs;
    [SerializeField] List<AudioSource> _SourceEffects;
    [SerializeField]
    AudioClip sfx_Base;

    private void Start()
    {
        PlayAudio();
    }
    public void PlayAudio()
    {
        ChangeMusicState(ProfileManager.Instance.GetSettingStatus(SettingId.Music));
        ChangeSoundState(ProfileManager.Instance.GetSettingStatus(SettingId.Sound));
    }

    public void ChangeMusicState(bool play)
    {
        if (play)
        {
            PlayMusic();
        }
        else
        {
            PauseMusic();
        }
    }
    public void PauseMusic()
    {
        _SourceBG.volume = 0;
    }
    public void PlayMusic()
    {
        if (ProfileManager.Instance.GetSettingStatus(SettingId.Music))
        {
            if (!_SourceBG.isPlaying) _SourceBG.Play();
            _SourceBG.volume = 1;
        }
    }

    public void ChangeSoundState(bool play)
    {
        if (play)
        {
            PlaySound();
        }
        else
        {
            PauseSound();
        }
    }
    public void PlaySound()
    {
        for (int i = 0; i < _SourceEffects.Count; i++)
        {
            _SourceEffects[i].volume = 1f;
        }

    }
    public void PauseSound()
    {
        for (int i = 0; i < _SourceEffects.Count; i++)
        {
            _SourceEffects[i].volume = 0f;
        }
    }

    public void PlayBGAudio(PlayMode playMode, float volume)
    {
        playedClip = _SourceBG.clip;
        _SourceBG.volume = volume;
        _SourceBG.clip = mainBg;

        if (playedClip != _SourceBG.clip)
        {
            _SourceBG.Play();
        }
    }

    public void PlaySoundEffect(SoundId soundId, float volume = 1f)
    {
        AudioClip audioClip = GetAudioClip(soundId);
        if (audioClip != null)
        {
            AudioSource selectedSource = GetFreeEffectSource();
            selectedSource.volume = volume;
            selectedSource.clip = audioClip;
            selectedSource.Play();
        }
        
    }
    AudioSource GetFreeEffectSource()
    {
        for (int i = 0; i < _SourceEffects.Count; i++)
        {
            if (!_SourceEffects[i].isPlaying)
            {
                return _SourceEffects[i];
            }
        }
        AudioSource newSource = Instantiate(_SourceEffectPrefabs, Transform);
        _SourceEffects.Add(newSource);
        return newSource;
    }

    public List<AudioClip> audioClips;
    public AudioClip GetAudioClip(SoundId soundId)
    {
        for (int i = 0; i < audioClips.Count; i++)
        {
            if (audioClips[i].name == soundId.ToString())
                return audioClips[i];
        }
        return null;
    }
}
