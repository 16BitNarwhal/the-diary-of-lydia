using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] private GameObject avatar;
    [SerializeField] private float fallForce;
    [SerializeField] private Vector2 offset;
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private float lifeSpan = 1f;
    [SerializeField] private bool destroyOnCollision = true;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float damage = 10f;
    [SerializeField] private bool randomRotate = false;
    private new Collider2D collider;
    private ContactFilter2D contactFilter;
    private Vector2 velocity;

    private int rotationDirection;
    void Start() {
        collider = GetComponentInChildren<Collider2D>();

        avatar.transform.position = transform.position + (Vector3)offset;

        Invoke("DestroyProjectile", lifeSpan);

        contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.SetLayerMask(collisionMask);

        if (randomRotate) {
            avatar.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            rotationDirection = Random.Range(0, 2) * 2 - 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.position += (Vector3)velocity * Time.fixedDeltaTime;

        // rotate avatar according to global velocity offset by 90
        if (!randomRotate) {
            avatar.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90);
        } else {
            avatar.transform.Rotate(0, 0, 360 * rotationDirection * Time.fixedDeltaTime);
        }
    }

    Collider2D[] collisions = new Collider2D[10];
    void Update() {
        if (avatar.transform.localPosition.y < 0) {
            DestroyProjectile();
        }
        
        // get collisions
        collider.OverlapCollider(contactFilter, collisions);

        foreach (Collider2D col in collisions) {
            if (col == null) continue;

            IDamageable damageable = col.gameObject.GetComponentInParent<IDamageable>();
            if (damageable == null) damageable = col.gameObject.GetComponent<IDamageable>();
            if (damageable != null) {
                damageable.TakeDamage(damage);

                if (destroyOnCollision)
                    DestroyProjectile();
            }

            // check if object is a projectile
            Projectile proj = col.gameObject.GetComponent<Projectile>();
            if (proj != null) {
                if (destroyOnCollision)
                    DestroyProjectile();
                if (proj.destroyOnCollision)
                    proj.DestroyProjectile();
            }
        }
    }

    public void SetVelocity(Vector2 vel) {
        velocity = vel;
    }

    void DestroyProjectile() {
        if (deathParticle != null)
            Instantiate(deathParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public float GetDamage() { return damage; }

}
