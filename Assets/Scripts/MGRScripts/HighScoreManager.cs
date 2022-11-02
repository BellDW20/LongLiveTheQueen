using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class HighScoreEntry {
    public int _playerType;
    public int _score;
    public HighScoreEntry(int playerType, int score) {
        _playerType = playerType;
        _score = score;
    }

    public override string ToString()
    {
        string playerTypeString = (_playerType == PlayerInfo.TYPE_COMMANDO) ? "Commando" : "Sniper";
        return $"{playerTypeString}: {_score}";
    }
};

public class HighScoreManager : MonoBehaviour {

    private const int HIGH_SCORE_TABLE_SIZE = 5;

    private static List<HighScoreEntry> _highScores;

    void Start() {
        if(File.Exists(HighScoreFilePath())) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(HighScoreFilePath(), FileMode.Open);
            _highScores = formatter.Deserialize(stream) as List<HighScoreEntry>;
            stream.Close();
        } else {
            _highScores = new List<HighScoreEntry>();
            for (int i = 0; i < HIGH_SCORE_TABLE_SIZE; i++) {
                _highScores.Add(new HighScoreEntry(0, (i+1)*1000));
            }
        }
        SaveToFile();
    }

    private static void i_LogScore(int playerType, int score) {
        for (int i = _highScores.Count-1; i >= 0; i--) {
            if (score > _highScores[i]._score) {
                for (int j = i; j >= 0; j--)
                {
                    if (j - 1 < 0) break;
                    HighScoreEntry temp = _highScores[j];
                    _highScores[j - 1] = temp;
                }
                _highScores[i] = new HighScoreEntry(playerType, score);
                SaveToFile();
                break;
            }
        }
    }

    private static void SaveToFile() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(HighScoreFilePath(), FileMode.Create);
        formatter.Serialize(stream, _highScores);
        stream.Close();
    }

    public static string HighScoreFilePath() {
        return Application.persistentDataPath + "/highScores.bin";
    }

    public static void LogScore(int playerType, int score) {
        i_LogScore(playerType, score);
    }

    public static List<HighScoreEntry> GetScores() => _highScores;

}
