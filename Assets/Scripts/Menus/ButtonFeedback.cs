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

    private void OnEnable()
    {
        CheckValues();
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

    private void CheckValues()
    {
        //hasSpeech = (bool)SettingsManager.Instance.GetValue(SettingType.TextReader);
        //source.mute = (bool)SettingsManager.Instance.GetValue(SettingType.PlayEffects);
    }

    public void PlayHover()
    {
        if (!hasSpeech)
        {
            Debug.Log($"{GetType().Name}.cs in {gameObject.name} > PLAYING hover sound");

            StopClip();
            source.clip = hover;
            source.Play();
        }
        else
        {
            Debug.Log($"{GetType().Name}.cs in {gameObject.name} > NOT PLAYING hover sound because hasSpeech is true");
        }
    }

    public void Playclick()
    {
        Debug.Log($"{GetType().Name}.cs in {gameObject.name} > PLAYING click sound");

        StopClip();
        source.clip = click;
        source.Play();
    }

    public void StopClip()
    {
        Debug.Log($"{GetType().Name}.cs in {gameObject.name} > STOPPING playing clip (if any)");

        source.Stop();
        source.clip = null;
    }

    private void ValueChanged(SettingType type, object value)
    {
        Debug.Log($"{GetType().Name}.cs in {gameObject.name} > A setting value is changed. Looking if it affects the component...");

        if (type == SettingType.PlayEffects)
        {
            Debug.Log($"{GetType().Name}.cs in {gameObject.name} > The play effects flag has changed. Settting AudioSource to {((bool)value ? "NOT MUTE" : "MUTE" )}");

            source.mute = (bool)value;
        }

        if (type == SettingType.TextReader)
        {
            Debug.Log($"{GetType().Name}.cs in {gameObject.name} > The text read flag has changed. Settting corresponding flag to {(bool)value}");

            hasSpeech = (bool)value;
        }
    }
}
