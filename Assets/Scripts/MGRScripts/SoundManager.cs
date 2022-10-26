using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    //music constants here
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
    public AudioSource sfxSource;
    public AudioClip[] sfx;

    private static SoundManager instance;

    void Awake() {
        instance = this;
    }

    private void Start() {
        //startup music depending on the scene
    }

    private void I_PlayMusic(int musicID) {
        musicSourceBegin.clip = begin[musicID];
        musicSourceLoop.clip = loop[musicID];
        musicSourceBegin.Play();
        musicSourceLoop.PlayScheduled(AudioSettings.dspTime + begin[musicID].length);
    }

    private void I_PlaySFX(int sfxID) {
        sfxSource.PlayOneShot(sfx[sfxID]);
    }

    public static void PlayMusic(int musicID) {
        instance.I_PlayMusic(musicID);
    }

    public static void PlaySFX(int sfxID) {
        instance.I_PlaySFX(sfxID);
    }

    public static void StopAllSounds() {
        instance.musicSourceBegin.Stop();
        instance.musicSourceLoop.Stop();
        instance.sfxSource.Stop();
    }

}
