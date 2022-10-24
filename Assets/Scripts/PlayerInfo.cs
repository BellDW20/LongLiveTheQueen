using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo {

    const int TYPE_COMMANDO = 0;
    const int TYPE_SNIPER = 1;
    public static readonly int[] PLAYER_MAX_HEALTH = {
        100, 100
    };

    public int type, score, level;
    public float health;

    public PlayerInfo(int type) {
        this.type = type;
        this.health = PLAYER_MAX_HEALTH[type];
        this.score = 0;
        this.level = 1;
    }

    public PlayerInfo(int type, float health, int score, int level) {
        this.type = type;
        this.health = health;
        this.score = score;
        this.level = level;
    }

    public void ClearForNextLevel() {
        health = PLAYER_MAX_HEALTH[type];
    }

}
