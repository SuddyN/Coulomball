using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController: MonoBehaviour {
    public bool chargeEnabled;
    public float charge;

    public Color positiveColor;
    public Color negativeColor;
    private SpriteRenderer spriteRenderer;

    public void UpdateCharge(float charge) {
        this.charge = charge;
        spriteRenderer.color = charge > 0 ? Color.Lerp(Color.white, positiveColor, charge / 15) : Color.Lerp(negativeColor, Color.white, charge / 15);
        GameManager.FieldManager.UpdateField();
    }

    private void Start() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = charge > 0 ? Color.Lerp(Color.white, positiveColor, charge / 15) : Color.Lerp(negativeColor, Color.white, charge / 15);
    }
}
