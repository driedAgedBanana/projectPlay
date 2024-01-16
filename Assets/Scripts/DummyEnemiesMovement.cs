using System.Collections;
using UnityEngine;

public class DummyEnemiesMovement : MonoBehaviour
{
    public float speed;
    public float checkRadius;
    public float AtkRadius;

    public bool shouldRotate;

    public LayerMask DetectPlayer;

    protected Transform target;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector2 movement;
    public Vector3 direction;

    public float chasingSpeed = 5f;

    public bool isInChasingRange;
    public bool isInAtkRange;

    protected bool isCrumbling;
    protected bool canAtk = true;

    public int damageDeal = 2;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }

        shouldRotate = true;
    }

    protected void FixedUpdate()
    {
        UpdateAnimatorBools();

        isInChasingRange = Vector2.Distance(transform.position, target.position) < checkRadius * 2;
        isInAtkRange = Vector2.Distance(transform.position, target.position) < AtkRadius * 2;

        direction = target.position - transform.position;
        direction.Normalize();
        movement = direction;

        if (shouldRotate)
        {
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);
        }

        if (isInChasingRange)
        {
            StartCoroutine(CrumbleAndMove());
            MoveCharacter(movement);
        }
        if (isInAtkRange && canAtk)
        {
            StopMoving();
            StartCoroutine(IsAttacking());
        }
    }

    protected void StopMoving()
    {
        rb.velocity = Vector2.zero;
        speed = 0;
    }

    private IEnumerator CrumbleAndMove()
    {
        isCrumbling = true;
        animator.SetTrigger("SetTrigger");
        yield return new WaitForSeconds(0.1f);
        speed = 0;
        yield return new WaitForSeconds(0.6f);
        isCrumbling = false;
        speed = chasingSpeed;
    }

    private IEnumerator IsAttacking()
    {
        if(canAtk)
        {
            animator.Play("ScorpionAtk");
            canAtk = false;
        }
        yield return new WaitForSeconds(1.1f);
        // Do damage to player
        yield return new WaitForSeconds(0.1f);
        canAtk = true;
        speed = 5;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // playerMovement.TakingDamage(damageDeal);
        }
    }

    private void MoveCharacter(Vector2 dir)
    {
        rb.MovePosition((Vector2)transform.position + (dir * speed * Time.fixedDeltaTime));
    }

    private void UpdateAnimatorBools()
    {
        animator.SetBool("IsMoving", isInChasingRange);
        animator.SetBool("IsAtk", isInAtkRange);
    }
}
