using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager: MonoBehaviour {
    private static GameManager _instance;
    private static InputManager _inputManager;
    private static FieldManager _fieldManager;

    public static GameManager Instance {
        get {
            if (_instance is null) {
                Debug.LogError("GameManager NULL");
            }
            return _instance;
        }
    }

    public static InputManager InputManager {
        get {
            if (_inputManager is null) {
                Debug.LogError("InputManager NULL");
            }
            return _inputManager;
        }
    }

    public static FieldManager FieldManager {
        get {
            if (_fieldManager is null) {
                Debug.LogError("FieldManager NULL");
            }
            return _fieldManager;
        }
    }

    public GameObject leftFlipper;
    public GameObject rightFlipper;
    public GameObject ball;
    public GameObject respawnPoint;
    [HideInInspector]
    public PlanetController[] planetControllers;
    public float coulombConstant;
    public int score;
    public int scoreIncrement;
    public int lives;
    public int startLives;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;

    public enum GameState {
        Playing, Menu
    }
    public GameState state;

    public static Vector2 calculateForce(float constant, float charge1, float charge2, Vector2 diff) {
        float dist = diff.magnitude;
        return diff.normalized * (constant * charge1 * charge2 / (dist * dist));
    }

    public void Pause() {
        state = GameState.Menu;
        Time.timeScale = 0;
    }

    public void UnPause() {
        state = GameState.Playing;
        Time.timeScale = 1;
    }

    public void UpdateScore(int newScore) {
        score = newScore;
        scoreText.text = "Score: " + newScore;
    }

    public void UpdateLives(int newLives) {
        lives = newLives;
        livesText.text = "Lives: " + newLives;
    }

    private void Awake() {
        DontDestroyOnLoad(this);
        _instance = this;
        _inputManager = gameObject.GetComponent<InputManager>();
        _fieldManager = gameObject.GetComponent<FieldManager>();
        UpdateLives(startLives);
        UpdateScore(0);

        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        planetControllers = new PlanetController[planets.Length];
        for (int i = 0; i < planets.Length; i++) {
            planetControllers[i] = planets[i].GetComponent<PlanetController>();
        }
    }

    private void Update() {
        if (lives <= 0) {
            Time.timeScale = 0;
            state = GameState.Menu;
            UpdateLives(startLives);
            UpdateScore(0);
        }
    }
}
