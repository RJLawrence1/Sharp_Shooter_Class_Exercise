using System.Collections;
using UnityEngine;

// Put this on the Enemy prefab (same as before, now with explosions).
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int hitPoints = 3;

    [Header("Explosion")]
    [SerializeField] GameObject explosionVFX;     // explosion particle prefab
    [SerializeField] float explosionRadius = 4f;
    [SerializeField] int explosionDamage = 1;
    [SerializeField] LayerMask enemyLayer;         // set to Enemy

    [Header("Hit flash")]
    [SerializeField] Renderer bodyRenderer;
    [SerializeField] Color hitColor = Color.white;
    [SerializeField] float flashTime = 0.1f;

    Color originalColor;
    bool isDead;                 // stops a robot from dying twice in a chain reaction
    GameManager gameManager;

    void Awake()
    {
        if (bodyRenderer == null)
        {
            bodyRenderer = GetComponent<Renderer>();
        }
        originalColor = bodyRenderer.material.color;

        // Spawned prefabs can't hold scene references, so find the GameManager in the scene
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Flash());
        }
    }

    void Die()
    {
        isDead = true;

        // Tell the GameManager a robot was defeated
        if (gameManager != null)
        {
            gameManager.EnemyDefeated();
        }

        Explode();
        Destroy(gameObject);
    }

    void Explode()
    {
        if (explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }

        // OverlapSphere: find every enemy collider inside the blast radius
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (Collider nearby in nearbyEnemies)
        {
            EnemyHealth enemy = nearby.GetComponent<EnemyHealth>();

            // Don't damage ourselves
            if (enemy != null && enemy.gameObject != gameObject)
            {
                enemy.TakeDamage(explosionDamage);
            }
        }
    }

    IEnumerator Flash()
    {
        bodyRenderer.material.color = hitColor;
        yield return new WaitForSeconds(flashTime);
        bodyRenderer.material.color = originalColor;
    }

    // Draws the blast radius in the Scene view when the enemy is selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
