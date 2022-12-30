using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {
    
    [SerializeField] private float stunTime = 3f;
    [SerializeField] private float lifeSpan = 5f;
    
    private new Collider2D collider;
    private ContactFilter2D filter = new ContactFilter2D();
    void Start() {
        collider = GetComponentInChildren<Collider2D>();
        filter.SetLayerMask(LayerMask.GetMask("Player"));
        Destroy(gameObject, lifeSpan);
    }


    Collider2D[] collisions = new Collider2D[10];
    void Update() {
        collider.OverlapCollider(filter, collisions);
        
        foreach (Collider2D col in collisions) {
            if (col == null) continue;
            
            Player.instance.Trap(stunTime);
            Destroy(gameObject);
        }
    }

}
