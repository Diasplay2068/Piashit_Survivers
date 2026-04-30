using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject projectilePrefab;
    public float fireRate = 1f;       // shots per second
    public float range = 8f;
    public float damage = 20f;

    float cooldown;

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown > 0) return;

        Transform target = FindClosestEnemy();
        if (target == null) return;

        Shoot(target);
        cooldown = 1f / fireRate;
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float bestDist = range;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < bestDist) { bestDist = d; closest = e.transform; }
        }
        return closest;
    }

    void Shoot(Transform target)
    {
        Vector2 dir = (target.position - transform.position).normalized;
        GameObject p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        p.GetComponent<Projectile>()?.Init(dir, damage);
    }

    // Call this on level-up upgrades
    public void UpgradeFireRate(float bonus) => fireRate += bonus;
    public void UpgradeDamage(float bonus) => damage += bonus;
    public void UpgradeRange(float bonus) => range += bonus;
}