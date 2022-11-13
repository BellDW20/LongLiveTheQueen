using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

//Data class which high score entries are described by
[System.Serializable]
public class HighScoreEntry {
    public int _playerType; //The type of player which made the high score
    public int _score; //The type of player which made the high score

    //Typical constructor to setup data
    public HighScoreEntry(int playerType, int score) {
        _playerType = playerType;
        _score = score;
    }

    //Returns a string representation of the entry
    public override string ToString()
    {
        string playerTypeString = PlayerInfo.PLAYER_NAME[_playerType];
        return $"{playerTypeString}: {_score}";
    }
};

public class HighScoreManager : MonoBehaviour {

    private const int HIGH_SCORE_TABLE_SIZE = 5;

    private static List<HighScoreEntry> _highScores;

    void Awake() {
        //On startup, if the high score file exists...
        if(File.Exists(HighScoreFilePath())) {
            //Load it by deserializing the list of high scores from binary format
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(HighScoreFilePath(), FileMode.Open);
            _highScores = formatter.Deserialize(stream) as List<HighScoreEntry>;

            //cleanup the file stream
            stream.Close();
        } else {
            //Otherwise, make a new list of high score entries
            _highScores = new List<HighScoreEntry>();
            //and add some dummy high scores to it
            for (int i = 0; i < HIGH_SCORE_TABLE_SIZE; i++) {
                _highScores.Add(new HighScoreEntry(0, (i+1)*1000));
            }
        }
        SaveToFile();
    }

    //Tries to log a score on game completion, returning the ranking
    //the score achieved on the leaderboard (1 through 5), or -1 if not a high score
    public static int LogScore(int playerType, int score) {
        int scorePos = -1;
        //find if the score fits in the current leaderboard
        for (int i = _highScores.Count - 1; i >= 0; i--) {
            if (score > _highScores[i]._score) {
                scorePos = i;
                break;
            }
        }
        //If not, we didn't get a high score
        if (scorePos == -1) { return -1; }

        //swap all lower values down to make room for high score
        for (int i = 0; i < scorePos; i++) {
            _highScores[i] = _highScores[i + 1];
        }
        //place the new high score in the correct position
        _highScores[scorePos] = new HighScoreEntry(playerType, score);
        SaveToFile(); //save our changes

        //and return the ranking of this score in the leaderboard
        return 5 - scorePos;
    }

    //Saves the current leaderboard to the persistent leaderboard file
    private static void SaveToFile() {
        //Open a file stream for writing binary data
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(HighScoreFilePath(), FileMode.Create);

        //using a binary format, serialize our list of high scores
        formatter.Serialize(stream, _highScores);

        //clean up our file stream
        stream.Close();
    }

    //Returns the path at which the persitent high score file resides
    public static string HighScoreFilePath() {
        return Application.persistentDataPath + "/highScores.bin";
    }

    //returns the list of high score entries
    public static List<HighScoreEntry> GetScores() => _highScores;

}
