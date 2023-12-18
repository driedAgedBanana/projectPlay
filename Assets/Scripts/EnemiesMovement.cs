using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemiesMovement : MonoBehaviour
{
    public GameObject player;
    public float speed;

    public float distanceBetween;

    private float distance;

    private Animator anim;
    private bool isStatue = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        // Limit movement to up, down, right, and left
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        int roundedAngle = Mathf.RoundToInt(angle / 90.0f) * 90; // Round to nearest 90 degrees
        Vector2 limitedDirection = Quaternion.Euler(0, 0, roundedAngle) * Vector2.right;

        if (distance < distanceBetween)
        {
            // Move the enemy
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + limitedDirection, speed * Time.deltaTime);
            // Rotate the enemy based on the movement direction
            transform.rotation = Quaternion.Euler(Vector3.forward * roundedAngle);

            // Trigger animations based on movement direction
            anim.SetBool("IsMovingUp", roundedAngle == 90);
            anim.SetBool("IsMovingDown", roundedAngle == -90);
            anim.SetBool("IsMoving", true);

            // Check for transforming condition
            if (isStatue)
            {
                // Trigger transforming animation
                anim.SetTrigger("ScorpionTransform");
                isStatue = false;
            }
        }
        else
        {
            // Stop moving and reset animation parameters
            anim.SetBool("IsMovingUp", false);
            anim.SetBool("IsMovingDown", false);
            anim.SetBool("IsMoving", false);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player entered the trigger zone, trigger transforming animation
            anim.SetTrigger("StatueTrigger");
            isStatue = false;
        }
    }
}
