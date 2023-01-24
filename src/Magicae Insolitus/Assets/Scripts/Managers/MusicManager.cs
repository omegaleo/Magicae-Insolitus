using System;
using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Runtime.Extensions;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class MusicManager : InstancedBehavior<MusicManager>
{
    [SerializeField] private Music _music;

    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = _music.intro;
        _source.Play();
    }

    private void Update()
    {
        if (!_source.isPlaying)
        {
            _source.clip = _music.segments.Random();
            _source.Play();
        }
    }

    public void SetVolume(float volume)
    {
        _source.volume = volume;
    }

    public float GetVolume()
    {
        if (_source == null) _source = GetComponent<AudioSource>();

        return _source.volume;
    }
}
