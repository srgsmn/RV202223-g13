using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MenuUtilities;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] SettingsManager settMng;
    [Header("Settings menu elements:")]
    [SerializeField] Toggle speechTgl;
    [SerializeField] Toggle effectsTgl, musicTgl;
    [SerializeField] Slider speechSlider, effectsSlider, musicSlider;

    private void Awake()
    {
        if (settMng == null)
        {
            Debug.LogWarning($"{GetType().Name}.cs > Settings Manager component not set in the inspector");
        }

        if (speechTgl == null)
        {
            Debug.LogWarning($"{GetType().Name}.cs > Text read toggle not set in the inspector");
        }

        if (musicTgl == null)
        {
            Debug.LogWarning($"{GetType().Name}.cs > Music toggle not set in the inspector");
        }

        EventsSubscriber();
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void Start()
    {
        settMng.ResetValues();
    }

    public void SetTextReader(bool isReading)
    {
        Debug.Log($"{GetType().Name}.cs > CHANGING text reader from {!isReading} to {isReading}");
        settMng.SetValue(SettingType.TextReader, isReading);
    }

    public void SetUIFeedback(bool hasFeedback)
    {
        Debug.Log($"{GetType().Name}.cs > CHANGING text reader from {!hasFeedback} to {hasFeedback}");
        settMng.SetValue(SettingType.PlayEffects, hasFeedback);
    }

    public void SetSpeechVolume(float volume)
    {
        Debug.Log($"{GetType().Name}.cs > CHANGING speech volume value to {volume}");

        if(volume>-20)
            settMng.SetValue(SettingType.SpeechVolume, volume);
        else
            settMng.SetValue(SettingType.SpeechVolume, -80f);
    }

    public void SetEffectsVolume(float volume)
    {
        Debug.Log($"{GetType().Name}.cs > CHANGING effects volume value to {volume}");

        if (volume > -20)
            settMng.SetValue(SettingType.EffectsVolume, volume);
        else
            settMng.SetValue(SettingType.EffectsVolume, -80f);
    }

    public void SetPlayMusic(bool hasMusic)
    {
        Debug.Log($"{GetType().Name}.cs > CHANGING play music from {!hasMusic} to {hasMusic}");

        settMng.SetValue(SettingType.PlayMusic, hasMusic);
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log($"{GetType().Name}.cs > CHANGING music volume value to {volume}");

        if (volume > -20)
            settMng.SetValue(SettingType.MusicVolume, volume);
        else
            settMng.SetValue(SettingType.MusicVolume, -80f);
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            // Event to subscribe
            SettingsManager.OnSettingsUpdate += SetValue;
        }
        else
        {
            // Events to unsubscribe
            SettingsManager.OnSettingsUpdate -= SetValue;
        }
    }

    private void SetValue(SettingType type, object value)
    {
        switch (type)
        {
            case SettingType.TextReader:

                speechTgl.isOn = (bool)value;

                break;

            case SettingType.SpeechVolume:

                speechSlider.value = (float)value;

                break;

            case SettingType.EffectsVolume:

                effectsSlider.value = (float)value;

                break;

            case SettingType.PlayEffects:

                effectsTgl.isOn = (bool)value;

                break;

            case SettingType.PlayMusic:

                musicTgl.isOn = (bool)value;

                break;

            case SettingType.MusicVolume:

                musicSlider.value = (float)value;

                break;
        }
    }
}
