using System;
using System.Collections;
using UnityEngine;

public class DummyEnemiesMovement : MonoBehaviour
{
    #region
    //public float speed = 5f;
    //public float chaseDistance = 5f;
    //public Animator animator;
    //public SpriteRenderer spriteRenderer;
    //public Transform player;
    //public bool isStatue = false;
    //private bool isCrumbling = false;
    //private bool isMoving = false;

    //private void Start()
    //{
    //    animator = GetComponent<Animator>();
    //    spriteRenderer = GetComponent<SpriteRenderer>();
    //}

    //void Update()
    //{
    //    if (!isCrumbling && player != null)
    //    {
    //        // Use the squared distance for efficiency
    //        float distanceSquared = (player.position - transform.position).sqrMagnitude;

    //        if (distanceSquared <= chaseDistance * chaseDistance)
    //        {
    //            Vector2 direction = (player.position - transform.position).normalized;

    //            // Round the components
    //            direction.x = Mathf.Round(direction.x);
    //            direction.y = Mathf.Round(direction.y);

    //            // Ensure movement only in one direction
    //            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
    //            {
    //                // Move horizontally
    //                transform.Translate(Vector2.right * direction.x * speed * Time.deltaTime);
    //                SetAnimationParameters(direction.x, 0);
    //            }
    //            else
    //            {
    //                // Move vertically
    //                transform.Translate(Vector2.up * direction.y * speed * Time.deltaTime);
    //                SetAnimationParameters(0, direction.y);
    //            }
    //        }
    //        else
    //        {
    //            SetAnimationParameters(0, 0);
    //        }
    //    }
    //}

    //// Set the animation parameters based on the movement direction
    //protected void SetAnimationParameters(float horizontal, float vertical)
    //{
    //    isMoving = horizontal != 0 || vertical != 0;

    //    // Set parameters for horizontal movement
    //    if (horizontal != 0)
    //    {
    //        animator.SetBool("MovingHorizontally", isMoving);
    //        animator.SetBool("MovingVertically", false);

    //        // Flip the sprite when moving to the right
    //        spriteRenderer.flipX = horizontal > 0;
    //    }
    //    // Set parameters for vertical movement
    //    else if (vertical != 0)
    //    {
    //        animator.SetBool("MovingHorizontally", false);
    //        animator.SetBool("MovingVertically", isMoving);

    //        // Set the correct animation for moving down or up immediately
    //        if (vertical < 0)
    //        {
    //            animator.SetBool("MovingDown", isMoving);
    //            animator.SetBool("MovingUp", false);
    //        }
    //        else if (vertical > 0)
    //        {
    //            animator.SetBool("MovingDown", false);
    //            animator.SetBool("MovingUp", isMoving);
    //        }
    //    }
    //    // Set parameters for no movement
    //    else
    //    {
    //        animator.SetBool("MovingHorizontally", false);
    //        animator.SetBool("MovingVertically", false);
    //        animator.SetBool("MovingDown", false);
    //        animator.SetBool("MovingUp", false);
    //    }
    //}

    //protected void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        animator.SetTrigger("SetTrigger");
    //        isStatue = false;
    //        StartCoroutine(CantMove());
    //        StartCoroutine(MovingHorizon());
    //    }
    //    else
    //    {
    //        isStatue = true;
    //        speed = 0;
    //    }
    //}

    //IEnumerator CantMove()
    //{
    //    speed = 0;
    //    isCrumbling = true;
    //    // Wait for the crumbling animation to finish
    //    yield return null;
    //}

    //IEnumerator MovingHorizon()
    //{
    //    yield return new WaitForSeconds(0.1f); // Adjust the delay as needed

    //    // Resume movement based on the last known direction
    //    isCrumbling = false;
    //    speed = 5;
    //    chaseDistance = 8;
    //}
    #endregion

    public float speed = 5f;
    public float chaseDistance = 5f;

    private Animator animator;
    private bool isCrumbling = false;
    private bool isMoving = false;

    private Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Set the enemy to start in a static animation
        animator.SetBool("IsMoving", false);
        StartCoroutine(CrumbleAndMove());
    }

    void Update()
    {
        if (!isCrumbling && player != null)
        {
            // Use the squared distance for efficiency
            float distanceSquared = (player.position - transform.position).sqrMagnitude;

            if (distanceSquared <= chaseDistance * chaseDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;

                // Round the components
                direction.x = Mathf.Round(direction.x);
                direction.y = Mathf.Round(direction.y);

                // Ensure movement only in one direction
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    // Move horizontally
                    transform.Translate(Vector2.right * direction.x * speed * Time.deltaTime);
                    SetAnimationParameters(direction.x, 0);
                }
                else
                {
                    // Move vertically
                    transform.Translate(Vector2.up * direction.y * speed * Time.deltaTime);
                    SetAnimationParameters(0, direction.y);
                }
            }
            else
            {
                SetAnimationParameters(0, 0);
            }
        }
    }

    // Set the animation parameters based on the movement direction
    protected void SetAnimationParameters(float horizontal, float vertical)
    {
        isMoving = horizontal != 0 || vertical != 0;

        animator.SetBool("IsMoving", isMoving);

        // Set parameters for horizontal movement
        if (horizontal != 0)
        {
            // Flip the sprite when moving to the right
            if (horizontal > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private IEnumerator CrumbleAndMove()
    {
        isCrumbling = true;

        // Trigger the crumbling animation
        animator.SetBool("SetTrigger", true);

        // Wait for the crumbling animation to start (adjust the duration as needed)
        yield return new WaitForSeconds(0.1f);

        // Reset the trigger to avoid looping
        animator.SetBool("SetTrigger", false);

        // Halt movement during the animation
        speed = 0;

        // Wait for the crumbling animation to finish
        yield return new WaitForSeconds(0.5f);

        // Resume movement based on the last known direction
        isCrumbling = false;
        speed = 5f;
    }
}