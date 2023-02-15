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
    [SerializeField] Toggle effectsTgl;
    [SerializeField] Slider speechSlider, effectsSlider;

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

        EventsSubscriber();
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void Start()
    {
        //txtRead.isOn = (bool)settMng.GetValue(SettingType.TextReader);
    }

    public void SetTextReader(bool isReading)
    {
        Debug.Log($"{GetType().Name}.cs > CHANGING text reader from {!isReading} to {isReading}");
        settMng.SetValue(SettingType.TextReader, isReading);
    }

    public void SetSpeechVolume(float volume)
    {
        Debug.Log($"{GetType().Name}.cs > CHANGING volume value to {volume}");
        settMng.SetValue(SettingType.SpeechVolume, volume);
    }

    public void SetEffectsVolume(float volume)
    {
        Debug.Log($"{GetType().Name}.cs > CHANGING volume value to {volume}");
        settMng.SetValue(SettingType.EffectsVolume, volume);
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
        }
    }
}
