using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] bool _readTxt = false;
    [SerializeField] float _volume = 0;
    [SerializeField] AudioMixer _audioMixer;

    public delegate void SettingsUpdateEv(SettingType type, object value);
    public static SettingsUpdateEv OnSettingsUpdate;

    private void Start()
    {
        ResetValues();
    }

    public void ResetValues()
    {
        OnSettingsUpdate?.Invoke(SettingType.TextReader, _readTxt);
        OnSettingsUpdate?.Invoke(SettingType.Volume, _volume);
    }

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

    public void SetValue(SettingType type, object value)
    {
        switch (type)
        {
            case SettingType.TextReader:

                _readTxt = (bool)value;
                OnSettingsUpdate?.Invoke(SettingType.TextReader, _readTxt);

                break;

            case SettingType.Volume:

                _volume = (float)value;
                _audioMixer.SetFloat("Volume", _volume);
                OnSettingsUpdate?.Invoke(SettingType.Volume, _volume);

                break;
        }
    }
}
