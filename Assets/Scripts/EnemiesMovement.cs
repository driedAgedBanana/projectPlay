using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemiesMovement : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distanceBetween;

    private Animator anim;
    private bool isStatue = true;

    SpriteRenderer sr;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
        // Update is called once per frame
        void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < distanceBetween)
        {
            // Move the enemy towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            //Vector2 direction = (player.transform.position - transform.position).normalized;
            Vector2 difference = player.transform.position - transform.position;

            if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
            {
                if (difference.x > 0)
                {
                    anim.SetBool("IsMoving", true);
                    anim.SetBool("IsMovingUp", false);
                    anim.SetBool("IsMovingDown", false);
                    anim.SetBool("StatueTrigger", false);
                    
                    sr.flipX = true;
                }
                else if (difference.x < 0)
                {
                    anim.SetBool("IsMoving", true);
                    anim.SetBool("IsMovingUp", false);
                    anim.SetBool("IsMovingDown", false);
                    anim.SetBool("StatueTrigger", false);

                    sr.flipX = false;
                }
            }

            if (Mathf.Abs(difference.y) > Mathf.Abs(difference.x))
            {
                if (difference.y > 0)
                {
                    anim.SetBool("IsMovingUp", true);
                    anim.SetBool("IsMoving", false);
                    anim.SetBool("IsMovingDown", false);
                    anim.SetBool("StatueTrigger", false);
                }
                else if (difference.y < 0)
                {
                    anim.SetBool("IsMovingDown", true);
                    anim.SetBool("IsMoving", false);
                    anim.SetBool("IsMovingUp", false);
                    anim.SetBool("StatueTrigger", false);
                }
            }

            // Check for transforming condition
            if (isStatue && Mathf.Abs(difference.x) > 0.01f)
            {
                // Trigger transforming animation only for horizontal movement
                StartCoroutine(cantMove());
            }
        }
        else
        {
            // Reset the statue state when the player is not in the trigger zone
            isStatue = true;

            // Set animation parameters for idle state
            anim.SetBool("IsMovingUp", false);
            anim.SetBool("IsMovingDown", false);
            anim.SetBool("IsMoving", false);
            anim.SetBool("StatueTrigger", false);
        }
    }

    IEnumerator cantMove()
    {
        anim.SetTrigger("StatueTrigger");
        speed = 0;
        isStatue = false;
        yield return new WaitForSeconds(.5f);
        speed = 7;
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