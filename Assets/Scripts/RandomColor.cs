using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour {

    [SerializeField] private bool continuous = false;

    private SpriteRenderer sprite;
    private Color nextColor;
    void Start() {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        SetNextColor();
    }

    void Update() {
        if (continuous) {
            sprite.color = Color.Lerp(sprite.color, nextColor, Time.deltaTime);

            if (sprite.color == nextColor)
                SetNextColor();
        }
    }

    void SetNextColor() {
        nextColor = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
    }
}
