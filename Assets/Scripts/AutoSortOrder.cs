using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSortOrder : MonoBehaviour {
    
    private List<SpriteRenderer> sr;

    void Start() {
        sr = new List<SpriteRenderer>();
        SearchChildren(transform);
    }

    void Update() {
        foreach (SpriteRenderer childSr in sr) {
            childSr.sortingOrder = Mathf.RoundToInt(1 / (gameObject.transform.position.y+5) * 1000);
        }
    }

    void SearchChildren(Transform child) {
        if (child!=transform && child.GetComponent<AutoSortOrder>() != null) {
            return;
        }
        if (child.GetComponent<SpriteRenderer>() != null) {
            sr.Add(child.GetComponent<SpriteRenderer>());
        }
        foreach (Transform t in child) {
            SearchChildren(t);
        }
    }

}
