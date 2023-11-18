using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public List<AudioClip> sounds = new List<AudioClip>();

    public void Awake()
    {
        Instance = this;
    }

    public void SoundPlay(int i)
    {
        GetComponent<AudioSource>().clip = sounds[i];
        GetComponent<AudioSource>().Play();
    }
}
