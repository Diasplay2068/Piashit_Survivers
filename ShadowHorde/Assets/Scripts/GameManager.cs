using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public PlayerStats playerStats;
    public AutoShooter autoShooter;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public Slider healthBar;
    public Slider xpBar;
    public GameObject upgradePanel;
    public TextMeshProUGUI[] upgradeButtonTexts; // 3 buttons

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    int score;
    bool paused;

    // --- Possible upgrades pool ---
    readonly List<UpgradeOption> allUpgrades = new()
    {
        new("Cadência +0.5/s",  s => s.GetComponent<AutoShooter>().UpgradeFireRate(0.5f)),
        new("Dano +10",         s => s.GetComponent<AutoShooter>().UpgradeDamage(10f)),
        new("Alcance +2",       s => s.GetComponent<AutoShooter>().UpgradeRange(2f)),
        new("Velocidade +1",    s => s.GetComponent<PlayerController>().moveSpeed += 1f),
        new("Vida +30",         s => { var ps = s.GetComponent<PlayerStats>(); ps.Heal(30); }),
        new("Vida máx +30",     s => s.GetComponent<PlayerStats>().maxHealth += 30f),
    };

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        playerStats.onDeath.AddListener(GameOver);
        playerStats.onLevelUp.AddListener(ShowUpgradePanel);
    }

    void Update()
    {
        if (paused) return;
        score += Mathf.RoundToInt(Time.deltaTime * 10);
        scoreText.text = $"Score: {score}";
        levelText.text = $"Nível {playerStats.level}";
        healthBar.value = playerStats.currentHealth / playerStats.maxHealth;
        xpBar.value = playerStats.xp / playerStats.xpToNextLevel;
    }

    public void AddScore(int pts) => score += pts;

    // --- Level-up upgrades ---
    void ShowUpgradePanel()
    {
        paused = true;
        Time.timeScale = 0f;
        upgradePanel.SetActive(true);

        var choices = PickRandom(allUpgrades, 3);
        for (int i = 0; i < upgradeButtonTexts.Length; i++)
        {
            int idx = i;
            upgradeButtonTexts[i].text = choices[i].label;
            var btn = upgradeButtonTexts[i].GetComponentInParent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => ApplyUpgrade(choices[idx]));
        }
    }

    void ApplyUpgrade(UpgradeOption u)
    {
        u.apply(playerStats.gameObject);
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    // --- Game Over ---
    void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Score final: {score}";
    }

    public void Restart() =>
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

    // --- Helpers ---
    List<T> PickRandom<T>(List<T> src, int n)
    {
        var copy = new List<T>(src);
        var result = new List<T>();
        for (int i = 0; i < n && copy.Count > 0; i++)
        {
            int r = Random.Range(0, copy.Count);
            result.Add(copy[r]);
            copy.RemoveAt(r);
        }
        return result;
    }

    class UpgradeOption
    {
        public string label;
        public System.Action<GameObject> apply;
        public UpgradeOption(string label, System.Action<GameObject> apply)
        { this.label = label; this.apply = apply; }
    }
}