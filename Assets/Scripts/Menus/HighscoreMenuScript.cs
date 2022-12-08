using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreMenuScript : MonoBehaviour
{

    [SerializeField] private GameObject _mainCanvas; //Canvas full of main menu UI objects
    [SerializeField] private List<Text> _storyHighscoreTexts;
    [SerializeField] private List<Text> _storyTimeTexts;
    [SerializeField] private List<Text> _arcadeHighscoreTexts;
    [SerializeField] private List<Text> _arcadeTimeTexts;
    [SerializeField] private GameObject _storyMode;
    [SerializeField] private GameObject _hordeMode;

    // Start is called before the first frame update
    void Start()
    {
        _storyMode.SetActive(true);

        List<HighScoreEntry> storyHighScores = HighScoreManager.GetScores();
        List<BestTimeEntry> storyBestTimes = HighScoreManager.GetTimes();
        List<HighScoreEntry> arcadeHighScores = HighScoreManager.GetHordeScores();
        List<BestTimeEntry> arcadeBestTimes = HighScoreManager.GetHordeTimes();

        for (int i = 0; i < storyHighScores.Count; i++)
        {
            _storyHighscoreTexts[storyHighScores.Count - (i + 1)].text = $"{storyHighScores.Count - i}. {storyHighScores[i]}";
            _storyTimeTexts[storyBestTimes.Count - (i + 1)].text = $"{storyBestTimes.Count - i}. {storyBestTimes[i]}";
            _arcadeHighscoreTexts[arcadeHighScores.Count - (i + 1)].text = $"{arcadeHighScores.Count - i}. {arcadeHighScores[i]}";
            _arcadeTimeTexts[arcadeBestTimes.Count - (i + 1)].text = $"{arcadeBestTimes.Count - i}. {arcadeBestTimes[i]}";
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

    public void OnArcadeModePressed() {
        _storyMode.SetActive(true);
        _hordeMode.SetActive(false);
    }

    public void OnHordeModePressed() {
        _storyMode.SetActive(false);
        _hordeMode.SetActive(true);
    }

}
