using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;

//Data class which high score entries are described by
[System.Serializable]
public class HighScoreEntry {
    public int _playerType; //The type of player which made the high score
    public int _score; //That player's high score

    //Typical constructor to setup data
    public HighScoreEntry(int playerType, int score) 
    {
        _playerType = playerType;
        _score = score;
    }

    //Returns a string representation of the score entry
    public override string ToString()
    {
        string playerTypeString = PlayerInfo.PLAYER_NAME[_playerType];
        return $"{playerTypeString}: {_score}";
    }
};

//Data class which high score entries are described by
[System.Serializable]
public class BestTimeEntry
{
    public List<int> _playerTypes; //The types of player which made the time
    public TimeSpan _time; //That players' time

    //Typical constructor to setup data
    public BestTimeEntry(List<int> playerTypes, TimeSpan time)
    {
        _playerTypes = playerTypes;
        _time = time;
    }

    //Returns a string representation of the time entry
    public override string ToString()
    {
        string playerTypeString = "";

        foreach (int playerType in _playerTypes)
        {
            playerTypeString += $"{PlayerInfo.PLAYER_NAME[playerType]}, ";
        }

        //Remove trailing ", "
        char[] charsToTrim = {' ', ','};
        playerTypeString.TrimEnd(charsToTrim);

        return $"{playerTypeString}: {_time}";
    }
};

public class HighScoreManager : MonoBehaviour {

    private const int HIGH_SCORE_TABLE_SIZE = 5;

    private static List<HighScoreEntry> _highScores;
    private static List<BestTimeEntry> _bestTimes;

    private static List<HighScoreEntry> _hordeHighScores;
    private static List<BestTimeEntry> _hordeBestTimes;

    void Awake() {

        //Set up regular mode high score file
        List<HighScoreEntry> tempScores = new List<HighScoreEntry>();
        for (int i = 0; i < HIGH_SCORE_TABLE_SIZE; i++)
        {
            tempScores.Add(new HighScoreEntry(0, (i + 1) * 1000));
        }
        _highScores = SetupFile(HighScoreFilePath(), tempScores);

        //Set up regular mode best times file
        List<BestTimeEntry> tempTimes = new List<BestTimeEntry>();
        List<int> dummyPlayers = new List<int> {
                0, 1, 2
            };
        for (int i = 0; i < HIGH_SCORE_TABLE_SIZE; i++)
        {
            tempTimes.Add(new BestTimeEntry(dummyPlayers, TimeSpan.FromSeconds(3599 - i*60)));
        }
        _bestTimes = SetupFile(BestTimeFilePath(), tempTimes);

        //Set up horde mode high score file
        tempScores = new List<HighScoreEntry>();
        for (int i = 0; i < HIGH_SCORE_TABLE_SIZE; i++)
        {
            tempScores.Add(new HighScoreEntry(0, (i + 1) * 1000));
        }
        _hordeHighScores = SetupFile(HordeHighScoreFilePath(), tempScores);

        //Set up regular mode best times file
        tempTimes = new List<BestTimeEntry>();
        for (int i = 0; i < HIGH_SCORE_TABLE_SIZE; i++)
        {
            tempTimes.Add(new BestTimeEntry(dummyPlayers, TimeSpan.FromSeconds(59 + i*60)));
        }
        _bestTimes = SetupFile(HordeBestTimeFilePath(), tempTimes);
    }

    /// <summary>
    /// Generically sets up a file for recording scores or times
    /// </summary>
    /// <typeparam name="T">What we are recording (either HighScoreEntry or BestTimeEntry</typeparam>
    /// <param name="filePath">Path to file on device</param>
    /// <param name="constants">If the file does not exist yet, constants to populate file or list with</param>
    /// <returns>List of scores</returns>
    private static List<T> SetupFile<T>(string filePath, List<T> constants)
    {
        List<T> scores = new List<T>();

        //If file exists, add scores from file
        if (File.Exists(filePath))
        {
            //Load it by deserializing the list of high scores (or times) from binary format
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);
            scores = formatter.Deserialize(stream) as List<T>;

            //cleanup the file stream
            stream.Close();
        }
        else
        {
            //Otherwise, add some dummy high scores to it
            foreach (T constant in constants)
            {
                scores.Add(constant);
            }
        }
        SaveToFile(filePath, scores);

