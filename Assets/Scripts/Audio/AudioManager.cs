using System.Collections;
using System.Collections.Generic;
using AudioUtilities;
using MenuUtilities;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // SINGLETON

    [SerializeField] AudioSource _SFXSource, _musicSource;
    [SerializeField] AudioClip hover, click;
    [SerializeField] AudioMixerGroup sfxMx, speechMx, musicMx;
    [SerializeField][ReadOnlyInspector] bool speechOn, sfxOn, musicOn;

    private void Awake()
    {
        // SINGLETON
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }

        EventsSubscriber();
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            SettingsManager.OnSettingsUpdate += OnSettingsUpdate;

            AudioPlayer.OnPlaySound += OnPlaySound;
            AudioPlayer.OnPlaySpeech += OnPlaySpeech;
            AudioPlayer.OnStop += OnStop;
        }
        else
        {
            SettingsManager.OnSettingsUpdate -= OnSettingsUpdate;

            AudioPlayer.OnPlaySound -= OnPlaySound;
            AudioPlayer.OnPlaySpeech -= OnPlaySpeech;
            AudioPlayer.OnStop -= OnStop;
        }
    }

    private void OnSettingsUpdate(SettingType type, object value)
    {
        switch (type)
        {
            case SettingType.PlayEffects:

                sfxOn = (bool)value;

                break;

            case SettingType.TextReader:

                speechOn = (bool)value;

                break;

            case SettingType.PlayMusic:

                musicOn = (bool)value;

                break;
        }
    }

    private void OnPlaySound(SoundType type)
    {
        if (sfxOn)
        {
            switch (type)
            {
                case SoundType.hover:
                    if (!speechOn)
                    {
                        Debug.Log($"{GetType().Name}.cs >PLAYING hover sound...");

                        _SFXSource.clip = hover;
                    }

                    break;

                case SoundType.click:
                    Debug.Log($"{GetType().Name}.cs >PLAYING click sound...");

                    _SFXSource.clip = click;

                    break;
            }

            _SFXSource.outputAudioMixerGroup = sfxMx;

            _SFXSource.Play();
        }
    }

    private void OnPlaySpeech(AudioClip clip)
    {
        if (speechOn)
        {
            _SFXSource.clip = clip;

            _SFXSource.outputAudioMixerGroup = speechMx;

            _SFXSource.Play();
        }
    }

    private void OnStop()
    {
        if (_SFXSource.isPlaying)
        {
            _SFXSource.Stop();
        }
    }
}
