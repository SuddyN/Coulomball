using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager: MonoBehaviour {

    public float minLength;
    public float maxLength;
    public float multiplier;
    public Color negativeColor;
    public Color positiveColor;
    public Vector2 gridSize;
    public Vector2Int gridDensity;

    public float maxCalculationDist;

    public GameObject fieldParent;
    public GameObject fieldIndicatorPrefab;

    private Dictionary<GameObject, (Vector2, float)> fieldIndicators;

    private void InitField() {
        foreach (Transform child in fieldParent.transform) {
            Destroy(child.gameObject);
        }

        fieldIndicators = new Dictionary<GameObject, (Vector2, float)>();
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        GameManager.Instance.planetControllers = new PlanetController[planets.Length];
        for (int i = 0; i < planets.Length; i++) {
            GameManager.Instance.planetControllers[i] = planets[i].GetComponent<PlanetController>();
        }

        Vector3 gridOrigin = new Vector3(-(gridSize.x / 2), -(gridSize.y / 2), 0);
        for (int i = 0; i < gridDensity.x; i++) {
            for (int j = 0; j < gridDensity.y; j++) {
                Vector3 pos = gridOrigin + new Vector3(i * gridSize.x / gridDensity.x, j * gridSize.y / gridDensity.y, 0);
                GameObject newFieldIndicator = Instantiate(fieldIndicatorPrefab, fieldParent.transform.position, fieldParent.transform.rotation, fieldParent.transform);
                newFieldIndicator.transform.position = newFieldIndicator.transform.position + pos;
                fieldIndicators.Add(newFieldIndicator, (new Vector2(), 0));
            }
        }
    }

    public void UpdateField() {

        foreach (PlanetController planetController in GameManager.Instance.planetControllers) {
            if (!planetController.chargeEnabled) {
                continue;
            }
            Dictionary<GameObject, (Vector2, float)> updatedDict = new Dictionary<GameObject, (Vector2, float)>();
            foreach (var entry in fieldIndicators) {
                GameObject indicator = entry.Key;
                Vector2 force = entry.Value.Item1;
                float chargeTotal = entry.Value.Item2;
                Vector2 diff = planetController.transform.position - indicator.transform.position;
                force = force + GameManager.calculateForce(GameManager.Instance.coulombConstant, 1, planetController.charge, diff);
                chargeTotal += planetController.charge / (diff.magnitude * diff.magnitude);
                updatedDict.Add(indicator, (force, chargeTotal));
            }
            foreach (var entry in updatedDict) {
                GameObject indicator = entry.Key;
                Vector2 force = entry.Value.Item1;
                float chargeTotal = entry.Value.Item2;
                fieldIndicators[indicator] = (force, chargeTotal);
            }
        }

        foreach (var entry in fieldIndicators) {
            GameObject indicator = entry.Key;
            Vector2 force = entry.Value.Item1;
            float chargeTotal = entry.Value.Item2;

            Vector3 scale = new Vector3(Mathf.Clamp(force.magnitude * multiplier, minLength, maxLength), indicator.transform.localScale.y, indicator.transform.localScale.z);
            indicator.transform.localScale = scale;
            indicator.transform.right = force.normalized;

            SpriteRenderer spriteRenderer = indicator.GetComponent<SpriteRenderer>();
            Color newColor = Color.Lerp(negativeColor, positiveColor, chargeTotal / 15);
            newColor = Color.Lerp(new Color(0, 0, 0, 0), newColor, scale.x / maxLength);
            spriteRenderer.color = newColor;

        }
    }

    private void Start() {
        InitField();
        UpdateField();
    }
}
