using System.Collections;
using System.Collections.Generic;
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
    public PlanetController[] planetControllers;
    public float coulombConstant;

    public static Vector2 calculateForce(float constant, float charge1, float charge2, Vector2 diff) {
        float dist = diff.magnitude;
        return diff.normalized * (constant * charge1 * charge2 / (dist * dist));
    }

    private void Awake() {
        DontDestroyOnLoad(this);
        _instance = this;
        _inputManager = gameObject.GetComponent<InputManager>();
        _fieldManager = gameObject.GetComponent<FieldManager>();

        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        planetControllers = new PlanetController[planets.Length];
        for (int i = 0; i < planets.Length; i++) {
            planetControllers[i] = planets[i].GetComponent<PlanetController>();
        }
    }
}
