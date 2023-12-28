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

    public Animator anim;
    protected bool isStatue = true;

    protected bool isPlayerInrange = false;
    public float atkDistance;

    protected SpriteRenderer sr;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < distanceBetween)
        {
            // Get the normalized direction vector towards the player
            Vector2 difference = (player.transform.position - transform.position).normalized;

            // Restrict the movement to four directions by rounding the components
            difference.x = Mathf.Round(difference.x);
            difference.y = Mathf.Round(difference.y);

            // Move the enemy towards the player using the restricted direction
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + difference, speed * Time.deltaTime);


            if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
            {
                // Handle left and right movement
                HandleHorizontalMovement(difference);
            }
            else
            {
                // Handle up and down movement
                HandleVerticalMovement(difference);
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

    protected void HandleHorizontalMovement(Vector2 difference)
    {
        if (difference.x > 0)
        {
            SetHorizontalMovementAnimations(true);
        }
        else if (difference.x < 0)
        {
            SetHorizontalMovementAnimations(false);
        }
    }

    protected void HandleVerticalMovement(Vector2 difference)
    {
       float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(distance);

        if (distance < atkDistance)
        {
            if (difference.y > 0)
            {
                SetVerticalMovementAnimations(true);
                StartCoroutine(attackingUp());
            }
            else if (difference.y < 0) 
            {
                Debug.Log(" should attack down");
                StartCoroutine(attackingDown());
            }
        }  
    }
    protected void SetHorizontalMovementAnimations(bool isMovingRight)
    {
        anim.SetBool("IsMoving", true);
        anim.SetBool("IsMovingUp", false);
        anim.SetBool("IsMovingDown", false);
        anim.SetBool("StatueTrigger", false);
        sr.flipX = isMovingRight;
    }

    protected void SetVerticalMovementAnimations(bool isMovingUp)
    {
        anim.SetBool("IsMovingUp", isMovingUp);
        anim.SetBool("IsMoving", false);
        anim.SetBool("IsMovingDown", !isMovingUp);
        StartCoroutine(cantMove());

        float verticalSpeed = isMovingUp ? speed : -speed;
        transform.Translate(Vector2.up * verticalSpeed * Time.deltaTime);
    }


    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player entered the trigger zone, trigger transforming animation
            anim.SetTrigger("StatueTrigger");
            isStatue = false;
        }

        else if (other.CompareTag("PlayerIsInRange"))
        {
            isPlayerInrange = true;
            StartCoroutine(attacking());
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerIsInRange"))
        {
            Debug.Log("Exit the collider trigger");
            isPlayerInrange = false;
            anim.SetBool("ScorpionAtk", false);
            anim.SetBool("ScorAtkDown", false);
            anim.SetBool("IsMoving", true);
            speed = 6;
        }
    }

    IEnumerator cantMove()
    {
        anim.SetTrigger("StatueTrigger");
        speed = 0;
        isStatue = false;
        yield return new WaitForSeconds(.7f);
        speed = 7;
    }

    IEnumerator attacking()
    {
        anim.SetBool("IsMoving", false);
        speed = 0;
        anim.SetTrigger("ScorpionAtk");
        yield return null;
    }

    IEnumerator attackingUp()
    {
        Debug.Log("Executing AtkUp coroutine");
        anim.SetBool("IsMovingUp", true);
        speed = 0;
        anim.SetBool("AtkUp", true);
        yield return null;
    }

    IEnumerator attackingDown()
    {
        Debug.Log("Executing attackingDown coroutine");
        anim.SetBool("IsMovingDown", true);
        speed = 0;
        anim.SetTrigger("ScorAtkDown");
        yield return null;
    }
}
