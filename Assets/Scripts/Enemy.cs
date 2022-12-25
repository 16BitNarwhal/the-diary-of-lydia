using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable {
    
    [SerializeField] private float health = 100;
    private float baseSpeed = 3;
    private UnityEngine.AI.NavMeshAgent agent;
    private new Collider collider;
    private float damageCooldown = 0.1f;
    private List<SpriteRenderer> sprites;

    void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.angularSpeed = 0;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = baseSpeed;

        sprites = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        
        for (int i=sprites.Count-1;i>=0;i--) {
            if (sprites[i].sortingLayerName != "Entity") 
                sprites.RemoveAt(i);
        }

        collider = GetComponent<Collider>();
        if (collider == null) 
            collider = GetComponentInChildren<Collider>();
    }

    void Update() {
        
    }

    // when collide with projectile, lose health (by projectile)
    private float lastDamageTime = 0;
    public void TakeDamage(float damage) {
        if (Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;
        health -= damage;

        foreach (SpriteRenderer sprite in sprites) {
            StartCoroutine(FlashRed(sprite));
        }
        
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    IEnumerator FlashRed(SpriteRenderer sprite) {
        Color c = sprite.color;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = c;
    }

}
