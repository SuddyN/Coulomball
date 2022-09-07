using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballController: MonoBehaviour {
    public float charge;
    public float maxDist;
    private Rigidbody2D _rigidbody2D;

    private void Start() {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
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
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("DeathBox")) {
            transform.position = GameManager.Instance.respawnPoint.transform.position;
            _rigidbody2D.velocity = new Vector2(0, 0);
            _rigidbody2D.angularVelocity = 0;
        }
    }
}
