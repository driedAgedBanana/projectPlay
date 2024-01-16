using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAtk : MonoBehaviour
{
    public int damage = 1;
    public float Range = 3;
    public Animator playerAnimator; // Assign this in the Unity Editor

    private bool isAttacking = false;

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
        }
    }

    void Attack()
    {
        //get attack direction
        Vector2 Dir = Vector2.zero;
        PlayerMovement PM = GetComponent<PlayerMovement>();
        if(PM.MyDir == PlayerMovement.PlayerDir.Up)
        {
            Dir = Vector2.up;
        }
        else if(PM.MyDir == PlayerMovement.PlayerDir.Down)
        {
            Dir = Vector2.down;
        }
        else if (PM.MyDir == PlayerMovement.PlayerDir.Left)
        {
            Dir = Vector2.left;
        }
        else if (PM.MyDir == PlayerMovement.PlayerDir.Right)
        {
            Dir = Vector2.right;
        }

        //raycast detection for attack
        RaycastHit2D HitInfo = Physics2D.Raycast(transform.position, Dir, Range);
        if (HitInfo && HitInfo.transform.GetComponent<dummyEnemyAtk>())
        {
            dummyEnemyAtk DEA = HitInfo.transform.GetComponent<dummyEnemyAtk>();
            DEA.TakeDamage(damage);
        }

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
