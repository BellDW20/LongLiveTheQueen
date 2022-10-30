using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    //music constants here
    public const int MUS_LEVEL_1 = 0;
    public const int MUS_GAME_OVER = 1;
    public const int MUS_INTRO = 2;
    public AudioSource musicSourceBegin;
    public AudioSource musicSourceLoop;
    public AudioClip[] begin;
    public AudioClip[] loop;

    public const int SFX_MACHINE_GUN_SHOT = 0;
    public const int SFX_SNIPER_RIFLE_SHOT = 1;
    public const int SFX_EXPLOSION = 2;
    public const int SFX_ENEMY_DEATH = 3;
    public const int SFX_ENEMY_HIT = 4;
    public const int SFX_BOMB_WHISTLE = 5;
    public const int SFX_LEVEL_CLEAR = 6;
    public const int SFX_DASH = 7;
    public AudioSource sfxSource;
    public AudioClip[] sfx;

    private static SoundManager instance;

    void Awake() {
        instance = this;
    }

    private void Start() {
        SetVolume(1);
        //startup music depending on the scene
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

    private void I_PlayMusic(int musicID) {
        musicSourceBegin.clip = begin[musicID];
        musicSourceLoop.clip = loop[musicID];
        musicSourceBegin.Play();
        musicSourceLoop.PlayScheduled(AudioSettings.dspTime + begin[musicID].length);
        musicSourceLoop.loop = true;
    }

    private void I_PlaySFX(int sfxID) {
        sfxSource.PlayOneShot(sfx[sfxID]);
    }

    public static void PlayMusic(int musicID) {
        if (instance == null) { return; }
        instance.I_PlayMusic(musicID);
    }

    public static void PlaySFX(int sfxID) {
        if (instance == null) { return; }
        instance.I_PlaySFX(sfxID);
    }

    public static void StopAllSounds() {
        if (instance == null) { return; }
        instance.musicSourceBegin.Stop();
        instance.musicSourceLoop.Stop();
        instance.sfxSource.Stop();
    }

    public static void SetVolume(float volume) {
        if(instance==null) { return; }
        instance.musicSourceBegin.volume = volume;
        instance.musicSourceLoop.volume = volume;
        instance.sfxSource.volume = volume;
    }

}
