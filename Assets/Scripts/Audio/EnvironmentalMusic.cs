using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using UnityEngine;

public class EnvironmentalMusic : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;
    [SerializeField][ReadOnlyInspector] bool musicOn;

    private void Awake()
    {
        EventsSubcriber();
    }

    private void OnDestroy()
    {
        EventsSubcriber(false);
    }

    private void Start()
    {
        musicOn = (bool)SettingsManager.Instance.GetValue(SettingType.PlayMusic);
        if (clip != null)
        {
            Debug.Log($"{GetType().Name}.cs > Setting clip to play to {clip}.");

            source.clip = clip;

            if (musicOn)
                source.Play();
            else
                source.Pause();
        }
    }

    /*
    private void OnNewScene(int i)
    {
        switch (i)
        {
            case 1:
                currentClip = clips[0];

                break;

            case 2:
                currentClip = clips[0];

                break;

            case 3:
                currentClip = clips[0];

                break;

            default:
                currentClip = null;

                break;
        }

        if (currentClip != null)
        {
            Debug.Log($"{GetType().Name}.cs > Setting clip to play to {currentClip}.");

            source.clip = currentClip;

            source.Play();
        }
    }
    */

    private void EventsSubcriber(bool subscribing = true)
    {
        if (subscribing)
        {
            SettingsManager.OnSettingsUpdate += SetValues;
            //GameManager.OnNewSceneIndex += OnNewScene;
        }
        else
        {
            SettingsManager.OnSettingsUpdate -= SetValues;
            //GameManager.OnNewSceneIndex -= OnNewScene;
        }
    }

    private void SetValues(SettingType type, object value)
    {
        if (type == SettingType.PlayMusic)
        {
            musicOn = (bool)value;

            if (musicOn)
            {
                source.Play();
            }
            else
            {
                source.Pause();
            }
        }
    }
}