        return scores;
    }

    /// <summary>
    /// Tries to log a score on game completion, returning the ranking
    /// </summary>
    /// <param name="playerType">Player class (Commando, sniper, etc.)</param>
    /// <param name="score">Score achieved by player</param>
    /// <param name="gameMode">Mode score was achieved in (0 for regular play, 1 for horde mode)</param>
    /// <returns>The score achieved on the leaderboard (1 through 5), or -1 if not a high score</returns>
    public static int LogScore(int playerType, int score, int gameMode) {

        int scorePos = -1;
        //Only change scores for this gamemode
        List<HighScoreEntry> scores = (gameMode == 0) ? _highScores : _hordeHighScores;

        //find if the score fits in the current leaderboard
        for (int i = scores.Count - 1; i >= 0; i--) {
            if (score > scores[i]._score) {
                scorePos = i;
                break;
            }
        }
        //If not, we didn't get a high score
        if (scorePos == -1) { return -1; }

        //swap all lower values down to make room for high score
        for (int i = 0; i < scorePos; i++) {
            scores[i] = scores[i + 1];
        }
        //place the new high score in the correct position
        scores[scorePos] = new HighScoreEntry(playerType, score);

        if(gameMode == 0)
        {
            _highScores = scores;
            SaveToFile(HighScoreFilePath(), _highScores); //save our changes to regular game mode file
        }
        else
        {
            _hordeHighScores = scores;
            SaveToFile(HordeHighScoreFilePath(), _hordeHighScores);
        }

        //and return the ranking of this score in the leaderboard
        return 5 - scorePos;
    }

    /// <summary>
    /// Tries to log a time on game completion, returning the ranking
    /// </summary>
    /// <param name="playerTypes">Player class(es) (Commando, sniper, etc.)</param>
    /// <param name="time">Time achieved by player</param>
    /// <param name="gameMode">Mode score was achieved in (0 for regular play, 1 for horde mode)</param>
    /// <returns>The time achieved on the leaderboard (1 through 5), or -1 if not a high score</returns>
    public static int LogTime(List<int> playerTypes, TimeSpan time, int gameMode)
    {
        int timePos = -1;

        //find if the time fits in the current leaderboard (regular mode)
        if(gameMode == 0)
        {
            for (int i = _bestTimes.Count - 1; i >= 0; i--)
            {
                if (time < _bestTimes[i]._time)
                {
                    timePos = i;
                    break;
                }
            }
            //If not, we didn't get a high score
            if (timePos == -1) { return -1; }

            //swap all lower values down to make room for high score
            for (int i = 0; i < timePos; i++)
            {
                _bestTimes[i] = _bestTimes[i + 1];
            }
            //place the new high score in the correct position
            _bestTimes[timePos] = new BestTimeEntry(playerTypes, time);
            SaveToFile(BestTimeFilePath(), _bestTimes); //save our changes
        }
        //find if the time fits in the current leaderboard (horde mode)
        else
        {
            for (int i = _hordeBestTimes.Count - 1; i >= 0; i--)
            {
                if (time > _hordeBestTimes[i]._time)
                {
                    timePos = i;
                    break;
                }
            }
            //If not, we didn't get a high score
            if (timePos == -1) { return -1; }

            //swap all lower values down to make room for high score
            for (int i = 0; i < timePos; i++)
            {
                _hordeBestTimes[i] = _hordeBestTimes[i + 1];
            }
            //place the new high score in the correct position
            _hordeBestTimes[timePos] = new BestTimeEntry(playerTypes, time);
            SaveToFile(HordeBestTimeFilePath(), _hordeBestTimes); //save our changes
        }

        //and return the ranking of this score in the leaderboard
        return 5 - timePos;
    }

    //Generically saves the current leaderboard to the persistent leaderboard file
    private static void SaveToFile<T>(string fileName, List<T> toSave) {
        //Open a file stream for writing binary data
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(fileName, FileMode.Create);

        //using a binary format, serialize our list of high scores
        formatter.Serialize(stream, toSave);

        //clean up our file stream
        stream.Close();
    }

    //Returns the path at which the persitent high score file resides
    public static string HighScoreFilePath() {
        return Application.persistentDataPath + "/highScores.bin";
    }

    //Returns the path at which the persitent best time file resides
    public static string BestTimeFilePath()
    {
        return Application.persistentDataPath + "/bestTimes.bin";
    }

    //Returns the path at which the persitent horde high score file resides
    public static string HordeHighScoreFilePath()
    {
        return Application.persistentDataPath + "/hordeHighScores.bin";
    }

    //Returns the path at which the persitent horde best time file resides
    public static string HordeBestTimeFilePath()
    {
        return Application.persistentDataPath + "/hordeBestTimes.bin";
    }

    //returns the list of high score entries
    public static List<HighScoreEntry> GetScores() => _highScores;

    //returns the list of best time entries
    public static List<BestTimeEntry> GetTimes() => _bestTimes;

    //returns the list of horde high score entries
    public static List<HighScoreEntry> GetHordeScores() => _hordeHighScores;

    //returns the list of horde best time entries
    public static List<BestTimeEntry> GetHordeTimes() => _hordeBestTimes;

}
