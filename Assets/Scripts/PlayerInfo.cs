using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo {

    public const int TYPE_COMMANDO = 0;
    public const int TYPE_SNIPER = 1;
    public static readonly int[] PLAYER_MAX_HEALTH = {
        100, 100
    };

    public int type, score, level, stock;
    public float health, damageScale;

    public PlayerInfo(int type) {
        this.type = type;
        this.health = GetMaxHealth();
        this.score = 0;
        this.level = 1;
        this.stock = 3;
        this.damageScale = 1;
    }

    public PlayerInfo(int type, float health, int score, int level, int stock) {
        this.type = type;
        this.health = health;
        this.score = score;
        this.level = level;
        this.stock = stock;
    }

    public void ClearForNextLevel() {
        health = PLAYER_MAX_HEALTH[type];
    }

    public float GetMaxHealth() {
        return PLAYER_MAX_HEALTH[type];
    }

    public void AddToScore(int points) {
        score += points;
        while(score > ScoreForLevelUp(level)) {
            damageScale = DamageScaleAtLevel(level);
            level++;
        }
    }

    public float ProgressToNextLevel() {
        float lastLevelScore = ScoreForLevelUp(level - 1);
        return (score - lastLevelScore) / (ScoreForLevelUp(level) - lastLevelScore);
    }

    private static int ScoreForLevelUp(int level) {
        return 80 * level * level * level;
    }

    private static float DamageScaleAtLevel(int level) {
        return Mathf.Sqrt(level);
    }

}
