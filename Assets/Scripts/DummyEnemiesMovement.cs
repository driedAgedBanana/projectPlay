using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemiesMovement : MonoBehaviour
{
    public float speed = 5f;

    public float chaseDistance = 5f;

    public Transform player;

    // Update is called once per frame
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
            }
        }
    }
}
