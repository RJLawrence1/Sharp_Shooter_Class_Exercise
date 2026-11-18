using TMPro;
using UnityEngine;

// Put this on an empty GameObject called "GameManager".
public class GameManager : MonoBehaviour
{
    [SerializeField] int enemiesToWin = 10;
    [SerializeField] GameObject winPanel;      // MISSION COMPLETE panel
    [SerializeField] EnemySpawner spawner;
    [SerializeField] TMP_Text counterText;     // optional: "Robots: 3 / 10"
    [SerializeField] bool useDifficultyRamp = true; // optional extension on/off

    int enemiesDefeated;
    bool gameWon;

    void Start()
    {
        winPanel.SetActive(false);
        UpdateCounter();
    }

    // Called by EnemyHealth every time a robot dies
    public void EnemyDefeated()
    {
        if (gameWon) return;

        enemiesDefeated++;
        UpdateCounter();

        if (useDifficultyRamp)
        {
            spawner.IncreaseDifficulty();
        }

        if (enemiesDefeated >= enemiesToWin)
        {
            gameWon = true;
            spawner.StopSpawning();
            winPanel.SetActive(true);
        }
    }

    void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text = "Robots: " + enemiesDefeated + " / " + enemiesToWin;
        }
    }
}
