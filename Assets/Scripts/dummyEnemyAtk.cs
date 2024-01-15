using System.Collections;
using UnityEngine;

public class dummyEnemyAtk : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 10; // Update this to the correct max health value
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // dummyEnemyAtk script
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetBool("isHit", true);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Trigger death animation
        animator.SetBool("Die", true);

        // Destroy the enemy object after the death animation duration
        Destroy(gameObject, 1.5f);
    }

    IEnumerator DestroyAfterAnimation()
    {
        // Wait for the duration of the death animation
        yield return new WaitForSeconds(1.0f); // Adjust this time based on the death animation duration

        // Destroy the enemy object
        Destroy(gameObject);
    }
}