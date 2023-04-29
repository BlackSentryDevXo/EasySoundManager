using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    /// ///////////////
    public string ClipName;
    public bool isBGSound, isDefaultBG;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;
    public bool loop = false;
    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;
    /// ///////////////

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }

    public bool isPlaying()
    {
        return source.isPlaying;
    }

    public void SetVolume(float amount)
    {
        source.volume = amount * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
    }

    public void SetPitch(float amount)
    {
        source.pitch = amount * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
    }
}

public class SoundManager : MonoBehaviour
{
    private bool _muteBackgroundMusic = false;
    private bool _muteSoundFx = false;    

    [SerializeField]
    Sound[] sounds;
    
    public static SoundManager instance = null;
    [HideInInspector]
    public Sound overlapMusic, currentBG;
    
    float defaultBGVol = 0;
    bool hasReducedBG = false;

    private void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        { Destroy(this); }
    }


    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].ClipName);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
        PlayDefualtBG();
    }

    public void PlayDefualtBG(){        
        foreach(var sound in sounds)
        {
            if (sound.isBGSound && sound.isDefaultBG)
            {
                // Debug.LogError("playing default bg");
                currentBG = sound;

                if (!IsBackgroundMusicMuted())
                { sound.Play(); }

            }else if (sound.isBGSound && !sound.isDefaultBG)
            {
                sound.Stop();
            }
        }
        
    }

    public void StopDefaultBG(){
        foreach(var sound in sounds)
        {
            if (sound.isBGSound && sound.isPlaying() && sound.isDefaultBG)
            {
                sound.Stop();
            }
        }
    }

    public void PlaySound(string _name)
    {
        if (!IsSoundFXMuted())
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].ClipName == _name)
                {
                    // Debug.Log("Playing " + _name);
                    sounds[i].Play();
                    return;
                }
            }
        }

        // no sound with _name
        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }

    public void SetCurrentBG(string ClipName)
    {
        foreach(var sound in sounds)
        {
            if (sound.isBGSound)
            {
                if (sound.ClipName == ClipName)
                {
                    currentBG = sound;
                    if (IsBackgroundMusicMuted() == false && sound.isPlaying() == false)
                    {
                        sound.Play();                      
                        // Debug.LogError("playing default bg");
                    }
                }
                else{
                    sound.Stop();
                }
            }

        }
    }

    public void OverlapAndDistortCurrentBG(string ClipName)
    {
        foreach(var sound in sounds)
        {
            if (sound.ClipName == ClipName)
            {
                if (sound.isBGSound && !sound.isDefaultBG)
                {
                    overlapMusic = sound;
                    overlapMusic.Play();
                    DistortDefaultBG();
                }
            }
        }
    }

    public void CheckDefaultBGVolume()
    {
        if (!_muteBackgroundMusic)
        {
            foreach(var sound in sounds)
            {
                if (sound.isDefaultBG && hasReducedBG)
                {
                    Debug.LogError("inceasing background");
                    IncreaseBG();
                }
            }
        }
    }

    public void DistortDefaultBG()
    {
        foreach(var sound in sounds)
        {
            if (sound.isDefaultBG)
            {
                sound.SetPitch(0.9f);
            }
        }
    }

    public void RemoveDistortionBG()
    {
        foreach(var sound in sounds)
        {
            if (sound.isDefaultBG && sound.isBGSound)
            {
                sound.SetPitch(1);
            }
            else if(overlapMusic != null)
            {
                overlapMusic.Stop();
            }
        }    
    }


    public void ReduceBG()
    {
        foreach(var sound in sounds)
        {
            if (sound.isBGSound)
            {
                defaultBGVol = sound.volume;
                sound.SetVolume(0f);
                hasReducedBG = true;
            }
        }
    }


    public void IncreaseBG()
    {
        foreach(var sound in sounds)
        {
            if (sound.isBGSound)
            {
                sound.SetVolume(defaultBGVol);
                hasReducedBG = false;
            }
        }    
    }


    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].ClipName == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        // no sound with _name
        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }

    public void ToggleBackgroundMusic()
    {
        _muteBackgroundMusic = !_muteBackgroundMusic;
        if (_muteBackgroundMusic)
        {
            currentBG.Stop();
        }
        else
        {
            currentBG.Play();     
        }
    }

    public void ToggleSoundFX()
    {
        _muteSoundFx = !_muteSoundFx;
    }

    public bool IsBackgroundMusicMuted()
    {
        return _muteBackgroundMusic;
    }

    public bool IsSoundFXMuted()
    {
        return _muteSoundFx;
    }
}
