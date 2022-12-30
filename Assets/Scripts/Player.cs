using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

    public static Player instance;
    [SerializeField] private float health = 100;

    [SerializeField] private GameObject avatar;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject projectile;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float attackRate = 0.5f;
    [SerializeField] private float damageCooldown = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;

    private Vector2 input;
    private Vector2 mousePos;
    private Vector2 direction;
    private bool mouse_pressed;
    private float lastShot;

    // Start is called before the first frame update
    void Start() {
        instance = this;
        transform.position = new Vector2(0, 0);
        rb = GetComponent<Rigidbody2D>();
        animator = avatar.GetComponent<Animator>();
        sprite = avatar.GetComponent<SpriteRenderer>();
    }

    void Update() {
        HandleInput();
        HandleAttack();
    }

    void FixedUpdate() {
        HandleMovement();
        HandleAnimation();
    }

    void HandleInput() {
        // movement input
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // mousePos and direction
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)avatar.transform.position;
        direction.Normalize();

        // mouse pressed
        mouse_pressed = Input.GetMouseButton(0);
    }

    void HandleMovement() {
        rb.velocity = new Vector2(input.x, input.y) * speed;
    }

    void HandleAttack() {
        if (mouse_pressed && (Time.time - lastShot > attackRate)) {
            lastShot = Time.time;
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.GetComponent<Projectile>().SetVelocity(direction * projectileSpeed);
        }
    }

    void HandleAnimation() {
        // set animator values
        animator.SetBool("is_walking", input != Vector2.zero);

        // set arrow rotation
        arrow.transform.localPosition = direction * 0.75f;
        arrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mousePos.y - avatar.transform.position.y, mousePos.x - avatar.transform.position.x) * Mathf.Rad2Deg - 90);
    }

    private float lastDamageTime = 0;
    public void TakeDamage(float damage) {
        if (Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;
        health -= damage;
        StartCoroutine(FlashRed());
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    IEnumerator FlashRed() {
        Color c = sprite.color;
        for (int i=0;i<3;i++) {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Trap(float seconds) {
        StartCoroutine(Trapped(seconds));
    }

    IEnumerator Trapped(float seconds) {
        float oldSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(seconds);
        speed = oldSpeed;
    }

    public bool IsTrapped() {
        return speed == 0;
    }

}