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

        if ((isLeftPressed && leftRB.rotation < maxFlipperRotation) || leftRB.rotation < minFlipperRotation) {
            leftRB.angularVelocity = flipperSpeed * (maxFlipperRotation - leftRB.rotation);
        } else if ((!isLeftPressed && leftRB.rotation > minFlipperRotation) || leftRB.rotation > maxFlipperRotation) {
            leftRB.angularVelocity = -(flipperSpeed * (leftRB.rotation - minFlipperRotation));
        } else {
            leftRB.angularVelocity = 0;
        }

        leftRB.rotation = (leftRB.rotation > maxFlipperRotation) ? maxFlipperRotation : leftRB.rotation;
        leftRB.rotation = (leftRB.rotation < minFlipperRotation) ? minFlipperRotation : leftRB.rotation;

        if ((isRightPressed && -(rightRB.rotation - 180) < maxFlipperRotation) || -(rightRB.rotation - 180) < minFlipperRotation) {
            rightRB.angularVelocity = -(flipperSpeed * (maxFlipperRotation - -(rightRB.rotation - 180)));
        } else if ((!isRightPressed && -(rightRB.rotation - 180) > minFlipperRotation) || -(rightRB.rotation - 180) > maxFlipperRotation) {
            rightRB.angularVelocity = flipperSpeed * (-(rightRB.rotation - 180) - minFlipperRotation);
        } else {
            rightRB.angularVelocity = 0;
        }
        rightRB.rotation = (-(rightRB.rotation - 180) > maxFlipperRotation) ? -(maxFlipperRotation - 180) : rightRB.rotation;
        rightRB.rotation = (-(rightRB.rotation - 180) < minFlipperRotation) ? -(minFlipperRotation - 180) : rightRB.rotation;
    }

    void Update() {
        if (GameManager.Instance.state == GameManager.GameState.Playing) {
            ControllGame();
            if (pauseAction.WasPressedThisFrame()) {
                Debug.Log("paused");
                GameManager.Instance.state = GameManager.GameState.Menu;
                Time.timeScale = 0;
            }
        } else if (GameManager.Instance.state == GameManager.GameState.Menu) {
            if (pauseAction.WasPressedThisFrame()) {
                GameManager.Instance.state = GameManager.GameState.Playing;
                Time.timeScale = 1;
            }
        }
    }
}
