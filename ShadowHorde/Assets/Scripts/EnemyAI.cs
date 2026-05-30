using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 30f;
    public float speed = 2f;
    public float damage = 10f;
    public float attackRate = 1f;
    public float xpReward = 20f;

    float hp;
    float attackCooldown;
    bool isDead = false;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    Transform player;

    void Awake()
    {
        rb   = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr   = GetComponent<SpriteRenderer>();
        hp   = maxHealth;
    }

    void Start() => player = GameObject.FindGameObjectWithTag("Player")?.transform;

    void FixedUpdate()
    {
        if (player == null || isDead) return;

        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

        if (dir.x < 0) sr.flipX = true;
        else if (dir.x > 0) sr.flipX = false;
    }

    void Update()
    {
        if (isDead) return;
        attackCooldown -= Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (isDead || !col.CompareTag("Player") || attackCooldown > 0) return;
        anim?.SetTrigger("attack");
        col.GetComponent<PlayerStats>()?.TakeDamage(damage);
        attackCooldown = 1f / attackRate;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        hp -= amount;
        anim?.SetTrigger("hurt");
        if (hp <= 0) Die();
    }

    void Die()
    {
        isDead = true;

        // Para o movimento e física
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Desativa todos os colliders para não ser empurrado
        foreach (var col in GetComponents<Collider2D>())
            col.enabled = false;

        anim?.SetTrigger("death");

        player?.GetComponent<PlayerStats>()?.AddXP(xpReward);
        GameManager.Instance?.AddScore(1);

        Destroy(gameObject, 1f);
    }

    public void ScaleStats(float healthMult, float speedMult)
    {
        maxHealth *= healthMult;
        hp = maxHealth;
        speed *= speedMult;
    }
}
