using System.Collections;
using System.Collections.Generic;
using AudioUtilities;
using MenuUtilities;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip hover, click;
    [SerializeField] AudioMixerGroup sfxMx, speechMx;
    [SerializeField][ReadOnlyInspector] bool speechOn, sfxOn;

    private void Awake()
    {
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
            SettingsManager.OnSettingsUpdate += OnSettingsUpdate;

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

                        source.clip = hover;
                    }

                    break;

                case SoundType.click:
                    Debug.Log($"{GetType().Name}.cs >PLAYING click sound...");

                    source.clip = click;

                    break;
            }

            source.outputAudioMixerGroup = sfxMx;

            source.Play();
        }
    }

    private void OnPlaySpeech(AudioClip clip)
    {
        if (speechOn)
        {
            source.clip = clip;

            source.outputAudioMixerGroup = speechMx;

            source.Play();
        }
    }

    private void OnStop()
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
    }
}
