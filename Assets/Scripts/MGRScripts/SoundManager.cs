using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    //Music IDs to be used with PlayMusic and I_PlayMusic
    public const int MUS_LEVEL_1 = 0;
    public const int MUS_GAME_OVER = 1;
    public const int MUS_INTRO = 2;
    public AudioSource musicSourceBegin; //Used for playing the beginning portion of music
    public AudioSource musicSourceLoop; //Used for playing the looping portino of music
    public AudioClip[] begin; //References to beginning portions of music associated with music IDs
    public AudioClip[] loop; //References to looping portions of music associated with music IDs

    //Sound effect IDs to be used with PlaySFX and I_PlaySFX
    public const int SFX_MACHINE_GUN_SHOT = 0;
    public const int SFX_SNIPER_RIFLE_SHOT = 1;
    public const int SFX_EXPLOSION = 2;
    public const int SFX_ENEMY_DEATH = 3;
    public const int SFX_ENEMY_HIT = 4;
    public const int SFX_BOMB_WHISTLE = 5;
    public const int SFX_LEVEL_CLEAR = 6;
    public const int SFX_DASH = 7;
    public AudioSource sfxSource; //Used for playing sound effects
    public AudioClip[] sfx; //References to sound effect clips associated with SFX IDs

    private static SoundManager instance; //The sound manager of the active scene

    void Awake() {
        //Set this scene's sound manager object as the active sound manager
        instance = this;
    }

    private void Start() {
        //On startup, assume playback is at full volume
        SetVolume(1);

        //Play music corresponding to the scene we are in, if applicable
        switch(SceneManager.GetActiveScene().name) {
            case "Level1Scene":
                I_PlayMusic(MUS_LEVEL_1);
                break;
            case "GameOverScene":
                I_PlayMusic(MUS_GAME_OVER);
                break;
            case "IntroCutscene":
                I_PlayMusic(MUS_INTRO);
                break;
        }
    }

    //Internal play music command, plays the given music track
    //with looping setup by default
    private void I_PlayMusic(int musicID) {
        //Set the beginning and looping portions to the correct clips
        musicSourceBegin.clip = begin[musicID];
        musicSourceLoop.clip = loop[musicID];

        //Play the beginning portion of the music immediately
        musicSourceBegin.Play();

        //Schedule (accurately) for the looping portion to play
        //when the beginning portion of the track is over
        musicSourceLoop.PlayScheduled(AudioSettings.dspTime + begin[musicID].length);
        musicSourceLoop.loop = true;
    }

    //Internal play sound effects command, plays the given sound effect
    private void I_PlaySFX(int sfxID) {
        sfxSource.PlayOneShot(sfx[sfxID]);
    }

    //External play music command, plays the given music track
    //with looping setup by default
    public static void PlayMusic(int musicID) {
        if (instance == null) { return; }
        instance.I_PlayMusic(musicID);
    }

    //External play sound effects command, plays the given sound effect
    public static void PlaySFX(int sfxID) {
        if (instance == null) { return; }
        instance.I_PlaySFX(sfxID);
    }

    //Immediately stops all music and sound effects currently playing
    public static void StopAllSounds() {
        if (instance == null) { return; }
        instance.musicSourceBegin.Stop();
        instance.musicSourceLoop.Stop();
        instance.sfxSource.Stop();
    }

    //Sets the volume of both music and sound effects to the given
    //volume scale from 0 to 1
    public static void SetVolume(float volume) {
        if(instance==null) { return; }
        instance.musicSourceBegin.volume = volume;
        instance.musicSourceLoop.volume = volume;
        instance.sfxSource.volume = volume;
    }

}
