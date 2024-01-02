using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemiesMovement : MonoBehaviour
{
    public float speed = 5f;

    public float chaseDistance = 5f;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Transform player;

    public bool isStatue = false;

    private bool StartCoroutine = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= chaseDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;

                direction.x = Mathf.Round(direction.x);
                direction.y = Mathf.Round(direction.y);

                if (direction != Vector2.zero)
                {
                    transform.up = direction;
                }

                transform.Translate(Vector2.up * speed * Time.deltaTime);

                if (!StartCoroutine)
                {
                    StartCoroutine(CantMove());
                    StartCoroutine = true;
                }
                else
                {
                    StartCoroutine = false;
                }
            }
        }
    }

    IEnumerator CantMove()
    {
        animator.SetTrigger("SetTrigger");
        speed = 0;
        isStatue = false;
        yield return new WaitForSeconds(.7f);
        speed = 5;
    }
}
