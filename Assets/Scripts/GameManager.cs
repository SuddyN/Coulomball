using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour {
    private static GameManager _instance;
    private static InputManager _inputManager;

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

    public GameObject leftFlipper;
    public GameObject rightFlipper;
    public GameObject ball;
    public GameObject respawnPoint;

    private void Awake() {
        _instance = this;
        _inputManager = gameObject.GetComponent<InputManager>();
        DontDestroyOnLoad(this);
    }
}
