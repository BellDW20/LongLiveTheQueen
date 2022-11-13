using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo {

    //The maximum health that each type of player has,
    //indexed by the type constants above
    public static readonly int[] PLAYER_MAX_HEALTH = {
        100, 120, 100, 100
    };
    public static readonly string[] PLAYER_NAME = {
       "Commando", "Sniper", "Pyro", "Healer"
    };

    public int type; //The type of player (commando, sniper, etc.) this player is
    public int score; //This player's current score
    public int level; //The current level of this player
    public int stock; //How many stocks this player has left
    public float health; //The player's current health
    public float damageScale; //The damage scale depending on the player's current level

    //Sets up a player info for a given type of player
    public PlayerInfo(PlayerType type) {
        this.type = (int)type;
        this.health = GetMaxHealth();
        this.score = 0;
        this.level = 1;
        this.stock = 3;
        this.damageScale = 1;
    }

    //Resets the player's health for the next level
    public void ClearForNextLevel() {
        health = PLAYER_MAX_HEALTH[type];
    }

    //Returns the maximum health this player can have according to its type
    public float GetMaxHealth() {
        return PLAYER_MAX_HEALTH[type];
    }

    //Returns the name this player has according to its type
    public string GetName() {
        return PLAYER_NAME[type];
    }

    //Adds an amount of points to the player's score
    //returning true if the player leveled up as a result
    public bool AddToScore(int points) {
        score += points;
        int lastLevel = level;
        
        //While we still have enough points to level up...
        while(score > ScoreForLevelUp(level)) {
            //increase our level
            level++;
            //and set our damage scale to the next level's damage scale
            damageScale = DamageScaleAtLevel(level);
        }

        //if we leveled up, return true, otherwise return false
        return level > lastLevel;
    }

    //Returns a factor from 0 to 1 indicating how far
    //this player is from leveling up
    public float ProgressToNextLevel() {
        float lastLevelScore = ScoreForLevelUp(level - 1);
        //returns a factor of how many points we've earned since the last level
        //divided by the number of points needed to level up
        return (score - lastLevelScore) / (ScoreForLevelUp(level) - lastLevelScore);
    }

    //Returns how many points (cumulatively) are needed to achieve a specified level
    private static int ScoreForLevelUp(int level) {
        return 80 * level * level * level;
    }

    //Returns how much a player's damage output is scaled at a given level
    private static float DamageScaleAtLevel(int level) {
        return Mathf.Sqrt(level);
    }

}
