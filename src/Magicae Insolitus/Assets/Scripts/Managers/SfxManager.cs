using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Models;
using Unity.VisualScripting;
using UnityEngine;

public class SfxManager : InstancedBehavior<SfxManager>
{
    [SerializeField] private float _volume = .45f;

    public enum SfxType
    {
        Shoot,
        Hit,
        Heal
    }

    [SerializeField] private List<Sfx> _sfxList = new List<Sfx>();
    
    public void PlaySound(SfxType type)
    {
        var sfxSource = this.AddComponent<AudioSource>();
        sfxSource.volume = _volume;

        var sfx = _sfxList.FirstOrDefault(x => x.Type == type);
        sfxSource.clip = sfx.Clip;
        sfxSource.Play();

        StartCoroutine(DestroyOnFinish(sfxSource));
    }

    private IEnumerator DestroyOnFinish(AudioSource source)
    {
        while (source.isPlaying)
        {
            yield return new WaitForSeconds(.5f);
        }
        
        Destroy(source);
    }
}
