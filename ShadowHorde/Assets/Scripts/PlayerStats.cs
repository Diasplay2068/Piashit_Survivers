using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth { get; private set; }

    [Header("XP / Level")]
    public int level { get; private set; } = 1;
    public float xp { get; private set; }
    public float xpToNextLevel { get; private set; } = 100f;
    public float xpScaling = 1.4f; // multiplier per level

    public UnityEvent onDeath;
    public UnityEvent onLevelUp;

    void Start() => currentHealth = maxHealth;

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        if (currentHealth <= 0) onDeath?.Invoke();
    }

    public void Heal(float amount) =>
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

    public void AddXP(float amount)
    {
        xp += amount;
        while (xp >= xpToNextLevel)
        {
            xp -= xpToNextLevel;
            level++;
            xpToNextLevel *= xpScaling;
            onLevelUp?.Invoke();
        }
    }
}