using System.Collections;
using UnityEngine;

// Put this on the same Enemy as EnemyChase.
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int hitPoints = 3;

    [Header("Hit flash (optional feedback)")]
    [SerializeField] Renderer bodyRenderer;          // leave empty to use this object's Renderer
    [SerializeField] Color hitColor = Color.white;   // flash color when hit
    [SerializeField] float flashTime = 0.1f;

    Color originalColor;

    void Awake()
    {
        if (bodyRenderer == null)
        {
            bodyRenderer = GetComponent<Renderer>();
        }
        originalColor = bodyRenderer.material.color;
    }

    // "int damage" is the parameter. The player's raycast passes 1 as the argument.
    public void TakeDamage(int damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            gameObject.SetActive(false); // defeated
        }
        else
        {
            StartCoroutine(Flash());
        }
    }

    // Briefly change color so you can SEE the hit landed
    IEnumerator Flash()
    {
        bodyRenderer.material.color = hitColor;
        yield return new WaitForSeconds(flashTime);
        bodyRenderer.material.color = originalColor;
    }
}
