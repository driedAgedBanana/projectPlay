using System.Collections;
using UnityEngine;

public class DummyEnemiesMovement : MonoBehaviour
{
    public float speed = 5f;
    public float chaseDistance = 5f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Transform player;
    public bool isStatue = false;
    private bool isCrumbling = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isCrumbling && player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= chaseDistance)
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
                }
                else
                {
                    // Move vertically
                    transform.Translate(Vector2.up * direction.y * speed * Time.deltaTime);
                }
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("SetTrigger");
            isStatue = false;
            StartCoroutine(CantMove());
            StartCoroutine(MovingHorizon());
        }
        else
        {
            isStatue = true;
            speed = 0;
        }
    }

    IEnumerator CantMove()
    {
        isCrumbling = true;
        // Wait for the crumbling animation to finish
        yield return null;
    }

    IEnumerator MovingHorizon()
    {
        yield return new WaitForSeconds(0.8f);
        // Resume movement
        isCrumbling = false;
        animator.SetBool("MovingHorizontally", true);
        speed = 5;
        chaseDistance = 8;
    }
}
