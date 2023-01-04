using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

    public static Player instance;
    [SerializeField] private float health = 100;
    [SerializeField] private MoreMountains.Tools.MMProgressBar healthBarUI;

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

    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;
    void Start() {
        instance = this;
        transform.position = new Vector2(0, 0);
        rb = GetComponent<Rigidbody2D>();
        animator = avatar.GetComponent<Animator>();
        sprite = avatar.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
    }

    void Update() {
        HandleInput();
        HandleAttack();
        healthBarUI.UpdateBar(health, 0, 100);
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

        // shield
        if (Input.GetKeyDown(KeyCode.Q)) Shield();

        // heal
        if (Input.GetKeyDown(KeyCode.E)) Heal();
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
        if (isShielded || Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;
        health -= damage;
        StartCoroutine(DamageFlash());
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    IEnumerator DamageFlash() {
        for (int i=0;i<2;i++) {
            sprite.material.shader = shaderGUItext;
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.05f);
            sprite.material.shader = shaderSpritesDefault;
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.05f);
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

    //////////////////////////////
    // Nath's Abilities : Shield, Heal, etc.
    //////////////////////////////

    [SerializeField] private GameObject shieldObject;
    private bool isShielded = false;
    private float shieldCooldown = 5f;
    private float lastShieldTime = -1000f;
    private float shieldDuration = 3f;
    private float shieldFailChance = 0f;
    public void Shield() {
        Debug.Log("Q");
        if (isShielded || Time.time - lastShieldTime < shieldCooldown) return;
        Debug.Log("Shield!");
        if (Random.value < shieldFailChance) {
            isShielded = false;
            lastShieldTime = Time.time;
            return;
        }

        StartCoroutine(Shielded());
    }

    IEnumerator Shielded() {
        isShielded = true;
        GameObject shield = Instantiate(shieldObject, transform.position, Quaternion.identity);
        shield.transform.parent = transform;
        shield.transform.localPosition = new Vector3(0, 0.5f, 0);
        yield return new WaitForSeconds(shieldDuration);
        lastShieldTime = Time.time;
        isShielded = false;
        Destroy(shield);
    }

    [SerializeField] private GameObject healObject;
    private float healCooldown = 5f;
    private float lastHealTime = -1000f;
    private float healAmount = 10f;
    private float healFailChance = 0f;
    public void Heal() {
        Debug.Log("E");
        if (Time.time - lastHealTime < healCooldown) return;
        Debug.Log("Heal!");
        lastHealTime = Time.time;

        if (Random.value < healFailChance) return;
        health = Mathf.Min(health + healAmount, 100);

        GameObject heal = Instantiate(healObject, transform.position, Quaternion.identity);
        heal.transform.parent = transform;
        heal.transform.localPosition = new Vector3(0, 0.5f, 0);
    }

}