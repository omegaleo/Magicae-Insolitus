using System;
using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Runtime.Extensions;
using UnityEngine;

public class MusicManager : MonoBehaviour
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
}
