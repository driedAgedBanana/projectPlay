using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : MonoBehaviour
{
    public GameObject attackCollider; // Assign this in the Unity Editor
    public LayerMask enemyLayer;
    public int damage = 1;
    public Animator playerAnimator; // Assign this in the Unity Editor

    private bool isAttacking = false;

    private void Start()
    {
        attackCollider.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            Attack();
        }

        // Check if the attack animation is finished
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (isAttacking && stateInfo.normalizedTime >= 1.0f)
        {
            // Reset the "isAttacking" parameter
            playerAnimator.SetBool("isAttacking", false);
            isAttacking = false;

            // Disable the attack collider
            attackCollider.SetActive(false);
        }
    }

    void Attack()
    {
        isAttacking = true;
        attackCollider.SetActive(true);

        // Trigger the player's attack animation
        playerAnimator.SetBool("isAttacking", true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemies") && isAttacking)
        {
            EnemyHit(other.gameObject);
        }
    }

    void EnemyHit(GameObject enemy)
    {
        // Get the enemy script (replace "dummyEnemyAtk" with your actual script name)
        dummyEnemyAtk enemyScript = enemy.GetComponent<dummyEnemyAtk>();

        // Check if the enemy script is found
        if (enemyScript != null)
        {
            // Log a message to check if the method is called
            Debug.Log("Enemy Hit!");

            // Play hit animation and deal damage
            enemyScript.TakeDamage(damage);
        }
    }

}
