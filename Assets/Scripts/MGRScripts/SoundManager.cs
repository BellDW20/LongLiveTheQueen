using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {
    
    public AudioSource musicSourceBegin; //Used for playing the beginning portion of music
    public AudioSource musicSourceLoop; //Used for playing the looping portino of music
    public AudioClip[] begin; //References to beginning portions of music associated with music IDs
    public AudioClip[] loop; //References to looping portions of music associated with music IDs

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
                I_PlayMusic(Music.LEVEL_1);
                break;
            case "Level2Scene":
                I_PlayMusic(Music.LEVEL_2);
                break;
            case "BossRoom":
                I_PlayMusic(Music.BOSS_THEME);
                break;
            case "GameOverScene":
                if(LevelManagerScript.WasGameWon()) {
                    I_PlayMusic(Music.GAME_CLEAR);
                } else {
                    I_PlayMusic(Music.GAME_OVER);
                }
                break;
            case "IntroCutscene":
                I_PlayMusic(Music.INTRO);
                break;
            case "MainMenu":
                I_PlayMusic(Music.TITLE_THEME);
                break;
        }
    }

    //Internal play music command, plays the given music track
    //with looping setup by default
    private void I_PlayMusic(Music musicID) {
        //Set the beginning and looping portions to the correct clips
        musicSourceBegin.clip = begin[(int)musicID];
        musicSourceLoop.clip = loop[(int)musicID];

        //Play the beginning portion of the music immediately
        musicSourceBegin.Play();

        //Schedule (accurately) for the looping portion to play
        //when the beginning portion of the track is over
        musicSourceLoop.PlayScheduled(AudioSettings.dspTime + begin[(int)musicID].length);
        musicSourceLoop.loop = true;
    }

    //Internal play sound effects command, plays the given sound effect
    private void I_PlaySFX(SFX sfxID) {
        sfxSource.PlayOneShot(sfx[(int)sfxID]);
    }

    //External play music command, plays the given music track
    //with looping setup by default
    public static void PlayMusic(Music musicID) {
        if (instance == null) { return; }
        instance.I_PlayMusic(musicID);
    }

    //External play sound effects command, plays the given sound effect
    public static void PlaySFX(SFX sfxID) {
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
