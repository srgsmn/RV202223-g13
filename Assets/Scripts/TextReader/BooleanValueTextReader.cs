using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(EventTrigger))]
public class BooleanValueTextReader : TextReader
{
    [SerializeField] AudioClip _true, _false;
    [SerializeField] Toggle toggle;
    AudioClip defaultClip;

    private void Start()
    {
        defaultClip = _audio.clip;
    }

    public override void PlaySound()
    {
        if (readTxt)
        {
            _audio.Play();

            StartCoroutine(SayValue(_audio.clip));
        }
    }

    public override void StopSound()
    {
        if (readTxt) _audio.Stop();

        _audio.clip = defaultClip;
    }

    IEnumerator SayValue(AudioClip clip)
    {
        AudioClip nextClip = null;
        

        yield return new WaitForSeconds(clip.length);

        nextClip = ValueSelector();

        _audio.clip = nextClip;
        _audio.Play();

        yield return new WaitForSeconds(_audio.clip.length);

        _audio.clip = defaultClip;
    }

    private AudioClip ValueSelector()
    {
        if (toggle.isOn)
            return _true;
        else
            return _false;
    }
}
