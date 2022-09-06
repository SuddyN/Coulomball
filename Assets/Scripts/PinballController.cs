using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballController: MonoBehaviour {
    public float charge;
    public float coulombConstant;
    public float maxDist;
    private GameObject[] planets;
    private Rigidbody2D _rigidbody2D;

    private void Start() {
        planets = GameObject.FindGameObjectsWithTag("Planet");
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        foreach (GameObject planet in planets) {
            PlanetController planetController = planet.GetComponent<PlanetController>();
            if (!planetController.chargeEnabled) {
                continue;
            }
            Vector2 diff = planet.transform.position - transform.position;
            float dist = diff.magnitude;
            if (dist > maxDist) {
                continue;
            }
            // F = kQ_1Q_2/r^2
            _rigidbody2D.AddForce(diff.normalized * (coulombConstant * charge * planetController.charge / (dist * dist)));
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
