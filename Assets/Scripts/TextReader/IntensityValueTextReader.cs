using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(EventTrigger))]
public class IntensityValueTextReader : TextReader
{
    [SerializeField] AudioClip _min, _low, _medium, _high, _max;
    [SerializeField] Slider slider;
    AudioClip defaultClip;

    float min, max, avg, medL, medR;

    private void Start()
    {
        defaultClip = _audio.clip;

        FindRangeValues();
    }

    private void FindRangeValues()
    {
        min = slider.minValue;
        max = slider.maxValue;

        medL = (max - min) / 3 + min;
        medR = (max - min) / 3 * 2 + min;
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
        float value = slider.value;

        if (value == min)
            return _min;
        else if (value > min && value < medL)
            return _low;
        else if (value >= medL && value <= medR)
            return _medium;
        else if (value > medR && value < max)
            return _high;
        else
            return _max;
    }
}
