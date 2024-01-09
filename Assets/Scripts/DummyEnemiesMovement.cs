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

    public float speed;
    public float checkRadius;
    public float AtkRadius;

    public bool shouldRotate;

    public LayerMask DetectPlayer;

    private Transform target;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    public Vector3 direction;

    private bool isInChasingRange;
    private bool isInAtkRange;

    private bool isCrumbling;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        shouldRotate = true;
    }

    private void Update()
    {
        Debug.Log("shouldRotate: " + shouldRotate);  // Add this line for debugging

        animator.SetBool("IsMoving", isInChasingRange);

        isInChasingRange = Physics2D.OverlapCircle(transform.position, checkRadius, DetectPlayer);
        isInAtkRange = Physics2D.OverlapCircle(transform.position, AtkRadius, DetectPlayer);

        direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction.Normalize();
        movement = direction;
        if (shouldRotate)
        {
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);
        }

        if (isInChasingRange && !isInAtkRange && !isCrumbling)
        {
            StartCoroutine(CrumbleAndMove());
        }
    }

    private IEnumerator CrumbleAndMove()
    {
        isCrumbling = true;
        // Trigger the crumbling animation
        animator.SetTrigger("SetTrigger");

        // Wait for the crumbling animation to finish
        yield return new WaitForSeconds(0.1f);

        // Resume movement based on the last known direction
        isCrumbling = false;
        speed = 5;
        MoveCharacter(movement);
    }

    private void MoveCharacter(Vector2 dir)
    {
        Debug.Log("Moving character");
        rb.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
    }
}