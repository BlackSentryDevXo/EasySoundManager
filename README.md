# EasySoundManager
An easy sound manager to use in any Unity project
This script was created by BlackSentryDev for learning purposes. Do not sell this script!!

Anyways thanks for checking this out. To use this sound manager in your project, simply create a new gameobject and attach this script.

- In the inspector, you'll see the option to set a Global Volume for Background sounds (Global volumes don't affect SFX) 
- The two bool variables (Can play BG and Can Play SFX) are made public so that you can easily call them in other scripts if you want to mute BG or SFX
- In the inspector, you have to set the number of sounds you would like to play.
- In each sound, you can set the sound type, sound Name, Sound Clip, Volume, Pitch, Loop, Global Volume, Random Volume and Random Pitch.
- I recommend that the volume is set to 0.7
- I also recommend that pitch is set to 1
- Depending on the type of game, you can set random volumes and pitch if you want.

SCRIPTING--------------------------------------------------------------------------------------------------------

- You can play Background Sound (BG) from any script by calling "SoundManager.instance.PlayBGSound(name of sound)"
- You can play any SFX sound from any script by calling SoundManager.instance.PlaySFX(name of sound)"
- You can toggle BG from any script by calling "SoundManager.instance.ToggleBackgroundMusic()" 
- You can toggle SFX from any script by calling "SoundManager.instance.ToggleSoundFX()"
- You can Adjust Global volume from any script by calling "SoundManager.instance.AdjustGlobalVolume(float volume)" 
	Adjusting global volume will affect only sounds that Use global volume in inspector.
- You can stop a specific sound from any script by calling "SoundManager.instance.StopSouund(name of sound)"
- You can stop all Background Music by calling "SoundManager.instance.StopAllBackgroundMusic()"

EXTRA FUNCTIONS--------------------------------------------------------------------------------------------------
- You can check if background sound is muted by by calling "SoundManager.instance.IsBackgroundMusicMuted()"
- You can check if SFX is muted by by calling "SoundManager.instance.IsSoundFXMuted()"

 
