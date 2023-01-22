using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "music", menuName = "Music")]
public class Music : ScriptableObject
{
    public AudioClip intro;
    public List<AudioClip> segments = new List<AudioClip>();
}