using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MenuUtilities;

public class ButtonFeedback : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip hover, click;
    [SerializeField][ReadOnlyInspector] bool hasSpeech;

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
            // Event to subscribe
            SettingsManager.OnSettingsUpdate += ValueChanged;
        }
        else
        {
            // Events to unsubscribe
            SettingsManager.OnSettingsUpdate -= ValueChanged;
        }
    }

    public void PlayHover()
    {
        if (!hasSpeech)
        {
            StopClip();
            source.clip = hover;
            source.Play();
        }
    }

    public void Playclick()
    {
        StopClip();
        source.clip = click;
        source.Play();
    }

    public void StopClip()
    {
        source.Stop();
        source.clip = null;
    }

    private void ValueChanged(SettingType type, object value)
    {
        if (type == SettingType.PlayEffects)
        {
            source.mute = (bool)value;
        }

        if (type == SettingType.TextReader)
        {
            hasSpeech = (bool)value;
        }
    }
}
