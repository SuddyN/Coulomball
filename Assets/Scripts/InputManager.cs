using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager: MonoBehaviour {

    public InputAction leftFlipperAction;
    public InputAction rightFlipperAction;
    public InputAction pauseAction;
    public float minFlipperRotation = -10;
    public float maxFlipperRotation = 30;
    public float flipperSpeed = 50;

    private void OnEnable() {
        leftFlipperAction.Enable();
        rightFlipperAction.Enable();
        pauseAction.Enable();
    }

    private void OnDisable() {
        leftFlipperAction.Disable();
        rightFlipperAction.Disable();
        pauseAction.Disable();
    }

    private void ControllGame() {
        bool isLeftPressed = leftFlipperAction.IsPressed();
        bool isRightPressed = rightFlipperAction.IsPressed();
        var leftRB = GameManager.Instance.leftFlipper.GetComponent<Rigidbody2D>();
        var rightRB = GameManager.Instance.rightFlipper.GetComponent<Rigidbody2D>();

        if (isLeftPressed && leftRB.rotation < maxFlipperRotation) {
            leftRB.angularVelocity = flipperSpeed * (maxFlipperRotation - leftRB.rotation);
        } else if (!isLeftPressed && leftRB.rotation > minFlipperRotation) {
            leftRB.angularVelocity = -(flipperSpeed * (leftRB.rotation - minFlipperRotation));
        } else {
            leftRB.angularVelocity = 0;
        }

        if (leftRB.rotation > maxFlipperRotation) {
            leftRB.rotation = maxFlipperRotation - 1;
            leftRB.angularVelocity = 0;
        }
        if (leftRB.rotation < minFlipperRotation) {
            leftRB.rotation = minFlipperRotation;
            leftRB.angularVelocity = 0;
        }

        if (isRightPressed && -(rightRB.rotation - 180) < maxFlipperRotation) {
            rightRB.angularVelocity = -(flipperSpeed * (maxFlipperRotation - -(rightRB.rotation - 180)));
        } else if (!isRightPressed && -(rightRB.rotation - 180) > minFlipperRotation) {
            rightRB.angularVelocity = flipperSpeed * (-(rightRB.rotation - 180) - minFlipperRotation);
        } else {
            rightRB.angularVelocity = 0;
        }

        if (-(rightRB.rotation - 180) > maxFlipperRotation) {
            rightRB.rotation = -(maxFlipperRotation - 181);
            rightRB.angularVelocity = 0;
        }
        if (-(rightRB.rotation - 180) < minFlipperRotation) {
            rightRB.rotation = -(minFlipperRotation - 180);
            rightRB.angularVelocity = 0;
        }
    }

    void Update() {
        if (GameManager.Instance.state == GameManager.GameState.Playing) {
            ControllGame();
            if (pauseAction.WasPressedThisFrame()) {
                GameManager.Instance.messageText.text = "Press Esc to Continue";
                GameManager.Instance.state = GameManager.GameState.Menu;
                Time.timeScale = 0;
            }
        } else if (GameManager.Instance.state == GameManager.GameState.Menu) {
            if (pauseAction.WasPressedThisFrame()) {
                GameManager.Instance.messageText.text = "";
                if (GameManager.Instance.lives <= 0) {
                    GameManager.Instance.game.SetActive(true);
                    GameManager.Instance.SetLives(GameManager.Instance.startLives);
                    GameManager.Instance.SetScore(0);
                }
                GameManager.Instance.state = GameManager.GameState.Playing;
                Time.timeScale = 1;
            }
        }
    }
}
