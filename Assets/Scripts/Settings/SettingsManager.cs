using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance; // SINGLETON

    [SerializeField][ReadOnlyInspector] bool _readTxt = false, _playEffects = true, _playMusic = true;
    [SerializeField][ReadOnlyInspector] float _speechVolume = 0f, _effectsVolume = 0f, _musicVolume = 0f;
    [SerializeField] AudioMixer _speechMixer, _UIFbMixer, _musixMixer;

    public delegate void SettingsUpdateEv(SettingType type, object value);
    public static SettingsUpdateEv OnSettingsUpdate;

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
    }

    private void OnEnable()
    {
        ResetValues();
    }

    /// <summary>
    /// Sends to every listener the default values of the app parameters
    /// </summary>
    public void ResetValues()
    {
        OnSettingsUpdate?.Invoke(SettingType.TextReader, _readTxt);
        OnSettingsUpdate?.Invoke(SettingType.SpeechVolume, _speechVolume);
        OnSettingsUpdate?.Invoke(SettingType.PlayEffects, _playEffects);
        OnSettingsUpdate?.Invoke(SettingType.EffectsVolume, _effectsVolume);
        OnSettingsUpdate?.Invoke(SettingType.PlayMusic, _playMusic);
        OnSettingsUpdate?.Invoke(SettingType.MusicVolume, _musicVolume);
    }

    /// <summary>
    /// Gets the value of an app parameter
    /// </summary>
    /// <param name="type">App parameter that can be set in the settings</param>
    /// <returns>An object containing the value already set of the setting type sent</returns>
    public object GetValue(SettingType type)
    {
        object value = null;

        switch (type)
        {
            case SettingType.TextReader:

                value = _readTxt;
                break;

            case SettingType.SpeechVolume:

                value = _speechVolume;

                break;

            case SettingType.PlayEffects:

                value = _playEffects;

                break;

            case SettingType.EffectsVolume:

                value = _effectsVolume;

                break;

            case SettingType.PlayMusic:

                value = _playMusic;

                break;

            case SettingType.MusicVolume:

                value = _musicVolume;

                break;
        }

        Debug.Log($"{GetType().Name}.cs > SENDING {type} value of {value}");

        return value;
    }

    /// <summary>
    /// Sets the value of an app parameter
    /// </summary>
    /// <param name="type">The property that changes its value</param>
    /// <param name="value">The value passed as a generic object</param>
    public void SetValue(SettingType type, object value)
    {
        switch (type)
        {
            case SettingType.TextReader:

                _readTxt = (bool)value;
                OnSettingsUpdate?.Invoke(SettingType.TextReader, _readTxt);

                break;

            case SettingType.SpeechVolume:

                _speechVolume = (float)value;
                _speechMixer.SetFloat("Volume", _speechVolume);
                OnSettingsUpdate?.Invoke(SettingType.SpeechVolume, _speechVolume);

                break;

            case SettingType.PlayEffects:

                _playEffects = (bool)value;

                OnSettingsUpdate?.Invoke(SettingType.PlayEffects, _playEffects);

                break;

            case SettingType.EffectsVolume:

                _effectsVolume = (float)value;

                _UIFbMixer.SetFloat("Volume", _effectsVolume);
                OnSettingsUpdate?.Invoke(SettingType.EffectsVolume, _effectsVolume);

                break;

            case SettingType.PlayMusic:

                _playMusic = (bool)value;

                OnSettingsUpdate?.Invoke(SettingType.PlayMusic, _playMusic);

                break;

            case SettingType.MusicVolume:

                _musicVolume = (float)value;

                _musixMixer.SetFloat("Volume", _musicVolume);
                OnSettingsUpdate?.Invoke(SettingType.MusicVolume, _musicVolume);

                break;
        }
    }
}
