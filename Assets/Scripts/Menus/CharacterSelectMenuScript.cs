using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectMenuScript : MonoBehaviour {

    [SerializeField] private GameObject _playerSelectorPrefab;
    private List<PlayerSelectorScript> _playersJoined;
    private bool[] _joysticksJoined;
    void Start() {
        _playersJoined = new List<PlayerSelectorScript>();
        _joysticksJoined = new bool[4];
    }

    void Update() {
        //For each controller...
        for(int joystickNum = 0; joystickNum < 4; joystickNum++) {
            //If that controller hasn't joined as a player yet and tries to join...
            if (!_joysticksJoined[joystickNum] && InputManager.GetFireInput(joystickNum)) {
                //Add that controller as the next player...
                int playerNum = _playersJoined.Count;
                InputManager.AssignPlayerToJoystick(playerNum, joystickNum);

                //Giving that player a selection menu
                GameObject psObj = Instantiate(_playerSelectorPrefab, gameObject.transform);
                _playersJoined.Add(psObj.GetComponent<PlayerSelectorScript>());
                _playersJoined[playerNum].Init(playerNum);
                
                _joysticksJoined[joystickNum] = true;
            } else if(InputManager.GetSpecialInput(joystickNum) && _playersJoined.Count > 0) {
                //Start the game if any controller tries to start
                BeginGame();
            } else if (InputManager.GetBackInput(joystickNum)) {
                //Go back to the main menu if any controller presses back
                SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "MainMenu");
            }
        }
    }

    private void BeginGame() {
        //Setup variables in the level manager for the game
        PlayerType[] pTypes = new PlayerType[_playersJoined.Count];
        for (int i = 0; i < _playersJoined.Count; i++) {
            pTypes[i] = _playersJoined[i].GetSelection();
        }
        LevelManagerScript.SetupGame(pTypes);

        //Then tell the level manager to start the first level using a full level transition
        if (LevelManagerScript.GetMode() == GameMode.ARCADE_MODE) {
            LevelManagerScript.BeginLevel(Level.LEVEL_1, LevelManagerScript.LEVEL_TRANSITION);
        } else {
            LevelManagerScript.BeginLevel(Level.HORDE_MODE, LevelManagerScript.LEVEL_TRANSITION);
        }
    }

}
