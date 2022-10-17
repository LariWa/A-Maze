using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public Sound[] sounds;
    public Sound theme;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        theme = Array.Find(sounds, sound => sound.name == "spooky");
        theme.source.loop = true;        
        theme.source.Play();
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);      
        if (name == "chase" ) {
            theme.source.Stop();  
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);        
        s.source.Stop();
        theme.source.Play();
    }
}
