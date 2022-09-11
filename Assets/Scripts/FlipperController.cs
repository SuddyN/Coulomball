using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperController: MonoBehaviour {

    public float charge;

    public Color positiveColor;
    public Color negativeColor;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = charge > 0 ? Color.Lerp(Color.white, positiveColor, charge) : Color.Lerp(negativeColor, Color.white, charge);
    }
}
