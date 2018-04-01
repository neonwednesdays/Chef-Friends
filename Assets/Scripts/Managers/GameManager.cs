using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Super Awesome Game Controller
*/
public class GameManager : MonoBehaviour {
    // game stuff
    private string GameState;

    private ProgressView progressComponent;
    private int _currentTurn;
    public int CurrentTurn {
        get { return _currentTurn; }
        set {
            _currentTurn = value;
            progressComponent.Text = "Turn: " + _currentTurn;
        }
    }
    public int turnInterval = 3;

    // player stuff
    PlayerManager Player1;
    // PlayerManager Player2;

    // Use this for initialization
    void Start () {
        Player1 = new PlayerManager();
        progressComponent = new ProgressView();

        _currentTurn = 0;
        CurrentTurn = _currentTurn;
    }

    // Update is called once per frame
    void Update () {
    }

    // uses the Cook card
    public void useCook(int power) {
    }

    // create singleton
    private static GameManager _instance;
    public static GameManager getInstance { get { return _instance; } }
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
}
