using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballController: MonoBehaviour {
    public float charge;
    public float maxDist;
    private Rigidbody2D _rigidbody2D;
    private int scoreMultiplier;

    public Color positiveColor;
    public Color negativeColor;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = charge > 0 ? Color.Lerp(Color.white, positiveColor, charge) : Color.Lerp(negativeColor, Color.white, charge);
        scoreMultiplier = 1;
    }

    private void Update() {
        foreach (PlanetController planetController in GameManager.Instance.planetControllers) {
            if (!planetController.chargeEnabled) {
                continue;
            }
            Vector2 diff = planetController.transform.position - transform.position;
            float dist = diff.magnitude;
            if (dist > maxDist) {
                continue;
            }
            // F = kQ_1Q_2/r^2
            _rigidbody2D.AddForce(GameManager.calculateForce(GameManager.Instance.coulombConstant, charge, planetController.charge, diff));
        }
        if (scoreMultiplier <= 0) {
            scoreMultiplier = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("DeathBox")) {
            transform.position = GameManager.Instance.respawnPoint.transform.position;
            _rigidbody2D.velocity = new Vector2(0, 0);
            _rigidbody2D.angularVelocity = 0;
            GameManager.Instance.SetLives(GameManager.Instance.lives - 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        switch (collision.gameObject.tag) {
            case ("Planet"):
                GameManager.Instance.SetScore(GameManager.Instance.score + (GameManager.Instance.scoreIncrement * scoreMultiplier++));
                break;
            case ("Flipper"):
                FlipperController flipperController = collision.gameObject.GetComponent<FlipperController>();
                this.charge = flipperController.charge;
                spriteRenderer.color = charge > 0 ? Color.Lerp(Color.white, positiveColor, charge) : Color.Lerp(negativeColor, Color.white, charge);
                scoreMultiplier = 1;
                break;
        }
    }
}
