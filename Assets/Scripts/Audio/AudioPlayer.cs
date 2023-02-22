using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AudioUtilities;

public class AudioPlayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] AudioClip speechClip;
    [SerializeField] bool hasSFX = true;
    [SerializeField][ReadOnlyInspector] bool hasSpeech = false;
    [SerializeField][ReadOnlyInspector] bool isButton = false;



    public delegate void PlaySoundEv(SoundType type);
    public static event PlaySoundEv OnPlaySound;
    public delegate void PlaySpeechEv(AudioClip clip);
    public static event PlaySpeechEv OnPlaySpeech;
    public delegate void StopSoundEv();
    public static event StopSoundEv OnStop;

    private void Awake()
    {
        if (gameObject.GetComponent<Button>() != null)
            isButton = true;

        if (speechClip != null)
            hasSpeech = true;
    }

    // POINTER EVENTS
    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log($"{GetType().Name}.cs > Pointer entered on {gameObject.name}");

        if(hasSFX)
            OnPlaySound?.Invoke(SoundType.hover);
        if(hasSpeech)
            OnPlaySpeech?.Invoke(speechClip);
    }

    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log($"{GetType().Name}.cs > Pointer clicked on {gameObject.name}");

        OnPlaySound?.Invoke(SoundType.click);
    }

    public void OnPointerExit(PointerEventData data)
    {
        Debug.Log($"{GetType().Name}.cs > Pointer exited from {gameObject.name}");

        OnStop?.Invoke();
    }

    public void OnSelect(BaseEventData data)
    {
        Debug.Log($"{GetType().Name}.cs > Selection on {gameObject.name}");

        if (hasSFX)
            OnPlaySound?.Invoke(SoundType.hover);
        if(hasSpeech)
            OnPlaySpeech?.Invoke(speechClip);
    }

    public void OnDeselect(BaseEventData data)
    {
        Debug.Log($"{GetType().Name}.cs > Deselection from {gameObject.name}");

        OnStop?.Invoke();
    }
}
