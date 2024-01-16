using System.Collections;
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
            StartCoroutine(AttackSequence());
        }
    }

    IEnumerator AttackSequence()
    {
        if (!isAttacking)
        {
            playerAnimator.Play("AttackNeko");
            isAttacking = true;
        }

        yield return new WaitForSeconds(0.6f);
        Attack();
    }

    void Attack()
    {
        Vector2 direction = GetAttackDirection();

        // Raycast detection for attack
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, Range);
        if (hitInfo && hitInfo.transform.TryGetComponent(out dummyEnemyAtk enemy))
        {
            enemy.TakeDamage(damage);
        }

        isAttacking=false;
    }

    Vector2 GetAttackDirection()
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        switch (playerMovement.MyDir)
        {
            case PlayerMovement.PlayerDir.Up:
                return Vector2.up;
            case PlayerMovement.PlayerDir.Down:
                return Vector2.down;
            case PlayerMovement.PlayerDir.Left:
                return Vector2.left;
            case PlayerMovement.PlayerDir.Right:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemies") && isAttacking)
        {
            EnemyHit(other.gameObject);
        }
    }

    void EnemyHit(GameObject enemy)
    {
        // Get the enemy script (replace "dummyEnemyAtk" with your actual script name)
        if (enemy.TryGetComponent(out dummyEnemyAtk enemyScript))
        {
            // Log a message to check if the method is called
            Debug.Log("Enemy Hit!");

            // Play hit animation and deal damage
            enemyScript.TakeDamage(damage);
        }
    }
}
