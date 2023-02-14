using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(EventTrigger))]
public class TextReader : MonoBehaviour
{
    [SerializeField] public bool readTxt = false;
    [SerializeField] public AudioSource _audio;

    // FUNCTIONS
    private void Awake()
    {
        EventSubscriber();

        if (_audio == null)
        {
            Debug.LogWarning($"{GetType().Name}.cs > AudioSource component is missing. Adding it...");
            _audio = GetComponent<AudioSource>();
        }
    }

    private void OnDestroy()
    {
        EventSubscriber(false);
    }

    public virtual void PlaySound()
    {
        if(readTxt) _audio.Play();
    }

    public virtual void StopSound()
    {
        if (readTxt) _audio.Stop();
    }

    private void EventSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            // Event to subscribe
            SettingsManager.OnSettingsUpdate += UpdateValues;
        }
        else
        {
            // Events to unsubscribe
            SettingsManager.OnSettingsUpdate -= UpdateValues;
        }
    }

    private void UpdateValues(SettingType type, object value)
    {
        switch (type)
        {
            case SettingType.TextReader:
                readTxt = (bool)value;

                break;
        }
    }
}
