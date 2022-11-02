using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreMenuScript : MonoBehaviour
{

    [SerializeField] private GameObject _mainCanvas; //Canvas full of main menu UI objects
    [SerializeField] private List<Text> _highscoreTexts;

    // Start is called before the first frame update
    void Start()
    {
        List<HighScoreEntry> highScores = HighScoreManager.GetScores();
        for (int i = 0; i < highScores.Count; i++)
        {
            _highscoreTexts[highScores.Count - (i + 1)].text = $"{highScores.Count - i}. {highScores[i]}";
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
