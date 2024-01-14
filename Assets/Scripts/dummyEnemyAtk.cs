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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth > 0)
        {
            // Trigger hit animation
            animator.SetTrigger("isHit");
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        // Trigger death animation
        animator.SetBool("Die", true);
        Destroy(gameObject, 1.5f); // Destroy the enemy after 1.5 seconds (adjust as needed)
    }
    IEnumerator DestroyAfterAnimation()
    {
        // Wait for the duration of the death animation
        yield return new WaitForSeconds(1.0f); // Adjust this time based on the death animation duration

        // Destroy the enemy object
        Destroy(gameObject);
    }
}