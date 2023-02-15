using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance; // SINGLETON

    [SerializeField][ReadOnlyInspector] bool _readTxt = false, _playEffects = true;
    [SerializeField][ReadOnlyInspector] float _speechVolume = 0, _effectsVolume = 0;
    [SerializeField] AudioMixer _audioMixer, _UIFeedback;

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

    private void Start()
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
                _audioMixer.SetFloat("Volume", _speechVolume);
                OnSettingsUpdate?.Invoke(SettingType.SpeechVolume, _speechVolume);

                break;
        }
    }
}
