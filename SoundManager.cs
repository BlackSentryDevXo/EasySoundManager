using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//< -------------------------Black Sentry Dev---------------------------------------> 

[System.Serializable]
public class Sound
{
    public enum SoundType
    {
        Background, 
        SFX
    }

    [Tooltip("Type of sound to be played")]
    public SoundType soundType;

    [Tooltip("Name of sound you want to play")]
    public string SoundName;

    [Tooltip("Drag and drop desired soundclip here")]
    public AudioClip soundClip;

    [Tooltip("Set Volume for sound (0.7 is recommended)")]
    [Range(0f, 1f)]
    public float volume = 0.7f;

    [Tooltip("Set Desired pitch here (1 is recommended)")]
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Tooltip("Should the sound loop?")]
    public bool loop = false;

    [Tooltip("Should the sound use Global Volume?")]
    public bool useGlobalVolume = true;

    [Tooltip("Random volume work best for footsteps and attack sounds")]
    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;

    [Tooltip("Random pitch work best for footsteps and attack sounds")]
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    [HideInInspector]
    public bool isCurrentBG = false;


    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = soundClip;
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

    public bool isCurrentBGSouund()
    {
        return isCurrentBG;
    }
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float GlobalVolume = 0.7f;

    [Tooltip("Is Background Sound Muted?")]
    public bool _canPlayBGMusic = true;

    [Tooltip("is SFX sound muted?")]
    public bool _canPlaySFX = true;

    public static SoundManager instance;
    
    private AudioSource _audioSource;

    [SerializeField]
    List<Sound> sounds = new List<Sound>();

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
        ResolveSoundList();
    }

    void ResolveSoundList()
    {
        foreach (var sound in sounds)
        {
            if (sound.SoundName != "" && sound.soundClip != null)
            {
                //setting name for new sound object
                GameObject _go = new GameObject("Sound_" + sound.SoundName);
                _go.transform.SetParent(this.transform);
                //adding audio source to object
                sound.SetSource(_go.AddComponent<AudioSource>());

                //let's check if sound is using global volume
                if (sound.useGlobalVolume)
                {
                    sound.SetVolume(GlobalVolume);
                    sound.pitch = 1f;
                }

                //let's make sure all sounds have pitch and volumes
                if (sound.pitch == 0)
                {
                    sound.pitch = 1f;
                }

                if (sound.volume == 0)
                {
                    if(sound.soundType.ToString() == "Background")
                    {
                        sound.SetVolume(GlobalVolume);
                    }
                    else if (sound.soundType.ToString() == "SFX")
                    {
                        sound.SetVolume(0.4f);
                    }

                }
            }
            else{
                Debug.LogWarning("AudioManager: Make sure all sounds have a Name and a Clip");
            }
        }
    }

    public void PlayBGSound(string _name)
    {
        //lets check if we muted bg sounds
        if(_canPlayBGMusic)
        {
            foreach(var sound in sounds)
            {
                //lets set current BG sound for all sounds false
                sound.isCurrentBG = false;

                //Let's check if sound type is Background
                if (sound.soundType.ToString() == "Background")
                {
                    //let's check if we were already playing a sound
                    //if we are, let's stop that sound
                    if (sound.isPlaying())
                    {
                        sound.Stop();
                    }

                    //Let's check if sound name is in the available sounds
                    if (sound.SoundName == _name)
                    {
                        //let's check if we are using global volume
                        if(sound.useGlobalVolume)
                        {
                            sound.SetVolume(GlobalVolume);
                            //lets set this sound as our current sound
                            sound.isCurrentBG = true;
                            //lets play sound
                            sound.Play();
                        }
                        else{
                            sound.Play();
                        }
                        return;
                    }
                }
            }
            // no sound with _name
            Debug.LogWarning("AudioManager: Background Sound not found in list, " + _name);
        }else{
            // if BG has been muted yet we try to play it
            Debug.LogWarning("AudioManager: BG Music has been muted");
        }
    }

    public void PlaySFX(string _name)
    {
        //Let's check if sound FX is muted or not
        if(_canPlaySFX)
        {
            foreach(var sound in sounds)
            {
                //Let's check if sound type is FX
                if (sound.soundType.ToString() == "SFX")
                {
                    //Let's check if sound name is in the available sounds
                    if (sound.SoundName == _name)
                    {
                        sound.Play();
                        return;
                    }
                }
            }
            // no sound with _name
            Debug.LogWarning("AudioManager: SFX not found in list, " + _name);
        }else{
            // if SFX has been muted yet we try to play it
            Debug.LogWarning("AudioManager: SFX has been muted");
        }
    }

    //for when you have an ad implementation, you might want to set volume low
    public void ToggleBackgroundMusic()
    {
        if (_canPlayBGMusic == true)
        {
            foreach (var sound in sounds)
            {                
                //lets check if we are playing any sound and if that is our current sound
                if (sound.isPlaying() && sound.isCurrentBG && sound.soundType.ToString() == "Background")
                {
                    //let's reduce its volume
                    sound.SetVolume(0f);
                }
            }
        }
        else{
            foreach (var sound in sounds)
            {
                //lets check if we are playing any sound
                if (sound.isPlaying() && sound.isCurrentBG && sound.soundType.ToString() == "Background")
                {
                    //let's increase its volume
                    if (sound.useGlobalVolume)
                    {
                        sound.SetVolume(GlobalVolume);
                    }
                    else{
                        sound.SetVolume(0.7f);
                    }
                }
            }
        }
        
        //we can't play BG music so we set this to false
        _canPlayBGMusic = !_canPlayBGMusic;
    }

    //Setting global volume
    public void AdjustGlobalVolume(float _volume)
    {
        GlobalVolume = _volume;

        foreach(var sound in sounds)
        {
            if (sound.soundType.ToString() == "Background")
            {
                sound.SetVolume(GlobalVolume); 
            }
        }
    } 

    //For when you want to completely stop a specific sound.
    public void StopSound(string _name)
    {
        foreach (var sound in sounds)
        {
            if (sound.SoundName == _name)
            {
                sound.Stop();
                return;
            }
        }
        // no sound with _name
        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }

    //For when you want to stop all background music 
    public void StopAllBackgroundMusic()
    {
        foreach (var sound in sounds)
        {                
            //lets check for all background sounds
            if (sound.soundType.ToString() == "Background")
            {
                //let's reduce its volume
                sound.Stop();
            }
        }
    }

    //for when you want to disable all SFX
    public void ToggleSoundFX()
    {
        _canPlaySFX = !_canPlaySFX;
    }

    //Use this to check if backgrouund music is muted
    public bool IsBackgroundMusicMuted()
    {
        return _canPlayBGMusic;
    }

    //use this to check if SFX is muted
    public bool IsSoundFXMuted()
    {
        return _canPlaySFX;
    }
}

//< -------------------------Black Sentry Dev---------------------------------------> 
