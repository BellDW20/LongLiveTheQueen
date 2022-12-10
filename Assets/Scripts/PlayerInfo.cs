using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo {

    //The maximum health that each type of player has,
    //indexed by the type constants above
    public static readonly int[] PLAYER_MAX_HEALTH = {
        100, 90, 120, 100
    };
    public static readonly string[] PLAYER_NAME = {
       "Commando", "Sniper", "Pyro", "Healer"
    };

    public static readonly Color[] PLAYER_NUM_COLORS = {
        Color.cyan,
        Color.magenta,
        Color.yellow,
        Color.red
    };

    //Begin Leveling Info
    public const int MAX_HEALTH_LEVELS = 4;
    public static readonly float[] MAX_HEALTH_SCALE = {
        1, 1.5f, 1.75f, 1.9f, 2
    };
    public static readonly int[] MAX_HEALTH_COST = {
        0, 1, 1, 1, 2
    };

    public const int DAMAGE_LEVELS = 4;
    public static readonly float[] DAMAGE_SCALE = {
        1, 1.7f, 1.95f, 2.05f, 2.1f
    };
    public static readonly int[] DAMAGE_COST = {
        0, 1, 1, 2, 2
    };

    public const int SPREAD_LEVELS = 2;
    public static readonly int[] SPREAD_COST = {
        0, 2, 3
    };

    public const int SPECIAL_COOLDOWN_LEVELS = 3;
    public static readonly float[] SPECIAL_COOLDOWN_SCALE = {
        1, 0.75f, 0.6f, 0.5f
    };
    public static readonly int[] SPECIAL_COOLDOWN_COST = {
        0, 1, 2, 2
    };
    //End Leveling Info

    public int type; //The type of player (commando, sniper, etc.) this player is
    
    public int score; //This player's current score
    public int spendableScore; //The player's current score less how much they have spent.

    public int level; //The current level of this player
    public int spendableLevels; //The player's current level less how much they have spent
    public float time; //The time the player has taken for the game

    public int stock; //How many stocks this player has left
    public float health; //The player's current health


    public int maxHealthLevel; //The level to which the player's max health has been upgraded
    public int damageLevel; //The level to which the player's damage has been upgraded
    public int spreadLevel; //The level to which the player's spread shot has been upgraded
    public int specialCooldownLevel; //The level to which the player's special cooldown has been upgraded

    //public float damageScale; //The damage scale depending on the player's current level

    //Sets up a player info for a given type of player
    public PlayerInfo(PlayerType type) {
        this.type = (int)type;
        this.health = GetMaxHealth();
        this.score = 0;
        this.spendableScore = 0;
        this.level = 1;
        this.spendableLevels = 0;
        this.time = 0;
        this.stock = 3;

        this.maxHealthLevel = 0;
        this.damageLevel = 0;
        this.specialCooldownLevel = 0;
        this.spreadLevel = 0;
    }

    //Resets the player's health for the next level
    public void ClearForNextLevel() {
        ResetHealth();
    }

    public void ResetHealth() {
        health = GetMaxHealth();
    }

    //Returns the maximum health this player can have according to its type & max health level
    public float GetMaxHealth() {
        return MAX_HEALTH_SCALE[maxHealthLevel] * PLAYER_MAX_HEALTH[type];
    }

    //Returns the special cooldown scale this player has according to its level
    public float GetSpecialCooldownScale() {
        return SPECIAL_COOLDOWN_SCALE[specialCooldownLevel];
    }

    //Returns the damage scale of this player according to its level
    public float GetDamageScale() {
        return DAMAGE_SCALE[damageLevel];
    }

    //Returns the name this player has according to its type
    public string GetName() {
        return PLAYER_NAME[type];
    }

    //Attempts to buy the next level of a skill (if there is one).
    //Returns true if the purchase is successful and false if not.
    public bool TryToPurchaseSkill(PlayerSkill skill) {
        bool bought = false;
        if (skill == PlayerSkill.DAMAGE_SCALE && damageLevel != DAMAGE_LEVELS && DAMAGE_COST[damageLevel + 1] <= spendableLevels) {
            damageLevel++;
            spendableLevels -= DAMAGE_COST[damageLevel];
            bought = true;
        }
        else if (skill == PlayerSkill.MAX_HEALTH_SCALE && maxHealthLevel != MAX_HEALTH_LEVELS && MAX_HEALTH_COST[maxHealthLevel + 1] <= spendableLevels) {
            maxHealthLevel++;
            spendableLevels -= MAX_HEALTH_COST[maxHealthLevel];
            bought = true;
        }
        else if (skill == PlayerSkill.SPECIAL_COOLDOWN_SCALE && specialCooldownLevel != SPECIAL_COOLDOWN_LEVELS && SPECIAL_COOLDOWN_COST[specialCooldownLevel + 1] <= spendableLevels) {
            specialCooldownLevel++;
            spendableLevels -= SPECIAL_COOLDOWN_COST[specialCooldownLevel];
            bought = true;
        }
        else if (skill == PlayerSkill.SPREAD && spreadLevel != SPREAD_LEVELS && SPREAD_COST[spreadLevel + 1] <= spendableLevels) {
            spreadLevel++;
            spendableLevels -= SPREAD_COST[spreadLevel];
            bought = true;
        }
        return bought;
    }

    //Attempts to complete a purchase using spendable points if there is enough.
    //Returns true if the purchase is successful and false if not.
    public bool TryToPurchase(int points) {
        if(spendableScore >= points) {
            spendableScore -= points;
            return true;
        }
        return false;
    }


    //Attempts to complete a purchase using spendable levels if there is enough.
    //Returns true if the purchase is successful and false if not.
    public bool TryToPurchaseLevel(int lvlPoints) {
        if(spendableLevels >= lvlPoints) {
            spendableLevels -= lvlPoints;
            return true;
        }
        return false;
    }

    //Adds an amount of points to the player's score
    //returning true if the player leveled up as a result
    public bool AddToScore(int points) {
        score += points;
        spendableScore += points;
        int lastLevel = level;
        
        //While we still have enough points to level up...
        while(score > ScoreForLevelUp(level)) {
            //increase our level
            level++;
            //give the player one level skill point
            spendableLevels++;
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

    /*//Returns how much a player's damage output is scaled at a given level
    private static float DamageScaleAtLevel(int level) {
        return Mathf.Sqrt(level);
    }*/

}