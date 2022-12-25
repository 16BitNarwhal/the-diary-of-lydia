using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSortOrder : MonoBehaviour {
    
    private List<SpriteRenderer> sr;

    void Start() {
        sr = new List<SpriteRenderer>();
        if (GetComponent<SpriteRenderer>() != null) {
            sr.Add(GetComponent<SpriteRenderer>());
        }
        foreach (Transform child in transform) {
            SpriteRenderer childSr = child.GetComponent<SpriteRenderer>();
            if (childSr != null) {
                sr.Add(childSr);
            }
        }
    }

    void Update() {
        foreach (SpriteRenderer childSr in sr) {
            childSr.sortingOrder = Mathf.RoundToInt(1 / (gameObject.transform.position.y+5) * 1000);
        }
    }
}
