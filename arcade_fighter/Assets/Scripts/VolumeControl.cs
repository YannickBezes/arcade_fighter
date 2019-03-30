using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioSource[] audioSources;
    public Slider volumeSlider;
    public bool isMusicVolume;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.onValueChanged.AddListener(ChangeValue);
    }

    void ChangeValue(float newVal)
    {
        foreach(AudioSource a in audioSources)
            a.volume = newVal;
    }
}
