using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable {
    
    private static float randomDamageModifier = 0.1f;

    [SerializeField] private float health = 100;
    private float baseSpeed = 3;
    private UnityEngine.AI.NavMeshAgent agent;
    private List<Collider2D> colliders;
    [SerializeField] private float damage = 5f;
    private float damageCooldown = 0.1f;
    private List<SpriteRenderer> sprites;
    private ContactFilter2D filter = new ContactFilter2D();

    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;
    void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.angularSpeed = 0;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = baseSpeed;

        sprites = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        
        for (int i=sprites.Count-1;i>=0;i--) {
            if (sprites[i].sortingLayerName != "Entity") 
                sprites.RemoveAt(i);
        }

        colliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
        if (GetComponent<Collider2D>() != null)
            colliders.Add(GetComponent<Collider2D>());

        filter.SetLayerMask(LayerMask.GetMask("Player"));

    }

    private Collider2D[] collisions = new Collider2D[1];
    void Update() {
        foreach (Collider2D collider in colliders) {
            collisions = new Collider2D[1];
            collider.OverlapCollider(filter, collisions);

            if (collisions[0] == null) continue;
            float trueDamage = damage * (1 + Random.Range(-randomDamageModifier, randomDamageModifier));
            Player.instance.TakeDamage(trueDamage);
        }
    }

    // when collide with projectile, lose health (by projectile)
    private float lastDamageTime = 0;
    public void TakeDamage(float damage) {
        if (Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;
        health -= damage;

        foreach (SpriteRenderer sprite in sprites) {
            StartCoroutine(DamageFlash(sprite));
        }
        
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    IEnumerator DamageFlash(SpriteRenderer sprite) {
        sprite.material.shader = shaderGUItext;
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        sprite.material.shader = shaderSpritesDefault;
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }

}
