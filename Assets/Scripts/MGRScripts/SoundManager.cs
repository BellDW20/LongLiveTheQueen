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

    //sfx constants here
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
