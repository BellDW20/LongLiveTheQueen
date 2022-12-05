using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreMenuScript : MonoBehaviour
{

    [SerializeField] private GameObject _mainCanvas; //Canvas full of main menu UI objects
    [SerializeField] private List<Text> _highscoreTexts;
    [SerializeField] private List<Text> _timeTexts;

    // Start is called before the first frame update
    void Start()
    {
        List<HighScoreEntry> highScores = HighScoreManager.GetScores();
        for (int i = 0; i < highScores.Count; i++)
        {
            _highscoreTexts[highScores.Count - (i + 1)].text = $"{highScores.Count - i}. {highScores[i]}";
        }

        List<BestTimeEntry> bestTimes = HighScoreManager.GetTimes();
        for (int i = 0; i < bestTimes.Count; i++)
        {
            _timeTexts[bestTimes.Count - (i + 1)].text = $"{bestTimes.Count - i}. {bestTimes[i]}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetBackInput(0) || InputManager.GetBackInput(1))
        {
            _mainCanvas.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
