using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Slider>().value = MusicManager.instance.GetVolume();
    }

    public void SetVolume()
    {
        try
        {
            MusicManager.instance.SetVolume(GetComponent<Slider>().value);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
