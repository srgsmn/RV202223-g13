using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using UnityEngine;

public class EnvironmentalMusic : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] clips;
    [SerializeField][ReadOnlyInspector] AudioClip currentClip;
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
        switch (GameManager.Instance.sceneIndex)
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
            source.clip = currentClip;

            source.Play();
        }
    }

    private void EventsSubcriber(bool subscribing = true)
    {
        if (subscribing)
        {
            SettingsManager.OnSettingsUpdate += SetValues;
        }
        else
        {
            SettingsManager.OnSettingsUpdate -= SetValues;
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
