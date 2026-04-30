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
    Rigidbody2D rb;
    Transform player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hp = maxHealth;
    }

    void Start() => player = GameObject.FindGameObjectWithTag("Player")?.transform;

    void FixedUpdate()
    {
        if (player == null) return;
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag("Player") || attackCooldown > 0) return;
        col.GetComponent<PlayerStats>()?.TakeDamage(damage);
        attackCooldown = 1f / attackRate;
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0) Die();
    }

    void Die()
    {
        player?.GetComponent<PlayerStats>()?.AddXP(xpReward);
        Destroy(gameObject);
    }

    // Call from EnemySpawner to scale difficulty
    public void ScaleStats(float healthMult, float speedMult)
    {
        maxHealth *= healthMult;
        hp = maxHealth;
        speed *= speedMult;
    }
}