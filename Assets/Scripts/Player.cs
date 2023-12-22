using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class Player : MonoBehaviour, IDamageable {

    public static Player instance;
    [SerializeField] private float health = 50;
    [SerializeField] private MoreMountains.Tools.MMProgressBar healthBarUI;

    [SerializeField] private GameObject avatar;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject cheatProjectile;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float attackRate = 0.5f;
    [SerializeField] private float damageCooldown = 1f;

    [SerializeField] private GameObject blankLydia;
    [SerializeField] private AudioClip hurtSound, deathSound, shieldSound, healSound;
    [SerializeField] private float volume;

    public int bossesDefeated = 0;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;

    private Vector2 input;
    private Vector2 mousePos;
    private Vector2 direction;
    private bool mousePressed;
    private bool cheatActive;
    private float lastShot;

    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;
    private bool isDead;
    private bool isWin;
    private AudioSource audioSource;

    void Start() {
        isDead = false;
        instance = this;
        // transform.position = new Vector2(0, 0);
        rb = GetComponent<Rigidbody2D>();
        animator = avatar.GetComponent<Animator>();
        sprite = avatar.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update() {
        if (isDead) return;
        if (bossesDefeated == 5) {
            gameWinUI.SetActive(true);
            isWin = true;
        }
        HandleInput();
        HandleAttack();
    }

    void FixedUpdate() {
        HandleUI();
        HandleMovement();
        HandleAnimation();
    }

    void HandleUI() {
        if (healthBarUI != null)
            healthBarUI.UpdateBar(health, 0, 50);
        // update healUI opacity based on cooldown
        if (shieldUI != null) {
            Color c = shieldUI.color;
            c.a = Mathf.Clamp01((Time.time - lastShieldTime) / shieldCooldown);
            shieldUI.color = c;
        }
        if (healUI != null) {
            Color c = healUI.color;
            c.a = Mathf.Clamp01((Time.time - lastHealTime) / healCooldown);
            healUI.color = c;
        }
    }

    void HandleInput() {
        // movement input
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // mousePos and direction
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)avatar.transform.position;
        direction.Normalize();

        // mouse pressed
        mousePressed = Input.GetMouseButton(0);

        // cheat
        cheatActive = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space);

        // shield
        if (Input.GetKeyDown(KeyCode.Q)) Shield();

        // heal
        if (Input.GetKeyDown(KeyCode.E)) Heal();
    }

    void HandleMovement() {
        rb.velocity = new Vector2(input.x, input.y) * speed;
    }

    float lastCheat;
    void HandleAttack() {
        if (mousePressed && (Time.time - lastShot > attackRate)) {
            lastShot = Time.time;
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.GetComponent<Projectile>().SetVelocity(direction * projectileSpeed);
        }
        if (cheatActive && (Time.time - lastCheat > 0.1f)) {
            lastCheat = Time.time;
            GameObject proj = Instantiate(cheatProjectile, transform.position, Quaternion.identity);
            proj.GetComponent<Projectile>().SetVelocity(direction * projectileSpeed * 2.5f);
        }
    }

    void HandleAnimation() {
        // set animator values
        animator.SetBool("is_walking", input != Vector2.zero);
        blankLydia.GetComponent<Animator>().SetBool("is_walking", input != Vector2.zero);

        // update blank opacity based on bosses defeated
        blankLydia.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Max(0, 1 - bossesDefeated * 0.3f));

        // set arrow rotation
        arrow.transform.localPosition = direction * 0.75f;
        arrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mousePos.y - avatar.transform.position.y, mousePos.x - avatar.transform.position.x) * Mathf.Rad2Deg - 90);
    }

    private bool canDamage = true;
    public void TakeDamage(float damage) {
        if (isWin) return;
        if (isDead) return;
        if (!canDamage) return;
        health -= damage;
        StartCoroutine(DamageFlash());
        if (health <= 0) {
            isDead = true;
            input = Vector2.zero;
            gameOverUI.SetActive(true);
            
            audioSource.PlayOneShot(deathSound, volume);
            // DEATH ANIM
        } else {
            audioSource.PlayOneShot(hurtSound, volume);
        }
    }

    IEnumerator DamageFlash() {
        canDamage = false;
        for (int i=1;i<=7;i++) {
            // change sprite opacity
            sprite.color = new Color(1, 1, 1, 0);
            blankLydia.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(.2f/(float)i);
            sprite.color = new Color(1, 1, 1, 1);
            blankLydia.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(.2f/(float)i);
            // sprite.material.shader = shaderGUItext;
            // sprite.color = Color.white;
            // yield return new WaitForSeconds(0.05f);
            // sprite.material.shader = shaderSpritesDefault;
            // sprite.color = Color.white;
            // yield return new WaitForSeconds(0.05f);
        }
        canDamage = true;
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

    [SerializeField] private Image shieldUI;
    [SerializeField] private GameObject shieldObject;
    private bool isShielded = false;
    private float shieldCooldown = 9f;
    private float lastShieldTime = -1000f;
    private float shieldDuration = 1.5f;
    private float shieldFailChance = .5f;
    public void Shield() {
        if (shieldObject == null) return;
        if (isShielded || Time.time - lastShieldTime < shieldCooldown) return;
        lastShieldTime = Time.time;

        if (Random.value < shieldFailChance) {
            isShielded = false;
            return;
        }

        StartCoroutine(Shielded());
    }

    IEnumerator Shielded() {
        isShielded = true;
        GameObject shield = Instantiate(shieldObject, transform.position, Quaternion.identity);
        shield.transform.parent = transform;
        shield.transform.localPosition = new Vector3(0, 0.5f, 0);

        audioSource.PlayOneShot(shieldSound, volume);

        yield return new WaitForSeconds(shieldDuration);
        isShielded = false;
        Destroy(shield);
    }

    [SerializeField] private Image healUI;
    [SerializeField] private GameObject healObject;
    private float healCooldown = 10f;
    private float lastHealTime = -1000f;
    private float healAmount = 7.213f;
    private float healFailChance = .6f;
    public void Heal() {
        if (healObject == null) return;
        if (Time.time - lastHealTime < healCooldown) return;
        lastHealTime = Time.time;

        if (Random.value < healFailChance) return;
        health = Mathf.Min(health + healAmount, 50f);


        GameObject heal = Instantiate(healObject, transform.position, Quaternion.identity);
        heal.transform.parent = transform;
        heal.transform.localPosition = new Vector3(0, 0.5f, 0);

        audioSource.PlayOneShot(healSound, volume);
    }

}